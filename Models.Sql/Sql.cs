using MyAirport.Pim.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace MyAirport.Pim.Models
{
    public class Sql : AbstractDefinition
    {

        string strCnx = ConfigurationManager.ConnectionStrings["MyAiport.Pim.Settings.DbConnect"].ConnectionString;
        string commandGetBagageIata = "SELECT " +
            "b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.LIGNE, b.DATE_CREATION, b.ESCALE, b.PRIORITAIRE, b.EN_CONTINUATION, cast(iif(bp.PARTICULARITE is null, 0, 1) as bit) as 'RUSH' " +
            "FROM BAGAGE b " + 
            "INNER JOIN BAGAGE_A_POUR_PARTICULARITE tmp ON tmp.ID_BAGAGE = b.ID_BAGAGE " + 
            "INNER JOIN BAGAGE_PARTICULARITE bp ON bp.ID_PART = tmp.ID_PARTICULARITE " +
            "WHERE b.CODE_IATA LIKE @iata";

        string commandGetBagageId = "SELECT " +
            "b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.LIGNE, b.DATE_CREATION, b.ESCALE, b.PRIORITAIRE, b.EN_CONTINUATION, cast(iif(bp.PARTICULARITE is null, 0, 1) as bit) as 'RUSH' " +
            "FROM BAGAGE b " +
            "INNER JOIN BAGAGE_A_POUR_PARTICULARITE tmp ON tmp.ID_BAGAGE = b.ID_BAGAGE " +
            "INNER JOIN BAGAGE_PARTICULARITE bp ON bp.ID_PART = tmp.ID_PARTICULARITE " +
            "WHERE b.ID_BAGAGE = @id";

        string commandGetAllForTest = "SELECT * FROM BAGAGE";

        string commandInsertBagage = "INSERT " +
        "INTO BAGAGE (CODE_IATA, COMPAGNIE, LIGNE, DATE_CREATION, ESCALE, PRIORITAIRE, EN_CONTINUATION, JOUR_EXPLOITATION, ORIGINE_CREATION) " +
        "VALUES (@iata, @compagnie, @ligne, GETDATE(), @escale, @prioritaire, @en_continuation, @jour_exploitation, @origine_creation)";

        public override void CreateBagage(BagageDefinition bagage) {
            using (SqlConnection cnx = new SqlConnection(strCnx))
            {
                SqlCommand cmd = new SqlCommand(this.commandInsertBagage, cnx);
                cmd.Parameters.AddWithValue("@iata", bagage.CodeIata);
                cmd.Parameters.AddWithValue("@compagnie", bagage.Compagnie);
                cmd.Parameters.AddWithValue("@ligne", bagage.Ligne);
                cmd.Parameters.AddWithValue("@escale", bagage.Itineraire);
                cmd.Parameters.AddWithValue("@prioritaire", bagage.Prioritaire);
                cmd.Parameters.AddWithValue("@en_continuation", bagage.EnContinuation);
                cmd.Parameters.AddWithValue("@jour_exploitation", 2);
                cmd.Parameters.AddWithValue("@origine_creation", "D");
                cnx.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public override BagageDefinition GetBagage(int idBagage)
        {
            BagageDefinition bagRes = null;

            using (SqlConnection cnx = new SqlConnection(strCnx))
            {
                SqlCommand cmd = new SqlCommand(this.commandGetBagageId, cnx);
                cmd.Parameters.AddWithValue("@id", idBagage);
                cnx.Open();

                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["ID_BAGAGE"] + " " + reader["CODE_IATA"]);
                        bagRes = ReaderToBagage(reader);
                        Console.WriteLine(bagRes);
                    }
                }
            }
            
            return bagRes;
        }

        public override List<BagageDefinition> GetBagage(string codeIataBagage)
        {
            var bagList = new List<BagageDefinition>();

            using (SqlConnection cnx = new SqlConnection(strCnx))
            {
                SqlCommand cmd = new SqlCommand(this.commandGetBagageIata, cnx);
                cmd.Parameters.AddWithValue("@iata", "%" + codeIataBagage + "%");
                cnx.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var bagRes = ReaderToBagage(reader);
                        bagList.Add(bagRes);
                    }
                }
            }

            return bagList;
        }

        private BagageDefinition ReaderToBagage(SqlDataReader reader)
        {
            var bagRes = new BagageDefinition
            {
                IdBagage = reader.GetInt32(reader.GetOrdinal("ID_BAGAGE")),
                CodeIata = reader.GetString(reader.GetOrdinal("CODE_IATA")),
                Ligne = reader.GetString(reader.GetOrdinal("LIGNE")),
                DateVol = reader.GetDateTime(reader.GetOrdinal("DATE_CREATION")),
                Rush = reader.GetBoolean(reader.GetOrdinal("RUSH")),
                EnContinuation = reader.GetBoolean(reader.GetOrdinal("EN_CONTINUATION")),
                Compagnie = reader.GetString(reader.GetOrdinal("COMPAGNIE"))
            };
            int index = reader.GetOrdinal("ESCALE");
            if (!reader.IsDBNull(index))
                bagRes.Itineraire = reader.GetString(index);

            index = reader.GetOrdinal("PRIORITAIRE");
            if (!reader.IsDBNull(index))
                bagRes.Prioritaire = reader.GetBoolean(index);
            return bagRes;
        }
    }
}
