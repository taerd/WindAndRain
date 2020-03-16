using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace WindAndRain
{
    class Animator
    {
        private Graphics mainG;
        private int width, height;
        private Brush b;
        private List<Drop> drops = new List<Drop>();
        private Thread t;
        private bool stop = false;
        private BufferedGraphics bg;
        private Pen pen = new Pen(Color.Black);
        public int Lvl { get; set; }//speed of drawing and moving drop
        public Animator(Graphics g,Rectangle r)
        {
            Update(g, r);
            b = new SolidBrush(Drop.color);
        }
        public void Update(Graphics g,Rectangle r)
        {
            mainG = g;
            width = r.Width;
            height = r.Height;
            bg = BufferedGraphicsManager.Current.Allocate(mainG, new Rectangle(0, 0, width, height));
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Update(r);
            }
            Monitor.Exit(drops);
        }
        private void Animate()
        {
            while (!stop)
            {
                Graphics g = bg.Graphics;
                g.Clear(Color.White);
                DrawTank();
                Monitor.Enter(drops);
                int cnt = drops.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (!drops[i].IsAlive) drops.Remove(drops[i]);
                    i--;
                    cnt--;
                }
                foreach (var d in drops)
                {
                    g.FillEllipse(b, d.X, d.Y, d.DropD, d.DropD);
                    //можно добавить обводку
                }
                Monitor.Exit(drops);
                try
                {
                    bg.Render();
                }
                catch (Exception e) { }
                Thread.Sleep(Lvl);
            }
        }
        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Animate);
                t = new Thread(th);
                t.Start();
            }
            var rect = new Rectangle(0, 0, width, height);
            Drop.speed = Lvl;
            Drop d = new Drop(rect);
            d.Start();
            Monitor.Enter(drops);
            drops.Add(d);
            Monitor.Exit(drops);
        }
        public void Stop()
        {
            stop = true;
            Monitor.Enter(drops);
            foreach (var b in drops)
            {
                b.Stop();
            }
            drops.Clear();
            Monitor.Exit(drops);
        } 
        private void DrawTank()
        {
            Graphics g = bg.Graphics;
            //фиксить заливку
           // g.FillEllipse(b, Tank.X, Tank.Y + (height - Tank.Y) * (1 - (Tank.Count / Tank.MaxCount)), 5, 5);
            g.FillRectangle(b, Tank.X, height - (Tank.Y+(height-Tank.Y)*(Tank.MaxCount - Tank.Count)/Tank.MaxCount), 10,Tank.Count*5);
            //g.FillRectangle(b, Tank.X, Tank.Y - ((Tank.Y - height) * (Tank.Count/Tank.MaxCount)), 10, ((height-Tank.Y)-Tank.Count*2));
            g.DrawRectangle(pen, Tank.X, Tank.Y, 10, (int)(height - Tank.Y));
        }
    }
}
