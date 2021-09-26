using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TrueStoryMVC.Models
{
    public interface IImage
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public byte[] Data { get; set; }
    }

    [Owned]
    public class RectImg : IImage
    {
        private byte[] data;
        public int Height { get; set; }
        public int Width { get; set; }
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                using (var reader = new MemoryStream(value))
                {
                    using (Image image = Image.FromStream(reader))
                    {
                        int _defaultWidth = 600;

                        int w = image.Width;
                        int h = image.Height;
                        double k = (double)h / w;

                        w = _defaultWidth;
                        h = (int)(w * k);

                        Width = w;
                        Height = h;

                        using (Bitmap b = new Bitmap(image, w, h))
                        {
                            using (var reader2 = new MemoryStream())
                            {
                                b.Save(reader2, ImageFormat.Jpeg);
                                data = new byte[reader2.ToArray().Length];
                                data = reader2.ToArray();
                            }
                        }
                    }
                }
            }
        }
    }

    [Owned]
    public class SquareImg : IImage
    {
        private byte[] data;
        public int Height { get; set; }
        public int Width { get; set; }
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                using (var reader = new MemoryStream(value))
                {
                    using (Image image = Image.FromStream(reader))
                    {
                        int _defaultWidth = 600;

                        int w = image.Width;
                        int h = image.Height;
                        double k = (double)h / w;

                        w = _defaultWidth;
                        h = _defaultWidth;

                        Width = w;
                        Height = w;

                        using (Bitmap b = new Bitmap(image, w, h))
                        {
                            using (var reader2 = new MemoryStream())
                            {
                                b.Save(reader2, ImageFormat.Jpeg);
                                data = new byte[reader2.ToArray().Length];
                                data = reader2.ToArray();
                            }
                        }
                    }
                }
            }
        }

    }
}
