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

        private static int errorCode = -1;
        private static YACE.YACE Yace;

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

            Program.Yace = new YACE.YACE(parameters);
            YACE.YACE yace = Program.Yace;

            System.Console.Out.WriteLine(yace.Context);
            string message;
            string[] decomposedMessage;
            while (Program.errorCode < 0)
            {
                message = System.Console.In.ReadLine();
                decomposedMessage = message.Split(' ');
                if (decomposedMessage.Length < 1)
                {
                    Program.errorCode = 1;
                }

                int value;

                switch (decomposedMessage[0])
                {
                    case "quit":
                    case "q":
                    case "exit":
                        Program.errorCode = 0;

                        break;
                    case "next":
                    case "end turn":
                    case "end":
                        yace.EndPlayerTurn();

                        break;
                    case "spend":
                        if (decomposedMessage.Length < 3)
                        {
                            System.Console.Out.WriteLine("Not enought parameter");
                            break;
                        }

                        if (!int.TryParse(decomposedMessage[1], out value))
                        {
                            System.Console.Out.WriteLine(string.Format("Unkown Value {0}", decomposedMessage[1]));
                            break;
                        }

                        if (!yace.AlterCurrency(decomposedMessage[2], -value))
                        {
                            System.Console.Out.WriteLine(string.Format("Unkown Currency {0}", decomposedMessage[2]));
                        }

                        break;
                    case "give":
                        if (decomposedMessage.Length < 3)
                        {
                            System.Console.Out.WriteLine("Not enought parameter");
                            break;
                        }

                        if (!int.TryParse(decomposedMessage[1], out value))
                        {
                            System.Console.Out.WriteLine(string.Format("Unkown Value {0}", decomposedMessage[1]));
                            break;
                        }

                        if (!yace.AlterCurrency(decomposedMessage[2], value))
                        {
                            System.Console.Out.WriteLine(string.Format("Unkown Currency {0}", decomposedMessage[2]));
                        }

                        break;
                    default:
                        break;
                }

                Program.PrintGame();
            }


            System.Console.Out.WriteLine("Exited with code : " + Program.errorCode);
            System.Console.In.ReadLine();
        }

        internal static void PrintGame()
        {
            System.Console.Out.WriteLine(Yace.Context);
        }
    }
}
