using MyAirport.Pim.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;


namespace MyAirport.Pim.Models
{
    public class Sql : AbstractDefinition
    {

        string strCnx = ConfigurationManager.ConnectionStrings["MyAiport.Pim.Settings.DbConnect"].ConnectionString;
        string commandGetBagageIata = "SELECT " +
            "b.ID_BAGAGE, b.CODE_IATA, c.NOM, b.LIGNE, b.DATE_CREATION, b.ESCALE, b.PRIORITAIRE, b.EN_CONTINUATION, cast(iif(bp.PARTICULARITE is null, 0, 1) as bit) as 'RUSH' " +
            "FROM BAGAGE b " + 
            "INNER JOIN BAGAGE_A_POUR_PARTICULARITE tmp ON tmp.ID_BAGAGE = b.ID_BAGAGE " + 
            "INNER JOIN BAGAGE_PARTICULARITE bp ON bp.ID_PART = tmp.ID_PARTICULARITE " +
            "INNER JOIN COMPAGNIE c ON c.CODE_IATA = b.COMPAGNIE " +
            "WHERE b.CODE_IATA LIKE '%@iata%'";
        string commandGetBagageId = "SELECT " +
            "b.ID_BAGAGE, b.CODE_IATA, c.NOM, b.LIGNE, b.DATE_CREATION, b.ESCALE, b.PRIORITAIRE, b.EN_CONTINUATION, cast(iif(bp.PARTICULARITE is null, 0, 1) as bit) as 'RUSH' " +
            "FROM BAGAGE b " +
            "INNER JOIN BAGAGE_A_POUR_PARTICULARITE tmp ON tmp.ID_BAGAGE = b.ID_BAGAGE " +
            "INNER JOIN BAGAGE_PARTICULARITE bp ON bp.ID_PART = tmp.ID_PARTICULARITE " +
            "INNER JOIN COMPAGNIE c ON c.CODE_IATA = b.COMPAGNIE " +
            "WHERE b.ID_BAGAGE = @id";


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
                cmd.Parameters.AddWithValue("@iata", codeIataBagage);
                cnx.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var bagRes = ReaderToBagage(reader);
                        Console.WriteLine(bagRes);
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
                EnContinuation = reader.GetBoolean(reader.GetOrdinal("EN_CONTINUATION"))
            };

            int index = reader.GetOrdinal("ESCALE");
            if (!reader.IsDBNull(index))
                bagRes.Itineraire = reader.GetString(index);

            index = reader.GetOrdinal("NOM");
            if (!reader.IsDBNull(index))
                bagRes.Compagnie = reader.GetString(index);

            index = reader.GetOrdinal("PRIORITAIRE");
            if (!reader.IsDBNull(index))
                bagRes.Prioritaire = reader.GetBoolean(index);

            return bagRes;
        }
    }
}
