using MyAirport.Pim.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAirport.Pim.Models
{
    public class Sql : AbstractDefinition
    {

        string strCnx = ConfigurationManager.ConnectionStrings["MyAiport.Pim.Settings.DbConnect"].ConnectionString;
        string commandGetBagageIata = "SELECT " +
            "b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.LIGNE, b.DATE_CREATION, b.ESCALE, b.CLASSE, b.CONTINUATION, cast(iif(bp.PARTICULARITE is null, 0, 1) as bit) as 'RUSH' " +
            "FROM BAGAGE b INNER JOIN BAGAGE_A_POUR_PARTICULARITE tmp ON tmp.ID_BAGAGE = b.ID_BAGAGE INNER JOIN BAGAGE_PARTICULARITE bp ON bp.ID_PART = tmp.ID_PARTICULARITE " +
            "WHERE b.CODE_IATA = @iata";
        string commandGetBagageId = "SELECT " +
            "b.ID_BAGAGE, b.CODE_IATA, b.COMPAGNIE, b.LIGNE, b.DATE_CREATION, b.ESCALE, b.CLASSE, b.CONTINUATION, cast(iif(bp.PARTICULARITE is null, 0, 1) as bit) as 'RUSH' " +
            "FROM BAGAGE b INNER JOIN BAGAGE_A_POUR_PARTICULARITE tmp ON tmp.ID_BAGAGE = b.ID_BAGAGE INNER JOIN BAGAGE_PARTICULARITE bp ON bp.ID_PART = tmp.ID_PARTICULARITE " +
            "WHERE b.ID_BAGAGE = @id";


        public override BagageDefinition GetBagage(int idBagage)
        {
            var bagRes = new BagageDefinition();
            using (SqlConnection cnx = new SqlConnection(strCnx))
            {
                SqlCommand cmd = new SqlCommand(this.commandGetBagageId, cnx);
                cmd.Parameters.AddWithValue("@id", idBagage);
                cnx.Open();
            //Implémenter ici le code de récupération et de convertion
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                                // Call Read before accessing data.
            while (reader.Read())
            {
                        bagRes.IdBagage = (int)reader["ID_BAGAGE"];
                        bagRes.CodeIata = (string)reader["CODE_IATA"];
                        bagRes.Compagnie = (string)reader["COMPAGNIE"];
                        bagRes.Ligne = (string)reader["LIGNE"];
                        bagRes.DateVol = (DateTime)reader["DATE_CREATION"];
                        var res = reader["ESCALE"];
                        if (res != DBNull.Value)
                        {
                            bagRes.Itineraire = (string)res;

                        }
                        res = reader["CLASSE"];
                        if (res != DBNull.Value)
                        {
                            bagRes.Prioritaire = !((string)res == "Y"); // todo


                        }
                        bagRes.EnContinuation = (string)reader["CONTINUATION"] == "Y";
                        bagRes.Rush = (bool)reader["RUSH"];
                    }
                }
            }
            Console.WriteLine(bagRes.ToString());
            return bagRes;
        }

        public override List<BagageDefinition> GetBagage(string codeIataBagage)
        {
            return null;
        }
    }
}
