using app_web_net.Models;
using app_web_net.ViewModels;
using app_web_net.Repositories;
using System.Collections.ObjectModel;

namespace app_web_net.ViewModels
{
    public class OrderViewModel
    {
        private readonly OrderDataService _service;//pour utiliser ses methodes

        // On utilise un Dictionary pour un accès rapide (O(1)) aux commandes déjà chargées
        //pour ameliorer les performances
        private readonly Dictionary<int, Order> _salesCache = new();

        public Order? ActiveOrder { get; set; }

        //on utilise ObservableCollection plutôt qu'une simple List car elle notifie automatiquement
        public ObservableCollection<Order> Sales { get; } = new();


        //initalisation des proprietées des ventes
        public int SalesPending => Sales.Count(o => o.Statut == "Pending");
        public int SalesConfirmed => Sales.Count(o => o.Statut == "Confirmed");
        public int SalesShipped => Sales.Count(o => o.Statut == "Shipped");
        public int SalesCancelled => Sales.Count(o => o.Statut == "Cancelled");

        public OrderViewModel(OrderDataService service)
        {
            _service = service;
        }

        public async Task RetrieveOrdersAsync()
        {
            var orders = await _service.ExtractAll();
            Sales.Clear();
            _salesCache.Clear();
            foreach (var order in orders)
            {
                Sales.Add(order);
                _salesCache[order.CommandeId] = order;
            }
        }

        public async Task<Order?> SelectOrderAsync(int id)
        {
            ActiveOrder = await _service.LocateById(id);
            return ActiveOrder;
        }

        public async Task<bool> PlaceOrderAsync(List<PanierItemViewModel> cartItems, string shippingAddress, int buyerId)
        {
            try
            {
                if (!cartItems.Any())
                {
                    return false;
                }

                var order = new Order
                {
                    AdresseExpedition = shippingAddress,
                    AcheteurId = buyerId,
                    Statut = "Pending",
                    Montant = cartItems.Sum(p => p.MontantTotal),
                    DateEnregistrement = DateTime.UtcNow,
                    ElementsCommande = new List<OrderItem>()
                };

                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        ProduitId = cartItem.ProduitId,
                        LibelleProduit = cartItem.Libelle,
                        CoutUnitaire = cartItem.CoutUnitaire,
                        NombreUnites = cartItem.NombreUnites
                    };
                    order.ElementsCommande.Add(orderItem);
                }

                var result = await _service.Record(order);
                if (result)
                {
                    await RetrieveOrdersAsync();
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SetStatusAsync(int orderId, string status)
        {
            var result = await _service.ChangeStatus(orderId, status);
            if (result)
            {
                await RetrieveOrdersAsync();
            }
            return result;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var result = await _service.Remove(orderId);
            if (result)
            {
                await RetrieveOrdersAsync();
            }
            return result;
        }

        public async Task<List<Order>> FindByStatusAsync(string status)
        {
            return await _service.FilterByStatus(status);
        }

        public int TotalSales => Sales.Count;
        public decimal TotalSalesAmount => Sales.Sum(o => o.Montant);
        
    }
}