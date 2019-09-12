
namespace BridgeSharedFolderTest
{
    using Bridge.Html5;
    using Bridge.jQuery2;
    using YACE;

    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting");
            CardGame.Bataille bataille = new CardGame.Bataille();
            System.Console.WriteLine("Started");

            GameVue gameVue = bataille.GetVue();

            HTMLDivElement vueDiv = new HTMLDivElement();
            HTMLDivElement cardDiv = new HTMLDivElement();
            cardDiv.Style.Border = "5px";

            HTMLLabelElement cardLabel = new HTMLLabelElement();
            GameVue.CardVue firstCard = gameVue.Zones[0].Cards[0];

            int cardValue = firstCard.TagValues[0];
            cardLabel.TextContent = string.Format("Card : {0}", cardValue);

            cardDiv.AppendChild(cardLabel);
            vueDiv.AppendChild(cardDiv);

            Document.Body.AppendChild(vueDiv);

            System.Console.WriteLine("Ended");
        }
    }
}
