using DataPOO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryPOO;

namespace AP_FINAL.Controllers
{
    [ApiController]
    [Route("Categorie")]
    public class CategorieController : Controller
    {
        private readonly string _connectionString;

        public CategorieController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        /// <summary>
        /// Renvoie tous les objets avec les paramètres spécifiés.
        /// </summary>
        [HttpGet("Search")]
        public ActionResult<IEnumerable<Categorie>> GetAllCategorie([FromQuery] Categorie b)
        {
            var repo = new MysqlRepository(_connectionString);
            var categories = repo.GetByPredicate(b).Cast<Categorie>().ToList();

            return Ok(categories);
        }

        [HttpGet("Single")]
        public ActionResult<Categorie> GetCategorieById(int id)
        {
            var repo = new MysqlRepository(_connectionString);
            var categorie = (Categorie)repo.GetObjectById(new Categorie { Id = id });

            if (categorie == null)
            {
                return NotFound();
            }

            return Ok(categorie);
        }


        [Route("Add")]
        [HttpPut]
        public ActionResult<Categorie> Insert(Categorie categorie)
        {
            MysqlRepository repo = new MysqlRepository(_connectionString);

            int id = repo.SaveObject(categorie);

            if (id > 0)
            {
                return Ok(categorie);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [Route("Update")]
        [HttpPost]
        public ActionResult<Categorie> Update(Categorie categorie)
        {
            MysqlRepository repo = new MysqlRepository(_connectionString);

            if (categorie.Id > 0)
            {
                int id = repo.SaveObject(categorie);

                if (id > 0)
                {
                    return Ok(categorie);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return StatusCode(400);
            }
        }

        [Route("Delete")]
        [HttpDelete]
        public ActionResult<Categorie> Delete(Categorie categorie)
        {
            var repo = new MysqlRepository(_connectionString);

            if (categorie.Id > 0)
            {
                bool res = repo.DeleteObject(categorie);

                if (res)
                {
                    return Ok(categorie);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return StatusCode(400);
            }
        }


        // pour récuperer les catégories avec les matériels associés 
        // a revoir pour bien comprendre
        [HttpGet("CategoriesAvecMateriels")]
        public ActionResult GetCategoriesAvecMateriels()
        {
            var repo = new MysqlRepository(_connectionString);
            var  categories = repo.GetByPredicate(new Categorie()).Cast<Categorie>().ToList();
            var materiels = repo.GetByPredicate(new Materiel()).Cast<Materiel>().ToList();


            var resultat = categories.Select(categories => new
            {
                id = categories.Id,
                libelleCategorie = categories.LibelleCategorie,
                modeles = materiels
                .Where(materiels => materiels.IdCategorie == categories.Id)
                .Select(materiels => new
                {
                    id = materiels.Id,
                    nomMateriel = materiels.NomMateriel,
                    description = materiels.Description,
                    valeur = materiels.Valeur,
                    image = materiels.Image,
                    prix = materiels.Prix,
                    idCategorie = materiels.IdCategorie  // ← ajoute ça

                }).ToList()
            });
            return Ok(resultat);
        }
    }
}