using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
//using System.Timers;
using ColorLand.game;

namespace ColorLand
{
    public class Cursor : GameObject
    {

        //how many seconds are necessary to complete an event
        private const int cEVENT_SECONDS = 3;

        //private bool mKinectBased = false;

          //INDEXES
        public const int sSTATE_NORMAL = 0;

        private Color mCurrentColor;

        public const int sSTATE_BLUE = 1;
        public const int sSTATE_GREEN = 2;
        public const int sSTATE_RED = 3;
        
        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpriteBlue;
        private Sprite mSpriteGreen;
        private Sprite mSpriteRed;

        //
        //private Timer mTimer;
        private MTimer mTimer;
        private int mSecs;

        private bool mActiveInsideButton;
        private bool mEventCompleted;

        private static Cursor instance;

        //tracers
        Tracer[] tracers = new Tracer[5];
        int frames = 0;

        private Cursor()
        {

            int size = 170;
            mSpriteNormal = new Sprite(1, "gameplay\\cursors\\cursor", new int[] { 0 }, 10, size, size);
            mSpriteBlue = new Sprite(ExtraFunctions.fillArrayWithImages(14, "gameplay\\cursors\\cursor_blue"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 14 }, 2, 100, 100, false, false);
            mSpriteGreen = new Sprite(ExtraFunctions.fillArrayWithImages(14, "gameplay\\cursors\\cursor_green"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 14 }, 2, 100, 100, false, false);
            mSpriteRed = new Sprite(ExtraFunctions.fillArrayWithImages(14, "gameplay\\cursors\\cursor_red"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 14 }, 2, 100, 100, false, false);
            
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteBlue,   sSTATE_BLUE);
            addSprite(mSpriteGreen, sSTATE_GREEN);
            addSprite(mSpriteRed, sSTATE_RED);
            
            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(20,20,60,60);

            for (int i = 0; i < tracers.Length; i++)
            {
                tracers[i] = new Tracer();
                tracers[i].alive = false;
            }
        }

        public static Cursor getInstance()
        {
            if (instance == null)
            {
                instance = new Cursor();
            }
            return instance;
        }

        public override void loadContent(ContentManager content) {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);//getCurrentSprite().update();

            updateTimer(gameTime);

            if (!Game1.sKINECT_BASED)
            {
                MouseState mouseState = Mouse.GetState();
                setCenter(mouseState.X, mouseState.Y);
            }
            else
            {
                //setLocation(KinectManager.sMAIN_HAND_COORD.X, KinectManager.sMAIN_HAND_COORD.Y);
            }

           /*  if (frames % 2 == 0)
            {
                for (int i = 0; i < tracers.Length; i++)
                {
                    if (!tracers[i].alive)
                    {
                        tracers[i].alive = true;
                        tracers[i].alpha = 1.0f;
                        tracers[i].pos = getLocation();
                        break;
                    }
                }
            }
            for (int i = 0; i < tracers.Length; i++)
            {
                if (tracers[i].alive)
                {
                    tracers[i].alpha -= 0.1f;
                    if (tracers[i].alpha <= 0.0f)
                        tracers[i].alive = false;                  
                }
            }
            frames++;   */     
        }

        public override void draw(SpriteBatch spriteBatch)
        {

           /* for (int i = 0; i < tracers.Length; i++)
            {
                if (tracers[i].alive)
                {
                    spriteBatch.Draw(getCurrentSprite().getCurrentTexture2D(), new Rectangle((int)tracers[i].pos.X, (int)tracers[i].pos.Y, 32, 32), Color.White * tracers[i].alpha);
                }
            }
            */
            
            base.draw(spriteBatch, Color.AliceBlue); 

        }


        public void changeColor(Color color)
        {

            mCurrentColor = color;
            if (color == Color.Blue) changeToSprite(sSTATE_BLUE);
            if (color == Color.Green) changeToSprite(sSTATE_GREEN);
            if (color == Color.Red) changeToSprite(sSTATE_RED);
            
        }

        public Color getColor()
        {
            return mCurrentColor;
        }

        public void nextColor()
        {
            if (mCurrentColor == Color.Blue)
            {
                mCurrentColor = Color.Red;
                changeToSprite(sSTATE_RED);
            }
            else
                if (mCurrentColor == Color.Green)
                {
                    mCurrentColor = Color.Blue;
                    changeToSprite(sSTATE_BLUE);
                }
                else
                    if (mCurrentColor == Color.Red)
                    {
                        mCurrentColor = Color.Green;
                        changeToSprite(sSTATE_GREEN);
                    }
        }

        //must be called while colliding with a button
        public void setActiveInsideButton(bool state)
        {

            //this AND is necessary to specify if the cursor is entering in the button
            if(mActiveInsideButton == false && state == true){
                restartTimer(cEVENT_SECONDS);
            }
            
            mActiveInsideButton = state;

            if (mTimer != null)
            {
                mTimer.stop();
            }
            //mTimer.Stop();
            //mTimer.Enabled = false;

        }

        private void restartTimer(int seconds)
        {
            /*mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            mTimer.Interval = seconds * 1000;
            mTimer.Enabled = true;*/

            mTimer = new MTimer();
            mTimer.start();
        }


        private void updateTimer(GameTime gameTime)
        {
            if (mTimer != null)
            {
                mTimer.update(gameTime);

                if (mTimer.getTimeAndLock(cEVENT_SECONDS))
                {
                    if (mActiveInsideButton)
                    {
                        mEventCompleted = true;

                        mTimer.stop();
                        mTimer = null;
                    }
                }
            }
        }

        public bool isEventCompleted()
        {

            return this.mEventCompleted;

        }

    }
}
