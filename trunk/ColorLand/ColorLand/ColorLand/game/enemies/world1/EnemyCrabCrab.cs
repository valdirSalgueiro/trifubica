using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class EnemyCrabCrab : BaseEnemy
    {

        private const String cSOUND_PATADA = "sound\\fx\\patada8bit";

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_WALKING = 0;
        public const int sSTATE_ATTACKING = 1;


        //SPRITES
        private Sprite mSpriteWalking;
        private Sprite mSpriteAttacking;

        //Specific
        private int mType;
        private int mInitX;
        private int mInitY;
        private const int cHORIZONTAL_MARGIN = 40;


        //TODO Construir mecanismo de chamar um delegate method when finish animation

        
        public EnemyCrabCrab(Color color) : this (color, new Vector2(0,0))
        {
            
        }

        public EnemyCrabCrab(Color color, Vector2 origin)
            : base(color, origin)
        {

            if(color == Color.Red)
            {
                 //os sprites andando do red sao diferentes. Tem que ter cuidado caso for mexer
                mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 16, "enemies\\CrabCrab\\red\\Crab_walk"), new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 2, 300, 110, false, false);
                mSpriteAttacking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 11, "enemies\\CrabCrab\\red\\Crab_attack"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, 2, 300, 110, false, false);
            }
            if (color == Color.Blue)
            {
                mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 16, "enemies\\CrabCrab\\blue\\Crab_walk"), new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 2, 300, 110, false, false);
                mSpriteAttacking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 11, "enemies\\CrabCrab\\blue\\Crab_attack"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, 2, 300, 110, false, false);
            }
            if (color == Color.Green)
            {
                mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 16, "enemies\\CrabCrab\\green\\Crab_walk"), new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 2, 300, 110, false, false);
                mSpriteAttacking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 11, "enemies\\CrabCrab\\green\\Crab_attack"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, 2, 300, 110, false, false);
            }

            addSprite(mSpriteWalking, sSTATE_WALKING);
            addSprite(mSpriteAttacking, sSTATE_ATTACKING);

            changeToSprite(sSTATE_WALKING);

            setCenterHotspot(new Vector2(274, 101));
            setCollisionRect(30,30,220,80);
            setLocation(origin);

        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
            SoundManager.LoadSound(cSOUND_PATADA);
        }

        public override void update(GameTime gameTime)
        {
            Vector2 playerLocation = getPlayerPosition();


            //PLAYER IS LEFT FROM CRAB
            if (getPlayerCenter() < getCenterX())
            {
                //setFlipDirection(FlipDirection.Left);
                if (getPlayerCenter() >= getCenterX() - 110)
                {
                    if (getState() != sSTATE_ATTACKING)
                        changeState(sSTATE_ATTACKING);
                }
                else
                {
                    if (getState() != sSTATE_WALKING)
                        changeState(sSTATE_WALKING);
                }

                if (getState() == sSTATE_WALKING)
                {
                    moveLeft(1);
                }

                getCurrentSprite().setFlip(false);
            }
            else
            {
               //PLAYER IS RIGHT FROM CRAB
               
                if (getPlayerCenter() <=  getCenterX() + 110)
                {
                    if (getState() != sSTATE_ATTACKING)
                    {
                        changeState(sSTATE_ATTACKING);
                    }
                }
                else
                {
                    if (getState() != sSTATE_WALKING)
                        changeState(sSTATE_WALKING);
                }

                if (getState() == sSTATE_WALKING)
                {
                    moveRight(1);
                }
                
                 getCurrentSprite().setFlip(true);
                
            }

            if(getState() == sSTATE_ATTACKING){
                if (getCurrentSprite().getCurrentFrame() == 7)
                {
                    SoundManager.PlaySound(cSOUND_PATADA);
                    if (getCurrentSprite().isFlipped())
                    {
                        setAttackRectangle(140, 0, 135, 100);
                    }
                    else
                    {
                        setAttackRectangle(140 - 135, 0, 135, 100);
                    }
                }
                else
                {
                    setAttackRectangle(0,0,0,0);
                }
            }

            base.update(gameTime);//getCurrentSprite().update();
            //LOGICA 
        }

        public void attack()
        {
            changeState(sSTATE_ATTACKING);
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
                case sSTATE_ATTACKING:
                    if (getState() != sSTATE_ATTACKING)
                    {
                        setState(sSTATE_ATTACKING);
                        changeToSprite(sSTATE_ATTACKING);
                        getCurrentSprite().resetAnimationFlag();
                    }
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
