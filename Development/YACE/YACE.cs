﻿namespace YACE
{
    using AMG.Entity;
    using System;
    using System.Text;
    using System.Collections.Generic;

    public class YACE
    {
        public Context Context;

        private readonly Dictionary<string, int> globalRessourceIndexes = new Dictionary<string, int>();
        private readonly Dictionary<string, int> playerRessourceIndexes = new Dictionary<string, int>();

        private readonly Dictionary<string, int> playerZoneIndexes = new Dictionary<string, int>();
        private readonly Dictionary<string, int> globalZoneIndexes = new Dictionary<string, int>();

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
                        Context.Players[0].Zones[playerCounter] = new Zone { Name = zoneDefinition.Name, ZoneDefinition = zoneDefinition };
                        Context.Players[1].Zones[playerCounter] = new Zone { Name = zoneDefinition.Name, ZoneDefinition = zoneDefinition };
                        playerCounter++;
                    }
                    else
                    {
                        Context.GlobalZones[globalCounter] = new Zone { Name = zoneDefinition.Name, ZoneDefinition = zoneDefinition };
                        globalCounter++;
                    }
                }
            }
        }

        public void EndPlayerTurn()
        {
            Context.CurrentPlayer = (Context.CurrentPlayer + 1) % 2;
        }

        public void SetCardToZone(CardInstance card, string zoneName, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            Zone zone = this.GetZone(zoneName, playerIndex);

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

        public void DrawCardToZone(string from, PlayerIndex fromIndex, string to, PlayerIndex toIndex)
        {
            Zone fromZone = this.GetZone(from, fromIndex);
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
            Zone zone = this.GetZone(zoneName, playerIndex);
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

        public Zone GetZone(string zoneName, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            Zone zone = null;
            if (this.globalZoneIndexes.ContainsKey(zoneName))
            {
                zone = this.Context.GlobalZones[this.globalZoneIndexes[zoneName]];
            }
            else if (this.playerZoneIndexes.ContainsKey(zoneName))
            {
                zone = this.Context.Players[this.Context.GetPlayerIndex(playerIndex)].Zones[this.playerZoneIndexes[zoneName]];
            }

            return zone;
        }

        public int GetRessourceValue(string ressource, PlayerIndex playerIndex = PlayerIndex.Current)
        {
            if (this.globalRessourceIndexes.ContainsKey(ressource))
            {
                return this.Context.GlobalRessources[this.globalRessourceIndexes[ressource]].Value;
            }
            else if (this.playerRessourceIndexes.ContainsKey(ressource))
            {
                int index = this.Context.GetPlayerIndex(playerIndex);
                return this.Context.Players[index].Ressources[this.playerRessourceIndexes[ressource]].Value;
            }

            System.Console.WriteLine(string.Format("Unkown ressource '{0}'", ressource));
            return int.MinValue;
        }

        public void AlterRessource(string ressource, int delta)
        {
            if (globalRessourceIndexes.ContainsKey(ressource))
            {
                Context.GlobalRessources[globalRessourceIndexes[ressource]].Value = Context.GlobalRessources[globalRessourceIndexes[ressource]].Value + delta;
                return;
            }
            else if (playerRessourceIndexes.ContainsKey(ressource))
            {
                Context.Players[Context.CurrentPlayer].Ressources[playerRessourceIndexes[ressource]].Value += delta;
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
                int pi = this.Context.GetPlayerIndex(playerIndex);
                this.Context.Players[pi].Ressources[this.playerRessourceIndexes[ressource]].Value = value;
            }

            System.Console.WriteLine(string.Format("Unkown ressource '{0}'", ressource));
        }
    }

    public struct YACEParameters
    {
        public ResourceDefinition[] ResourceDefinitions;
        public ZoneDefinition[] ZoneDefinitions;
    }

    public struct ResourceDefinition
    {
        public string Name;
        public int MinValue;
        public int MaxValue;
        public int BaseValue;

        public bool IsPlayerBound;
        public bool ConsumeOnUse;

        public string[] ResetOnPhases;
    }

    public class Ressource
    {
        public ResourceDefinition Definition;
        public int Value;

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append(Definition.Name).Append(" ").Append(Value);

            return builder.ToString();
        }
    }

    public struct ZoneDefinition
    {
        public string Name;
        public bool IsPlayerBound;
        public bool IsOredered;
    }

    public class Context
    {
        public PlayerContext[] Players;
        public int CurrentPlayer;
        public Ressource[] GlobalRessources;
        public Zone[] GlobalZones;

        public int GetPlayerIndex(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.Player1)
            {
                return 0;
            }
            else if (playerIndex == PlayerIndex.Player2)
            {
                return 1;
            }

            return (this.CurrentPlayer + (int)playerIndex) % 2;
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append("Context :[");
            for(int i = 0; i < Players.Length; ++i)
            {
                if (i == CurrentPlayer)
                {
                    builder.Append("*");
                }

                PlayerContext player = Players[i];
                builder.Append(player.ToString()).Append(", ");
            }

            builder.Append("Ressources [");
            foreach(Ressource ressource in GlobalRessources)
            {
                builder.Append(ressource.ToString()).Append(", ");
            }
            builder.Append("], Zones");

            foreach (Zone zone in this.GlobalZones)
            {
                builder.Append(zone.ToString()).Append(",");
            }

            builder.Append("]");
            return builder.ToString();
        }
    }

    public class PlayerContext
    {
        public Ressource[] Ressources;
        public Zone[] Zones;

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append("Player [ Ressources : [");
            foreach (Ressource ressource in Ressources)
            {
                builder.Append(ressource.ToString()).Append(", ");
            }

            builder.Append("], Zones [");

            foreach (Zone zone in this.Zones)
            {
                builder.Append(zone.ToString()).Append(",");
            }

            builder.Append("]");
            return builder.ToString();
        }
    }

    public class CardDefinition
    {
        public string Name = string.Empty;
    }

    public class CardInstance : Entity
    {
        public CardDefinition Definition;
        internal Zone Zone;

        public CardInstance(CardDefinition definition)
        {
            this.Definition = definition;
        }
    }

    public class Zone
    {
        public string Name = string.Empty;
        public List<CardInstance> Cards = new List<CardInstance>();
        public ZoneDefinition ZoneDefinition;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(Name).Append(" : [");

            foreach (CardInstance card in this.Cards)
            {
                stringBuilder.Append(card.Definition.Name).Append("(").Append(card.GetTagsString()).Append(")");
            }

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }
    }

    public enum PlayerIndex : byte
    {
        Current = 0,
        Other = 1,

        Player1 = 2,
        Player2 = 3,
    }
}
