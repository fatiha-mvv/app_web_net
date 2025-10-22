using app_web_net.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace app_web_net.ViewModels
{
    public class CartViewModel
    {
        private readonly Dictionary<int, PanierItemViewModel> _itemMap = new();
        public ObservableCollection<PanierItemViewModel> Basket { get; } = new();

        public int TotalUnits => Basket.Sum(i => i.NombreUnites);

        public decimal TotalCost => Basket.Sum(i => i.MontantTotal);


        public void AddProduct(Article product, int units = 1)
        {
            if (_itemMap.TryGetValue(product.ArticleId, out var existingItem))
            {
                existingItem.NombreUnites += units;
            }
            else
            {
                var newItem = new PanierItemViewModel
                {
                    ProduitId = product.ArticleId,
                    Libelle = product.Libelle,
                    CoutUnitaire = product.Tarif,
                    NombreUnites = units
                };
                Basket.Add(newItem);
                _itemMap[product.ArticleId] = newItem;
            }
        }



        public void ImportFromJson(List<PanierItemViewModel> items)
        {
            Basket.Clear();
            _itemMap.Clear();
            foreach (var item in items)
            {
                Basket.Add(item);
                _itemMap[item.ProduitId] = item;
            }
        }


        public void DropProduct(int productId)
        {
            if (_itemMap.TryGetValue(productId, out var item))
            {
                Basket.Remove(item);
                _itemMap.Remove(productId);
            }
        }

        public void ResetCart()
        {
            Basket.Clear();
            _itemMap.Clear();
        }

        public List<PanierItemViewModel> GetJsonData()
        {
            return Basket.ToList();
        }

        
        public void SetUnits(int productId, int units)
        {
            if (_itemMap.TryGetValue(productId, out var item))
            {
                if (units <= 0)
                {
                    Basket.Remove(item);
                    _itemMap.Remove(productId);
                }
                else
                {
                    item.NombreUnites = units;
                }
            }
        }

    }
}