using System;
using System.ComponentModel.DataAnnotations;

namespace app_web_net.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "Le libellé est requis")]
        public string Libelle { get; set; } = string.Empty;

        [Required(ErrorMessage = "La catégorie est requise")]
        public string Categorie { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est requise")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le tarif est requis")]
        public decimal Tarif { get; set; }

        [Required(ErrorMessage = "Le stock disponible est requis")]
        public int StockDisponible { get; set; }

        public DateTime DateEnregistrement { get; set; } = DateTime.UtcNow;
    }
}
