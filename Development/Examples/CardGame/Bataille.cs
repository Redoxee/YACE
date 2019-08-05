using YACE;

namespace CardGame
{
    class Bataille
    {
        private YACE.YACE yace;

        public void Main()
        {
            YACE.YACEParameters parameters = new YACEParameters();

            CardDefinition battailDefinition = new CardDefinition()
            {
                Name = "BatailleCard",
            };

            parameters.ResourceDefinitions = new ResourceDefinition[0];
            parameters.ZoneDefinitions = new ZoneDefinition[] {
                new ZoneDefinition
                {
                    Name = "MainDeck",
                    IsPlayerBound = false,
                    IsOredered = false,
                },
                new ZoneDefinition
                {
                    Name = "PlayerHand",
                    IsPlayerBound = true,
                    IsOredered = false,
                }
            };

            yace = new YACE.YACE(parameters);

            for (int i = 0; i < 10; ++i)
            {
                CardInstance card = new CardInstance(battailDefinition);
                card.AddTag("Value", i + 1);
                yace.SetCardToZone(card, "MainDeck");
            }

            System.Console.WriteLine(yace.Context.ToString());
        }
    }
}
