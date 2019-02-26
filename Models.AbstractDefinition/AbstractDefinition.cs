using System;
using System.Collections.Generic;
using MyAirport.Pim.Entities;

namespace MyAirport.Pim.Models
{
    public abstract class AbstractDefinition
    {
        public abstract void CreateBagage(BagageDefinition bagage);
        
        public abstract BagageDefinition GetBagage(int idBagage);

        public abstract List<BagageDefinition> GetBagage(string codeIataBagage);
    }
}
