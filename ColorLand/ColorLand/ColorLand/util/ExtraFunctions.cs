using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ColorLand
{
    public class ExtraFunctions
    {

        public static float percentToValue(int percent, int range)
        {

            float x;

            x = percent * range / 100;

            return x;

        }

        public static float valueToPercent(int value, int range)
        {

            float x;

            x = value * range / 100;

            return x;

        }

        public static String[] fillArrayWithImages(int length, String baseName)
        {
            String[] a = new String[length];

            for (int x = 0; x < length; x++)
            {
                a[x] = baseName + "" + (x+1);
            }

            return a;
        }

        //Trim double v to n decimal places
        public static double trimDouble(double number, double decimalsWanted)
        {
            double p = Math.Pow(10, decimalsWanted);
            return (Math.Round(number * p)) / p;
        }

    }
}
