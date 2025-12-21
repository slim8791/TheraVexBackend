using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TheravexBackend.Data;
using TheravexBackend.DTOs;
using TheravexBackend.Helpers;
using TheravexBackend.Models;
using TheravexBackend.Services;

[ApiController]
[Route("api/factures")]
[Authorize]
public class FacturesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IRazorViewToStringRenderer _renderer;
    private readonly IConverter _converter;

    public FacturesController(ApplicationDbContext context, IRazorViewToStringRenderer renderer, IConverter converter)
    {
        _context = context;
        _renderer = renderer;
        _converter = converter;
    }

    // ---------------- CREATE FACTURE ----------------
    [HttpPost]
    public async Task<IActionResult> Create(FacturePdfDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        var lastDocument = _context.Factures.Where(d => d.TypeDocument == dto.TypeDocument).OrderByDescending(f => f.Numero).FirstOrDefault();
        string numDoc = lastDocument== null ? "00001": (lastDocument.Numero++).ToString("00000");
        try
        {
            var facture = new Facture
            {
                ClientId = int.Parse(dto.Client),
                Numero = int.Parse(numDoc),
                FullNumber = $"{dto.TypeDocument}-{numDoc}-{DateTime.Now:yyyy}",
                TypeDocument = dto.TypeDocument,
                Date = dto.Date,
                

            };

            decimal totalHT = 0, totalTVA = 0;

            foreach (var l in dto.Lignes)
            {
                var article = await _context.Articles.Include(a => a.Tva)
                    .FirstOrDefaultAsync(a => a.Id == int.Parse(l.Article));

                if (article == null || article.Stock < l.Quantite)
                    throw new Exception($"Stock insuffisant pour {int.Parse(l.Article)}");

                decimal prix = l.PrixUnitaire != 0 ? l.PrixUnitaire : article.PrixVente;
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


        foreach (var l in facture.Lignes)
        {
            l.Article.Stock += l.Quantite;
        }

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
        decimal totalAvantRemise = 0, totalHT = 0, totalTVA = 0, totalTTC = 0;
        decimal totalTva7 = 0, totalTva19 = 0;
        decimal tva7 = 0, tva19 = 0;

        var facture = await _context.Factures
            .Include(f => f.Client)
            .Include(f => f.Lignes)
            .ThenInclude(l => l.Article)
            .ThenInclude(a => a.Tva)
            .FirstOrDefaultAsync(f => f.Id == id);
        List<FactureLigneVM> factureLigneVMs = new List<FactureLigneVM>();
        foreach (var item in facture.Lignes)
        {
            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.Id == item.ArticleId);

            switch(item.TauxTva)
            {
                case 7:
                    totalTva7 += (item.PrixUnitaire * item.Quantite * (1 - (item.Remise / 100))) * (item.TauxTva / 100);
                    tva7 += item.PrixUnitaire * item.Quantite * (1 - (item.Remise / 100));
                    break;
                case 19:
                    totalTva19 += (item.PrixUnitaire * item.Quantite * (1 - (item.Remise / 100))) * (item.TauxTva / 100);
                    tva19 += item.PrixUnitaire * item.Quantite * (1 - (item.Remise / 100));
                    break;
            }

            totalTVA += (item.PrixUnitaire * item.Quantite * (1 - (item.Remise / 100))) * (item.TauxTva / 100);
            totalHT += item.PrixUnitaire * item.Quantite * (1 - (item.Remise / 100));
            totalAvantRemise += item.PrixUnitaire * item.Quantite;

            factureLigneVMs.Add(new FactureLigneVM
            {
                Designation = article.Nom,
                PUHT = item.PrixUnitaire,
                Qte = item.Quantite,
                Reference = article.Code,
                TotalHT = item.PrixUnitaire * item.Quantite * (1 - item.Remise / 100),
                TVA = (int)item.TauxTva,
                Remise = item.Remise

            });
        }

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == facture.ClientId);

        FactureViewModel fvm = new FactureViewModel
        {
            ClientNom = client.Nom,
            Numero = facture.FullNumber,
            ClientAdresse = client.Adresse,
            ClientMF = client.MatriculeFiscale,
            ClientTel = client.Telephone.ToString(),
            Date = DateTime.Now,
            Lignes = factureLigneVMs,
            TypeDocument = facture.TypeDocument,
            Timbre = facture.TypeDocument == TypeDocument.FCT ? 1:0,
            TotalTva7 = totalTva7,
            Tva7 = tva7,
            Tva19 = tva19,
            SommeTauxTva = tva7 + tva19,
            TotalTva19 = totalTva19,
            TotalHT = totalHT,
            TotalTVA = totalTVA,
            TotalAvantRemise = totalAvantRemise,
            TotalRemise = totalAvantRemise - totalHT
        };
        fvm.TotalTTC = fvm.TotalHT + fvm.TotalTVA + fvm.Timbre;
        fvm.MontantEnLettres = FrenchNumberToWords.ToWords(fvm.TotalTTC);
        fvm.NetAPayer = fvm.TotalTTC;
        var html = await _renderer.RenderViewToStringAsync("Facture", fvm);

        // Build file:// base url pointing to the wwwroot folder so relative URLs like /Images/logo.png resolve
        var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        // wkhtmltopdf expects forward slashes in file:// URLs
        var baseUrl = $"file:///{wwwroot.Replace('\\', '/')}/";

        // Inject a <base> tag into the rendered HTML so wkhtmltopdf resolves relative paths.
        // DinkToPdf's WebSettings does not expose a BaseUrl property, so use the HTML base element instead.
        html = $"<base href=\"{baseUrl}\">{html}";

        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = {
            PaperSize = PaperKind.A4,
            Orientation = Orientation.Portrait,
            Margins = new MarginSettings { Top = 10 }
        },
            Objects = {
            new ObjectSettings {
                HtmlContent = html,
                WebSettings = {
                    DefaultEncoding = "utf-8",
                    LoadImages = true
                }
            }
        }
        };

        var file = _converter.Convert(pdf);

        // Save generated PDF to disk (project content root / GeneratedPdfs)
        string docType = "";
        switch (facture.TypeDocument)
        {
            case TypeDocument.Avoir:
                docType = "Avoir";
                break;
            case TypeDocument.BL:
                docType = "Bon_Livraison";
                break;
            case TypeDocument.BS:
                docType = "Bon_Sortie";
                break;
            case TypeDocument.FCT:
                docType = "Facture";
                break;

        }
        string fileName = $"{docType}_{client.Nom}_{facture.FullNumber}.pdf";
        string saveDir = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedPdfs",docType, client.Nom);
        Directory.CreateDirectory(saveDir);
        string fullPath = Path.Combine(saveDir, fileName);
        await System.IO.File.WriteAllBytesAsync(fullPath, file);

        return File(file, "application/pdf", $"Facture_{facture.FullNumber}.pdf");
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
