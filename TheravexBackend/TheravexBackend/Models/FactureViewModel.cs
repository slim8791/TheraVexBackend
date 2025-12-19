namespace TheravexBackend.Models
{
    public class FactureViewModel
    {
        public string Numero { get; set; }
        public DateTime Date { get; set; }

        public string ClientNom { get; set; }
        public string ClientActivite { get; set; }
        public string ClientMF { get; set; }
        public string ClientAdresse { get; set; }
        public string ClientTel { get; set; }

        public List<FactureLigneVM> Lignes { get; set; }

        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal Timbre { get; set; }
        public decimal NetAPayer { get; set; }

        public string MontantEnLettres { get; set; }
    }

    public class FactureLigneVM
    {
        public string Reference { get; set; }
        public string Designation { get; set; }
        public int Qte { get; set; }
        public decimal PUHT { get; set; }
        public int TVA { get; set; }
        public decimal TotalTTC { get; set; }
    }
}
