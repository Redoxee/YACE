namespace AMG.Game
{
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
                    Name = "PlayerDeck",
                    IsPlayerBound = true,
                    IsOrdered = false,
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

            // Cards
            // - At the end of A turn gives you 1 victory point
            // - At the Start of YOUR turn gives you 1 point
            // - At the start of YOUR turn gives you 5 point and discard YOUR board
            // - At the end of A turn Discrad THE board
            // - At the start of OPONENT turn remove him 1 point
            // - At the start of YOUR Turn gives you One Action point

            this.Yace = new YACE(parameters);
        }

        internal abstract class SpecializedCardDefinition : CardDefinition
        {
            public virtual void CardActionOnPlay(YACE yace)
            {
            }
        }

        internal class AddPointCardDefinition : SpecializedCardDefinition
        {
            public int pointDelta = 1;

            public override void CardActionOnPlay(YACE yace)
            {
                yace.AlterRessource("VictoryPoints", this.pointDelta);
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
    }
}
