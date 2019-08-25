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

            myBrush.Dispose();
            formGraphics.Dispose();

            this.DrawButton(scoreButton);
            this.DrawButton(multiplyButton);

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
