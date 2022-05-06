﻿using static DanOS.Libraries.Graphics.GUI.WindowManager;

namespace DanOS.Libraries.Graphics.GUI.Elements
{
    public class Panel : Element
    {
        public Color Color;

        public override void Update(Canvas Canvas, Window Parent)
        {
            if (Visible && Parent.Visible)
            {
                Canvas.DrawFilledRectangle(Parent.X + X, Parent.Y + Y, Width, Height, Radius, Color);
            }
        }
    }
}