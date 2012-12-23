using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ColorLand.managers;

namespace ColorLand
{
    class Rocker : BaseEnemy
    {

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_FLYING_RIGHT = 0;
        public const int sSTATE_FLYING_LEFT = 1;
        public const int sSTATE_FLYING_STOPPED = 2;
        public const int sSTATE_FLYING_ATTACKING = 3;


        //SPRITES
        private Sprite mSpriteFlyingRight;
        private Sprite mSpriteFlyingLeft;
        private Sprite mSpriteFlyingStopped;
        private Sprite mSpriteFlyingAttacking;

        //Specific
        private int mType;
        private int mInitX;
        private int mInitY;
        private const int cHORIZONTAL_MARGIN = 40;

        private bool tempMove;
        private float x;

        private RockerWing rockerWing;
        private Boolean attacking = false;

        private int attackTimer = 0;
        private int waitTimer = 0;


        private Boolean isRockActive = true;
        private Texture2D texture;
        private float alphaTexture = 0.0f;

        private Vector2 rockPos;

        int offsetX;
        int offsetY;


        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public Rocker(Color color)
            : this(color, new Vector2(0, 0))
        {
            
        }

        public Rocker(Color color, Vector2 origin)
            : base(color, origin)
        {
            rockerWing = new RockerWing();

            if (color == Color.Red)
            {
                mSpriteFlyingRight = new Sprite(ExtraFunctions.fillArrayWithImages2(0,4, "enemies\\rocker\\red\\red"), new int[] { 2 }, 4, 72, 110, false, false);
                mSpriteFlyingLeft = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\red\\red"), new int[] { 3 }, 4, 72, 110, false, false);
                mSpriteFlyingStopped = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\red\\red"), new int[] { 0 }, 4, 72, 110, false, false);
                mSpriteFlyingAttacking = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\red\\red"), new int[] { 1 }, 4, 72, 110, false, false);
            }
            if (color == Color.Blue)
            {
                mSpriteFlyingRight = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\blue\\blue"), new int[] { 2 }, 4, 72, 110, false, false);
                mSpriteFlyingLeft = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\blue\\blue"), new int[] { 3 }, 4, 72, 110, false, false);
                mSpriteFlyingStopped = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\blue\\blue"), new int[] { 0 }, 4, 72, 110, false, false);
                mSpriteFlyingAttacking = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\blue\\blue"), new int[] { 1 }, 4, 72, 110, false, false);
            }
            if (color == Color.Green)
            {

                mSpriteFlyingRight = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\green\\green"), new int[] { 2 }, 4, 72, 110, false, false);
                mSpriteFlyingLeft = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\green\\green"), new int[] { 3 }, 4, 72, 110, false, false);
                mSpriteFlyingStopped = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\green\\green"), new int[] { 0 }, 4, 72, 110, false, false);
                mSpriteFlyingAttacking = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 4, "enemies\\rocker\\green\\green"), new int[] { 1 }, 4, 72, 110, false, false);
            }

            addSprite(mSpriteFlyingRight, sSTATE_FLYING_RIGHT);
            addSprite(mSpriteFlyingLeft, sSTATE_FLYING_LEFT);
            addSprite(mSpriteFlyingStopped, sSTATE_FLYING_STOPPED);
            addSprite(mSpriteFlyingAttacking, sSTATE_FLYING_ATTACKING);

            changeToSprite(sSTATE_FLYING_RIGHT);

            setCollisionRect(15, 15, 70, 70);
            setLocation(origin);
        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
            rockerWing.loadContent(content);
            texture = content.Load<Texture2D>("enemies\\rocker\\rock\\Pedra");
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);//getCurrentSprite().update();
            rockerWing.mX = mX - getCurrentSprite().getWidth()/2 - 2;
            rockerWing.mY = mY - getCurrentSprite().getHeight()/2 + 30;
            rockerWing.update(gameTime);
            rockPos.X = mX + 12;
            rockPos.Y = mY + 65;

            //provisorio
            if (!attacking)
            {
                if (mX < GamePlayScreen.sCURRENT_STAGE_X && tempMove == false)
                {
                    offsetX = -3;
                    tempMove = true;
                }

                if (tempMove == true)
                {
                    changeState(sSTATE_FLYING_RIGHT);
                    moveRight(3);
                }

                if (mX > GamePlayScreen.sCURRENT_STAGE_X + 800 - 120 && tempMove == true)
                {
                    tempMove = false;
                }

                if (tempMove == false)
                {
                    offsetX = 3;
                    changeState(sSTATE_FLYING_LEFT);
                    moveLeft(3);
                }

                if (mX + (getCurrentSprite().getWidth() / 2) > getPlayerCenter() - 2 && (mX + getCurrentSprite().getWidth()/2) < getPlayerCenter() + 2)
                {
                    attacking = true;
                    offsetX = 0;
                    changeState(sSTATE_FLYING_STOPPED);
                }
                x += 0.1f;
                float sinMov = 5.0f * (float)Math.Sin(x);
                mY += sinMov;
            }

            if (getState() == sSTATE_FLYING_STOPPED) {
                if (attackTimer < 60)
                {
                    attackTimer++;
                }
                else {
                    changeState(sSTATE_FLYING_ATTACKING);
                    RockManager.getInstance().createObject(new Vector2((int)rockPos.X + offsetX, (int)rockPos.Y + offsetY));
                    isRockActive = false;
                }
            }

            if (getState() == sSTATE_FLYING_ATTACKING)
            {
                if (waitTimer < 60)
                {
                    waitTimer++;
                }
                else
                {
                    attackTimer = 0;
                    waitTimer = 0;
                    alphaTexture = 0;
                    attacking = false;
                    isRockActive = true;
                }
            }
            if (alphaTexture < 1)
                alphaTexture += 0.01f;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            //if (isActive())
            //{
            rockerWing.draw(spriteBatch);
            if(isRockActive)
                spriteBatch.Draw(texture, new Rectangle((int)rockPos.X +offsetX, (int)rockPos.Y +offsetY, 44, 45), Color.White*alphaTexture);
            base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
            
            // spriteBatch.DrawString(mFontDebug, /*" ATE: " + mAlreadyAte + " ColEnabled: " + collisionEnabled() +*/" Rect: " + getCollisionRect(), new Vector2(0, 150), Color.Yellow);
            //}
            //getCurrentSprite().draw(spriteBatch);
        }

        public void changeState(int state)
        {

            switch (state)
            {
                case sSTATE_FLYING_RIGHT:
                    setState(sSTATE_FLYING_RIGHT);
                    changeToSprite(sSTATE_FLYING_RIGHT);
                break;
                case sSTATE_FLYING_LEFT:
                    setState(sSTATE_FLYING_LEFT);
                    changeToSprite(sSTATE_FLYING_LEFT);
                break;
                case sSTATE_FLYING_STOPPED:
                    setState(sSTATE_FLYING_STOPPED);
                    changeToSprite(sSTATE_FLYING_STOPPED);
                break;
                case sSTATE_FLYING_ATTACKING:
                    setState(sSTATE_FLYING_ATTACKING);
                    changeToSprite(sSTATE_FLYING_ATTACKING);
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

        private class RockerWing : GameObject
        {
            //INDEXES
            public const int sFLYING = 0;


            //SPRITES
            private Sprite mSprite;


            public RockerWing()
            {

                int sizeW = 150;
                int sizeH = 93;

                mSprite = new Sprite(ExtraFunctions.fillArrayWithImages2(3, "enemies\\rocker\\wings\\asas"), new int[] { 0,1,2 },3, sizeW, sizeH, false, false);

                addSprite(mSprite, sFLYING);
                changeToSprite(sFLYING);

                setCollisionRect(75, 80);
            }

            public void changeState(int state)
            {

                setState(state);

                switch (state)
                {
                    case sFLYING:
                        changeToSprite(sFLYING);
                        break;
                }

            }

            public override void draw(SpriteBatch spriteBatch)
            {
                base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
                //getCurrentSprite().draw(spriteBatch);
            }

            public override void update(GameTime gameTime)
            {
                base.update(gameTime);//getCurrentSprite().update();
            }
        }

    }



}
