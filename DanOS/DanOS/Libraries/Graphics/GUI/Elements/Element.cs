using static DanOS.Libraries.Graphics.GUI.WindowManager;

namespace DanOS.Libraries.Graphics.GUI.Elements
{
    public abstract class Element
    {
        public delegate void Event(ref Element This, ref Window Parent);
        public int X, Y, Width, Height, Radius;
        public Event OnClick, OnUpdate;
        public bool Clicked, Hovering, Visible = true;

        public abstract void Update(Canvas canvas, Window Parent);
    }
}