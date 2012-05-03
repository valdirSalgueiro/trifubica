using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public class Balloon : GameObject
    {
        //INDEXES
        public const int sSTATE_FLYING_RED = 0;
        public const int sSTATE_FLYING_GREEN = 1;
        public const int sSTATE_FLYING_BLUE = 2;
 
                //SPRITES
        private Sprite mSpriteRed;
        private Sprite mSpriteGreen;
        private Sprite mSpriteBlue;
 
        
        private const int cHORIZONTAL_MARGIN = 40;

        private float mSinComplement = 5.0f;

        private bool tempMove;
        private float x;

        Texture2D bubble;

        private MTimer mTimerChangeColor;

        float elapsedTime;

        Random rand=new Random();

        Color enemyColor;

        Dictionary<Color, Sprite> dic = new Dictionary<Color, Sprite>();
        
        //TODO Construir mecanismo de chamar um delegate method when finish animation

        private BTYPE mType;

        public enum BTYPE
        {
            INSTANT_BALLOON,
            HEART
        }

        public Balloon(Color color) : this (color, new Vector2(0,0),BTYPE.INSTANT_BALLOON)
        {
            
        }

        public Balloon(Color color, Vector2 origin, BTYPE type)
            : base()
        {

            mType = type;

            mTimerChangeColor = new MTimer(true);

            enemyColor = color;

            if (mType == BTYPE.INSTANT_BALLOON)
            {
                mSpriteRed = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 12, "enemies\\balloon\\red\\red"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);
                mSpriteGreen = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 12, "enemies\\balloon\\green\\green"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);
                mSpriteBlue = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 12, "enemies\\balloon\\blue\\blue"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);
            }else
            if (mType == BTYPE.HEART)
            {
                /*hmSpriteRed = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 12, "enemies\\balloon\\red\\red"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);
                mSpriteGreen = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 12, "enemies\\balloon\\green\\green"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);
                mSpriteBlue = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 12, "enemies\\balloon\\blue\\blue"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);*/
            }
                        
            addSprite(mSpriteRed,   sSTATE_FLYING_RED);
            addSprite(mSpriteGreen, sSTATE_FLYING_GREEN);
            addSprite(mSpriteBlue,  sSTATE_FLYING_BLUE);

            changeToSprite(sSTATE_FLYING_RED);

            setCollisionRect(15,15, 70, 70);
            setLocation(origin);
        }

        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
            bubble = content.Load<Texture2D>("enemies\\balloon\\bolha\\Bolha_01"); 
        }

        public override void update(GameTime gameTime)
        {
            
            base.update(gameTime);//getCurrentSprite().update();

            if (mX < GamePlayScreen.sCURRENT_STAGE_X && tempMove == false)
            {
                tempMove = true;
            }

            if (tempMove == true)
            {
                moveRight(1);
            }

            if (mX > GamePlayScreen.sCURRENT_STAGE_X + 800 - 120 && tempMove == true)
            {
                tempMove = false;
            }

            if (tempMove == false)
            {
                moveLeft(1);
            }

            x += 0.05f;
            float sinMov = 2*(float)Math.Sin(x);
            mY += sinMov;

            if (mTimerChangeColor != null)
            {
                mTimerChangeColor.update(gameTime);

                if (mTimerChangeColor.getTimeAndLock(1))
                {
                    if (getState() == sSTATE_FLYING_RED) changeState(sSTATE_FLYING_GREEN);
                    if (getState() == sSTATE_FLYING_GREEN) changeState(sSTATE_FLYING_BLUE);
                    if (getState() == sSTATE_FLYING_BLUE) changeState(sSTATE_FLYING_RED);
                }
            }

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bubble, new Rectangle((int)(mX), (int)(mY), 100, 100), Color.White);
            mSpriteRed = dic[enemyColor];
            base.draw(spriteBatch);
        }

        public Color getColor()
        {
            return enemyColor;
        }

        public void changeState(int state)
        {

            switch (state)
            {
                case sSTATE_FLYING_RED:
                    setState(sSTATE_FLYING_RED);
                    changeToSprite(sSTATE_FLYING_RED);
                    break;
                case sSTATE_FLYING_GREEN:
                    setState(sSTATE_FLYING_GREEN);
                    changeToSprite(sSTATE_FLYING_GREEN);
                    break;
                case sSTATE_FLYING_BLUE:
                    setState(sSTATE_FLYING_BLUE);
                    changeToSprite(sSTATE_FLYING_BLUE);
                    break;
    
            }

        }

    }

}
