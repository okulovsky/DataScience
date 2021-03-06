﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    public static class Colors
    {
        public static string ToHtml(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static IEnumerable<Color> Grayscale(int steps, double from=0, double to=1 )
        {
            var delta = (to-from) / (steps - 1);

            for (int i=0;i<steps;i++)
            {
                var k = (int)(from + steps * i);
                yield return Color.FromArgb(k, k, k);
            }
        }

     

        public static IEnumerable<Color> Gradient(int steps, Color from, Color to)
        {
            var deltaR = (double)(to.R - from.R) / (steps-1);
            var deltaG = (double)(to.G - from.G) / (steps - 1);
            var deltaB = (double)(to.B - from.B) / (steps - 1);
            for (int i = 0; i < steps; i++)
                yield return Color.FromArgb(
                    (int)(from.R + deltaR * i),
                    (int)(from.G + deltaG * i),
                    (int)(from.B + deltaB * i));
        }

        public static IEnumerable<Color> Rainbow(int steps, double s=0.5, double l=0.5)
        {
            var delta = 360.0 / steps;
            for (int i = 0; i < steps; i++)
                yield return HSB.ToColor(i * delta, s, l);
        }
    }


    public static class HSB
    {

        public static Color ToColor(double h, double s, double v)
        {
            byte r, g, b;
            HsvToRgb(h, s, v, out r, out g, out b);
            return Color.FromArgb(r, g, b);
        }

        public static string ToHTML(double h, double s, double v)
        {
            byte r, g, b;
            HsvToRgb(h, s, v, out r, out g, out b);
            return "#" + r.ToString("X2") + b.ToString("X2") + g.ToString("X2");
        }
 
        // <summary>
        /// Convert HSV to RGB
        /// h is from 0-360
        /// s,v values are 0-1
        /// r,g,b values are 0-255
        /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
        /// </summary>
        static void HsvToRgb(double h, double S, double V, out byte r, out byte g, out byte b)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        static byte Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return (byte)i;
        }
    }
}
