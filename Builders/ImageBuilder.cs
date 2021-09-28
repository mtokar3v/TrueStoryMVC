using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TrueStoryMVC.Models;

namespace TrueStoryMVC.Builders
{
    public abstract class ImageBuilder
    {
        public abstract void SetData(byte[] value);
        public abstract Img GetResult();
    }

    public class RectImageBuilder : ImageBuilder
    {
        private Img img = new Img();
        public override void SetData(byte[] value)
        {
            using (var reader = new MemoryStream(value))
            {
                using (Image image = Image.FromStream(reader))
                {
                    const int _defaultWidth = 600;

                    int w = image.Width;
                    int h = image.Height;

                    double k = (double)h / w;

                    w = _defaultWidth;
                    h = (int)(w * k);

                    img.Width = w;
                    img.Height = h;

                    using (Bitmap b = new Bitmap(image, w, h))
                    {
                        using (var reader2 = new MemoryStream())
                        {
                            b.Save(reader2, ImageFormat.Jpeg);
                            img.Data = reader2.ToArray();
                        }
                    }
                }
            }
        }

        public override Img GetResult() => img;
    }

    public class SquareImageBuilder : ImageBuilder
    {
        private Img img = new Img();
        public override void SetData(byte[] value)
        {
            using (var reader = new MemoryStream(value))
            {
                using (Image image = Image.FromStream(reader))
                {
                    const int _defaultSize = 250;

                    int w = image.Width;
                    int h = image.Height;

                    double k;

                    if(w<h)
                    {
                        k = (double)h / w;
                        w = _defaultSize;
                        h = (int)(w * k);
                    }
                    else
                    {
                        k = (double)w / h;
                        h = _defaultSize;
                        w = (int)(h * k);
                    }


                    using (Bitmap b = new Bitmap(image, w, h))
                    {
                        int delta = Math.Abs(h - w) / 2;

                        Rectangle cropCenterOfImage = w < h ? new Rectangle(0, delta, _defaultSize, _defaultSize) : new Rectangle(delta, 0, _defaultSize, _defaultSize);

                        using (Bitmap rect_b = b.Clone(cropCenterOfImage, b.PixelFormat))
                        {
                            img.Height = img.Width = _defaultSize;

                            using (var reader2 = new MemoryStream())
                            {
                                rect_b.Save(reader2, ImageFormat.Jpeg);
                                img.Data = reader2.ToArray();
                            }
                        }
                    }
                }
            }
        }

        public override Img GetResult() => img;
    }

    //public class ImageConstructor
    //{
    //    private ImageBuilder _builder;
    //    public ImageConstructor(ImageBuilder builder)
    //    {
    //        _builder = builder;
    //    }

    //    public void ConstructImage(byte[] value)
    //    {
    //        _builder.SetData(value);
    //    }
    //}
}
