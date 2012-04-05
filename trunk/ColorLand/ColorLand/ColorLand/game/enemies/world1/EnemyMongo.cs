using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class EnemyMongo : BaseEnemy
    {

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_WALKING = 0;
 

        //SPRITES
        private Sprite mSpriteWalking;
 
        //Specific
        private int mType;
        private int mInitX;
        private int mInitY;
        private const int cHORIZONTAL_MARGIN = 40;

        private bool tempMove;
        private float x;

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        
        public EnemyMongo(Color color) : this (color, new Vector2(0,0))
        {
            
        }

        public EnemyMongo(Color color, Vector2 origin)
            : base(color, origin)
        {

            if(color == Color.Red)
            {
                mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(9, "enemies\\Mongo\\red\\01\\mongo_red"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, 2, 100, 100, false, false);
            }
            if (color == Color.Blue)
            {
                mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(9, "enemies\\Mongo\\blue\\01\\mongo_blue"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, 2, 100, 100, false, false);
            }
            if (color == Color.Green)
            {
                mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(9, "enemies\\Mongo\\green\\01\\mongo_green"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, 2, 100, 100, false, false);
            }

            addSprite(mSpriteWalking, sSTATE_WALKING);

            changeToSprite(sSTATE_WALKING);

            setCollisionRect(0,0, 100, 100);
            setLocation(origin);


        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            
            base.update(gameTime);//getCurrentSprite().update();

            //provisorio
            if (mX < 0 && tempMove == false)
            {
                tempMove = true;
            }

            if (tempMove == true)
            {
                moveRight(3);
            }

            if (mX > 800-120 && tempMove == true)
            {
                tempMove = false;
            }

            if (tempMove == false)
            {
                moveLeft(3);
            }

            x += 0.1f;
            float sinMov = 5.0f * (float)Math.Sin(x);
            mY += sinMov;



        }

        public override void draw(SpriteBatch spriteBatch)
        {
            //if (isActive())
            //{
            base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);

            // spriteBatch.DrawString(mFontDebug, /*" ATE: " + mAlreadyAte + " ColEnabled: " + collisionEnabled() +*/" Rect: " + getCollisionRect(), new Vector2(0, 150), Color.Yellow);
            //}
            //getCurrentSprite().draw(spriteBatch);
        }

        public void changeState(int state)
        {

            switch (state)
            {
                case sSTATE_WALKING:
                    setState(sSTATE_WALKING);
                    changeToSprite(sSTATE_WALKING);
                    break;
    
            }

        }


        public void setType(int type)
        {
            this.mType = type;
        }

        public int getType()
        {
            return this.mType;
        }

        public void setInitXY(int x, int y)
        {
            this.mInitX = x;
            this.mInitY = y;
        }

    }

}
