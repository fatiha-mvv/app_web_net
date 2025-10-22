using app_web_net.Models;
using app_web_net.Repositories;
using System.Collections.ObjectModel;

namespace app_web_net.ViewModels
{
    public class ArticleViewModel
    {
        private readonly ArticleDataService _service; //pour appeler ses methodes

        //on utilise ObservableCollection plutôt qu'une simple List car elle notifie automatiquement
        public ObservableCollection<Article> Inventaire { get; set; } = new();

        public ArticleViewModel(ArticleDataService service)
        {
            _service = service;
        }

        public async Task FetchItems()
        {
            try
            {
                var items = await _service.RetrieveAll();
                Inventaire.Clear();
                foreach (var item in items)
                {
                    Inventaire.Add(item);
                }
            }
            catch (Exception)
            {
                Inventaire.Clear();
            }
        }

        public async Task<bool> StoreItem(Article item)
        {
            try
            {
                var result = await _service.Insert(item);
                if (result)
                {
                    await FetchItems();
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AlterItem(Article item)
        {
            try
            {
                var result = await _service.Update(item);
                if (result)
                {
                    await FetchItems();
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EraseItem(int itemId)
        {
            try
            {
                var result = await _service.Delete(itemId);
                if (result)
                {
                    await FetchItems();
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateInventory(int itemId, int stockLevel)
        {
            try
            {
                var result = await _service.SetStock(itemId, stockLevel);
                if (result)
                {
                    await FetchItems();
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}