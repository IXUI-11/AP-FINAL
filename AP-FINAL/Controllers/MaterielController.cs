using DataPOO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

        /// <summary>
        /// Renvoie tous les objets avec les paramètres 
        /// </summary>
        /// <returns></returns>
        [Route("Search")]
        [HttpGet]
        public IEnumerable<Materiel> GetAllMatriel([FromQuery] Materiel b)
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
    }
}