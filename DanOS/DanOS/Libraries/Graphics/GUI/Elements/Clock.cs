﻿using static DanOS.Libraries.Graphics.GUI.WindowManager;
using System;

namespace DanOS.Libraries.Graphics.GUI.Elements
{
    public class Clock : Element
    {
        public DateTime Time;

        public override void Update(Canvas Canvas, Window Parent)
        {
            if (Visible && Parent.Visible)
            {
                Canvas.DrawFilledCircle(Parent.X + X, Parent.Y + Y, Radius, Color.White);
                Canvas.DrawAngledLine(Parent.X + X, Parent.Y + Y, DateTime.Now.Hour * 30, Radius - 45, Color.Black);
                Canvas.DrawAngledLine(Parent.X + X, Parent.Y + Y, DateTime.Now.Minute * 6, Radius - 25, Color.Black);
                Canvas.DrawAngledLine(Parent.X + X, Parent.Y + Y, DateTime.Now.Second * 6, Radius - 5, Color.Red);
            }
        }
    }
}