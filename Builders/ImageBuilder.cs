using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TrueStoryMVC.Models;

namespace TrueStoryMVC.Builders
{
    public class ImageBuilder
    {
        private readonly Img _image;

        public ImageBuilder()
        {
            _image = new Img();
        }

        public Img Build() => _image;

        public ImageBuilder CreateRectangleImage(byte[] value)
        {
            if (value == null) throw new Exception("Invalid image data");

            using var reader = new MemoryStream(value);
            using var image = Image.FromStream(reader);
                
            //real value
            var width = image.Width;
            var height = image.Height;
            var coefficient = (double)height / width;

            //standart value
            const int _defaultWidth = 600;
            width = _defaultWidth;
            height = (int)(width * coefficient);

            _image.Width = width;
            _image.Height = height;

            using var bitmap = new Bitmap(image, width, height);
            
            bitmap.Save(reader, ImageFormat.Jpeg);
            _image.Data = reader.ToArray();

            return this;
        }

        public ImageBuilder CreateSquareImage(byte[] value)
        {
            if (value == null) throw new Exception("Invalid image data");

            using var reader = new MemoryStream(value);
            using var image = Image.FromStream(reader);

            //real value
            var width = image.Width;
            var height = image.Height;

            //standart value
            const int _defaultSize = 250;

            if (height > width)
            {
                CropImageToSquare(ref height, ref width, _defaultSize);
            }
            else
            {
                CropImageToSquare(ref width, ref height, _defaultSize);
            }

            _image.Width = width;
            _image.Height = height;

            var delta = Math.Abs(height - width) / 2;
            var cropCenterOfImage = width < height ? 
                new Rectangle(0, delta, _defaultSize, _defaultSize) : 
                new Rectangle(delta, 0, _defaultSize, _defaultSize);

            using Bitmap bitmap = new Bitmap(image, width, height) ;
            using Bitmap rect_b = bitmap.Clone(cropCenterOfImage, bitmap.PixelFormat);

            _image.Height = _image.Width = _defaultSize;
            rect_b.Save(reader, ImageFormat.Jpeg);

            _image.Data = reader.ToArray();

            return this;
        }

        private void CropImageToSquare(ref int greatestSide, ref int smallestSide, int defaultSize)
        {
            var coefficient = (double)greatestSide / smallestSide;
            smallestSide = defaultSize;
            greatestSide = (int)(smallestSide * coefficient);
        }
    }
}
