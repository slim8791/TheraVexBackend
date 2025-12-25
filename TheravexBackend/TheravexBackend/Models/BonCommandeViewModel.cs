using TheravexBackend.DTOs;

namespace TheravexBackend.Models
{
    public class BonCommandeViewModel
    {
        public string Numero { get; set; }
        public DateTime Date { get; set; }
        public string FournisseurNom { get; set; }
        public string FournisseurMF { get; set; }
        public string FournisseurAdresse { get; set; }
        public string FournisseurTel { get; set; }
        public List<BonCommandeLigneVM> Lignes { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal NetAPayer { get; set; }
        public decimal TotalTva7 { get; set; }
        public decimal Tva7 { get; set; }
        public decimal TotalTva19 { get; set; }
        public decimal Tva19 { get; set; }
        public decimal SommeTauxTva { get; set; }
        public decimal TotalTTC { get; set; }
    }

    public class BonCommandeLigneVM
    {
        public string Reference { get; set; }
        public string Designation { get; set; }
        public int Qte { get; set; }
        public decimal PUHT { get; set; }
        public int TVA { get; set; }
        public decimal TotalHT { get; set; }
    }
}
