using DataPOO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryPOO;
using System.Diagnostics.Eventing.Reader;

namespace AP_FINAL.Controllers
{


    [ApiController]
    [Route("Reservation")]
    public class ReservationController : Controller
    {
        private readonly IConfiguration _configuration;

        public ReservationController(IConfiguration configuration)
        {

            _configuration = configuration;

        }


        
        // Pour ajouter une Résa
        [HttpPost("Add")]
        [Authorize]
        public ActionResult<Reservation> AddReservation([FromBody] Reservation reservation)
        {
         
                var repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection"));
                reservation.DateReservation = DateTime.UtcNow;
                // Le statut par défaut est 1 
                reservation.IdStatus = 1;
                int id = repo.SaveObject(reservation);
                if (id > 0)
                {
                    reservation.Id = id;
                    return Ok(reservation);
                }
                else
                {
                    return BadRequest("non trouvé");
                }
            
            
        }

        // Récupre tout les Résa par l'Admin
        [HttpGet("Search")]
        [Authorize]
        public ActionResult<IEnumerable<Reservation>> GetReservations()
        {
            var repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection"));
            var reservations = repo.GetByPredicate(new Reservation()).Cast<Reservation>().ToList();
            return Ok(reservations);
        }
        
        // Update de Status par l'Admin
        [HttpPut("UpdateStatut")]
        [Authorize]
        public ActionResult UpdateStatut([FromBody] Reservation reservation)
        {
            var repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection"));
            int result = repo.SaveObject(reservation);
            if (result > 0)
            {
                return Ok(reservation);
            }
            else
            {
                return BadRequest("Erreur lors de la mise à jour.");
            }
        }

        /// <summary>
        /// Vérifie la disponibilité d'un matériel pour une période donnée.
        /// </summary>
        /// <remarks>
        /// La vérification ignore les réservations refusées (IdStatus == 3).
        /// La logique de chevauchement s'assure qu'aucune réservation existante ne croise les dates demandées.
        /// </remarks>
        /// <param name="idMateriel">L'identifiant unique du matériel à vérifier.</param>
        /// <param name="dateDebut">La date et l'heure de début de la réservation souhaitée.</param>
        /// <param name="dateFin">La date et l'heure de fin de la réservation souhaitée.</param>
        /// <returns>Retourne true si le matériel est disponible, sinon false.</returns>
        [HttpGet("Disponibilite")]
        public ActionResult<bool> VerifierDisponibilite([FromQuery] int idMateriel, [FromQuery] DateTime dateDebut, [FromQuery] DateTime dateFin)
        {
            var repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection"));

            // Récupère toutes les réservations liées à ce matériel
            var reservations = repo.GetByPredicate(new Reservation() { IdMateriel = idMateriel }).Cast<Reservation>().ToList();

            // Un matériel est disponible SI aucune réservation ne correspond aux critères suivants :
            bool disponible = !reservations.Any(r =>
                r.IdStatus != 3 && // Le statut n'est pas "Refusée"
                r.DateDebut < dateFin && // La réservation existante commence avant la fin de la nouvelle
                r.DateFin > dateDebut    // ET la réservation existante se termine après le début de la nouvelle
            );

            return Ok(disponible);
        }


    }
}