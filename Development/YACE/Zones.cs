namespace YACE
{
    using System.Collections.Generic;
    using System.Text;

    public struct ZoneDefinition
    {
        public string Name;
        public bool IsPlayerBound;
        public bool IsOredered;
    }

    public class Zone
    {
        public string Name = string.Empty;
        public List<CardInstance> Cards = new List<CardInstance>();
        public ZoneDefinition ZoneDefinition;

        public GameVue.ZoneVue GetVue()
        {
            GameVue.ZoneVue zoneVue = new GameVue.ZoneVue();
            zoneVue.ZoneName = this.Name;

            int cardCount = this.Cards.Count;
            zoneVue.Cards = new GameVue.CardVue[cardCount];
            for (int cardIndex = 0; cardIndex < cardCount; ++cardIndex)
            {
                zoneVue.Cards[cardIndex] = this.Cards[cardIndex].GetVue();
            }

            return zoneVue;
        }

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
}
