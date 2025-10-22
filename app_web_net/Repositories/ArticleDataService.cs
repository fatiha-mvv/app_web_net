using Microsoft.EntityFrameworkCore;
using app_web_net.Models;
using app_web_net.Repositories;
using Microsoft.Extensions.Logging;

namespace app_web_net.Repositories
{
    public class ArticleDataService
    {
        private readonly AppDbContext _database;
        private readonly ILogger<ArticleDataService> _log;

        public ArticleDataService(AppDbContext database, ILogger<ArticleDataService> log)
        {
            _database = database;
            _log = log;
        }

        // implementation des operations CRUD ...
        public Task<bool> Insert(Article item)
        {
            _log.LogInformation("Tentative d'insertion d'un nouvel élément à {Time}", DateTime.UtcNow.ToString("o"));
            try
            {
                item.DateEnregistrement = DateTime.UtcNow;
                _database.StockProduits.Add(item);
                return _database.SaveChangesAsync().ContinueWith(t => t.Result > 0);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Échec de l'insertion de l'élément à {Time}", DateTime.UtcNow.ToString("o"));
                return Task.FromResult(false);
            }
        }


        public Task<bool> Update(Article item)
        {
            _log.LogInformation("Mise à jour de l'élément ID {ItemId} à {Time}", item.ArticleId, DateTime.UtcNow.ToString("o"));
            try
            {
                var existingItem = _database.StockProduits
                    .FirstOrDefault(p => p.ArticleId == item.ArticleId);
                if (existingItem == null)
                {
                    _log.LogWarning("Élément ID {ItemId} introuvable pour mise à jour à {Time}", item.ArticleId, DateTime.UtcNow.ToString("o"));
                    return Task.FromResult(false);
                }

                existingItem.Libelle = item.Libelle;
                existingItem.Categorie = item.Categorie;
                existingItem.Description = item.Description;
                existingItem.Tarif = item.Tarif;
                existingItem.StockDisponible = item.StockDisponible;
                existingItem.DateEnregistrement = DateTime.UtcNow;

                return _database.SaveChangesAsync().ContinueWith(t => t.Result > 0);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Échec de la mise à jour de l'élément ID {ItemId} à {Time}", item.ArticleId, DateTime.UtcNow.ToString("o"));
                return Task.FromResult(false);
            }
        }


        public Task<bool> Delete(int itemId)
        {
            _log.LogInformation("Suppression de l'élément ID {ItemId} à {Time}", itemId, DateTime.UtcNow.ToString("o"));
            try
            {
                var item = _database.StockProduits
                    .FirstOrDefault(p => p.ArticleId == itemId);
                if (item == null)
                {
                    _log.LogWarning("Élément ID {ItemId} introuvable pour suppression à {Time}", itemId, DateTime.UtcNow.ToString("o"));
                    return Task.FromResult(false);
                }

                _database.StockProduits.Remove(item);
                return _database.SaveChangesAsync().ContinueWith(t => t.Result > 0);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Échec de la suppression de l'élément ID {ItemId} à {Time}", itemId, DateTime.UtcNow.ToString("o"));
                return Task.FromResult(false);
            }
        }

        public Task<List<Article>> RetrieveAll()
        {
            _log.LogInformation("Lancement de l'extraction des produits à {Time}", DateTime.UtcNow.ToString("o"));
            try
            {
                return _database.StockProduits
                    .OrderByDescending(p => p.StockDisponible)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Problème lors de l'extraction des produits à {Time}", DateTime.UtcNow.ToString("o"));
                return Task.FromResult(new List<Article>());
            }
        }

        public Task<Article?> GetById(int itemId)
        {
            _log.LogInformation("Recherche de l'élément avec l'ID {ItemId} à {Time}", itemId, DateTime.UtcNow.ToString("o"));
            try
            {
                return _database.StockProduits
                    .FirstOrDefaultAsync(p => p.ArticleId == itemId);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Problème lors de la recherche de l'élément ID {ItemId} à {Time}", itemId, DateTime.UtcNow.ToString("o"));
                return Task.FromResult<Article?>(null);
            }
        }

        

        public Task<bool> SetStock(int itemId, int stockValue)
        {
            _log.LogInformation("Ajustement du stock pour l'élément ID {ItemId} à {Time}", itemId, DateTime.UtcNow.ToString("o"));
            try
            {
                if (stockValue < 0)
                {
                    _log.LogWarning("Valeur de stock invalide ({StockValue}) pour l'élément ID {ItemId} à {Time}", stockValue, itemId, DateTime.UtcNow.ToString("o"));
                    return Task.FromResult(false);
                }

                var item = _database.StockProduits
                    .FirstOrDefault(p => p.ArticleId == itemId);
                if (item == null)
                {
                    _log.LogWarning("Élément ID {ItemId} introuvable pour ajustement du stock à {Time}", itemId, DateTime.UtcNow.ToString("o"));
                    return Task.FromResult(false);
                }

                item.StockDisponible = stockValue;
                return _database.SaveChangesAsync().ContinueWith(t => t.Result > 0);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Échec de l'ajustement du stock pour l'élément ID {ItemId} à {Time}", itemId, DateTime.UtcNow.ToString("o"));
                return Task.FromResult(false);
            }
        }
    }
}