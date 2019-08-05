using YACE;

namespace CardGame
{
    class Program
    {

        private static int errorCode = -1;

        static void Main(string[] args)
        {
            Bataille game = new Bataille();
            game.Main();
            System.Console.In.ReadLine();
        }
    }
}
