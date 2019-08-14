using YACE;
using AMG.Entity;

namespace CardGame
{
    class Bataille
    {
        private YACE.YACE yace;

        public void Main()
        {
            YACE.YACEParameters parameters = new YACEParameters();

            CardDefinition battailDefinition = new CardDefinition()
            {
                Name = "BatailleCard",
            };

            parameters.ResourceDefinitions = new ResourceDefinition[]
            {
                new ResourceDefinition
                {
                    Name = "Multiplier",
                    BaseValue = 1,
                    IsPlayerBound = true,
                    MinValue = 1,
                },

                new ResourceDefinition
                {
                    Name = "Score",
                    BaseValue = 0,
                    IsPlayerBound = true,
                }
            };

            parameters.ZoneDefinitions = new ZoneDefinition[] 
            {
                new ZoneDefinition
                {
                    Name = "MainDeck",
                    IsPlayerBound = false,
                    IsOredered = false,
                },
                new ZoneDefinition
                {
                    Name = "PlayerHand",
                    IsPlayerBound = true,
                    IsOredered = false,
                },
                new ZoneDefinition
                {
                    Name ="DiscardPile",
                },
            };

            yace = new YACE.YACE(parameters);
            CardInstance lastCard = null;
            for (int i = 0; i < 10; ++i)
            {
                CardInstance card = new CardInstance(battailDefinition);
                card.AddTag("Value", i + 1);
                yace.SetCardToZone(card, "MainDeck");
                lastCard = card;
            }
            yace.ShuffleZone("MainDeck");

            Zone mainDeck = yace.GetZone("MainDeck");
            while (mainDeck.Cards.Count > 0)
            {
                int currentPlayer = yace.GetRessourceValue("Score", PlayerIndex.Current);
                int opponentPlayer = yace.GetRessourceValue("Score", PlayerIndex.Other);
                int cardInDeck = mainDeck.Cards.Count;

                System.Console.WriteLine(string.Format("Player current : {0} \nOpponent : {1}\nCards in the deck : {2}", currentPlayer, opponentPlayer, cardInDeck));
                this.PlayOneTurn();

                yace.EndPlayerTurn();
            }

            int player1Score = yace.GetRessourceValue("Score", PlayerIndex.Player1);
            int player2Score = yace.GetRessourceValue("Score", PlayerIndex.Player2);
            if (player1Score > player2Score)
            {
                System.Console.WriteLine("Player 1 Won!");
            }
            else if (player1Score < player2Score)
            {
                System.Console.WriteLine("Player 2 Won!");
            }
            else
            {
                System.Console.WriteLine("Draw.");
            }

            System.Console.ReadLine();
        }

        private void PlayOneTurn()
        {
            yace.DrawCardToZone("MainDeck", PlayerIndex.Current, "PlayerHand", PlayerIndex.Current);
            Zone playerHand = yace.GetZone("PlayerHand");

            System.Console.WriteLine(playerHand.ToString());
            PlayerAction playerAction = ReadNextAction();

            if (playerAction == PlayerAction.Multiply)
            {
                yace.AlterRessource("Multiplier", 1);
            }
            else if (playerAction == PlayerAction.Score)
            {
                int multiplier = yace.GetRessourceValue("Multiplier");
                CardInstance card = playerHand.Cards[0];
                int value = card.GetValue("Value");
                yace.AlterRessource("Score", multiplier * value);
                //TODO reset ressource
                yace.SetRessource("Multiplier", 1, PlayerIndex.Current);
            }

            yace.DrawCartToZone("PlayerHand", "DiscardPile");
        }

        private PlayerAction ReadNextAction()
        {
            while (true)
            {
                System.Console.WriteLine(string.Format("Player {0}, 'score' or 'multiply'", yace.Context.CurrentPlayer));
                string command = System.Console.ReadLine().Trim().ToLower();
                if (command== "s")
                {
                    return PlayerAction.Score;
                }
                if (command == "m")
                {
                    return PlayerAction.Multiply;
                }
            }
        }

        private enum PlayerAction
        {
            None = 0,
            Score = 1,
            Multiply = 2,
        }
    }
}
