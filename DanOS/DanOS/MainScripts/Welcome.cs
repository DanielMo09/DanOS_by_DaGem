using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanOS.Libraries.Graphics;
using DanOS.Libraries.Formats;
using sys = Cosmos.System;

namespace DanOS.MainScripts
{
    public class Welcome
    {
        #region variables
        private Bitmap screen;
        #endregion
        public Welcome(Canvas canvas, int sizeX, int sizeY, byte[] pic, string name, int fontsize)
        {
            screen = new Bitmap(pic);
            screen.Resize(sizeX, sizeY);
            canvas.Clear();
            canvas.DrawBitmap(0, 0, screen);
            
        }
    }
}
