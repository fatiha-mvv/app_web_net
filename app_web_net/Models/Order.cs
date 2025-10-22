using app_web_net.Models;
using System.ComponentModel.DataAnnotations;

namespace app_web_net.Models
{
    public class Order
    {
        public int CommandeId { get; set; }

        [Required(ErrorMessage = "L'adresse d'expédition est requise")]
        public string AdresseExpedition { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'identifiant de l'acheteur est requis")]
        public int AcheteurId { get; set; }

        public List<OrderItem> ElementsCommande { get; set; } = new();

        [Required(ErrorMessage = "Le statut est requis")]
        public string Statut { get; set; } = "En traitement"; // En traitement, Confirmée, Envoyée, Annulée

        [Required(ErrorMessage = "Le montant est requis")]
        public decimal Montant { get; set; }

        [Required(ErrorMessage = "La date d'enregistrement est requise")]
        public DateTime DateEnregistrement { get; set; } = DateTime.UtcNow;

        public int QuantiteTotale => ElementsCommande.Sum(ec => ec.NombreUnites);

    }
}