namespace YACE
{
    using System;
    using System.Collections.Generic;

    using FSM;

    public class YACE
    {
        public Context Context;
        public readonly FiniteStateMachine StateMachine;

        private readonly Dictionary<string, int> globalRessourceIndexes = new Dictionary<string, int>();
        private readonly Dictionary<string, int> playerRessourceIndexes = new Dictionary<string, int>();

        private readonly Dictionary<string, int> playerZoneIndexes = new Dictionary<string, int>();
        private readonly Dictionary<string, int> globalZoneIndexes = new Dictionary<string, int>();

        private readonly Zone[] workingOneZoneArray = new Zone[1];
        private readonly Zone[] workingTwoZoneArray = new Zone[2];

        public YACE(YACEParameters parameters)
        {
            System.Diagnostics.Debug.Assert(parameters.ResourceDefinitions != null, "Provide at least one ressource");

            Context = new Context
            {
                Players = new PlayerContext[]
                {
                    new PlayerContext(),
                    new PlayerContext()
                },
                CurrentPlayer = 0
            };

            this.globalRessourceIndexes.Clear();
            this.playerRessourceIndexes.Clear();

            int globalRessourceCount = 0;
            int playerRessourceCount = 0;

            if (parameters.ResourceDefinitions != null)
            {
                foreach (ResourceDefinition ressourceDefinition in parameters.ResourceDefinitions)
                {
                    if (ressourceDefinition.IsPlayerBound)
                    {
                        playerRessourceIndexes[ressourceDefinition.Name] = playerRessourceCount++;
                    }
                    else
                    {
                        globalRessourceIndexes[ressourceDefinition.Name] = globalRessourceCount++;
                    }
                }

                Context.GlobalRessources = new Ressource[globalRessourceCount];
                Context.Players[0].Ressources = new Ressource[playerRessourceCount];
                Context.Players[1].Ressources = new Ressource[playerRessourceCount];

                int globalCounter = 0;
                int playerCounter = 0;
                foreach (ResourceDefinition ressourceDefinition in parameters.ResourceDefinitions)
                {
                    if (ressourceDefinition.IsPlayerBound)
                    {
                        Context.Players[0].Ressources[playerCounter] = new Ressource { Definition = ressourceDefinition, Value = ressourceDefinition.BaseValue };
                        Context.Players[1].Ressources[playerCounter] = new Ressource { Definition = ressourceDefinition, Value = ressourceDefinition.BaseValue };
                        playerCounter++;
                    }
                    else
                    {
                        Context.GlobalRessources[globalCounter] = new Ressource { Definition = ressourceDefinition, Value = ressourceDefinition.BaseValue };
                        globalCounter++;
                    }
                }
            }


            if (parameters.ZoneDefinitions != null)
            {
                int nbGlobalZones = 0;
                int nbPlayerZones = 0;
                for (int i = 0; i < parameters.ZoneDefinitions.Length; ++i)
                {
                    if (parameters.ZoneDefinitions[i].IsPlayerBound)
                    {
                        playerZoneIndexes[parameters.ZoneDefinitions[i].Name] = nbPlayerZones++;
                    }
                    else
                    {
                        globalZoneIndexes[parameters.ZoneDefinitions[i].Name] = nbGlobalZones++;
                    }
                }

                Context.GlobalZones = new Zone[nbGlobalZones];
                Context.Players[0].Zones = new Zone[nbPlayerZones];
                Context.Players[1].Zones = new Zone[nbPlayerZones];


                int playerCounter = 0;
                int globalCounter = 0;
                foreach (ZoneDefinition zoneDefinition in parameters.ZoneDefinitions)
                {
                    if (zoneDefinition.IsPlayerBound)
                    {
                        Context.Players[0].Zones[playerCounter] = new Zone { Name = zoneDefinition.Name, ZoneDefinition = zoneDefinition , PlayerIndex = PlayerIndex.Player0};
                        Context.Players[1].Zones[playerCounter] = new Zone { Name = zoneDefinition.Name, ZoneDefinition = zoneDefinition , PlayerIndex = PlayerIndex.Player1};
                        playerCounter++;
                    }
                    else
                    {
                        Context.GlobalZones[globalCounter] = new Zone { Name = zoneDefinition.Name, ZoneDefinition = zoneDefinition };
                        globalCounter++;
                    }
                }
            }

            if (parameters.States != null)
            {
                this.StateMachine = new FiniteStateMachine(parameters.States, parameters.InitialState, this);
            }
        }

        public void EndPlayerTurn()
        {
            Context.CurrentPlayer = (Context.CurrentPlayer + 1) % 2;
        }

        public void SetCardToZone(CardInstance card, string zoneName, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            Zone[] zones = this.GetZones(zoneName, playerIndex);
            System.Diagnostics.Debug.Assert(zones.Length == 1);
            Zone zone = zones[0];

            if (zone == null)
            {
                System.Console.WriteLine(string.Format("[Warning] unkown zone '{0}'", zoneName));
                return;
            }

            if (zone == card.Zone)
            {
                return;
            }

            // Removing the precedent zone.
            if (card.Zone != null)
            {
                card.Zone.Cards.Remove(card);
                card.RemoveTag(card.Zone.Name);
                card.Zone = null;
            }

            zone.Cards.Add(card);
            card.AddTag(zoneName);
            card.Zone = zone;
        }

        public CardInstance InstanciateCardToZone(CardDefinition cardDefinition, string zoneName, PlayerIndex playerIndex)
        {
            CardInstance card = new CardInstance(cardDefinition);
            this.SetCardToZone(card, zoneName, playerIndex);
            return card;
        }

        public void DrawCardToZone(string from, PlayerIndex fromIndex, string to, PlayerIndex toIndex)
        {
            Zone[] zones = this.GetZones(from, fromIndex);
            System.Diagnostics.Debug.Assert(zones.Length == 1);
            Zone fromZone = zones[0];

            if (fromZone.Cards.Count > 0)
            {
                this.SetCardToZone(fromZone.Cards[0], to, toIndex);
            }
        }

        public void DrawCartToZone(string from, string to)
        {
            this.DrawCardToZone(from, PlayerIndex.Current, to, PlayerIndex.Current);
        }

        public void ShuffleZone(string zoneName, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            Zone[] zones = this.GetZones(zoneName, playerIndex);
            for (int zoneIndex = 0; zoneIndex < zones.Length; ++zoneIndex)
            {
                Zone zone = zones[zoneIndex];

                int nbCards = zone.Cards.Count;
                Random random = new Random();

                for (int i = 0; i < nbCards - 1; ++i)
                {
                    int newIndex = random.Next(i, nbCards);
                    CardInstance swap = zone.Cards[newIndex];
                    zone.Cards[newIndex] = zone.Cards[i];
                    zone.Cards[i] = swap;
                }
            }
        }

        public Zone[] GetZones(string zoneName, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            if (this.globalZoneIndexes.ContainsKey(zoneName))
            {
                this.workingOneZoneArray[0] = this.Context.GlobalZones[this.globalZoneIndexes[zoneName]];
                return this.workingOneZoneArray;
            }
            else if (this.playerZoneIndexes.ContainsKey(zoneName))
            {
                int zoneIndex = this.playerZoneIndexes[zoneName];
                int index = this.Context.ConvertPlayerIndex(playerIndex);
                if (index > -1)
                {
                    this.workingOneZoneArray[0] = this.Context.Players[index].Zones[zoneIndex];
                    return this.workingOneZoneArray;
                }
                else
                {
                    this.workingTwoZoneArray[0] = this.Context.Players[0].Zones[zoneIndex];
                    this.workingTwoZoneArray[1] = this.Context.Players[1].Zones[zoneIndex];
                    return this.workingTwoZoneArray;
                }
            }

            return null;
        }

        public Zone GetSingleZone(string zoneName, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            Zone[] zones = this.GetZones(zoneName, playerIndex);
            System.Diagnostics.Debug.Assert(zones.Length == 1);
            return zones[0];
        }

        public int GetRessourceValue(string ressource, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            if (this.globalRessourceIndexes.ContainsKey(ressource))
            {
                return this.Context.GlobalRessources[this.globalRessourceIndexes[ressource]].Value;
            }
            else if (this.playerRessourceIndexes.ContainsKey(ressource))
            {
                int index = this.Context.ConvertPlayerIndex(playerIndex);
                return this.Context.Players[index].Ressources[this.playerRessourceIndexes[ressource]].Value;
            }

            System.Console.WriteLine(string.Format("Unkown ressource '{0}'", ressource));
            return int.MinValue;
        }

        public void AlterRessource(string ressource, int delta, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            if (globalRessourceIndexes.ContainsKey(ressource))
            {
                Context.GlobalRessources[globalRessourceIndexes[ressource]].Value = Context.GlobalRessources[globalRessourceIndexes[ressource]].Value + delta;
                return;
            }
            else if (playerRessourceIndexes.ContainsKey(ressource))
            {
                int pi = this.Context.ConvertPlayerIndex(playerIndex);

                Context.Players[pi].Ressources[playerRessourceIndexes[ressource]].Value += delta;
                return;
            }

            System.Console.WriteLine(string.Format("Unkown ressource '{0}'", ressource));
        }

        public void SetRessource(string ressource, int value, PlayerIndex playerIndex)
        {
            if (this.globalRessourceIndexes.ContainsKey(ressource))
            {
                this.Context.GlobalRessources[globalRessourceIndexes[ressource]].Value = value;
                return;
            }
            else if (this.playerRessourceIndexes.ContainsKey(ressource))
            {
                int pi = this.Context.ConvertPlayerIndex(playerIndex);
                this.Context.Players[pi].Ressources[this.playerRessourceIndexes[ressource]].Value = value;
                return;
            }

            System.Console.WriteLine(string.Format("Unkown ressource '{0}'", ressource));
        }
    }

    public struct YACEParameters
    {
        public ResourceDefinition[] ResourceDefinitions;
        public ZoneDefinition[] ZoneDefinitions;

        public StateDefinition[] States;
        public StateDefinition InitialState;
    }
}
