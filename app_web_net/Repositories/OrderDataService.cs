using Microsoft.EntityFrameworkCore;
using app_web_net.Models;
using app_web_net.Repositories;
using Microsoft.Extensions.Logging;

namespace app_web_net.Repositories
{
    public class OrderDataService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<OrderDataService> _log;

        public OrderDataService(AppDbContext db, ILogger<OrderDataService> log)
        {
            _db = db;
            _log = log;
        }


        public Task<bool> Record(Order commande)
        {
            _log.LogInformation("Tentative d'enregistrement d'une commande à {Time}", DateTime.UtcNow.ToString("o"));
            try
            {
                commande.DateEnregistrement = DateTime.UtcNow;
                _db.Ventes.Add(commande);
                return _db.SaveChangesAsync().ContinueWith(t => t.Result > 0);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Échec de l'enregistrement de la commande à {Time}", DateTime.UtcNow.ToString("o"));
                return Task.FromResult(false);
            }
        }


        public Task<Order?> LocateById(int commandeId)
        {
            _log.LogInformation("Recherche de la commande ID {CommandeId} à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
            try
            {
                return _db.Ventes
                    .SingleOrDefaultAsync(c => c.CommandeId == commandeId);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Problème survenu lors de la recherche de la commande ID {CommandeId} à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
                return Task.FromResult<Order?>(null);
            }
        }


        public Task<List<Order>> ExtractAll()
        {
            _log.LogInformation("Début de l'extraction des commandes à {Time}", DateTime.UtcNow.ToString("o"));
            try
            {
                return _db.Ventes
                    .OrderByDescending(c => c.Montant)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Problème survenu lors de l'extraction des commandes à {Time}", DateTime.UtcNow.ToString("o"));
                return Task.FromResult(new List<Order>());
            }
        }

        public Task<bool> Remove(int commandeId)
        {
            _log.LogInformation("Suppression de la commande ID {CommandeId} à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
            try
            {
                var commande = _db.Ventes
                    .SingleOrDefault(c => c.CommandeId == commandeId);
                if (commande == null)
                {
                    _log.LogWarning("Commande ID {CommandeId} introuvable pour suppression à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
                    return Task.FromResult(false);
                }

                _db.Ventes.Remove(commande);
                return _db.SaveChangesAsync().ContinueWith(t => t.Result > 0);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Échec de la suppression de la commande ID {CommandeId} à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
                return Task.FromResult(false);
            }
        }

        public Task<bool> ChangeStatus(int commandeId, string etat)
        {
            _log.LogInformation("Modification de l'état de la commande ID {CommandeId} à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
            try
            {
                if (string.IsNullOrEmpty(etat))
                {
                    _log.LogWarning("État vide pour la commande ID {CommandeId} à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
                    return Task.FromResult(false);
                }

                var commande = _db.Ventes
                    .SingleOrDefault(c => c.CommandeId == commandeId);
                if (commande == null)
                {
                    _log.LogWarning("Commande ID {CommandeId} introuvable pour modification d'état à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
                    return Task.FromResult(false);
                }

                commande.Statut = etat;
                return _db.SaveChangesAsync().ContinueWith(t => t.Result > 0);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Échec de la modification de l'état de la commande ID {CommandeId} à {Time}", commandeId, DateTime.UtcNow.ToString("o"));
                return Task.FromResult(false);
            }
        }


        public Task<List<Order>> FilterByStatus(string etat)
        {
            _log.LogInformation("Filtrage des commandes par état {Etat} à {Time}", etat, DateTime.UtcNow.ToString("o"));
            try
            {
                return _db.Ventes
                    .Where(c => c.Statut == etat)
                    .OrderBy(c => c.DateEnregistrement)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Problème survenu lors du filtrage des commandes par état {Etat} à {Time}", etat, DateTime.UtcNow.ToString("o"));
                return Task.FromResult(new List<Order>());
            }
        }
    }
}