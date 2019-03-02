using System;
using System.Collections.Generic;
using MyAirport.Pim.Entities;

namespace MyAirport.Pim.Models
{
    public class Natif : AbstractDefinition
    {
        public override void CreateBagage(BagageDefinition bagage)
        {
            throw new NotImplementedException(); // TODO test
        }

        public override BagageDefinition GetBagage(int idBagage)
        {
            var bagageDefinition = new BagageDefinition();
            bagageDefinition.IdBagage = idBagage;

            return bagageDefinition;
        }

        public override List<BagageDefinition> GetBagage(string codeIataBagage)
        {
            var list = new List<BagageDefinition>();
            for (int i = 42; i < 55; i++)
            {
                list.Add(this.GetBagage(i));
            }
            return list;
        }
    }
}
