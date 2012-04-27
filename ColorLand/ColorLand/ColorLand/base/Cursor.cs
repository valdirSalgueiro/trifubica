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


        private float mZoomClick = 1.0f;


        private MTimer mTimerParalyzed;//when hit an enemy of wrong color

        private bool mActiveInsideButton;
        private bool mEventCompleted;

        private static Cursor instance;

        //tracers
        Tracer[] tracers = new Tracer[5];
        int frames = 0;

        private bool mCanMove = true;
        private float mRotation = 0;

        public Cursor()
        {

            int size = 170;
            mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\cursors\\cursor"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 1 }, 1, 93, 150, false, false);                            
            mSpriteBlue = new Sprite(ExtraFunctions.fillArrayWithImages2(14, "gameplay\\cursors\\blue\\cursor"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 14 }, 1, 100, 100, false, false);
            mSpriteGreen = new Sprite(ExtraFunctions.fillArrayWithImages2(14, "gameplay\\cursors\\green\\cursor"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 14 }, 1, 100, 100, false, false);
            mSpriteRed = new Sprite(ExtraFunctions.fillArrayWithImages2(14, "gameplay\\cursors\\red\\cursor"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 14 }, 1, 100, 100, false, false);
            
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteBlue,   sSTATE_BLUE);
            addSprite(mSpriteGreen, sSTATE_GREEN);
            addSprite(mSpriteRed, sSTATE_RED);
            
            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(0,0,40,40);

            for (int i = 0; i < tracers.Length; i++)
            {
                tracers[i] = new Tracer();
                tracers[i].alive = false;
            }

        }

        /*public static Cursor getInstance()
        {
            if (instance == null)
            {
                instance = new Cursor();
            }
            return instance;
        }*/

        public override void loadContent(ContentManager content) {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);//getCurrentSprite().update();

            updateTimer(gameTime);

            updateInput();

            if (!Game1.sKINECT_BASED)
            {
                //if (mCanMove)
                //{
                    MouseState mouseState = Mouse.GetState();
                    //setCenter(mouseState.X, mouseState.Y);
                    setLocation(mouseState.X + GamePlayScreen.sCURRENT_STAGE_X_PROGRESSIVE, mouseState.Y); //half half image
                    //setLocation(mouseState.X - 40, mouseState.Y - 40); //half half image
                //}
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

            if (getCurrentSprite() == mSpriteNormal)
            {
                base.draw(spriteBatch, 0, mZoomClick);
            }
            else
            {

                if (mCanMove)
                {
                    base.draw(spriteBatch);
                }
                else
                {
                    base.draw(spriteBatch, mRotation += 0.8f, new Rectangle((int)mX + 40, (int)mY + 40, 80, 80), new Vector2(80, 80)); //0.4
                }

            }
            //spriteBatch.Draw(getCurrentSprite().getCurrentTexture2D(), new Vector2(mX, mY), new Rectangle(0, 0, getCurrentSprite().getWidth(), getCurrentSprite().getHeight()), Color.White, mRotation+=0.3f, new Vector2(300, 300), 1, SpriteEffects.None, 0);

            //base.draw(spriteBatch, Color.AliceBlue); 

        }


        public void changeColor(Color color)
        {

            mCurrentColor = color;
            if (color == Color.Blue) changeToSprite(sSTATE_BLUE);
            if (color == Color.Green) changeToSprite(sSTATE_GREEN);
            if (color == Color.Red) changeToSprite(sSTATE_RED);

            setCollisionRect(24, 24, 53, 53);

        }

        public void backToMenuCursor()
        {
            mCurrentColor = Color.Blue;
            changeToSprite(sSTATE_NORMAL);
            setCollisionRect(0, 0, 40, 40);
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

        public void previousColor()
        {
            nextColor();
            nextColor();
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

        private void updateInput()
        {
            if (getCurrentSprite() == mSpriteNormal)
            {
                MouseState mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (mZoomClick > 0.7f)
                    {
                        mZoomClick -= 0.04f;
                    }
                    else
                    {
                        mZoomClick = 0.7f;
                    }

                }
                else
                {
                    if (mZoomClick < 1.0f)
                    {
                        mZoomClick += 0.04f;
                    }
                    else
                    {
                        mZoomClick = 1.0f;
                    }
                }

            }
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


            if (mTimerParalyzed != null)
            {
                mTimerParalyzed.update(gameTime);

                if (mTimerParalyzed.getTimeAndLock(0.6))
                {
                    mTimerParalyzed.stop();
                    mTimerParalyzed = null;

                    mCanMove = true;
                }
            }
        }

        public void paralyze()
        {
            if (mCanMove)
            {
                mTimerParalyzed = new MTimer(true);
                mCanMove = false;
            }
        }

        public bool isParalyzed()
        {
            return !mCanMove;
        }

        public bool isEventCompleted()
        {

            return this.mEventCompleted;

        }

    }
}
