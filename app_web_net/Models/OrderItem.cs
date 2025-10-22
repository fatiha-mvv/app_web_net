using app_web_net.Models;
using System.ComponentModel.DataAnnotations;

namespace app_web_net.Models
{
    // Modele d'un élément dans une commande
    public class OrderItem
    {
        public int ElementId { get; set; }

        [Required(ErrorMessage = "L'identifiant de la commande est requis")]
        public int CommandeId { get; set; }


        [Required(ErrorMessage = "Le libellé du produit est requis")]
        public string LibelleProduit { get; set; } = string.Empty;


        [Required(ErrorMessage = "Le coût unitaire est requis")]
        public decimal CoutUnitaire { get; set; }

        [Required(ErrorMessage = "L'identifiant du produit est requis")]
        public int ProduitId { get; set; }


        [Required(ErrorMessage = "Le nombre d'unités est requis")]
        public int NombreUnites { get; set; }

        // calculé
        public decimal TotalPartiel => CoutUnitaire * NombreUnites;

    }
}