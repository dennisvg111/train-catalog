using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public class AspectRatio
    {
        private readonly int horizontal;
        private readonly int vertical;
        private readonly double ratio;

        public double CalculatedRatio { get { return (double)Horizontal / (double)Vertical; } }
        public double OriginalRatio { get { return ratio; } }
        public int Horizontal { get { return horizontal; } }
        public int Vertical { get { return vertical; } }
        public bool Exact { get { return Math.Abs(OriginalRatio - CalculatedRatio) < 0.00001; } }

        private AspectRatio(double h, double v, double r)
        {
            this.horizontal = Convert.ToInt32(h);
            this.vertical = Convert.ToInt32(v);
            this.ratio = r;
        }

        public override string ToString()
        {
            return Horizontal + ":" + Vertical;
        }

        public static AspectRatio FromImage(Image image, int limit = 50)
        {
            var width = (double)image.Width;
            var height = (double)image.Height;
            return FromSize(width, height, limit);
        }

        public static AspectRatio FromSize(int width, int height, int limit = 50)
        {
            return FromSize((double)width, (double)height, limit);
        }

        public static AspectRatio FromSize(double width, double height, int limit = 50)
        {
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
                        return new AspectRatio(upper[0], upper[1], val);
                    }
                    lower = mediant;
                }
                else if (val * mediant[1] == mediant[0])
                {
                    if (lim >= mediant[1])
                    {
                        return new AspectRatio(mediant[0], mediant[1], val);
                    }
                    if (lower[1] < upper[1])
                    {
                        return new AspectRatio(lower[0], lower[1], val);
                    }
                    return new AspectRatio(upper[0], upper[1], val);
                }
                else
                {
                    if (lim < mediant[1])
                    {
                        return new AspectRatio(lower[0], lower[1], val);
                    }
                    upper = mediant;
                }
            }
        }
    }
}
