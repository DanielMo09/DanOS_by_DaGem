using static DanOS.Libraries.Graphics.GUI.WindowManager;

namespace DanOS.Libraries.Graphics.GUI.Elements
{
    public class Label : Element
    {
        public string Text;
        public Color Color;

        public override void Update(Canvas Canvas, Window Parent)
        {
            if (Visible && Parent.Visible)
            {
                Canvas.DrawString(Parent.X + X, Parent.Y + Y, Text, Color);
            }
        }
    }
}