namespace YACE
{
    public struct GameVue
    {
        public string[] Ressources;
        public int[] RessourcesValues;

        public int currentPlayer;

        public ZoneVue[] Zones;
        public PlayerVue[] Players;

        public int GetRessource(string ressourceName)
        {
            int index = System.Array.IndexOf(this.Ressources, ressourceName);
            if (index < 0)
            {
                return int.MinValue;
            }

            return this.RessourcesValues[index];
        }

        public struct PlayerVue
        {
            public string[] Tags;
            public int[] TagValues;

            public string[] Ressources;
            public int[] RessourcesValues;

            public ZoneVue[] Zones;

            public int GetRessource(string ressourceName)
            {
                int index = System.Array.IndexOf(this.Ressources, ressourceName);
                if (index < 0)
                {
                    return int.MinValue;
                }

                return this.RessourcesValues[index];
            }

            public int GetTag(string tagName)
            {
                int index = System.Array.IndexOf(this.Tags, tagName);
                if (index < 0)
                {
                    return int.MinValue;
                }

                return this.TagValues[index];
            }
        }

        public struct ZoneVue
        {
            public string ZoneName;
            public CardVue[] Cards;
        }

        public struct CardVue
        {
            public string[] Tags;
            public int[] TagValues;

            public string DefinitionName;

            public int GetTag(string tagName)
            {
                int index = System.Array.IndexOf(this.Tags, tagName);
                if (index < 0)
                {
                    return int.MinValue;
                }

                return this.TagValues[index];
            }
        }
    }
}
