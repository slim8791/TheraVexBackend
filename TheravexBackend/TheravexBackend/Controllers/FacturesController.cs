using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TheravexBackend.Data;
using TheravexBackend.DTOs;
using TheravexBackend.Models;

[ApiController]
[Route("api/factures")]
[Authorize]
public class FacturesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FacturesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ---------------- CREATE FACTURE ----------------
    [HttpPost]
    public async Task<IActionResult> Create(FacturePdfDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var facture = new Facture
            {
                ClientId = int.Parse(dto.Client),
                Numero = $"FAC-{DateTime.Now:yyyyMMddHHmmss}"
            };

            decimal totalHT = 0, totalTVA = 0;

            foreach (var l in dto.Lignes)
            {
                var article = await _context.Articles.Include(a => a.Tva)
                    .FirstOrDefaultAsync(a => a.Id == int.Parse(l.Article));

                if (article == null || article.Stock < l.Quantite)
                    throw new Exception($"Stock insuffisant pour {int.Parse(l.Article)}");

                decimal prix = l.PrixUnitaire ?? article.PrixVente;
                decimal ligneHT = l.Quantite * prix * (1 - l.Remise / 100);
                decimal ligneTVA = ligneHT * (article.Tva.Taux / 100);

                article.Stock -= l.Quantite;

                facture.Lignes.Add(new FactureLigne
                {
                    ArticleId = article.Id,
                    Quantite = l.Quantite,
                    PrixUnitaire = prix,
                    Remise = l.Remise,
                    TotalHT = ligneHT,
                    TauxTva = article.Tva.Taux,
                    MontantTva = ligneTVA,
                    TotalTTC = ligneHT + ligneTVA
                });

                totalHT += ligneHT;
                totalTVA += ligneTVA;
            }

            facture.TotalHT = totalHT;
            facture.TotalTVA = totalTVA;
            facture.TotalTTC = totalHT + totalTVA;

            _context.Factures.Add(facture);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(facture);
        }
        catch
        {
            await transaction.RollbackAsync();
            return BadRequest("Erreur lors de la création de la facture");
        }
    }

    // ---------------- ANNULATION FACTURE ----------------
    [HttpPost("{id}/annuler")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Annuler(int id)
    {
        var facture = await _context.Factures
            .Include(f => f.Lignes)
            .ThenInclude(l => l.Article)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (facture == null || facture.Statut == StatutFacture.Annulee)
            return BadRequest();

        foreach (var l in facture.Lignes)
        {
            l.Article.Stock += l.Quantite;
        }

        facture.Statut = StatutFacture.Annulee;
        await _context.SaveChangesAsync();

        return Ok("Facture annulée (avoir généré)");
    }

    // ---------------- LISTE FACTURES ----------------
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var factures = await _context.Factures
            .Include(f => f.Client)
            .Include(f => f.Lignes)
            .ThenInclude(l => l.Article)
            .ToListAsync();

        return Ok(factures);
    }

    // ---------------- GENERATION PDF ----------------
    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GetPdf(int id)
    {
        var facture = await _context.Factures
            .Include(f => f.Client)
            .Include(f => f.Lignes)
            .ThenInclude(l => l.Article)
            .ThenInclude(a => a.Tva)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (facture == null) return NotFound();

        // Préparer DTO PDF
        var pdfDto = new FacturePdfDto
        {
            Numero = facture.Numero,
            Date = facture.Date,
            Client = facture.Client.Nom,
            TotalHT = facture.Total,
            TotalTVA = facture.t,
            TotalTTC = facture.TotalTTC,
            Lignes = facture.Lignes.Select(l => new FacturePdfLigneDto
            {
                Article = l.Article.Nom,
                Quantite = l.Quantite,
                PrixUnitaire = l.PrixUnitaire,
                TotalHT = l.TotalHT,
                TVA = l.MontantTva,
                TotalTTC = l.TotalTTC
            }).ToList()
        };

        // Génération PDF via QuestPDF
        var pdfBytes = GeneratePdf(pdfDto);

        return File(pdfBytes, "application/pdf", $"{facture.Numero}.pdf");
    }

    // ---------------- Méthode Génération PDF ----------------
    private byte[] GeneratePdf(FacturePdfDto dto)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);
                page.Header().Text($"Facture {dto.Numero}").Bold().FontSize(20);
                page.Content().Stack(stack =>
                {
                    stack.Item().Text($"Date: {dto.Date:dd/MM/yyyy}");
                    stack.Item().Text($"Client: {dto.Client}");
                    stack.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Article").Bold();
                            header.Cell().Text("Qté").Bold();
                            header.Cell().Text("Prix U").Bold();
                            header.Cell().Text("Total HT").Bold();
                            header.Cell().Text("TVA").Bold();
                            header.Cell().Text("Total TTC").Bold();
                        });

                        foreach (var l in dto.Lignes)
                        {
                            table.Cell().Text(l.Article);
                            table.Cell().Text(l.Quantite.ToString());
                            table.Cell().Text(l.PrixUnitaire.ToString("0.00"));
                            table.Cell().Text(l.TotalHT.ToString("0.00"));
                            table.Cell().Text(l.TVA.ToString("0.00"));
                            table.Cell().Text(l.TotalTTC.ToString("0.00"));
                        }
                    });

                    stack.Item().Text($"Total HT: {dto.TotalHT:0.00}");
                    stack.Item().Text($"Total TVA: {dto.TotalTVA:0.00}");
                    stack.Item().Text($"Total TTC: {dto.TotalTTC:0.00}");
                });
            });
        });

        return doc.GeneratePdf();
    }
}
