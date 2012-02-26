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

    }
}
