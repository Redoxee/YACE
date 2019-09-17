
namespace BridgeSharedFolderTest
{
    using Bridge.Html5;
    using Bridge.jQuery2;
    using YACE;

    class Program
    {
        private static GameVue gameVue;
        private static CardGame.Bataille Game;
        private static HTMLDivElement mainDiv;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting");
            CardGame.Bataille bataille = new CardGame.Bataille();
            Program.Game = bataille;
            System.Console.WriteLine("Started");

            Program.mainDiv = new HTMLDivElement();
            Document.Body.AppendChild(Program.mainDiv);

            Program.gameVue = Program.Game.GetVue();
            Program.DisplayPlayState();
            
            System.Console.WriteLine("Ended");
        }

        private static HTMLDivElement DivPlayState()
        {
            HTMLDivElement playDiv = new HTMLDivElement();
            if (Program.gameVue.Zones[0].Cards.Length > 0)
            {
                HTMLLabelElement PlayerOneScoreLabel = new HTMLLabelElement();
                int playerOneScore = Program.gameVue.Players[0].GetRessource("Score");
                int playerOneMultiplier = Program.gameVue.Players[0].GetRessource("Multiplier");
                PlayerOneScoreLabel.TextContent = string.Format("Player 1 : '{0}', Multiplier : '{1}'", playerOneScore, playerOneMultiplier);
                playDiv.AppendChild(PlayerOneScoreLabel);
                playDiv.AppendChild(new HTMLParagraphElement());
                HTMLLabelElement playerTwoScoreLabel = new HTMLLabelElement();
                int playerTwoScore = Program.gameVue.Players[1].GetRessource("Score");
                int playerTwoMultiplier = Program.gameVue.Players[1].GetRessource("Multiplier");
                playerTwoScoreLabel.TextContent = $"Player 2 : {playerTwoScore}', Multiplier : '{playerTwoMultiplier}'";
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

                    deckDiv.AppendChild(new HTMLParagraphElement());
                    deckDiv.AppendChild(new HTMLLabelElement() { TextContent = $"'{Program.gameVue.Zones[0].Cards.Length}' Cards left in the deck" });

                    playDiv.AppendChild(deckDiv);
                }

                playDiv.AppendChild(new HTMLDivElement() { TextContent = string.Format("Current player {0}", Program.gameVue.currentPlayer + 1)});
            }
            else
            {
                HTMLLabelElement PlayerOneScoreLabel = new HTMLLabelElement();
                int playerOneScore = Program.gameVue.Players[0].GetRessource("Score");
                PlayerOneScoreLabel.TextContent = string.Format("Player 1 : {0}", playerOneScore);
                playDiv.AppendChild(PlayerOneScoreLabel);
                playDiv.AppendChild(new HTMLParagraphElement());
                HTMLLabelElement playerTwoScoreLabel = new HTMLLabelElement();
                int playerTwoScore = Program.gameVue.Players[1].GetRessource("Score");
                playerTwoScoreLabel.TextContent = $"Player 2 : {playerTwoScore}'";
                playDiv.AppendChild(playerTwoScoreLabel);
                playDiv.AppendChild(new HTMLParagraphElement());

                string winnerMessage;
                if (playerOneScore > playerTwoScore)
                {
                    winnerMessage = "Player 1 Win!";
                }
                else
                {
                    winnerMessage = "Player 2 Win!";
                }

                HTMLLabelElement winnerLabel = new HTMLLabelElement
                {
                    TextContent = winnerMessage,
                };

                playDiv.AppendChild(winnerLabel);
            }

            return playDiv;
        }

        private static void OnBankButtonCliked()
        {
            Program.Game.ProcessOrder(new CardGame.Order_Score());
            Program.gameVue = Program.Game.GetVue();
            Program.DisplayPlayState();
        }

        private static void OnMultiplyButtonClicked()
        {
            Program.Game.ProcessOrder(new CardGame.Order_Multiply());
            Program.gameVue = Program.Game.GetVue();
            Program.DisplayPlayState();
        }

        private static void DisplayPlayState()
        {
            HTMLDivElement vueDiv = new HTMLDivElement();

            HTMLDivElement playDiv = Program.DivPlayState();
            vueDiv.AppendChild(playDiv);

            if(Program.gameVue.Zones[0].Cards.Length > 0)
            { 
                HTMLButtonElement bankButton = new HTMLButtonElement
                {
                    TextContent = "Bank",
                    OnClick = (ev) => { Program.OnBankButtonCliked(); }
                };
                vueDiv.AppendChild(bankButton);

                HTMLButtonElement multiplyButton = new HTMLButtonElement
                {
                    TextContent = "Multiply",
                    OnClick = (ev) => { Program.OnMultiplyButtonClicked(); }
                };
                vueDiv.AppendChild(multiplyButton);
            }

            if (Program.mainDiv.ChildElementCount > 0)
            {
                Program.mainDiv.RemoveChild(Program.mainDiv.ChildNodes[0]);
            }

            Program.mainDiv.AppendChild(vueDiv);
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
