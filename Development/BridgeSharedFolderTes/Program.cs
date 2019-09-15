
namespace BridgeSharedFolderTest
{
    using Bridge.Html5;
    using Bridge.jQuery2;
    using YACE;

    class Program
    {
        private static GameVue gameVue;
        private static CardGame.Bataille Game;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting");
            CardGame.Bataille bataille = new CardGame.Bataille();
            Program.Game = bataille;
            System.Console.WriteLine("Started");

            Program.gameVue = bataille.GetVue();

            HTMLDivElement vueDiv = new HTMLDivElement();
            
            HTMLDivElement playDiv = Program.DivPlayState();
            vueDiv.AppendChild(playDiv);
            HTMLButtonElement bankButton = new HTMLButtonElement
            {
                TextContent = "Bank",
                OnClick = (ev) => { Program.OnBankButtonCliked(); }
            };
            vueDiv.AppendChild(bankButton);

            Document.Body.AppendChild(vueDiv);

            System.Console.WriteLine("Ended");
        }

        private static HTMLDivElement DivPlayState()
        {
            HTMLDivElement playDiv = new HTMLDivElement();

            HTMLLabelElement PlayerOneScoreLabel = new HTMLLabelElement();
            int playerOneScore = Program.gameVue.Players[0].GetRessource("Score");
            PlayerOneScoreLabel.TextContent = string.Format("Player 1 : {0}", playerOneScore);
            playDiv.AppendChild(PlayerOneScoreLabel);

            HTMLLabelElement playerTwoScoreLabel = new HTMLLabelElement();
            int playerTwoScore = Program.gameVue.Players[1].GetRessource("Score");
            playerTwoScoreLabel.TextContent = $"Player 2 : {playerTwoScore}'";
            playDiv.AppendChild(playerTwoScoreLabel);

            if (Program.gameVue.Zones[0].Cards.Length > 0)
            {
                GameVue.CardVue topCard = Program.gameVue.Zones[0].Cards[0];
                HTMLDivElement deckDiv = new HTMLDivElement();
                HTMLLabelElement deckTitleLabel = new HTMLLabelElement();
                deckTitleLabel.TextContent = "Top deck";
                deckDiv.AppendChild(deckTitleLabel);

                HTMLDivElement topCardDiv = Program.GetCardDiv(topCard);
                deckDiv.AppendChild(topCardDiv);

                playDiv.AppendChild(deckDiv);
            }

            return playDiv;
        }

        private static void OnBankButtonCliked()
        {
            Program.Game.ProcessOrder(new CardGame.Order_Score());
            Program.gameVue = Program.Game.GetVue();

            Document.RemoveChild(Document.ChildNodes[0]);
            HTMLDivElement vueDiv = new HTMLDivElement();

            HTMLDivElement playDiv = Program.DivPlayState();
            vueDiv.AppendChild(playDiv);
            HTMLButtonElement BankButton = new HTMLButtonElement
            {
                TextContent = "Bank",
                OnClick = (ev) => { Program.OnBankButtonCliked(); }
            };

            Document.Body.AppendChild(vueDiv);

        }

        private static HTMLDivElement GetCardDiv(GameVue.CardVue card)
        {
            HTMLDivElement cardDiv = new HTMLDivElement();
            cardDiv.Style.Border = "solid";
            cardDiv.Style.BorderColor = "black";
            cardDiv.Style.BorderWidth = "3px";
            cardDiv.Style.Width = "150px";
            cardDiv.Style.Height = "200px";

            HTMLLabelElement valueLabel = new HTMLLabelElement();
            valueLabel.TextContent = card.GetTag("Value").ToString();
            cardDiv.AppendChild(valueLabel);

            return cardDiv;
        }
    }
}
