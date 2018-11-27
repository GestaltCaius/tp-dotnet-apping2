using System;
using System.Collections.Generic;
using MyAirport.Pim.Entities;

namespace MyAirport.Pim.Models
{
    public class Natif : AbstractDefinition
    {
        public override BagageDefinition GetBagage(int idBagage)
        {
            var bagageDefinition = new BagageDefinition();
            bagageDefinition.IdBagage = idBagage;

            return bagageDefinition;
        }

        public override List<BagageDefinition> GetBagage(string codeIataBagage)
        {

            return new List<BagageDefinition>();
        }
    }
}
