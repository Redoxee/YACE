namespace YACE
{
    using AMG.Entity;
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

                Context.GlobalRessource = new Ressource[globalRessourceCount];
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
                        Context.GlobalRessource[globalCounter] = new Ressource { Definition = ressourceDefinition, Value = ressourceDefinition.BaseValue };
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
                        Context.Players[0].Zones[playerCounter] = new Zone { Name = zoneDefinition.Name };
                        Context.Players[1].Zones[playerCounter] = new Zone { Name = zoneDefinition.Name };
                        playerCounter++;
                    }
                    else
                    {
                        Context.GlobalZones[globalCounter] = new Zone { Name = zoneDefinition.Name };
                        globalCounter++;
                    }
                }
            }
        }

        public void EndPlayerTurn()
        {
            Context.CurrentPlayer = (Context.CurrentPlayer + 1) % 2;
        }

        public bool AlterCurrency(string currency, int delta)
        {
            if (globalRessourceIndexes.ContainsKey(currency))
            {
                Context.GlobalRessource[globalRessourceIndexes[currency]].Value = Context.GlobalRessource[globalRessourceIndexes[currency]].Value + delta;
                return true;
            }
            else if (playerRessourceIndexes.ContainsKey(currency))
            {
                Context.Players[Context.CurrentPlayer].Ressources[playerRessourceIndexes[currency]].Value += delta;
                return true;
            }

            return false;
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
        public Ressource[] GlobalRessource;
        public Zone[] GlobalZones;

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

            foreach(Ressource ressource in GlobalRessource)
            {
                builder.Append(ressource.ToString()).Append(", ");
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

            builder.Append("Player [");
            foreach (Ressource ressource in Ressources)
            {
                builder.Append(ressource.ToString()).Append(", ");
            }

            builder.Append("]");
            return builder.ToString();
        }
    }

    public class CardDefinition
    {
    }

    public class CardInstance : Entity
    {
        public CardDefinition Definition;
    }

    public class Zone
    {
        public string Name = string.Empty;
        public List<CardInstance> Cards = new List<CardInstance>();
    }
}
