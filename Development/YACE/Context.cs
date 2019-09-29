namespace YACE
{
    public enum PlayerIndex : byte
    {
        Player0 = 0,
        Player1 = 1,

        Current = 2,
        Other = 3,
        All = 4,
    }

    public class Context
    {
        public PlayerContext[] Players;
        public int CurrentPlayer;
        public Ressource[] GlobalRessources;
        public Zone[] GlobalZones;

        public int ConvertPlayerIndex(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.Current)
            {
                return this.CurrentPlayer;
            }
            else if (playerIndex == PlayerIndex.Other)
            {
                return ((this.CurrentPlayer + 1) % 2);
            }
            else if (playerIndex == PlayerIndex.All)
            {
                return -1;
            }

            return (int)playerIndex;
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append("Context :[");
            for (int i = 0; i < Players.Length; ++i)
            {
                if (i == CurrentPlayer)
                {
                    builder.Append("*");
                }

                PlayerContext player = Players[i];
                builder.Append(player.ToString()).Append(", ");
            }

            builder.Append("Ressources [");
            foreach (Ressource ressource in GlobalRessources)
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

        public GameVue GetVue()
        {
            GameVue gameVue = new GameVue();

            gameVue.Ressources = new string[this.GlobalRessources.Length];
            gameVue.RessourcesValues = new int[this.GlobalRessources.Length];
            for (int ressourceIndex = 0; ressourceIndex < this.GlobalRessources.Length; ++ressourceIndex)
            {
                gameVue.Ressources[ressourceIndex] = this.GlobalRessources[ressourceIndex].Definition.Name;
                gameVue.RessourcesValues[ressourceIndex] = this.GlobalRessources[ressourceIndex].Value;
            }

            gameVue.currentPlayer = this.CurrentPlayer;

            gameVue.Zones = new GameVue.ZoneVue[this.GlobalZones.Length];
            for (int zoneIndex = 0; zoneIndex < this.GlobalZones.Length; ++zoneIndex)
            {
                gameVue.Zones[zoneIndex] = this.GlobalZones[zoneIndex].GetVue();
            }

            gameVue.Players = new GameVue.PlayerVue[this.Players.Length];
            for (int playerIndex = 0; playerIndex < this.Players.Length; ++playerIndex)
            {
                gameVue.Players[playerIndex] = this.Players[playerIndex].GetVue();
            }

            return gameVue;
        }
    }

    public class PlayerContext
    {
        public Ressource[] Ressources;
        public Zone[] Zones;

        public GameVue.PlayerVue GetVue()
        {
            GameVue.PlayerVue vue = new GameVue.PlayerVue();

            vue.Ressources = new string[this.Ressources.Length];
            vue.RessourcesValues = new int[this.Ressources.Length];
            for (int ressourceIndex = 0; ressourceIndex < this.Ressources.Length; ++ressourceIndex)
            {
                vue.Ressources[ressourceIndex] = this.Ressources[ressourceIndex].Definition.Name;
                vue.RessourcesValues[ressourceIndex] = this.Ressources[ressourceIndex].Value;
            }

            vue.Zones = new GameVue.ZoneVue[this.Zones.Length];
            for (int zoneIndex = 0; zoneIndex < this.Zones.Length; ++zoneIndex)
            {
                vue.Zones[zoneIndex] = this.Zones[zoneIndex].GetVue();
            }

            return vue;
        }

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
}
