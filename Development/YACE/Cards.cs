namespace YACE
{
    using System.Collections.Generic;
    using AMG.Entity;

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

        public GameVue.CardVue GetVue()
        {
            GameVue.CardVue vue = new GameVue.CardVue();

            vue.DefinitionName = this.Definition.Name;
            int tagCount = this.Tags.Count;

            vue.Tags = new string[tagCount];
            vue.TagValues = new int[tagCount];

            int tagIndex = 0;
            foreach (var kvp in this.Tags)
            {
                vue.Tags[tagIndex] = kvp.Key;
                vue.TagValues[tagIndex] = kvp.Value;
                tagIndex++;
            }

            return vue;
        }
    }
}
