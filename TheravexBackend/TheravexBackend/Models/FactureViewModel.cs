using TheravexBackend.DTOs;

namespace TheravexBackend.Models
{
    public class FactureViewModel
    {
        public string Numero { get; set; }
        public DateTime Date { get; set; }
        public TypeDocument TypeDocument { get; set; }
        public string ClientNom { get; set; }
        public string ClientMF { get; set; }
        public string ClientAdresse { get; set; }
        public string ClientTel { get; set; }
        public List<FactureLigneVM> Lignes { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal Timbre { get; set; }
        public decimal NetAPayer { get; set; }
        public decimal TotalTva7 { get; set; }
        public decimal Tva7 { get; set; }
        public decimal TotalTva19 { get; set; }
        public decimal Tva19 { get; set; }
        public decimal SommeTauxTva { get; set; }
        public decimal TotalAvantRemise { get; set; }
        public decimal TotalRemise { get; set; }
        public decimal TotalTTC { get; set; }
        public string MontantEnLettres { get; set; }
    }

    public class FactureLigneVM
    {
        public string Reference { get; set; }
        public string Designation { get; set; }
        public int Qte { get; set; }
        public decimal PUHT { get; set; }
        public decimal Remise { get; set; }
        public int TVA { get; set; }
        public decimal TotalHT { get; set; }
    }
}
