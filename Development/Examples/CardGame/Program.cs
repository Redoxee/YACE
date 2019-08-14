namespace CardGame
{
    using System.Windows.Forms;
    using System.Drawing;

    class Program
    {
        static void Main(string[] args)
        {
            //GUIHandler gUIHandler = new GUIHandler();

            //gUIHandler.Start();

            //while (true)
            //{
            //}
            Bataille game = new Bataille();
            game.Main();
        }

        class GUIHandler
        {
            public void Start()
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.Main));
                thread.Start();
            }

            public void Main()
            {
                // Create a new instance of the form.
                Form gameForm = new GameForm();
                gameForm.Show();
                Application.Run();
            }
        }

        private class GameForm : Form
        {
            public GameForm() : base()
            {
                this.Width = 1024;
                this.Height = 720;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                System.Drawing.Pen pen = new Pen(myBrush, 5);
                System.Drawing.Graphics formGraphics;
                formGraphics = this.CreateGraphics();
                formGraphics.DrawRectangle(pen, new Rectangle(0, 0, 200, 300));
                System.Drawing.Font font = new Font("Arial", 14);
                myBrush.Color = Color.Black;
                formGraphics.DrawString("This is a string", font, myBrush, 10, 10);
                myBrush.Dispose();
                formGraphics.Dispose();
            }
        }
    }
}
