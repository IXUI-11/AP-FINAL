using MySql.Data.MySqlClient;
using RepositoryPOO;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataPOO
{
    /// <summary>
    /// Classe gérant les catégories
    /// </summary>
    public class Categorie : BDDObject
    {
        #region Attributs
        /// <summary>
        /// Libellé de la catégorie
        /// </summary>
        private string libelleCategorie;
        public string LibelleCategorie { get => libelleCategorie; set => libelleCategorie = value; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Categorie() { }

        /// <summary>
        /// Constructeur complet appelant le constructeur parent (BDDObject)
        /// </summary>
        /// <param name="libelleCategorie">Libellé de la catégorie</param>
        public Categorie(string libelleCategorie) : base()
        {
            this.LibelleCategorie = libelleCategorie;
        }
        #endregion

        #region SQL Commun
        /// <summary>
        /// Renvoie le nom de la table stockant les catégories
        /// </summary>
        public override string GetTableName()
        {
            return "Categorie";
        }

        /// <summary>
        /// Renvoie un dictionnaire contenant le nom de colonne PK et paramètre associé
        /// </summary>
        public override Dictionary<string, string> GetPrimaryKeyColumn()
        {
            return new Dictionary<string, string> { { "Id_Categorie", "@Id_Categorie" } };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour filtrer sur la clé primaire de l'objet
        /// </summary>
        public override List<IDataParameter> GetPrimaryKeyParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@Id_Categorie", MySqlDbType.Int32) { Value = Id });

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
            return new Dictionary<string, string> { { "libelle_categorie", "@libelle_categorie" } };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour la requête insert des catégories
        /// </summary>
        public override List<IDataParameter> GetInsertUpdateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@libelle_categorie", MySqlDbType.VarChar, 50) { Value = this.LibelleCategorie });

            return mySqlParameters;
        }
        #endregion

        #region SELECT
        /// <summary>
        /// Remplis l'objet catégorie avec ses données venant de la BDD
        /// Fait appel à la classe mère pour remplir les champs communs à tous les objets BDD
        /// </summary>
        /// <param name="reader">Le dictionnaire de données</param>
        public override void FillWithDataReader(DbDataReader reader)
        {
            this.Id = (int)reader["Id_Categorie"];
            this.LibelleCategorie = reader["libelle_categorie"].ToString();
        }

        public override Dictionary<string, string> GetPredicateColumns()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(LibelleCategorie))
                columns.Add("libelle_categorie", "@libelle_categorie");

            return columns;
        }

        public override List<IDataParameter> GetPredicateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();

            if (!string.IsNullOrEmpty(LibelleCategorie))
                mySqlParameters.Add(new MySqlParameter("@libelle_categorie", MySqlDbType.VarChar, 50) { Value = this.LibelleCategorie });

            return mySqlParameters;
        }

        public override IBDDConnector getNewInstance()
        {
            return new Categorie();
        }
        #endregion
    }
}

