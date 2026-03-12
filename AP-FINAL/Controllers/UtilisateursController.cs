using DataPOO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryPOO;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;

namespace AP_FINAL.Controllers
{
    [ApiController]
    [Route("Utilisateurs")]
    public class UtilisateursController : Controller
    {
        public readonly string _connectionString;

        public UtilisateursController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        [Route("Search")]
        public ActionResult<IEnumerable<Utilisateurs>> GetAllUtilisateurs([FromQuery] Utilisateurs b)
        {
            var repo = new MysqlRepository(_connectionString);
            var utilisateurs = repo.GetByPredicate(b).Cast<Utilisateurs>().ToList();
            return Ok(utilisateurs);
        }

        [HttpGet]
        [Route("Single")]
        public ActionResult<Utilisateurs> GetUtilisateursById(int id)
        {
            var repo = new MysqlRepository(_connectionString);
            var utilisateur = (Utilisateurs)repo.GetObjectById(new Utilisateurs { Id = id });
            if (utilisateur == null)
                return NotFound();
            return Ok(utilisateur);
        }
    }
}

