namespace CardGame
{

    class Program
    {
        static void Main(string[] args)
        {
            Bataille game = new Bataille();
            ComunicationLayer comunicationLayer = new ComunicationLayer(game);
            GUIHandler gui = new GUIHandler(comunicationLayer);
            gui.Start();

            while (!comunicationLayer.IsOver)
            {
                if (comunicationLayer.ProcessOrders())
                {
                    gui.NotifyChanges();
                }
            }

            System.Console.WriteLine("Program End.");
        }
    }
}
