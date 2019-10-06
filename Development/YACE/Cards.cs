namespace YACE
{
    using System.Collections.Generic;
    using AMG.Entity;

    public class CardDefinition
    {
        public string Name;
        public TagDefinition[] BaseTags;
    }

    public class CardInstance : Entity
    {
        public CardDefinition Definition;
        internal Zone Zone;

        public CardInstance(CardDefinition definition)
        {
            this.Definition = definition;

            if (definition.BaseTags != null)
            {
                for (int tagIndex = 0; tagIndex < definition.BaseTags.Length; ++tagIndex)
                {
                    this.AddTag(definition.BaseTags[tagIndex].Tag, definition.BaseTags[tagIndex].BaseValue);
                }
            }
        }

        public GameVue.CardVue GetVue()
        {
            GameVue.CardVue vue;

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
