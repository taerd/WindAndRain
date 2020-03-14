using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindAndRain
{
    public partial class Form1 : Form
    {
        private Animator a;
        private bool stop = true;
        private Thread t;
        public Form1()
        {
            InitializeComponent();
            a = new Animator(panelMain.CreateGraphics(), panelMain.ClientRectangle);
            a.Lvl = 30;//speed of drawing //bigger-more slowly //
            Tank.X = (int)panelMain.Width / 2;
            Tank.Y = (int)(panelMain.Height- panelMain.Height / 8);
            Tank.Count = 0;
            Tank.MaxCount = 10;
        }

        private void Move()
        {
            while (!stop)
            {
                Thread.Sleep(150);
                a.Start();
            }
        }

        private void panelMain_Resize(object sender, EventArgs e)
        {
            if(this.WindowState != System.Windows.Forms.FormWindowState.Minimized && a!=null)
            {
                a.Update(panelMain.CreateGraphics(), panelMain.ClientRectangle);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            //
            /*
            MouseEventArgs e1 = new MouseEventArgs(MouseButtons.Right,3,10,10,10);
            panelMain_MouseClick(sender, e1);
            */
            //
        }

        private void panelMain_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                stop = true;
                Stop();
            }
            else if(e.Button == MouseButtons.Left)
            {
                Start();
            }
        }
        private void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Move);
                t = new Thread(th);
                t.Start();
            }
        }
        private void Stop()
        {
            stop = true;
            //a.Stop = true;
            a.Stop();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Drop.dx = trackBar1.Value - 5;
        }   

        private void trackBar1_KeyPress(object sender, KeyPressEventArgs e)
        {
            label1.Text = $"{e.KeyChar}";
            switch (e.KeyChar)
            {
                case ((char)ConsoleKey.A):
                    label1.Text = "A/a pressed";
                    if (Tank.X <= 10)
                    {
                        Tank.X = panelMain.Width - 10;
                    }
                    else
                    {
                        Tank.X -= 2;
                    }
                    break;
                case ((char)ConsoleKey.D):
                    label1.Text = "D/d pressed";
                    if (panelMain.Width - Tank.X <= 10)
                    {
                        Tank.X = 10;
                    }
                    else
                    {
                        Tank.X += 2;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
