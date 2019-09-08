//namespace GameBataille
//{
//    using YACE;
//    using YACE.Comunication;

//    public class GameSimulation
//    {
//        private YACE yace;

//        public GameSimulation()
//        {
//            YACEParameters parameters = new YACEParameters();

//            CardDefinition battailDefinition = new CardDefinition()
//            {
//                Name = "BatailleCard",
//            };

//            parameters.ResourceDefinitions = new ResourceDefinition[]
//            {
//                new ResourceDefinition
//                {
//                    Name = "Multiplier",
//                    BaseValue = 1,
//                    IsPlayerBound = true,
//                    MinValue = 1,
//                },

//                new ResourceDefinition
//                {
//                    Name = "Score",
//                    BaseValue = 0,
//                    IsPlayerBound = true,
//                }
//            };

//            parameters.ZoneDefinitions = new ZoneDefinition[]
//            {
//                new ZoneDefinition
//                {
//                    Name = "MainDeck",
//                    IsPlayerBound = false,
//                    IsOredered = false,
//                },
//                new ZoneDefinition
//                {
//                    Name = "PlayerHand",
//                    IsPlayerBound = true,
//                    IsOredered = false,
//                },
//                new ZoneDefinition
//                {
//                    Name ="DiscardPile",
//                },
//            };

//            yace = new YACE(parameters);
//            CardInstance lastCard = null;
//            for (int i = 0; i < 10; ++i)
//            {
//                CardInstance card = new CardInstance(battailDefinition);
//                card.AddTag("Value", i + 1);
//                yace.SetCardToZone(card, "MainDeck");
//                lastCard = card;
//            }

//            yace.ShuffleZone("MainDeck");
//        }

//        public void ProcessOrder(GameOrder order)
//        {
//            yace.DrawCardToZone("MainDeck", PlayerIndex.Current, "PlayerHand", PlayerIndex.Current);
//            Zone playerHand = yace.GetZone("PlayerHand");

//            if (order is Order_Score)
//            {
//                int multiplier = yace.GetRessourceValue("Multiplier");
//                CardInstance card = playerHand.Cards[0];
//                int value = card.GetValue("Value");
//                yace.AlterRessource("Score", multiplier * value);
//                yace.SetRessource("Multiplier", 1, PlayerIndex.Current);
//            }
//            else if (order is Order_Multiply)
//            {
//                yace.AlterRessource("Multiplier", 1);
//            }

//            yace.DrawCartToZone("PlayerHand", "DiscardPile");
//            yace.EndPlayerTurn();
//        }

//        public GameVue GetVue()
//        {
//            return this.yace.Context.GetVue();
//        }
//    }

//    public abstract class GameOrder : Order
//    {
//    }

//    public class Order_Score : GameOrder
//    {
//    }

//    public class Order_Multiply : GameOrder
//    {
//    }
//}

