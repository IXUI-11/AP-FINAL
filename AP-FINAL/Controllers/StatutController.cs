using DataPOO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryPOO;
using System.Diagnostics.Eventing.Reader;

namespace AP_FINAL.Controllers
{
    [ApiController]
    [Route("Statut")]
    public class statutController : Controller
    {

        private readonly string _connectionString;

        public statutController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        [Route("Searche")]
        public ActionResult<IEnumerable<Statut>> GetAllStatut([FromQuery] Statut b)
        {
            var repo = new MysqlRepository(_connectionString);
            var statuts = repo.GetByPredicate(b).Cast<Statut>().ToList();
            return Ok(statuts);
        }

        [HttpGet]
        [Route("Single")]
        public ActionResult<Statut> GetStatusById(int id)
        {
            var repo = new MysqlRepository(_connectionString);
            var statut = (Statut)repo.GetObjectById(new Statut { Id = id });
            if (statut == null)
            {
                return NotFound();
            }
            return Ok(statut);
        }

        

    }

}
