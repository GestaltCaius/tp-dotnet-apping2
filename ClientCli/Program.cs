using MyAirport.Pim.Entities;
using MyAirport.Pim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCli
{
    class Program
    {
        static void Main(string[] args)
        {
            AbstractDefinition model = Factory.Model; // get either Natif or SQL model
            while (true)
            {
                Console.WriteLine("$ Type IATA code to search for luggages, or type 'quit' to quit");
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input == "quit") { break; }
                Console.WriteLine("Looking for luggage " + input);
                List<BagageDefinition> bagageList = model.GetBagage(input);
                switch (bagageList.Count)
                {
                    case 0:
                        // TODO create
                        Console.WriteLine("Create luggage " + input);
                        continue;
                    case 1:
                        Console.WriteLine(bagageList.First());
                        break;
                    default:
                            Console.WriteLine("Several luggages have been found:");
                        foreach (var bagage in bagageList)
                        {
                            Console.WriteLine("\t" + bagage); // TODO update toString() method
                        }
                        break;
                }
            }
        }
    }
}
