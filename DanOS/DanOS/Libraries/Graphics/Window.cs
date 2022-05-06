using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using sys = Cosmos.System;
namespace DanOS.Libraries.Graphics
{
    public class Window
    {
        public int X;
        public int Y;
        int Width;
        int Height;
        int offsetX;
        int offsetY;
        bool dragging;
        private Canvas canvas;
        public Window(Canvas canvas, int Width, int Height, int X = default, int Y = default)
        {
            this.Width = Width;
            this.Height = Height;
            this.canvas = canvas;
            if (X == default) this.X = (canvas.Width / 2) - (Width / 2); else this.X = X;
            if (Y == default) this.Y = (canvas.Height / 2) - (Height / 2); else this.Y = Y;
        }
        public void Update()
        {
            if (sys.MouseManager.MouseState == sys.MouseState.Left && sys.MouseManager.X > X && sys.MouseManager.X < X + Width && sys.MouseManager.Y > Y && sys.MouseManager.Y < Y + 20)
            {
                offsetX = (int)sys.MouseManager.X - X;
                offsetY = (int)sys.MouseManager.Y - Y;
                if (!dragging) dragging = true;
            } else
            {
                if (dragging) dragging = false;
            }
            if (dragging)
            {
                X = (int)sys.MouseManager.X + offsetX;
                Y = (int)sys.MouseManager.Y + offsetY;
            }
        }
        public void Draw()
        {
            canvas.DrawFilledRectangle(X, Y, Width, Height, 0, Color.White);
            canvas.DrawFilledRectangle(X, Y - 20, Width, 20, 0, Color.Blue);
            canvas.DrawRectangle(X - 1, Y - 21, Width + 1, Height + 21, 0, Color.LightGray);
        }
    }
}
