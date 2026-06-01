using MySql.Data.MySqlClient;
using RepositoryPOO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataPOO
{
    /// <summary>
    /// Classe gérant les réservations
    /// </summary>
    public class Reservation : BDDObject
    {
        #region Attributs
        /// <summary>
        /// Date de début de la réservation
        /// </summary>
        private DateTime dateDebut;
        public DateTime DateDebut { get => dateDebut; set => dateDebut = value; }

        /// <summary>
        /// Date de fin de la réservation
        /// </summary>
        private DateTime dateFin;
        public DateTime DateFin { get => dateFin; set => dateFin = value; }

        /// <summary>
        /// Date de la réservation
        /// </summary>
        private DateTime dateReservation;
        public DateTime DateReservation { get => dateReservation; set => dateReservation = value; }

        /// <summary>
        /// Statut de la réservation
        /// </summary>
        //private string statut;
        //public string Statut { get => statut; set => statut = value; }

        /// <summary>
        /// Prix total de la réservation
        /// </summary>
        private decimal prixTotal;
        public decimal PrixTotal { get => prixTotal; set => prixTotal = value; }

        /// <summary>
        /// Identifiant de l'utilisateur
        /// </summary>
        private int idUtilisateurs;
        public int IdUtilisateurs { get => idUtilisateurs; set => idUtilisateurs = value; }
        #endregion

        private int idMateriel;
        public int IdMateriel { get => idMateriel; set => idMateriel = value; }

        private int idStatus;

        public int IdStatus { get => idStatus; set => idStatus = value; }

        #region Constructeurs
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Reservation() { }

        /// <summary>
        /// Constructeur complet appelant le constructeur parent (BDDObject)
        /// </summary>
        public Reservation(DateTime dateDebut, DateTime dateFin, DateTime dateReservation, /*string statut*/  decimal prixTotal, int idUtilisateurs, int idMateriel, int idStatus) : base()
        {
            this.DateDebut = dateDebut;
            this.DateFin = dateFin;
            this.DateReservation = dateReservation;
            //this.Statut = statut;
            this.PrixTotal = prixTotal;
            this.IdUtilisateurs = idUtilisateurs;
            this.IdMateriel = idMateriel;
            this.IdStatus = idStatus;
        }
        #endregion

        #region SQL Commun
        /// <summary>
        /// Renvoie le nom de la table stockant les réservations
        /// </summary>
        public override string GetTableName()
        {
            return "Reservation";
        }

        /// <summary>
        /// Renvoie un dictionnaire contenant le nom de colonne PK et paramètre associé
        /// </summary>
        public override Dictionary<string, string> GetPrimaryKeyColumn()
        {
            return new Dictionary<string, string> { { "Id_Resa", "@Id_Resa" } };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour filtrer sur la clé primaire de l'objet
        /// </summary>
        public override List<IDataParameter> GetPrimaryKeyParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@Id_Resa", MySqlDbType.Int32) { Value = Id });

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
                { "date_debut", "@date_debut" },
                { "date_fin", "@date_fin" },
                { "date_reservation", "@date_reservation" },
                { "prix_total", "@prix_total" },
                { "Id_Utilisateurs", "@Id_Utilisateurs" },
                { "Id_Materiel", "@Id_Materiel" },
                { "Id_statut", "@Id_statut" }
            };
        }

        /// <summary>
        /// Renvoie la liste des paramètres pour la requête insert des réservations
        /// </summary>
        public override List<IDataParameter> GetInsertUpdateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();
            mySqlParameters.Add(new MySqlParameter("@date_debut", MySqlDbType.DateTime) { Value = this.DateDebut });
            mySqlParameters.Add(new MySqlParameter("@date_fin", MySqlDbType.DateTime) { Value = this.DateFin });
            mySqlParameters.Add(new MySqlParameter("@date_reservation", MySqlDbType.DateTime) { Value = this.DateReservation });
            var prixParam = new MySqlParameter("@prix_total", MySqlDbType.Decimal) { Value = this.PrixTotal };
            prixParam.Precision = 15;
            prixParam.Scale = 2;
            mySqlParameters.Add(prixParam);
            mySqlParameters.Add(new MySqlParameter("@Id_Utilisateurs", MySqlDbType.Int32) { Value = this.IdUtilisateurs });
            mySqlParameters.Add(new MySqlParameter("@Id_Materiel", MySqlDbType.Int32) { Value = this.IdMateriel });
            mySqlParameters.Add(new MySqlParameter("@Id_statut", MySqlDbType.Int32) { Value = this.IdStatus });

            return mySqlParameters;
        }
        #endregion

        #region SELECT
        /// <summary>
        /// Remplis l'objet réservation avec ses données venant de la BDD
        /// Fait appel à la classe mère pour remplir les champs communs à tous les objets BDD
        /// </summary>
        /// <param name="reader">Le dictionnaire de données</param>
        public override void FillWithDataReader(DbDataReader reader)
        {
            this.Id = (int)reader["Id_Resa"];
            this.DateDebut = (DateTime)reader["date_debut"];
            this.DateFin = (DateTime)reader["date_fin"];
            this.DateReservation = (DateTime)reader["date_reservation"];
            //this.Statut = reader["statut"].ToString();
            this.PrixTotal = (decimal)reader["prix_total"];
            this.IdUtilisateurs = (int)reader["Id_Utilisateurs"];
            this.idMateriel = (int)reader["Id_Materiel"];
            this.IdStatus = (int)reader["Id_statut"];
        }

        public override Dictionary<string, string> GetPredicateColumns()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();

            if (DateDebut != default(DateTime))
                columns.Add("date_debut", "@date_debut");

            if (DateFin != default(DateTime))
                columns.Add("date_fin", "@date_fin");

            if (DateReservation != default(DateTime))
                columns.Add("date_reservation", "@date_reservation");

            //if (!string.IsNullOrEmpty(Statut))
            //    columns.Add("statut", "@statut");

            if (PrixTotal > 0)
                columns.Add("prix_total", "@prix_total");

            if (IdUtilisateurs > 0)
                columns.Add("Id_Utilisateurs", "@Id_Utilisateurs");

            return columns;
        }

        public override List<IDataParameter> GetPredicateParameters()
        {
            List<IDataParameter> mySqlParameters = new List<IDataParameter>();

            if (DateDebut != default(DateTime))
                mySqlParameters.Add(new MySqlParameter("@date_debut", MySqlDbType.DateTime) { Value = this.DateDebut });

            if (DateFin != default(DateTime))
                mySqlParameters.Add(new MySqlParameter("@date_fin", MySqlDbType.DateTime) { Value = this.DateFin });

            if (DateReservation != default(DateTime))
                mySqlParameters.Add(new MySqlParameter("@date_reservation", MySqlDbType.DateTime) { Value = this.DateReservation });

            //if (!string.IsNullOrEmpty(Statut))
            //    mySqlParameters.Add(new MySqlParameter("@statut", MySqlDbType.VarChar, 50) { Value = this.Statut });

            if (PrixTotal > 0)
            {
                var param = new MySqlParameter("@prix_total", MySqlDbType.Decimal) { Value = this.PrixTotal };
                param.Precision = 15;
                param.Scale = 2;
                mySqlParameters.Add(param);
            }

            if (IdUtilisateurs > 0)
                mySqlParameters.Add(new MySqlParameter("@Id_Utilisateurs", MySqlDbType.Int32) { Value = this.IdUtilisateurs });

            return mySqlParameters;
        }

        public override IBDDConnector getNewInstance()
        {
            return new Reservation();
        }
        #endregion
    }
}

