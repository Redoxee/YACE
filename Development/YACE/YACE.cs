using System.Collections.Generic;

namespace YACE
{
    public class YACE
    {
        public Context Context;

        public YACE(YACEParameters parameters)
        {
            System.Diagnostics.Debug.Assert(parameters.RessourceDefinitions != null, "Provide at least one ressource");

            Context.Players = new PlayerContext[2];
            Context.CurrentPlayer = 0;


            int globalRessourceCount = 0;
            int playerRessourceCount = 0;

            foreach (RessourceDefinition ressourceDefinition in parameters.RessourceDefinitions)
            {
                if (ressourceDefinition.IsBoundToPlayer)
                {
                    playerRessourceCount++;
                }
                else
                {
                    globalRessourceCount++;
                }
            }

            Context.GlobalRessource = new Ressource[globalRessourceCount];
            Context.Players[0].Ressources = new Ressource[playerRessourceCount];
            Context.Players[1].Ressources = new Ressource[playerRessourceCount];

            int globalCounter = 0;
            int playerCounter = 0;
            foreach (RessourceDefinition ressourceDefinition in parameters.RessourceDefinitions)
            {
                if (ressourceDefinition.IsBoundToPlayer)
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
    }

    public struct YACEParameters
    {
        public RessourceDefinition[] RessourceDefinitions;
    }

    public struct RessourceDefinition
    {
        public string Name;
        public int MinValue;
        public int MaxValue;
        public int BaseValue;

        public bool IsBoundToPlayer;
        public bool ConsumeOnUse;

        public string[] ResetOnPhases;
    }

    public struct Ressource
    {
        public RessourceDefinition Definition;
        public int Value;

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append(Definition.Name).Append(" ").Append(Value);

            return builder.ToString();
        }
    }

    public struct Context
    {
        public PlayerContext[] Players;
        public int CurrentPlayer;
        public Ressource[] GlobalRessource;

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

    public struct PlayerContext
    {
        public Ressource[] Ressources;

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
}
