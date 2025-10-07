using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Info_Projektkurs1
{
    public class ImageData
    {
        private Image<Bgr, byte> _image;

        public int Width => _image.Width;

        public int Height => _image.Height;

        public ImageData(Mat pFrame)
        {
            _image = pFrame.ToImage<Bgr, byte>();
        }

        //gives Rgb instead of Brg values
        //(f(x, y) = (r,g,b))
        public (double R, double G, double B) GetPixel(int x, int y)
        {
            // In Emgu its BRG not RGB
            var color = _image[y, x]; // Type: Bgr
            double r = color.Red;
            double g = color.Green;
            double b = color.Blue;
            return (r, g, b);
        }
    }
}
