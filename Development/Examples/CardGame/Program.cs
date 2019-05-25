using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YACE;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            YACEParameters parameters;
            parameters.RessourceDefinitions = new RessourceDefinition[] {
                new RessourceDefinition {
                    Name = "Money",
                    IsBoundToPlayer = false,
                    BaseValue = 100,
                    ConsumeOnUse = true
                },

                new RessourceDefinition {
                    Name = "BankAcount",
                    IsBoundToPlayer = true,
                    BaseValue = 0,
                    ConsumeOnUse = true
                }
            };

            YACE.YACE Yace = new YACE.YACE(parameters);

            System.Console.Out.WriteLine(Yace.Context);
            System.Console.In.Read();
        }
    }
}
