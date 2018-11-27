using System;

namespace Client.FormIhm
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var bagage = MyAirport.Pim.Models.Factory.Model.GetBagage("023232546100");
            Console.WriteLine(bagage);
        }
    }
}
