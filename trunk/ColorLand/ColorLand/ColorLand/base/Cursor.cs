using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using ColorLand.game;

namespace ColorLand
{
    class Cursor : GameObject
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
        private Timer mTimer;
        private bool mActiveInsideButton;
        private bool mEventCompleted;

        //tracers
        Tracer[] tracers = new Tracer[5];
        int frames = 0;

        public Cursor()
        {
            mSpriteNormal = new Sprite(1, "gameplay\\cursors\\cursor", new int[] { 0 }, 10, 32, 32);
            mSpriteBlue   = new Sprite(1, "gameplay\\cursors\\cursorblue", new int[] { 0 }, 10, 32, 32);
            mSpriteGreen  = new Sprite(1, "gameplay\\cursors\\cursorgreen", new int[] { 0 }, 10, 32, 32);
            mSpriteRed    = new Sprite(1, "gameplay\\cursors\\cursorred", new int[] { 0 }, 10, 32, 32);
            
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteBlue,   sSTATE_BLUE);
            addSprite(mSpriteGreen, sSTATE_GREEN);
            addSprite(mSpriteRed, sSTATE_RED);
            
            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(32, 32);

            for (int i = 0; i < tracers.Length; i++)
            {
                tracers[i] = new Tracer();
                tracers[i].alive = false;
            }
        }

        public override void loadContent(ContentManager content) {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);//getCurrentSprite().update();

            if (!Game1.sKINECT_BASED)
            {
                MouseState mouseState = Mouse.GetState();
                setLocation(mouseState.X, mouseState.Y);
            }
            else
            {
                setLocation(KinectManager.sMAIN_HAND_COORD.X, KinectManager.sMAIN_HAND_COORD.Y);
            }

             if (frames % 2 == 0)
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
            frames++;        
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tracers.Length; i++)
            {
                if (tracers[i].alive)
                {
                    spriteBatch.Draw(getCurrentSprite().getCurrentTexture2D(), new Rectangle((int)tracers[i].pos.X, (int)tracers[i].pos.Y, 32, 32), Color.White * tracers[i].alpha);
                }
            }
            
            
            base.draw(spriteBatch, Color.AliceBlue); 
        }


        public void changeColor(Color color)
        {

            mCurrentColor = color;
            if (color == Color.Blue) changeToSprite(sSTATE_BLUE);
            if (color == Color.Green) changeToSprite(sSTATE_GREEN);
            if (color == Color.Red) changeToSprite(sSTATE_RED);
            
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

             mTimer.Stop();
            mTimer.Enabled = false;

        }

        private void restartTimer(int seconds)
        {
            mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            mTimer.Interval = seconds * 1000;
            mTimer.Enabled = true;
        }

        //qualquer coisa mete static aqui que funciona
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            mTimer.Stop();
            mTimer.Enabled = false;

            //if the cursor is yet over the button
            if (mActiveInsideButton)
            {
                mEventCompleted = true;
            }

        }

        public bool isEventCompleted()
        {

            return this.mEventCompleted;

        }

    }
}
