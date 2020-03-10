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
            a.Lvl = 30;
        }

        private void Move()
        {
            while (!stop)
            {
                Thread.Sleep(20);
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
        }

        private void panelMain_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Start();
            }
            else
            {
                Stop();
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
            a.Stop();
        }
    }
}
