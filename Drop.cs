using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindAndRain
{
    class Drop
    {
        private int width, height;
        public static Color color = Color.FromArgb(79, 66, 181);
        public bool IsAlive { get { return t != null && t.IsAlive; }}
        private bool stop = false;
        public int DropD { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        static public int dx { get; set; }
        static public int speed { get; set; }
        private static Random rand = null;
        private Thread t;
        public Drop(Rectangle r)
        {
            Update(r);
            if (rand == null) rand = new Random();
            X = rand.Next(0, width);
            Y = 0;
            //такое потом убрать
            DropD = 10;
        }
        public void Update(Rectangle r)
        {
            width = r.Width;
            height = r.Height;
        }
        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Move);
                t = new Thread(th);
                t.Start();
            }
        }
        private void Move()
        {
            while(!stop && Y < height)
            {
                Thread.Sleep(speed);
                Y += 1;
                if(Math.Abs(X-Tank.X) < DropD && Math.Abs(Y-Tank.Y) < 2)
                {
                    Y = height;
                    Tank.Count += 1;
                    if (Tank.Count == Tank.MaxCount)
                    {
                        Tank.Count = 0;
                        //something do
                    }
                }
                if(dx != 0)
                {
                    X += dx;
                    if (X < 0)
                    {
                        X = width - 1;
                    }
                    if (X > width)
                    {
                        X = 1;
                    }
                }
            }
        }
        public void Stop()
        {
            stop = true;
        }
    }
}
