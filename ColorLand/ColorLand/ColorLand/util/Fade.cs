using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ColorLand
{
    public class Fade
    {

        BaseScreen mContext;

        private float mSpeed;

        public const int sFADE_IN_EFFECT_GRADATIVE = 0;

        public const int sFADE_OUT_EFFECT_GRADATIVE = 10;

        private int mCurrentEffect;

        private Texture2D mFadeImage;

        private Rectangle mRectangle;
        private float mAlphaLevel;

        private bool mActive;
               
        private bool mRunning;

        public enum SPEED
        {
            SLOW,
            MEDIUM,
            FAST,
            ULTRAFAST
        }

        public Fade(BaseScreen context, String fadeImagePath)
        {
            mContext = context;

            mFadeImage = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>(fadeImagePath);

            mSpeed = 0.02f;

        }

        public Fade(BaseScreen context, String fadeImagePath, SPEED speedType) : this(context,fadeImagePath)
        {

            if (speedType == SPEED.SLOW)   mSpeed = 0.01f;
            if (speedType == SPEED.MEDIUM) mSpeed = 0.02f;
            if (speedType == SPEED.FAST)   mSpeed = 0.08f;
            if (speedType == SPEED.ULTRAFAST) mSpeed = 0.12f;
                
        }



        private void initEffect()
        {
            switch(mCurrentEffect){
                case sFADE_IN_EFFECT_GRADATIVE:
                    mRectangle = new Rectangle(0,0,Game1.sSCREEN_RESOLUTION_WIDTH + 200,Game1.sSCREEN_RESOLUTION_HEIGHT);
                    mAlphaLevel = 1;
                                        
                    break;
                case sFADE_OUT_EFFECT_GRADATIVE:
                    mRectangle = new Rectangle(0, 0, Game1.sSCREEN_RESOLUTION_WIDTH + 200, Game1.sSCREEN_RESOLUTION_HEIGHT);
                    mAlphaLevel = 0;

                    break;
            }
        }

        public void execute(int effect)
        {

            mCurrentEffect = effect;

            mRunning = true;
            initEffect();
        }


        public void update(GameTime gametime)
        {
            if (mRunning)
            {
                switch (mCurrentEffect)
                {
                    case sFADE_IN_EFFECT_GRADATIVE:
                            mAlphaLevel -= mSpeed;

                            if (mAlphaLevel <= 0.0f)
                            {
                                mRunning = false;
                            }
                        
                        break;
                    case sFADE_OUT_EFFECT_GRADATIVE:
                            mAlphaLevel += mSpeed;

                            if (mAlphaLevel >= 1.0f)
                            {
                                mRunning = false;
                            }
                        
                        break;
                }
        
                if(mRunning == false){
                    mContext.fadeFinished(this);
                }

            }
        }

        public int getEffect()
        {
            return mCurrentEffect;
        }

        public void draw(SpriteBatch spriteBatch)
        {
                switch (mCurrentEffect)
                {
                    case sFADE_IN_EFFECT_GRADATIVE:

                        if(mRunning){
                            spriteBatch.Draw(mFadeImage, mRectangle, new Color(0, 0, 0, mAlphaLevel));
                        }
                        break;
                    case sFADE_OUT_EFFECT_GRADATIVE:
                        //if (mRunning)
                        //{
                            spriteBatch.Draw(mFadeImage, mRectangle, new Color(0, 0, 0, mAlphaLevel));
                        //}
                        break;
                }
        }

    }
}
