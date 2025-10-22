using System.ComponentModel.DataAnnotations;

namespace app_web_net.ViewModels
{
    public class PanierItemViewModel
    {
        [Required(ErrorMessage = "L'identifiant du produit est requis")]
        public int ProduitId { get; set; }

        [Required(ErrorMessage = "Le libellé est requis")]
        public string Libelle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le coût unitaire est requis")]
        public decimal CoutUnitaire { get; set; }

        [Required(ErrorMessage = "Le nombre d'unités est requis")]
        public int NombreUnites { get; set; }

        public decimal MontantTotal => CoutUnitaire * NombreUnites;
    }
}