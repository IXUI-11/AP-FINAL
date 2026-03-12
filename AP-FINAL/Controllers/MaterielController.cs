using DataPOO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryPOO;
using System.Diagnostics.Eventing.Reader;

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

        /// <summary>
        /// Renvoie tous les objets avec les paramètres 
        /// </summary>
        /// <returns></returns>
        [Route("Search")]
        [HttpGet]
        public ActionResult <IEnumerable<Materiel>> GetAllMateriels([FromQuery] Materiel b)
        {
            MysqlRepository repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection"));

            List<Materiel> books = repo.GetByPredicate(b).Cast<Materiel>().ToList();
            return books;
        }

        [Route("Single")]
        [HttpGet]
        public Materiel GetMatrielById(int id)
        {
            MysqlRepository repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection"));

            Materiel book = (Materiel)repo.GetObjectById(new Materiel() { Id = id });

            return book;
        }

        [Route("Add")]
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<Materiel> Insert(Materiel materiel)
        {
            MysqlRepository repo = new MysqlRepository(_connectionString);

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

        [Route("Delete")]
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Materiel materiel)
        {
            var repo = new MysqlRepository(_configuration);

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

        }
    }
}
