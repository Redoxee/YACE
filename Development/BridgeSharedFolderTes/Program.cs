
namespace BridgeSharedFolderTest
{
    using Bridge.Html5;
    using Bridge.jQuery2;
    using YACE;

    class Program
    {
        private static GameVue gameVue;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting");
            CardGame.Bataille bataille = new CardGame.Bataille();
            System.Console.WriteLine("Started");

            Program.gameVue = bataille.GetVue();

            HTMLDivElement vueDiv = new HTMLDivElement();
            
            HTMLDivElement playDiv = Program.DivPlayState();
            vueDiv.AppendChild(playDiv);

            Document.Body.AppendChild(vueDiv);

            System.Console.WriteLine("Ended");
        }

        private static HTMLDivElement DivPlayState()
        {
            HTMLDivElement playDiv = new HTMLDivElement();

            HTMLLabelElement PlayerOneScoreLabel = new HTMLLabelElement();
            int playerScore = Program.gameVue.Players[0].GetRessource("Score");

            PlayerOneScoreLabel.TextContent = string.Format("Player 1 : {0}", playerScore);
            playDiv.AppendChild(PlayerOneScoreLabel);

            return playDiv;
        }

        private HTMLDivElement GetCardDiv()
        {
            HTMLDivElement cardDiv = new HTMLDivElement();
            cardDiv.Style.Border = "solid";
            cardDiv.Style.BorderColor = "black";
            cardDiv.Style.BorderWidth = "3px";
            cardDiv.Style.Width = "150px";
            cardDiv.Style.Height = "200px";
            return cardDiv;
        }
    }
}
