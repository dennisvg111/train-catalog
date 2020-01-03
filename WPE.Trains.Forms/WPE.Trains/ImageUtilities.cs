using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public static class ImageUtilities
    {
        public static AspectRatio GetAspectRatio(Image image, int limit = 50)
        {
            var width = (double)image.Width;
            var height = (double)image.Height;
            var val = width / height;
            var lim = (double)limit;

            double[] lower = new double[] { 0, 1 };
            double[] upper = new double[] { 1, 0 };

            while (true)
            {
                var mediant = new double[] { lower[0] + upper[0], lower[1] + upper[1] };

                if (val * mediant[1] > mediant[0])
                {
                    if (lim < mediant[1])
                    {
                        return new AspectRatio(upper[0], upper[1]);
                    }
                    lower = mediant;
                }
                else if (val * mediant[1] == mediant[0])
                {
                    if (lim >= mediant[1])
                    {
                        return new AspectRatio(mediant[0], mediant[1]);
                    }
                    if (lower[1] < upper[1])
                    {
                        return new AspectRatio(lower[0], lower[1]);
                    }
                    return new AspectRatio(upper[0], upper[1]);
                }
                else
                {
                    if (lim < mediant[1])
                    {
                        return new AspectRatio(lower[0], lower[1]);
                    }
                    upper = mediant;
                }
            }
        }

        public class AspectRatio
        {
            public int Horizontal { get; set; }
            public int Vertical { get; set; }

            internal AspectRatio(double h, double v)
            {
                Horizontal = Convert.ToInt32(h);
                Vertical = Convert.ToInt32(v);
            }

            public override string ToString()
            {
                return Horizontal + ":" + Vertical;
            }
        }
    }
}
