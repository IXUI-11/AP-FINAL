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
    public class utilisateursController : Controller
    {
        public readonly string _connectionString;

        public utilisateursController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        [HttpGet]
        [Route("Single")]
        public ActionResult<IConfiguration>>
    }
    

}