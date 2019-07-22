using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YACE;

namespace CardGame
{
    class Program
    {

        private static int errorCode = -1;
        private static YACE.YACE Yace;

        static void Main(string[] args)
        {

            System.Collections.Generic.List<ResourceDefinition> resourceList = new List<ResourceDefinition>();
            System.Diagnostics.Debug.Assert(args.Length > 0);
            string resourceFilePath = args[0];
            using (TextReader reader = File.OpenText(resourceFilePath))
            {
                string content = reader.ReadToEnd();

                Newtonsoft.Json.Linq.JObject root = Newtonsoft.Json.Linq.JObject.Parse(content);
                
                foreach (var resourceObj in root)
                {
                    ResourceDefinition resource = new ResourceDefinition();
                    resource.Name = resourceObj.Key;

                    resource.ConsumeOnUse = resourceObj.Value.Value<bool?>("ConsumeOnUse") ?? false;
                    resource.IsBoundToPlayer = resourceObj.Value.Value<bool?>("PlayerBound") ?? false;
                    resource.BaseValue = resourceObj.Value.Value<int?>("BaseValue") ?? 0;
                    resource.MinValue = resourceObj.Value.Value<int?>("MinValue") ?? int.MinValue;
                    resource.MaxValue = resourceObj.Value.Value<int?>("MaxValue") ?? int.MaxValue;
                    resourceList.Add(resource);
                }

            }

            YACEParameters parameters;
            parameters.ResourceDefinitions = resourceList.ToArray();

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
