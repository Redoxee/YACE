using System.Collections.Generic;
using YACE;

namespace CardGame
{
    class ComunicationLayer
    {
        private bool thinking = false;
        public bool IsOver = false;
        private List<Order> Orders = new List<Order>();

        public GameVue GameVue = new GameVue();
        private Bataille game;

        public ComunicationLayer(Bataille game)
        {
            this.game = game;
            this.GameVue = this.game.GetVue();
        }

        public bool IsThinking
        {
            get
            {
                return this.thinking;
            }
        }

        public void PostOrder(Order order)
        {
            lock (this)
            {
                this.Orders.Add(order);
                this.thinking = true;
            }
        }

        public bool ProcessOrders()
        {
            if (!thinking)
            {
                return false;
            }

            lock (this)
            {
                while (this.Orders.Count > 0)
                {
                    Order order = this.Orders[0];
                    this.Orders.RemoveAt(0);

                    System.Console.WriteLine(string.Format("Processing order : {0}", order.ToString()));

                    if (order is GameOrder)
                    {
                        this.game.ProcessOrder(order as GameOrder);
                    }
                    else if (order is Order_Terminate)
                    {
                        this.IsOver = true;
                    }

                    this.GameVue = this.game.GetVue();
                }
            }

            this.thinking = false;

            return true;
        }
    }

    abstract class Order
    {
    }

    class Order_Terminate : Order
    {
    }
}
