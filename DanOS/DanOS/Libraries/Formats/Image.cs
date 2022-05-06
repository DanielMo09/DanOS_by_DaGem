﻿using DanOS.Libraries.Graphics;
using System;
using System.IO;
using GC = Cosmos.Core.GCImplementation;

namespace DanOS.Libraries.Formats
{
    public unsafe class Image : IDisposable
    {
        public Image(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            Buffer = new int*[Width * Height];
        }
        public Image(byte[] Binary)
        {
            BinaryReader Reader = new(new MemoryStream(Binary));
            if (Binary == null || Binary.Length == 0)
            {
                throw new FileLoadException("Binary data cannot be null or blank.");
            }
            if (Binary[0] == 'B' && Binary[1] == 'M') // Bitmap file detected
            {
                // Using cosmos to get bitmaps for now
                Cosmos.System.Graphics.Bitmap BMP = new(Binary);
                Width = (int)BMP.Width;
                Height = (int)BMP.Height;
                Buffer = (int*[])(object)BMP.rawData;
                return;
            }
            if (Reader.ReadString() == "RAW")
            {
                Width = Reader.ReadInt32();
                Height = Reader.ReadInt32();
                Buffer = (int*[])(object)Reader.ReadBytes(Width * Height * 4);
                return;
            }
            Reader.Dispose();
        }

        public Color AverageColor
        {
            get
            {
                Color T = new(255, 0, 0, 0);
                for (int I = 0; I < Buffer.Length; I++)
                {
                    Color T2 = new((int)Buffer[I]);
                    T.R += (byte)(T2.R / Buffer.Length);
                    T.G += (byte)(T2.G / Buffer.Length);
                    T.B += (byte)(T2.B / Buffer.Length);
                }
                return T;
            }
        }
        public int Width, Height;
        public int*[] Buffer;

        public byte[] ToBinary()
        {
            BinaryWriter Writer = new(new MemoryStream());
            Writer.Write("RAW");
            Writer.Write(Width);
            Writer.Write(Height);
            Writer.Write((byte[])(object)Buffer);
            return (Writer.BaseStream as MemoryStream).ToArray();
        }

        public void Dispose()
        {
            GC.Free(Buffer);
        }

        #region Effects

        public Image Resize(int Width, int Height)
        {
            if (this == null)
            {
                throw new Exception("Cannot draw a null image file.");
            }
            Image Temp = new(Width, Height);
            for (int IX = 0; IX < this.Width; IX++)
            {
                for (int IY = 0; IY < this.Height; IY++)
                {
                    int X = IX / (this.Width / Width);
                    int Y = IY / (this.Height / Height);
                    Temp.Buffer[(Temp.Width * Y) + X] = Buffer[(this.Width * IY) + IX];
                }
            }
            return Temp;
        }
        public Image Grayscale()
        {
            Image Temp = new(Width, Height);
            for (int I = 0; I < Buffer.Length; I++)
            {
                Color C = new((int)Buffer[I]);
                Temp.Buffer[I] = (int*)new Color(C.R, C.R, C.R).ARGB;
            }
            return Temp;
        }
        public Image Threshhold(byte MinValue, byte MaxValue)
        {
            Image Temp = new(Width, Height);
            for (int I = 0; I < Buffer.Length; I++)
            {
                Color C = new((int)Buffer[I]);
                int T = (C.R / 3) + (C.G / 3) + (C.B / 3);

                if (T < MinValue || T > MaxValue)
                {
                    Temp.Buffer[I] = (int*)Color.Black.ARGB;
                }
            }
            return Temp;
        }
        public Image ShowChanged(Image Comparison)
        {
            Image Temp = new(Width, Height);
            for (int I = 0; I < Buffer.Length; I++)
            {
                if (Buffer[I] != Comparison.Buffer[I])
                {
                    Color C1 = new((int)Buffer[I]);
                    Color C2 = new((int)Comparison.Buffer[I]);

                    byte RDiff = (byte)Math.Abs(C1.R - C2.R);
                    byte GDiff = (byte)Math.Abs(C1.G - C2.G);
                    byte BDiff = (byte)Math.Abs(C1.B - C2.B);

                    Temp.Buffer[I] = (int*)new Color(RDiff, GDiff, BDiff).ARGB;
                }
                else
                {
                    Temp.Buffer[I] = (int*)Color.Black.ARGB;
                }
            }
            return Temp;
        }
        public Image Ghost(Image Comparison)
        {
            Image Temp = new(Width, Height);
            for (int I = 0; I < Buffer.Length; I++)
            {
                Color C1 = new((int)Comparison.Buffer[I]);
                Color C2 = new((int)Buffer[I]);

                byte R = (byte)((C1.R / 2) + (C2.R / 2));
                byte G = (byte)((C1.G / 2) + (C2.G / 2));
                byte B = (byte)((C1.B / 2) + (C2.B / 2));

                Temp.Buffer[I] = (int*)new Color(R, G, B).ARGB;
            }
            return Temp;
        }
        public Image GetNoise()
        {
            Image Temp = new(Width, Height);
            Random Random = new();
            for (int I = 0; I < Buffer.Length; I++)
            {
                Temp.Buffer[I] = (int*)new Color((byte)Random.Next(0, 255), (byte)Random.Next(0, 255), (byte)Random.Next(0, 255)).ARGB;
            }
            return Temp;
        }
        public Image GetSine(Color Color, double Freq = 40.0, double Amp = 0.25)
        {
            Image Temp = new(Width, Height);
            Amp *= Height;
            for (int X = 0; X < Width; X++)
            {
                Temp.Buffer[(Width * ((Height / 5) + ((short)(Amp * Math.Sin(2 * Math.PI * X * Freq / 8000)) / 2))) + X] = (int*)Color.ARGB;
            }
            return Temp;
        }

        #endregion
    }
}