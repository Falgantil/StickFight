using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace StickFight
{
    public class StatusBar
    {
        private Thread _thread;

        public bool LeftToRight { get; private set; }

        public Point Location { get; set; }

        public int Width { get; set; }

        public StatusBar(Point location, int width, bool leftToRight)
        {
            Location = location;
            Width = width;
            LeftToRight = leftToRight;
        }

        public void DrawText(string text, TimeSpan duration)
        {
            if (text.Length >= Width)
                throw new Exception("Text length must be smaller than the Width of the statusbar.");

            if (_thread != null)
                _thread.Abort();

            if (LeftToRight)
                Console.SetCursorPosition(Location.X, Location.Y);
            else
                Console.SetCursorPosition(Location.X - Width, Location.Y);

            for (int i = 0; i < Width; i++)
                Console.Write(" ");

            if (LeftToRight)
                Console.SetCursorPosition(Location.X, Location.Y);
            else
                Console.SetCursorPosition(Location.X - text.Length, Location.Y);

            Console.Write(text);

            _thread = new Thread(() =>
            {
                Thread.Sleep(duration);

                if (LeftToRight)
                {
                    Console.SetCursorPosition(Location.X, Location.Y);
                }
                else
                {
                    Console.SetCursorPosition(Location.X - Width, Location.Y);
                }

                for (int i = 0; i < Width; i++)
                    Console.Write(" ");
            });
            _thread.Start();
        }
    }
}