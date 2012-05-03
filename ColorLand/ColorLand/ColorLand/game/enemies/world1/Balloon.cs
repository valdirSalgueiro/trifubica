using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class Balloon : BaseEnemy
    {
        //INDEXES
        public const int sSTATE_FLYING = 0;
 

        //SPRITES
        private Sprite mSpriteFlying;
 
        //Specific
        private int mType;
        private int mInitX;
        private int mInitY;
        private const int cHORIZONTAL_MARGIN = 40;

        private float mSinComplement = 5.0f;

        private bool tempMove;
        private float x;

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public Balloon(Color color) : this (color, new Vector2(0,0))
        {
            
        }

        public Balloon(Color color, Vector2 origin)
            : base(color, origin)
        {
            if(color == Color.Red)
            {
                mSpriteFlying = new Sprite(ExtraFunctions.fillArrayWithImages2(1,12, "enemies\\balloon\\red\\red"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);
            }
            if (color == Color.Blue)
            {
                mSpriteFlying = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 12, "enemies\\balloon\\blue\\blue"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);
            }
            if (color == Color.Green)
            {
                mSpriteFlying = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 12, "enemies\\balloon\\green\\green"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, 100, 100, false, false);
            }

            addSprite(mSpriteFlying, sSTATE_FLYING);

            changeToSprite(sSTATE_FLYING);

            setCollisionRect(15,15, 70, 70);
            setLocation(origin);
        }

        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
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
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }

        public void changeState(int state)
        {

            switch (state)
            {
                case sSTATE_FLYING:
                    setState(sSTATE_FLYING);
                    changeToSprite(sSTATE_FLYING);
                    break;
    
            }

        }

    }

}
