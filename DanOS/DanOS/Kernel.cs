using DanOS.Libraries.Graphics;
using DanOS.Libraries.Formats;
using sound = DanOS.Libraries.Sound;
using System;
using Sys = Cosmos.System;
using Cosmos.HAL;
using IL2CPU.API.Attribs;
using System.Security.Cryptography;
using fs = Cosmos.System.FileSystem;
using System.Collections.Generic;

namespace DanOS
{
    public class Kernel : Sys.Kernel
    {
        [ManifestResourceStream(ResourceName = "DanOS.Resources.bg.bmp")]
        public static byte[] welcomescreen;
        [ManifestResourceStream(ResourceName = "DanOS.Resources.Cursor.bmp")]
        public static byte[] CursorIcon;
        [ManifestResourceStream(ResourceName = "DanOS.Resources.bg.bmp")]
        public static byte[] BgImage;
        [ManifestResourceStream(ResourceName = "DanOS.Resources.MainMenu.bmp")]
        public static byte[] MMImage;
        [ManifestResourceStream(ResourceName = "DanOS.Resources.power.bmp")]
        public static byte[] PowerIcon;
        [ManifestResourceStream(ResourceName = "DanOS.Resources.restart.bmp")]
        public static byte[] RestartIcon;
        [ManifestResourceStream(ResourceName = "DanOS.Resources.startup.wav")]
        public static byte[] startupbyte;
        [ManifestResourceStream(ResourceName = "DanOS.Resources.shutdown.wav")]
        public static byte[] shutdownbyte;
        //public static List<Window> windows;
        public static Bitmap cursor;
        public static Bitmap bg;
        public static Bitmap MM;
        public static Bitmap power;
        public static Bitmap restart;
        public static Canvas canvas;
        public static WAVFile startupwav;
        public static WAVFile shutdownwav;
        public static int mouseSize = 1;
        public static bool menuOpen = false;
        public static int X;
        public static int Y;
        public int a;
        public Bitmap screen;
        public void Welcome( int sizeX, int sizeY, byte[] pic, string name, int fontsize, string FullNameData = null)
        {
            screen = new Bitmap(pic);
            screen.Resize(sizeX, sizeY);
            canvas.Clear();
            canvas.DrawBitmap(0, 0, screen);
            FullNameData = "Welcome, " + name;
            a = FullNameData.Length / 2;
            canvas.DrawString(canvas.Width / 2 - a, canvas.Height / 2 - fontsize / 2, name, Color.Black);
        }
        protected override void BeforeRun()
        {
            canvas = new Canvas(800, 600);
            cursor = new Bitmap(CursorIcon);
            Welcome(canvas.Width, canvas.Height, welcomescreen, "test", Canvas.Font.Default.Height);
            System.Threading.Thread.Sleep(3);
            bg = new Bitmap(BgImage);
            MM = new Bitmap(MMImage);
            startupwav = new WAVFile(startupbyte);
            shutdownwav = new WAVFile(shutdownbyte);
            power = new Bitmap(PowerIcon);
            restart = new Bitmap(RestartIcon);
            power.Resize(50, 50);
            restart.Resize(50, 50);
            bg.Resize(canvas.Width, canvas.Height);
            cursor.Resize(595/60*mouseSize, 961/60*mouseSize);
            Sys.MouseManager.ScreenWidth = (uint)canvas.Width;
            Sys.MouseManager.ScreenHeight = (uint)canvas.Height;
            sound.PCSpeaker.Play(startupwav, 0);
            //windows = new List<Window>() {
                //new Window(canvas, 300, 275, 10)
            //};
        }
        protected override void Run()
        {
            canvas.Clear();
            canvas.DrawBitmap(0, 0, bg);
            if (Sys.MouseManager.MouseState == Sys.MouseState.Left && Sys.MouseManager.X > 10 && Sys.MouseManager.X < 50 && Sys.MouseManager.Y > canvas.Height - 40 && Sys.MouseManager.Y < canvas.Height && menuOpen == false)
            {
                menuOpen = true;
                while (Sys.MouseManager.MouseState == Sys.MouseState.Left)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
            else if (Sys.MouseManager.MouseState == Sys.MouseState.Left && Sys.MouseManager.X > 10 && Sys.MouseManager.X < 50 && Sys.MouseManager.Y > canvas.Height - 40 && Sys.MouseManager.Y < canvas.Height && menuOpen == true)
            {
                menuOpen = false;
                while (Sys.MouseManager.MouseState == Sys.MouseState.Left)
                {
                    canvas.Clear();
                    canvas.DrawBitmap(0, 0, bg);
                    canvas.DrawString(0, 0, canvas.FPS + " FPS", Color.White);
                    canvas.DrawString(canvas.Width - 75, canvas.Height - 20, $"{RTC.Hour % 12}:{RTC.Minute.ToString("00")} {((RTC.Hour > 12) ? "PM" : "AM")}", Color.White);
                    canvas.DrawString(canvas.Width - 85, canvas.Height - 40, $"{RTC.Month}.{RTC.DayOfTheMonth}.{RTC.Century}{RTC.Year}", Color.White);
                    //foreach (Window window in windows)
                    //window.Draw();
                    canvas.DrawFilledRectangle(0, canvas.Height - 50, canvas.Width, canvas.Height, 0, new Color(168, 108, 108, 108));
                    canvas.DrawFilledRectangle(10, canvas.Height - 40, 40, 40, 0, new Color(100, 255, 255, 255));
                    if (menuOpen)
                    {
                        canvas.DrawFilledRectangle(0, canvas.Height - 300, 200, 250, 0, new Color(255, 115, 115, 155));
                        canvas.DrawFilledRectangle(0, canvas.Height - 300, 50, 50, 0, Color.Red);
                        canvas.DrawBitmap(0, canvas.Height - 300, power);
                        canvas.DrawFilledRectangle(0, canvas.Height - 250, 50, 50, 0, Color.DeepOrange);
                        canvas.DrawBitmap(0, canvas.Height - 250, restart);
                    }
                    canvas.DrawBitmap(10, canvas.Height - 40, MM);
                    canvas.DrawBitmap((int)Sys.MouseManager.X, (int)Sys.MouseManager.Y, cursor);
                    X = (int)Sys.MouseManager.X;
                    Y = (int)Sys.MouseManager.Y;
                    canvas.Update();
                    System.Threading.Thread.Sleep(1);
                }
            }
            if (Sys.MouseManager.MouseState == Sys.MouseState.Left && Sys.MouseManager.X > 0 && Sys.MouseManager.X < 50 && Sys.MouseManager.Y > canvas.Height - 300 && Sys.MouseManager.Y < canvas.Height - 250) Sys.Power.Shutdown();
            if (Sys.MouseManager.MouseState == Sys.MouseState.Left && Sys.MouseManager.X > 0 && Sys.MouseManager.X < 50 && Sys.MouseManager.Y > canvas.Height - 250 && Sys.MouseManager.Y < canvas.Height - 200) Sys.Power.Reboot();
            canvas.DrawString(0, 0, canvas.FPS + " FPS", Color.White);
            canvas.DrawString(canvas.Width - 75, canvas.Height - 20, $"{RTC.Hour % 12}:{RTC.Minute.ToString("00")} {((RTC.Hour > 12) ? "PM" : "AM")}", Color.White);
            canvas.DrawString(canvas.Width - 85, canvas.Height - 40, $"{RTC.Month}.{RTC.DayOfTheMonth}.{RTC.Century}{RTC.Year}", Color.White);
            //foreach (Window window in windows)
            //window.Draw();
            canvas.DrawFilledRectangle(0, canvas.Height - 50, canvas.Width, canvas.Height, 0, new Color(168, 108, 108, 108));
            canvas.DrawFilledRectangle(10, canvas.Height - 40, 40, 40, 0, new Color(100, 255, 255, 255));
            if (menuOpen)
            {
                canvas.DrawFilledRectangle(0, canvas.Height - 300, 200, 250, 0, new Color(255, 115, 115, 155));
                canvas.DrawFilledRectangle(0, canvas.Height - 300, 50, 50, 0, Color.Red);
                canvas.DrawBitmap(0, canvas.Height - 300, power);
                canvas.DrawFilledRectangle(0, canvas.Height - 250, 50, 50, 0, Color.DeepOrange);
                canvas.DrawBitmap(0, canvas.Height - 250, restart);
            }
            canvas.DrawBitmap(10, canvas.Height - 40, MM);
            canvas.DrawBitmap((int)Sys.MouseManager.X, (int)Sys.MouseManager.Y, cursor);
            X = (int)Sys.MouseManager.X;
            Y = (int)Sys.MouseManager.Y;
            //foreach (Window window in windows)
            //window.Update();
            canvas.Update();
        }
    }
}