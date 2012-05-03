using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class SelectableCharacter : GameObject
    {

        private const String cSOUND_ROZ_THE_RED = "sound\\fx\\charsounds\\roz_red";
        private const String cSOUND_GRAHAM_THE_GREEN = "sound\\fx\\charsounds\\grahan_green";
        private const String cSOUND_BILLY_THE_BLUE = "sound\\fx\\charsounds\\billy_blue";

        private const String cSOUND_HIGHLIGHT = "sound\\fx\\colorswap8bit";
        private const String cSOUND_SELECT    = "sound\\fx\\charselect-8bit";//
        public const String  cSOUND_SELECTION = "sound\\fx\\charselected-8bits";

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_UNSELECTED   = 0;
        public const int sSTATE_HIGHLIGHTED  = 1;
        public const int sSTATE_SELECTED     = 2;
        public const int sSTATE_EXPLOSION    = 3;


        //SPRITES
        private Sprite mSpriteUnselected;
        private Sprite mSpriteHighlighted;
        private Sprite mSpriteSelected;
        private Sprite mSpriteExplosion;

        //Specific
        private int mInitX;
        private int mInitY;
        private const int cHORIZONTAL_MARGIN = 40;


        //sine movement
        private Vector2 pos;
        private double destAngle = 0;

        private Vector2 mDestiny;

        private Color mCurrentColor;

        private float mScale = 1;

        private bool mReachedMaxSize;

        private bool mGrowing;
        private float mGrowValue;

        private bool mMustMove;

        private Color mColor;

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public SelectableCharacter(Vector2 center, Color color)
        {

            mColor = color;

            if (mColor == Color.Red)
            {
                mSpriteUnselected  = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\selection\\RED\\Red_unselect_pos"), new int[] { 0 }, 9, 175, 231, false, false);
                mSpriteHighlighted = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\selection\\RED\\Red_select"), new int[] { 0 }, 9, 196, 248, false, false);
                mSpriteSelected = new Sprite(ExtraFunctions.fillArrayWithImages2(0,16, "gameplay\\selection\\RED\\Selected\\red_selected"), new int[] {Sprite.sALL_FRAMES_IN_ORDER,16}, 1, 525, 394, true, false);
                mSpriteExplosion = new Sprite(ExtraFunctions.fillArrayWithImages2(16, 17, "gameplay\\selection\\RED\\Selected\\red_selected"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 17 }, 1, 800, 600, true, false);
            }

            if (mColor == Color.Green)
            {
                mSpriteUnselected = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\selection\\GREEN\\green_unselect_pos"), new int[] { 0 }, 9, 203, 211, false, false);
                mSpriteHighlighted = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\selection\\GREEN\\green_select"), new int[] { 0 }, 9, 211, 232, false, false);
                mSpriteSelected = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 16, "gameplay\\selection\\GREEN\\Selected\\green_selected"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 16 }, 1, 401, 381, true, false);
                mSpriteExplosion = new Sprite(ExtraFunctions.fillArrayWithImages2(16, 17, "gameplay\\selection\\GREEN\\Selected\\green_selected"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 17 }, 1, 800, 600, true, false);
            }

            if (mColor == Color.Blue)
            {
                mSpriteUnselected = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\selection\\BLUE\\blue_unselect_pos"), new int[] { 0 }, 9, 167, 231, false, false);
                mSpriteHighlighted = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\selection\\BLUE\\blue_select"), new int[] { 0 }, 9, 185, 254, false, false);
                mSpriteSelected = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 17, "gameplay\\selection\\BLUE\\Selected\\blue_selected"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 17 }, 1, 545, 393, true, false);
                mSpriteExplosion = new Sprite(ExtraFunctions.fillArrayWithImages2(17, 16, "gameplay\\selection\\BLUE\\Selected\\blue_selected"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 16 }, 1, 800, 600, true, false);
            }

            addSprite(mSpriteUnselected, sSTATE_UNSELECTED);
            addSprite(mSpriteHighlighted, sSTATE_HIGHLIGHTED);
            addSprite(mSpriteSelected, sSTATE_SELECTED);
            addSprite(mSpriteExplosion, sSTATE_EXPLOSION);
            
            changeToSprite(sSTATE_UNSELECTED);

            setCollisionRect(30,30,150, 150);

            setCenter(center.X,center.Y);
            setVisible(true);

            pos = new Vector2(300, 0);

        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);

            SoundManager.LoadSound(cSOUND_ROZ_THE_RED);
            SoundManager.LoadSound(cSOUND_GRAHAM_THE_GREEN);
            SoundManager.LoadSound(cSOUND_BILLY_THE_BLUE);

            SoundManager.LoadSound(cSOUND_HIGHLIGHT);
            SoundManager.LoadSound(cSOUND_SELECT);
            SoundManager.LoadSound(cSOUND_SELECTION);

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

            if (getCurrentSprite() == mSpriteSelected && mSpriteSelected.getAnimationEnded())
            {
                changeState(sSTATE_EXPLOSION);
                if (mColor == Color.Red)
                {
                    SoundManager.PlaySound(cSOUND_ROZ_THE_RED);
                }
                if (mColor == Color.Green)
                {
                    SoundManager.PlaySound(cSOUND_GRAHAM_THE_GREEN);
                }
                if (mColor == Color.Blue)
                {
                    SoundManager.PlaySound(cSOUND_BILLY_THE_BLUE);
                }
            }

                      

            base.update(gameTime);//getCurrentSprite().update();
            //LOGICA 
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            //if (isActive())
            //{
            //getCurrentSprite().draw(spriteBatch);
           if (isVisible())
           {
               base.draw(spriteBatch);
               //spriteBatch.Draw(getCurrentSprite().getCurrentTexture2D(), new Vector2(mX, mY), new Rectangle(0, 0, getCurrentSprite().getWidth(), getCurrentSprite().getHeight()), Color.White, 0, new Vector2(30, 30), mScale, SpriteEffects.None, 0);
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
            if (!mReachedMaxSize)
            {
                if (mScale + valueToIncrease > 1)
                {
                    mScale = 1;
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


        public void changeState(int state)
        {

            switch (state)
            {
                case sSTATE_UNSELECTED:

                    if (getState() != sSTATE_UNSELECTED)
                    {
                        setState(sSTATE_UNSELECTED);
                        changeToSprite(sSTATE_UNSELECTED);

                        if (mColor == Color.Red) setCenter(160, 310);
                        if (mColor == Color.Green) setCenter(382 + 15, 310 + 6);
                        if (mColor == Color.Blue) setCenter(625 - 5, 308 + 6);

                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;
                
                case sSTATE_HIGHLIGHTED:
                    if (getState() != sSTATE_HIGHLIGHTED)
                    {
                        SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                        setState(sSTATE_HIGHLIGHTED);
                        changeToSprite(sSTATE_HIGHLIGHTED);

                        if (mColor == Color.Red) setCenter(160, 310);
                        if (mColor == Color.Green) setCenter(386 + 15, 312 + 6);
                        if (mColor == Color.Blue) setCenter(623 - 5, 311 + 6);

                        //setCollisionRect(30, 30, 150, 150);

                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;
                case sSTATE_SELECTED:
                    if (getState() != sSTATE_SELECTED)
                    {
                        SoundManager.PlaySound(cSOUND_SELECT);
                        setState(sSTATE_SELECTED);
                        changeToSprite(sSTATE_SELECTED);

                        if (mColor == Color.Red) setCenter(262, 322);
                        if (mColor == Color.Green) setCenter(406 + 15, 319 + 6);
                        if (mColor == Color.Blue) setCenter(531 - 5, 311 + 6);
    
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;

                case sSTATE_EXPLOSION:
                    if (getState() != sSTATE_EXPLOSION)
                    {
                        SoundManager.PlaySound(cSOUND_SELECTION);
                        setState(sSTATE_EXPLOSION);
                        changeToSprite(sSTATE_EXPLOSION);

                        //if (mColor == Color.Red) setCenter(400, 300);
                        setCenter(400, 300);

                        getCurrentSprite().resetAnimationFlag();
                    }
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
