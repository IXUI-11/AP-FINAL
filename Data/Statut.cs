using MySql.Data.MySqlClient;
using RepositoryPOO;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataPOO
{
    /// <summary>
    /// Classe gérant les statuts
    /// </summary>
    public class Statut : BDDObject
    {
        #region Attributs
        /// <summary>
        /// Libellé du statut
        /// </summary>
        private string libelleStatut;
        public string LibelleStatut { get => libelleStatut; set => libelleStatut = value; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Statut() { }

        /// <summary>
        /// Constructeur complet appelant le constructeur parent (BDDObject)
        /// </summary>
        /// <param name="libelleStatut">Libellé du statut</param>
        public Statut(string libelleStatut) : base()
        {
            this.LibelleStatut = libelleStatut;
        }
        #endregion

        #region SQL Commun
        /// <summary>
        /// Renvoie le nom de la table stockant les statuts
        /// </summary>
        public override string GetTableName()
        {
            return "Statut";
        }

        /// <summary>
        /// Renvoie un dictionnaire contenant le nom de colonne PK et paramètre associé
        /// </summary>
        public override Dictionary<string, string> GetPrimaryKeyColumn()
        {
            return new Dictionary<string, string> { { "Id_statut", "@Id_statut" } };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour filtrer sur la clé primaire de l'objet
        /// </summary>
        public override List<IDataParameter> GetPrimaryKeyParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@Id_statut", MySqlDbType.Int32) { Value = Id });

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
            return new Dictionary<string, string> { { "libelle_statut", "@libelle_statut" } };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour la requête insert des statuts
        /// </summary>
        public override List<IDataParameter> GetInsertUpdateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@libelle_statut", MySqlDbType.VarChar, 50) { Value = this.LibelleStatut });

            return mySqlParameters;
        }
        #endregion

        #region SELECT
        /// <summary>
        /// Remplis l'objet statut avec ses données venant de la BDD
        /// Fait appel à la classe mère pour remplir les champs communs à tous les objets BDD
        /// </summary>
        /// <param name="reader">Le dictionnaire de données</param>
        public override void FillWithDataReader(DbDataReader reader)
        {
            this.Id = (int)reader["Id_statut"];
            this.LibelleStatut = reader["libelle_statut"].ToString();
        }

        public override Dictionary<string, string> GetPredicateColumns()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(LibelleStatut))
                columns.Add("libelle_statut", "@libelle_statut");

            return columns;
        }

        public override List<IDataParameter> GetPredicateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();

            if (!string.IsNullOrEmpty(LibelleStatut))
                mySqlParameters.Add(new MySqlParameter("@libelle_statut", MySqlDbType.VarChar, 50) { Value = this.LibelleStatut });

            return mySqlParameters;
        }

        public override IBDDConnector getNewInstance()
        {
            return new Statut();
        }
        #endregion
    }
}

