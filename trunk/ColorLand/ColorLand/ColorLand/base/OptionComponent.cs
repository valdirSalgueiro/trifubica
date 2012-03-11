using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;


namespace ColorLand
{
	public class OptionComponent
	{

        //INDEXES
        public const int sSTATE_NORMAL  = 0;
        public const int sSTATE_PRESSED = 1;

        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpritePressed;

        private Button mButtonArrowLeft;
        private SpriteFont mFontOption;
        private Button mButtonArrowRight;
        

        private String[] mOptionsSound      = { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };
        private String[] mOptionsFullscreen = { "YES", "NO" };

        private OptionType mType;
        private String[] mSelectedOption;
        private int mCurrentIndex;

        public enum OptionType
        {
            Sound,
            Fullscreen
        }

        public OptionComponent(OptionType type, Vector2 location)
        {

            this.mType = type;
            fillOption();

            mButtonArrowLeft = new Button("test\\0009", "e1", new Rectangle((int)location.X, (int)location.Y, 40, 40));
            mButtonArrowRight = new Button("eblue", "egreen", new Rectangle((int)location.X + 100, (int)location.Y, 40, 40));            

        }

        private void fillOption()
        {
            switch(mType)
            {
                case OptionType.Sound:
                    mSelectedOption = mOptionsSound;
                    break;
                case OptionType.Fullscreen:
                    mSelectedOption = mOptionsFullscreen;
                    break;
            }
        }

        public void loadContent(ContentManager content)
        {
            mFontOption = content.Load<SpriteFont>("option_component");          
        }

        public void update(GameTime gameTime)
        {
            mButtonArrowLeft.update(gameTime);
            mButtonArrowRight.update(gameTime);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            mButtonArrowLeft.draw(spriteBatch);
            spriteBatch.DrawString(mFontOption, mSelectedOption[mCurrentIndex], new Vector2(mButtonArrowLeft.getRectangle().X + mButtonArrowLeft.getRectangle().Width + 30, mButtonArrowLeft.getRectangle().Y), Color.Black);
            mButtonArrowRight.draw(spriteBatch);
        }

        public void advanceIndex()
        {
            mCurrentIndex++;

            if (mCurrentIndex >= mSelectedOption.Length)
            {
                mCurrentIndex -= 1;
            }
        }

        public void reduceIndex()
        {
            mCurrentIndex--;

            if (mCurrentIndex < 0)
            {
                mCurrentIndex = 0;
            }
        }

        public void setIndex(int index)
        {
            mCurrentIndex = index;
        }

	}
}
