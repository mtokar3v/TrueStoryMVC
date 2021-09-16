using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TrueStoryMVC.Data;

namespace TrueStoryMVC.Models
{
    public class Picture : IPicture
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        private byte[] data;
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                const int DefaultWidth = 600;
                using (var reader = new MemoryStream(value))
                {
                    using (Image image = Image.FromStream(reader))
                    {

                        int w = image.Width;
                        int h = image.Height;

                        double k = (double)h / w;
                        Console.WriteLine(k);
                        w = DefaultWidth;
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
                                Console.WriteLine(data);
                            }
                        }
                    }
                }
            }
        }
        public int PostId { get; set; }
    }
}
