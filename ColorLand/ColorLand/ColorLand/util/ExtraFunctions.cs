using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


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

            x = value * 100 / range;

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

        /* Adaptado pro formato do Blender (4 digitos)*/
        public static String[] fillArrayWithImages2(int length, String baseName)
        {
            String[] a = new String[length];

            for (int x = 0; x < length; x++)
            {
                String zeros = "";
                if (x < 10 - 1) zeros = "000";
                else
                if (x < 100 - 1) zeros = "00";
                else
                if (x < 1000 - 1) zeros = "0";
                
                a[x] = baseName + zeros + "" + (x + 1);
            }

            return a;
        }

        public static String[] fillArrayWithImages2(int from, int length, String baseName)
        {
            
            String[] a = new String[length];

            for (int x = from, i = 0; x < from + length; x++, i++)
            {
                String zeros = "";
                if (x < 10) zeros = "000";
                else
                if (x < 100 - 1) zeros = "00";
                else
                if (x < 1000 - 1) zeros = "0";
                //Game1.print("carreguei " + baseName + zeros + "" + (x));
                a[i] = baseName + zeros + "" + (x);
            }

            return a;
        }

        //Trim double v to n decimal places
        public static double trimDouble(double number, double decimalsWanted)
        {
            double p = Math.Pow(10, decimalsWanted);
            return (Math.Round(number * p)) / p;
        }


        public static void saveProgress(ProgressObject po)
        {
            ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, po); 
        }

        //TODO saveLevelFirstTime

        public static ProgressObject loadProgress()
        {
            return ObjectSerialization.Load<ProgressObject>(Game1.sPROGRESS_FILE_NAME);
        }

        public static Color convertSavedColorToColor()
        {
            ProgressObject.PlayerColor p = ExtraFunctions.loadProgress().getColor();

            if(p == ProgressObject.PlayerColor.RED){
                return Color.Red;
            }else
            if (p == ProgressObject.PlayerColor.GREEN)
            {
                return Color.Green;
            }else
            {
                return Color.Blue;
            }
        }

        /*private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            mTimer.Stop();
            mTimer.Enabled = false;

            switch (mGameState)
            {
                case GAME_STATE_PREPARANDO:

                    if (mFlagTimer == FLAG_TIMER_PREPARANDO_WAIT_BEFORE_START)
                    {
                        this.setGameState(GAME_STATE_EM_JOGO);
                    }

                    break;
            }

            
        }

        private void restartTimer(int seconds)
        {
            mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            mTimer.Interval = seconds * 1000;
            mTimer.Enabled = true;
        }*/

    }
}
