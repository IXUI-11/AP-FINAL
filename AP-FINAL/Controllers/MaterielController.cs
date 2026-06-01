using DataPOO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryPOO;

namespace AP_FINAL.Controllers
{
    [ApiController]
    [Route("Materiel")]
    public class MaterielController : Controller
    {
        private readonly IConfiguration _configuration;
        public MaterielController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("Search")]
        [HttpGet]
        public ActionResult<IEnumerable<Materiel>> GetAllMateriels([FromQuery] Materiel m)
        {
            MysqlRepository repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection")!);
            List<Materiel> materiels = repo.GetByPredicate(m).Cast<Materiel>().ToList();
            return materiels;
        }

        [Route("Single")]
        [HttpGet]
        public Materiel GetMatrielById(int id)
        {
            MysqlRepository repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection")!);
            Materiel materiel = (Materiel)repo.GetObjectById(new Materiel() { Id = id });
            return materiel;
        }

        // UPDATE - Modifier un matériel existant
        [Route("Update")]
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<Materiel> Update(Materiel materiel)
        {
            MysqlRepository repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection")!);
            if (materiel.Id > 0)
            {
                int id = repo.SaveObject(materiel);
                if (id > 0)
                {
                    return Ok(materiel);
                }
                else
                {
                    return StatusCode(500);
                }
            }

            else
            {

                return BadRequest();
            }
          
        }

        // INSERT - Ajouter un nouveau matériel
        [Route("Add")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Materiel> Insert(Materiel materiel)
        {
            MysqlRepository repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection")!);
            int id = repo.SaveObject(materiel);
            if (id > 0)
            {
                materiel.Id = id;
                return Ok(materiel);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [Route("Delete")]
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Materiel materiel)
        {
            var repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection")!);
            if (materiel.Id > 0)
            {
                bool res = repo.DeleteObject(materiel);
                if (res)
                {
                    return Ok(materiel);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}