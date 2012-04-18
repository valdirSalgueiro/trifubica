using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ColorLand
{
    //Manual Timer
    public class MTimer
    {

        private double mCurrentTime;
        private const int cINITIAL_TIME = 0;

        private bool mActive;

        private double mBusyNumber;
        private bool mBusy;

        public MTimer()
        {

        }

        public MTimer(bool alreadyStarted)
        {
            if (alreadyStarted)
            {
                start();
            }
        }

        public void update(GameTime gameTime)
        {
            if (mActive)
            {
                mCurrentTime += gameTime.ElapsedGameTime.TotalSeconds;//= cINITIAL_TIME + gameTime.ElapsedGameTime.TotalGameTime.Seconds;
            }
        }

        public double getTime()
        {
            return mCurrentTime;
        }

        public int getTimeInt()
        {
            return (int)mCurrentTime;
        }

        public bool getTimeAndLock(int number){
            if (mActive)
            {
                int time = getTimeInt();
                if (!isBusyForNumber(number) && time == number)
                {
                    setBusyWithNumber(number);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool getTimeAndLock(double number)
        {
            if (mActive)
            {
                double time = ExtraFunctions.trimDouble(getTime(),1);

                //Game1.print("TIME: " + time);
                if (!isBusyForNumber(number) && time == number)
                {
                    setBusyWithNumber(number);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        
        public void start()
        {
            mActive = true;
            mCurrentTime = 0;
        }

        public void stop()
        {
            mCurrentTime = 0;
            mActive = false;
        }

        public void resume()
        {
            mActive = true;
        }

        public void pause()
        {
            mActive = false;
        }

        public bool isActive()
        {
            return this.mActive;
        }

        public bool isBusyForNumber(int num)
        {
            if (num == mBusyNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setBusyWithNumber(int num)
        {
            this.mBusyNumber = num;
        }

        public bool isBusyForNumber(double num)
        {
            if (num == mBusyNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setBusyWithNumber(double num)
        {
            this.mBusyNumber = num;
        }
        
    }
}
