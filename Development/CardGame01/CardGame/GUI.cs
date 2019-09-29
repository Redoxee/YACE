using System;
using System.Collections.Generic;

using System.Windows.Forms;
using System.Drawing;

namespace CardGame
{
    class GUIHandler
    {
        private GameForm gameForm;
        public ComunicationLayer interop;
        System.Threading.Thread guiThread;

        public bool NeedRefresh = false;

        public GUIHandler(ComunicationLayer comunicationLayer)
        {
            interop = comunicationLayer;
            this.gameForm = new GameForm(this);
        }

        public void Start()
        {
            this.guiThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.Main));
            this.guiThread.Start();
        }

        public void Main()
        {
            this.gameForm.Show();
            Application.Run();
        }

        public void NotifyChanges()
        {
            this.NeedRefresh = true;
            this.gameForm.Invalidate();
        }
    }

    class GameForm : Form
    {
        GUIHandler gui;

        Button scoreButton = new Button();
        Button multiplyButton = new Button();

        public GameForm(GUIHandler gui) : base()
        {
            this.Width = 1024;
            this.Height = 720;
            
            this.Controls.Add(this.scoreButton);
            this.scoreButton.Text = "Bank";
            this.scoreButton.Location = new Point(this.Width / 2 + 80, 200);

            this.scoreButton.Click += this.ScoreButtonClicked;

            this.Controls.Add(this.multiplyButton);
            this.multiplyButton.Text = "Multiply";
            this.multiplyButton.Location = new Point(this.Width / 2 - 80, 200);
            this.multiplyButton.Click += this.MultiplyButtonClicked;

            this.gui = gui;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            YACE.GameVue gameVue = this.gui.interop.GameVue;

            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.Pen pen = new Pen(myBrush, 5);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            System.Drawing.Font font = new Font("Arial", 14);
            myBrush.Color = Color.Black;
            string currentPlayerLabel = string.Format("Current player {0}" , gameVue.currentPlayer);
            formGraphics.DrawString(currentPlayerLabel, font, myBrush, 10, 10);

            Point point = new Point(10,40);

            this.PaintCard(gameVue.Zones[0].Cards[0], point, formGraphics, font);

            myBrush.Dispose();
            formGraphics.Dispose();

            this.DrawButton(scoreButton);
            this.DrawButton(multiplyButton);
        }

        private void PaintCard(YACE.GameVue.CardVue cardVue, Point position, Graphics graphics, Font font)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.Pen pen = new Pen(myBrush, 2);

            int cardWidth = 150;
            int lineSpacing = 20;

            int cardHeight = Math.Min(100, (cardVue.Tags.Length + 2) * lineSpacing);

            graphics.DrawRectangle(pen, position.X, position.Y, cardWidth, cardHeight);

            if (!string.IsNullOrEmpty(cardVue.DefinitionName))
            {
                graphics.DrawString(cardVue.DefinitionName, font, myBrush, position);
                position.Y += lineSpacing;
            }

            for (int tagIndex = 0; tagIndex < cardVue.Tags.Length; ++tagIndex)
            {

                string label;
                if (cardVue.TagValues[tagIndex] != int.MinValue)
                {
                    label = string.Format("{0} : {1}", cardVue.Tags[tagIndex], cardVue.TagValues[tagIndex]);
                }
                else
                {
                    label = string.Format("{0}", cardVue.Tags[tagIndex]);
                }

                graphics.DrawString(label, font, myBrush, position);
                position.Y += lineSpacing;
            }
        }
        

        private void DrawButton(Button button)
        {
            ControlPaint.DrawButton(System.Drawing.Graphics.FromHwnd(button.Handle), button.Location.X, button.Location.Y, button.Width, button.Height, ButtonState.Normal);
        }

        private void ScoreButtonClicked(object sender, System.EventArgs e)
        {
            this.gui.interop.PostOrder(new Order_Score());
        }

        private void MultiplyButtonClicked(object sender, System.EventArgs e)
        {
            this.gui.interop.PostOrder(new Order_Multiply());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.gui.interop.PostOrder(new Order_Terminate());
        }
    }
}
