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
            //se o timer foi iniciado, ele ta mActive==true
            if (mActive)
            {
                //isso aqui, eh que um double vem como 4.52349234923942384923. Essa funcao reduzi a 4.5
                double time = ExtraFunctions.trimDouble(getTime(),1);

                /*!isBusyForNumber(number) ---> o timer alguma vez ja teve o valor igual a NUMBER? Se ja, pode-se dizer que ele ta BUSY pra esse numero...
                    //isso evita, por exemplo, de se eu querer fazer uma acao no timer = 2 segundos. Se alguma vez o timer já foi igual a 2 segundos, ele ja fica BUSY
                    //o que esse metodo diz eh: o numero NUMBER ja foi "passado" alguma vez?
                
                //time == number  ---> o tempo atual do timer eh igual ao numero que foi passado por parametro?                
                 */
                if (!isBusyForNumber(number) && time == number)
                {
                    //se pedi, por exemplo, 3 segundos... ele entrou aqui pq: 3 segundos ainda nao tava "ocupado". Logo, a partir de agora
                      // ele deve ser travado (ou seja, setBusy)
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
