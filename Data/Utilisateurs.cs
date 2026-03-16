using MySql.Data.MySqlClient;
using RepositoryPOO;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataPOO
{
    /// <summary>
    /// Classe gérant les utilisateurs
    /// </summary>
    public class Utilisateurs : BDDObject
    {
        #region Attributs
        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        private string nom;
        public string Nom { get => nom; set => nom = value; }

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        private string motDePasse;
        public string MotDePasse { get => motDePasse; set => motDePasse = value; }

        /// <summary>
        /// Prénom de l'utilisateur
        /// </summary>
        private string prenom;
        public string Prenom { get => prenom; set => prenom = value; }

        /// <summary>
        /// Email de l'utilisateur
        /// </summary>
        private string email;
        public string Email { get => email; set => email = value; }

        /// <summary>
        /// Ville de l'utilisateur
        /// </summary>
        private string ville;
        public string Ville { get => ville; set => ville = value; }

        /// <summary>
        /// Rue de l'utilisateur
        /// </summary>
        private string rue;
        public string Rue { get => rue; set => rue = value; }

        /// <summary>
        /// Numéro de téléphone de l'utilisateur
        /// </summary>
        private string numeroDeTelephone;
        public string NumeroDeTelephone { get => numeroDeTelephone; set => numeroDeTelephone = value; }

        /// <summary>
        /// Code postal de l'utilisateur
        /// </summary>
        private string codePostal;
        public string CodePostal { get => codePostal; set => codePostal = value; }

        private string aspNetUserId;
        public string AspNetUserId { get => aspNetUserId; set => aspNetUserId = value; }


        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Utilisateurs() { }

        /// <summary>
        /// Constructeur complet appelant le constructeur parent (BDDObject)
        /// </summary>
        public Utilisateurs(string nom, string motDePasse, string prenom, string email, string ville, string rue, string numeroDeTelephone, string codePostal) : base()
        {
            this.Nom = nom;
            this.MotDePasse = motDePasse;
            this.Prenom = prenom;
            this.Email = email;
            this.Ville = ville;
            this.Rue = rue;
            this.NumeroDeTelephone = numeroDeTelephone;
            this.CodePostal = codePostal;
        }
        #endregion

        #region SQL Commun
        /// <summary>
        /// Renvoie le nom de la table stockant les utilisateurs
        /// </summary>
        public override string GetTableName()
        {
            return "Utilisateurs";
        }

        /// <summary>
        /// Renvoie un dictionnaire contenant le nom de colonne PK et paramètre associé
        /// </summary>
        public override Dictionary<string, string> GetPrimaryKeyColumn()
        {
            return new Dictionary<string, string> { { "Id_Utilisateurs", "@Id_Utilisateurs" } };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour filtrer sur la clé primaire de l'objet
        /// </summary>
        public override List<IDataParameter> GetPrimaryKeyParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@Id_Utilisateurs", MySqlDbType.Int32) { Value = Id });

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
                { "nom", "@nom" },
                { "mot_de_passe", "@mot_de_passe" },
                { "prenom", "@prenom" },
                { "email", "@email" },
                { "ville", "@ville" },
                { "rue", "@rue" },
                { "numero_de_telephone", "@numero_de_telephone" },
                { "code_postal", "@code_postal" },
                { "AspNetUserId" , "@AspNetUserId" }
            };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour la requête insert des utilisateurs
        /// </summary>
        public override List<IDataParameter> GetInsertUpdateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@nom", MySqlDbType.VarChar, 50) { Value = this.Nom });
            mySqlParameters.Add(new MySqlParameter("@mot_de_passe", MySqlDbType.VarChar, 50) { Value = this.MotDePasse });
            mySqlParameters.Add(new MySqlParameter("@prenom", MySqlDbType.VarChar, 50) { Value = this.Prenom });
            mySqlParameters.Add(new MySqlParameter("@email", MySqlDbType.VarChar, 50) { Value = this.Email });
            mySqlParameters.Add(new MySqlParameter("@ville", MySqlDbType.VarChar, 50) { Value = this.Ville });
            mySqlParameters.Add(new MySqlParameter("@rue", MySqlDbType.VarChar, 100) { Value = this.Rue });
            mySqlParameters.Add(new MySqlParameter("@numero_de_telephone", MySqlDbType.VarChar, 10) { Value = this.NumeroDeTelephone });
            mySqlParameters.Add(new MySqlParameter("@code_postal", MySqlDbType.VarChar, 10) { Value = this.CodePostal });
            mySqlParameters.Add(new MySqlParameter("@AspNetUserId", MySqlDbType.VarChar, 255) { Value = this.AspNetUserId });

            return mySqlParameters;
        }
        #endregion

        #region SELECT
        /// <summary>
        /// Remplis l'objet utilisateur avec ses données venant de la BDD
        /// Fait appel à la classe mère pour remplir les champs communs à tous les objets BDD
        /// </summary>
        /// <param name="reader">Le dictionnaire de données</param>
        public override void FillWithDataReader(DbDataReader reader)
        {
            this.Id = (int)reader["Id_Utilisateurs"];
            this.Nom = reader["nom"].ToString()!;
            this.MotDePasse = reader["mot_de_passe"].ToString()!;
            this.Prenom = reader["prenom"].ToString()!;
            this.Email = reader["email"].ToString()!;
            this.Ville = reader["ville"].ToString()!;
            this.Rue = reader["rue"].ToString()!;
            this.NumeroDeTelephone = reader["numero_de_telephone"].ToString()!;
            this.CodePostal = reader["code_postal"].ToString()!;
            this.AspNetUserId = reader["AspNetUserId"].ToString()!;
        }

        public override Dictionary<string, string> GetPredicateColumns()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(Nom))
                columns.Add("nom", "@nom");

            if (!string.IsNullOrEmpty(MotDePasse))
                columns.Add("mot_de_passe", "@mot_de_passe");

            if (!string.IsNullOrEmpty(Prenom))
                columns.Add("prenom", "@prenom");

            if (!string.IsNullOrEmpty(Email))
                columns.Add("email", "@email");

            if (!string.IsNullOrEmpty(Ville))
                columns.Add("ville", "@ville");

            if (!string.IsNullOrEmpty(Rue))
                columns.Add("rue", "@rue");

            if (!string.IsNullOrEmpty(NumeroDeTelephone))
                columns.Add("numero_de_telephone", "@numero_de_telephone");

            if (!string.IsNullOrEmpty(CodePostal))
                columns.Add("code_postal", "@code_postal");
            if (!string.IsNullOrEmpty(AspNetUserId))
                columns.Add("AspNetUserId", "@AspNetUserId");

            return columns;
        }

        public override List<IDataParameter> GetPredicateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();

            if (!string.IsNullOrEmpty(Nom))
                mySqlParameters.Add(new MySqlParameter("@nom", MySqlDbType.VarChar, 50) { Value = this.Nom });

            if (!string.IsNullOrEmpty(MotDePasse))
                mySqlParameters.Add(new MySqlParameter("@mot_de_passe", MySqlDbType.VarChar, 50) { Value = this.MotDePasse });

            if (!string.IsNullOrEmpty(Prenom))
                mySqlParameters.Add(new MySqlParameter("@prenom", MySqlDbType.VarChar, 50) { Value = this.Prenom });

            if (!string.IsNullOrEmpty(Email))
                mySqlParameters.Add(new MySqlParameter("@email", MySqlDbType.VarChar, 50) { Value = this.Email });

            if (!string.IsNullOrEmpty(Ville))
                mySqlParameters.Add(new MySqlParameter("@ville", MySqlDbType.VarChar, 50) { Value = this.Ville });

            if (!string.IsNullOrEmpty(Rue))
                mySqlParameters.Add(new MySqlParameter("@rue", MySqlDbType.VarChar, 100) { Value = this.Rue });

            if (!string.IsNullOrEmpty(NumeroDeTelephone))
                mySqlParameters.Add(new MySqlParameter("@numero_de_telephone", MySqlDbType.VarChar, 10) { Value = this.NumeroDeTelephone });

            if (!string.IsNullOrEmpty(CodePostal))
                mySqlParameters.Add(new MySqlParameter("@code_postal", MySqlDbType.VarChar, 10) { Value = this.CodePostal });

            if (!string.IsNullOrEmpty(AspNetUserId))
                mySqlParameters.Add(new MySqlParameter("@AspNetUserId", MySqlDbType.VarChar, 255) { Value = this.AspNetUserId });

            return mySqlParameters;
        }

        public override IBDDConnector getNewInstance()
        {
            return new Utilisateurs();
        }
        #endregion
    }
}

