using MySql.Data.MySqlClient;
using RepositoryPOO;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataPOO
{
    /// <summary>
    /// Classe gérant les matériels
    /// </summary>
    public class Materiel : BDDObject
    {
        #region Attributs
        /// <summary>
        /// Nom du matériel
        /// </summary>
        private string nomMateriel;
        public string NomMateriel { get => nomMateriel; set => nomMateriel = value; }

        /// <summary>
        /// Description du matériel
        /// </summary>
        private string description;
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// Valeur du matériel
        /// </summary>
        private string valeur;
        public string Valeur { get => valeur; set => valeur = value; }

        /// <summary>
        /// Identifiant de la catégorie
        /// </summary>
        private int idCategorie;
        public int IdCategorie { get => idCategorie; set => idCategorie = value; }

        /// <summary>
        /// Image du matériel
        /// </summary>
        private string image;
        public string Image { get => image; set => image = value; }

        /// <summary>
        /// Prix du matériel
        /// </summary>
        private decimal prix;
        public decimal Prix { get => prix; set => prix = value; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Materiel() { }

        /// <summary>
        /// Constructeur complet appelant le constructeur parent (BDDObject)
        /// </summary>
        public Materiel(string nomMateriel, string description, string valeur, int idCategorie, string image, decimal prix) : base()
        {
            this.NomMateriel = nomMateriel;
            this.Description = description;
            this.Valeur = valeur;
            this.IdCategorie = idCategorie;
            this.Image = image;
            this.Prix = prix;
        }
        #endregion

        #region SQL Commun
        /// <summary>
        /// Renvoie le nom de la table stockant les matériels
        /// </summary>
        public override string GetTableName()
        {
            return "Materiel";
        }

        /// <summary>
        /// Renvoie un dictionnaire contenant le nom de colonne PK et paramètre associé
        /// </summary>
        public override Dictionary<string, string> GetPrimaryKeyColumn()
        {
            return new Dictionary<string, string> { { "Id_Materiel", "@Id_Materiel" } };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour filtrer sur la clé primaire de l'objet
        /// </summary>
        public override List<IDataParameter> GetPrimaryKeyParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@Id_Materiel", MySqlDbType.Int32) { Value = Id });

            return mySqlParameters;
        }
        #endregion

        #region INSERT / UPDATE

        /// <summary>
        /// Renvoie un dictionnaire contenant les noms de colonnes et paramètres associés pour construire les requêtes insert / update
        /// Format : <NomColonne, @NomParamètre> 
        /// </summary>
        public override Dictionary<string, string> GetInsertUpdateColumns()
        {
            return new Dictionary<string, string> { 
                { "nom_materiel", "@nom_materiel" },
                { "description", "@description" },
                { "valeur", "@valeur" },
                { "id_categorie", "@id_categorie" },
                { "image", "@image" },
                { "prix", "@prix" }
            };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour la requête insert des matériels
        /// </summary>
        public override List<IDataParameter> GetInsertUpdateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@nom_materiel", MySqlDbType.VarChar, 50) { Value = this.NomMateriel });
            mySqlParameters.Add(new MySqlParameter("@description", MySqlDbType.VarChar, 50) { Value = this.Description });
            mySqlParameters.Add(new MySqlParameter("@valeur", MySqlDbType.VarChar, 50) { Value = this.Valeur });
            mySqlParameters.Add(new MySqlParameter("@id_categorie", MySqlDbType.Int32) { Value = this.IdCategorie });
            mySqlParameters.Add(new MySqlParameter("@image", MySqlDbType.Text) { Value = this.Image });
            var prixParam = new MySqlParameter("@prix", MySqlDbType.Decimal) { Value = this.Prix };
            prixParam.Precision = 15;
            prixParam.Scale = 2;
            mySqlParameters.Add(prixParam);

            return mySqlParameters;
        }
        #endregion

        #region SELECT
        /// <summary>
        /// Remplis l'objet matériel avec ses données venant de la BDD
        /// Fait appel à la classe mère pour remplir les champs communs à tous les objets BDD
        /// </summary>
        /// <param name="reader">Le dictionnaire de données</param>
        public override void FillWithDataReader(DbDataReader reader)
        {
            this.Id = (int)reader["Id_Materiel"];
            this.NomMateriel = reader["nom_materiel"].ToString()!;
            this.Description = reader["description"].ToString()!;
            this.Valeur = reader["valeur"].ToString()!;
            this.IdCategorie = (int)reader["id_categorie"];
            this.Image = reader["image"].ToString()!;
            this.Prix = (decimal)reader["prix"];
        }

        public override Dictionary<string, string> GetPredicateColumns()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(NomMateriel))
                columns.Add("nom_materiel", "@nom_materiel");

            if (!string.IsNullOrEmpty(Description))
                columns.Add("description", "@description");

            if (!string.IsNullOrEmpty(Valeur))
                columns.Add("valeur", "@valeur");

            if (IdCategorie > 0)
                columns.Add("id_categorie", "@id_categorie");

            if (!string.IsNullOrEmpty(Image))
                columns.Add("image", "@image");

            if (Prix > 0)
                columns.Add("prix", "@prix");

            return columns;
        }

        public override List<IDataParameter> GetPredicateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();

            if (!string.IsNullOrEmpty(NomMateriel))
                mySqlParameters.Add(new MySqlParameter("@nom_materiel", MySqlDbType.VarChar, 50) { Value = this.NomMateriel });

            if (!string.IsNullOrEmpty(Description))
                mySqlParameters.Add(new MySqlParameter("@description", MySqlDbType.VarChar, 50) { Value = this.Description });

            if (!string.IsNullOrEmpty(Valeur))
                mySqlParameters.Add(new MySqlParameter("@valeur", MySqlDbType.VarChar, 50) { Value = this.Valeur });

            if (IdCategorie > 0)
                mySqlParameters.Add(new MySqlParameter("@id_categorie", MySqlDbType.Int32) { Value = this.IdCategorie });

            if (!string.IsNullOrEmpty(Image))
                mySqlParameters.Add(new MySqlParameter("@image", MySqlDbType.Text) { Value = this.Image });

            if (Prix > 0)
            {
                var param = new MySqlParameter("@prix", MySqlDbType.Decimal) { Value = this.Prix };
                param.Precision = 15;
                param.Scale = 2;
                mySqlParameters.Add(param);
            }

            return mySqlParameters;
        }

        public override IBDDConnector getNewInstance()
        {
            return new Materiel();
        }
        #endregion
    }
}
