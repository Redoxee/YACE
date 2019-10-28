namespace AMG.Game
{
    using System.Collections.Generic;
    using YACE;
    using YACE.FSM;

    public class Game
    {
        public struct Names
        {
            public struct Ressources
            {
                public const string VictoryPoints = "VictoryPoints";
                public const string PlayerAction = "PlayerAction";
            }

            public struct Zones
            {
                public const string GameDeck = "GameDeck";
                public const string GameShop = "GameShop";
                public const string PlayerBoard = "PlayerBoard";
                public const string PlayerDiscard = "PlayerDiscard";
            }

            public struct States
            {
                public const string PlayerTurn = "PlayerTurn";
                public const string InterTurn = "InterTrurn";
            }
        }

        YACE Yace;

        public void Initialize()
        {
            YACEParameters parameters;
            parameters.ResourceDefinitions = new ResourceDefinition[]
            {
                new ResourceDefinition
                {
                    Name = Names.Ressources.VictoryPoints,
                    MinValue = 0,
                    BaseValue = 0,
                    IsPlayerBound = true,
                },

                new ResourceDefinition
                {
                    Name = Names.Ressources.PlayerAction,
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
                    Name = Names.Zones.GameDeck,
                    IsPlayerBound = false,
                    IsOrdered = false,
                },

                new ZoneDefinition
                {
                    Name = Names.Zones.GameShop,
                    IsPlayerBound = false,
                    IsOrdered = true,
                },

                new ZoneDefinition
                {
                    Name = Names.Zones.PlayerBoard,
                    IsPlayerBound = true,
                    IsOrdered = true, 
                },

                new ZoneDefinition
                {
                    Name = Names.Zones.PlayerDiscard,
                    IsPlayerBound = true,
                    IsOrdered = false,
                },
            };

            parameters.States = new StateDefinition[]
            {
                new StateDefinition()
                {
                    Name = Names.States.PlayerTurn,
                },

                new StateDefinition()
                {
                    Name = Names.States.InterTurn,
                },
            };

            parameters.InitialState = parameters.States[1];

            this.Yace = new YACE(parameters);
            // Cards
            // - At the end of A turn gives you 1 victory point
            SpecializedCardDefinition SelfPointGainerDef = new AddPointCardDefinition(this.Yace)
            {
                Name = "SelfPointGainer",
                PointDelta = 1,
            };

            // - At the Start of YOUR turn gives you 1 point

            SpecializedCardDefinition AllAroundPointGainerDef = new AddPointCardDefinition(this.Yace)
            {
                Name = "AllAroundPointGainer",
                PointDelta = 1,
            };

            // - At the start of YOUR turn gives you 5 point and discard YOUR board
            SpecializedCardDefinition PointGainerFlush = new AddPointAndFlushBoard(this.Yace)
            {
                Name = "PointGainAndFlusher",
                PointDelta = 5,
                BoardToFlush = PlayerIndex.Current,
            };

            // - At the end of A turn Discrad THE board

            SpecializedCardDefinition FlushBoarder = new FlushBoardCard(this.Yace)
            {
                Name = "BoardFlusher",
                BoardToFlush = PlayerIndex.All,
            };

            // - At the start of OPONENT turn remove him 1 point
            SpecializedCardDefinition PointRemoverDef = new AddPointCardDefinition(this.Yace)
            {
                Name = "PointRemover",
                PlayerIndex = PlayerIndex.Other,
                PointDelta = -1,
            };
        }

        internal void PlayCard(CardInstance card)
        {
            this.Yace.SetCardToZone(card, Names.Zones.PlayerBoard, PlayerIndex.Current);
            SpecializedCardDefinition cardDef = card.Definition as SpecializedCardDefinition;
            Yace.StateMachine.RegisterWatcher(Names.States.PlayerTurn, PlayerIndex.Current, cardDef.OnPlayerTurn);
            Yace.StateMachine.RegisterWatcher(Names.States.InterTurn, PlayerIndex.All, cardDef.OnInterPlay);
        }

        internal abstract class SpecializedCardDefinition : CardDefinition
        {
            protected YACE Yace;

            public SpecializedCardDefinition(YACE yace)
            {
                this.Yace = yace;
            }

            public virtual void CardActionOnPlay()
            {
            }

            public virtual void OnPlayerTurn()
            {
            }

            public virtual void OnInterPlay()
            {
            }
        }

        internal class AddPointCardDefinition : SpecializedCardDefinition
        {
            public int PointDelta = 1;
            public PlayerIndex PlayerIndex = PlayerIndex.Current;

            public AddPointCardDefinition(YACE yace) : base(yace) { }

            public override void CardActionOnPlay()
            {
                this.Yace.AlterRessource("VictoryPoints", this.PointDelta, this.PlayerIndex);
            }
        }

        internal class AddPointAndFlushBoard : AddPointCardDefinition
        {
            public PlayerIndex BoardToFlush = PlayerIndex.Current;

            public AddPointAndFlushBoard(YACE yace) : base(yace) { }


            public override void CardActionOnPlay()
            {
                base.CardActionOnPlay();

                Zone[] zones = this.Yace.GetZones("PlayerHand", this.BoardToFlush);
                for (int zoneIndex = 0; zoneIndex < zones.Length; ++zoneIndex)
                {
                    Zone zone = zones[zoneIndex];

                    for (int cardIndex = zone.Cards.Count - 1; cardIndex >= 0; --cardIndex)
                    {
                        this.Yace.SetCardToZone(zone.Cards[cardIndex], "PlayerDiscard", zone.PlayerIndex);
                    }
                }
            }
        }

        internal class FlushBoardCard : SpecializedCardDefinition
        {
            public PlayerIndex BoardToFlush = PlayerIndex.Current;

            public FlushBoardCard(YACE yace) : base(yace)
            {
            }

            public override void OnInterPlay()
            {
                base.OnInterPlay();

                Zone[] zones = this.Yace.GetZones("PlayerHand", this.BoardToFlush);
                for (int zoneIndex = 0; zoneIndex < zones.Length; ++zoneIndex)
                {
                    Zone zone = zones[zoneIndex];

                    for (int cardIndex = zone.Cards.Count - 1; cardIndex >= 0; --cardIndex)
                    {
                        this.Yace.SetCardToZone(zone.Cards[cardIndex], "PlayerDiscard", zone.PlayerIndex);
                    }
                }
            }
        }
    }
}
