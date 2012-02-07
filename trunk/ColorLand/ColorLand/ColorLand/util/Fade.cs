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

        public const int sFADE_IN_EFFECT_GRADATIVE = 0;

        public const int sFADE_OUT_EFFECT_GRADATIVE = 10;

        private int mCurrentEffect;

        private Texture2D mFadeImage;

        private Rectangle mRectangle;
        private float mAlphaLevel;

        private bool mActive;
        private bool mFadeComplete;

        public Fade(int effect, String fadeImagePath)
        {
            mCurrentEffect = effect;
            mFadeImage = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>(fadeImagePath);

            restartEffect(effect);
        }

        private void restartEffect(int effect)
        {
            switch(mCurrentEffect){
                case sFADE_IN_EFFECT_GRADATIVE:
                    mRectangle = new Rectangle(0,0,Game1.sSCREEN_RESOLUTION_WIDTH,Game1.sSCREEN_RESOLUTION_HEIGHT);
                    mAlphaLevel = 1;
                                        
                    break;
                case sFADE_OUT_EFFECT_GRADATIVE:
                    mRectangle = new Rectangle(0, 0, Game1.sSCREEN_RESOLUTION_WIDTH, Game1.sSCREEN_RESOLUTION_HEIGHT);
                    mAlphaLevel = 0;

                    break;
            }
        }

        public void update(GameTime gametime)
        {
            if (mActive)
            {
                switch (mCurrentEffect)
                {
                    case sFADE_IN_EFFECT_GRADATIVE:
                        if (!mFadeComplete)
                        {
                            mAlphaLevel -= 0.02f;

                            if (mAlphaLevel <= 0.0f)
                            {
                                mFadeComplete = true;
                            }
                        }
                        break;
                    case sFADE_OUT_EFFECT_GRADATIVE:
                        if (!mFadeComplete)
                        {
                            mAlphaLevel += 0.02f;

                            if (mAlphaLevel >= 1.0f)
                            {
                                mFadeComplete = true;
                            }
                        }
                        break;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (mActive)
            {
                switch (mCurrentEffect)
                {
                    case sFADE_IN_EFFECT_GRADATIVE:
                    case sFADE_OUT_EFFECT_GRADATIVE:
                        spriteBatch.Draw(mFadeImage, mRectangle, new Color(0, 0, 0, mAlphaLevel));
                        break;
                }
            }
        }

        public void activate()
        {
            this.mActive = true;
        }

        public void deactivate()
        {
            this.mActive = false;
        }

        public bool isFadeComplete()
        {
            return this.mFadeComplete;
        }

    }
}
