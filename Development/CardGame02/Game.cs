namespace AMG.Game
{
    using System.Collections.Generic;
    using YACE;
    using YACE.FSM;

    public class Game
    {
        YACE Yace;

        public void Initialize()
        {
            YACEParameters parameters;
            parameters.ResourceDefinitions = new ResourceDefinition[]
            {
                new ResourceDefinition
                {
                    Name = "VictoryPoints",
                    MinValue = 0,
                    BaseValue = 0,
                    IsPlayerBound = true,
                },

                new ResourceDefinition
                {
                    Name = "PlayerAction",
                    BaseValue = 2,
                    MinValue = 0,
                    IsPlayerBound = true,
                    ResetOnState = new string[]{"PlayerTurn" },
                }
            };

            parameters.ZoneDefinitions = new ZoneDefinition[]
            {
                new ZoneDefinition
                {
                    Name = "GameDeck",
                    IsPlayerBound = false,
                    IsOrdered = false,
                },

                new ZoneDefinition
                {
                    Name = "GameShop",
                    IsPlayerBound = false,
                    IsOrdered = true,
                },

                new ZoneDefinition
                {
                    Name = "PlayerHand",
                    IsPlayerBound = true,
                    IsOrdered = true, 
                },

                new ZoneDefinition
                {
                    Name = "PlayerDiscard",
                    IsPlayerBound = true,
                    IsOrdered = false,
                },
            };

            parameters.States = new StateDefinition[]
            {
                new StateDefinition()
                {
                    Name = "PlayState",
                },

                new StateDefinition()
                {
                    Name = "InterPlay",
                },
            };

            parameters.InitialState = parameters.States[1];

            this.Yace = new YACE(parameters);
            // Cards
            // - At the end of A turn gives you 1 victory point
            SpecializedCardDefinition SelfPointGainerDef = new AddPointCardDefinition()
            {
                Name = "SelfPointGainer",
                PointDelta = 1,
            };

            // - At the Start of YOUR turn gives you 1 point

            SpecializedCardDefinition AllAroundPointGainerDef = new AddPointCardDefinition()
            {
                Name = "AllAroundPointGainer",
                PointDelta = 1,
            };

            // - At the start of YOUR turn gives you 5 point and discard YOUR board
            SpecializedCardDefinition PointGainerFlush = new AddPointAndFlushBoard()
            {
                Name = "PointGainAndFlusher",
                PointDelta = 5,
                BoardToFlush = PlayerIndex.Current,
            };

            // - At the end of A turn Discrad THE board

            SpecializedCardDefinition FlushBoarder = new FlushBoardCard()
            {
                Name = "BoardFlusher",
                BoardToFlush = PlayerIndex.All,
            };

            // - At the start of OPONENT turn remove him 1 point
            SpecializedCardDefinition PointRemoverDef = new AddPointCardDefinition()
            {
                Name = "PointRemover",
                PlayerIndex = PlayerIndex.Other,
                PointDelta = -1,
            };

            // - At the start of YOUR Turn gives you One Action point
        }

        internal abstract class SpecializedCardDefinition : CardDefinition
        {
            public virtual void CardActionOnPlay(YACE yace)
            {
            }

            public virtual void OnPlayerTurn(YACE yace)
            {
            }

            public virtual void OnInterPlay(YACE yace)
            {
            }
        }

        internal class AddPointCardDefinition : SpecializedCardDefinition
        {
            public int PointDelta = 1;
            public PlayerIndex PlayerIndex = PlayerIndex.Current;

            public override void CardActionOnPlay(YACE yace)
            {
                yace.AlterRessource("VictoryPoints", this.PointDelta, this.PlayerIndex);
            }
        }

        internal class AddPointAndFlushBoard : AddPointCardDefinition
        {
            public PlayerIndex BoardToFlush = PlayerIndex.Current;

            public override void CardActionOnPlay(YACE yace)
            {
                base.CardActionOnPlay(yace);

                Zone[] zones = yace.GetZones("PlayerHand", this.BoardToFlush);
                for (int zoneIndex = 0; zoneIndex < zones.Length; ++zoneIndex)
                {
                    Zone zone = zones[zoneIndex];

                    for (int cardIndex = zone.Cards.Count - 1; cardIndex >= 0; --cardIndex)
                    {
                        yace.SetCardToZone(zone.Cards[cardIndex], "PlayerDiscard", zone.PlayerIndex);
                    }
                }
            }
        }

        internal class FlushBoardCard : SpecializedCardDefinition
        {
            public PlayerIndex BoardToFlush = PlayerIndex.Current;

            public override void OnInterPlay(YACE yace)
            {
                base.OnInterPlay(yace);

                Zone[] zones = yace.GetZones("PlayerHand", this.BoardToFlush);
                for (int zoneIndex = 0; zoneIndex < zones.Length; ++zoneIndex)
                {
                    Zone zone = zones[zoneIndex];

                    for (int cardIndex = zone.Cards.Count - 1; cardIndex >= 0; --cardIndex)
                    {
                        yace.SetCardToZone(zone.Cards[cardIndex], "PlayerDiscard", zone.PlayerIndex);
                    }
                }
            }
        }
    }
}
