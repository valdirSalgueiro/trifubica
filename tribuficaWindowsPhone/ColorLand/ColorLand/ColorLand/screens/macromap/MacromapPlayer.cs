using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class MacromapPlayer : BaseEnemy
    {

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_NORMAL = 0;
        public const int sSTATE_EXPLODING = 1;


        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpriteExploding;

        //Specific
        private int mInitX;
        private int mInitY;
        private const int cHORIZONTAL_MARGIN = 40;


        //sine movement
        private Vector2 pos;
        private double destAngle = 0;

        private Vector2 mDestiny;

        private Color mCurrentColor;

        private float mScale = 0;

        private bool mReachedMaxSize;

        private bool mGrowing;
        private float mGrowValue;

        private bool mMustMove;

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public MacromapPlayer(Color color, Vector2 position)
            : base(color, position)
        {

            mCurrentColor = color;

            if(mCurrentColor == Color.Red){
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\macromap\\RED_ICONE_POS_0"), new int[] { 0 }, 7, 107, 107, false, false);
            }else
            if(mCurrentColor == Color.Green){
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\macromap\\GREEN_ICONE_POS_0"), new int[] { 0 }, 7, 107, 107, false, false);
            }else
            if(mCurrentColor == Color.Blue){
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\macromap\\BLUE_ICONE_POS_0"), new int[] { 0 }, 7, 107, 107, false, false);
            }
            
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            //addSprite(mSpriteExploding, sSTATE_EXPLODING);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(40, 40);

            pos=new Vector2(300, 0);            
        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }


        public void moveTo(Vector2 destiny)
        {
            setDestiny(destiny);
            mMustMove = true;
            pos = getLocation();
        }

        public void setMustMove(bool mustMove)
        {
            mMustMove = mustMove;
        }

        public void setDestiny(Vector2 destiny)
        {
            this.mDestiny = destiny;
        }

        public void setDestiny(int x, int y)
        {
            this.mDestiny = new Vector2(x,y);
        }


        public override void update(GameTime gameTime)
        {
            if (mMustMove)
            {
                float distance;
                Vector2 playerPosition = getPlayerPosition();
                Vector2.Distance(ref mDestiny, ref pos, out distance);
                if (distance > 20)
                {
                    destAngle = Math.Atan2(mDestiny.Y - pos.Y, mDestiny.X - pos.X);
                    //altere "1.0f" para fazer com que ele se desloque mais rapidamente
                    pos.X += 1.0f * (float)Math.Cos(destAngle);
                    pos.Y += 1.0f * (float)Math.Sin(destAngle);
                }
                else
                {
                    //colidiu..
                }

                setLocation(pos);

            }

            if (mGrowing && !mReachedMaxSize)
            {
                increaseScaleIn(mGrowValue);
            }
                       

            base.update(gameTime);//getCurrentSprite().update();
            //LOGICA 
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            //if (isActive())
            //{
            //base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
            if (isVisible())
            {
                spriteBatch.Draw(getCurrentSprite().getCurrentTexture2D(), new Vector2(mX, mY), new Rectangle(0, 0, getCurrentSprite().getWidth(), getCurrentSprite().getHeight()), Color.White, 0, new Vector2(30, 30), mScale, SpriteEffects.None, 0);
            }
            // spriteBatch.DrawString(mFontDebug, /*" ATE: " + mAlreadyAte + " ColEnabled: " + collisionEnabled() +*/" Rect: " + getCollisionRect(), new Vector2(0, 150), Color.Yellow);
            //}
            //getCurrentSprite().draw(spriteBatch);
        }

        public void setScale(float scale)
        {
            mScale = scale;
        }

        private void decreaseScaleIn(float valueToReduce)
        {
            if (mScale - valueToReduce < 0)
            {
                mScale = 0;
            }
            else
            {
                mScale -= valueToReduce;
            }

            mReachedMaxSize = false;
        }

        private void increaseScaleIn(float valueToIncrease)
        {
            float maxScale = 0.7f;
            if (!mReachedMaxSize)
            {
                if (mScale + valueToIncrease > maxScale)
                {
                    mScale = maxScale;
                    mReachedMaxSize = true;
                }
                else
                {
                    mScale += valueToIncrease;
                }
            }
        }

        public void growUp(float value)
        {
            mGrowing = true;
            mGrowValue = value;
        }

        public void perfectSize()
        {
            mScale = 0.7f;
        }


        public void changeState(int state)
        {

            switch (state)
            {
                case sSTATE_NORMAL:
                    setState(sSTATE_NORMAL);
                    changeToSprite(sSTATE_NORMAL);
                    break;
                case sSTATE_EXPLODING:
                    setState(sSTATE_EXPLODING);
                    changeToSprite(sSTATE_EXPLODING);
                    break;

            }

        }


        public Vector2 getPlayerPosition()
        {
            BaseScreen currentScreen = Game1.getInstance().getScreenManager().getCurrentScreen();
            if (currentScreen is GamePlayScreen)
            {
                return ((GamePlayScreen)currentScreen).getPlayerLocation();
            }

            return new Vector2();
        }

    }

}
