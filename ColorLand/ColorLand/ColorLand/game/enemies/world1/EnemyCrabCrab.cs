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

        public EnemyCrabCrab(Color color, Vector2 origin)
            : base(color, origin)
        {

            if(color == Color.Red)
            {
                
                  mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages(20, "enemies\\world1\\red\\crab_walk"), new int[] { 3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19 }, 2, 300, 300, false, false);
                  mSpriteAttacking = new Sprite(ExtraFunctions.fillArrayWithImages(11, "enemies\\world1\\red\\crab_attack"), new int[] { Sprite.sALL_FRAMES_IN_ORDER,11 }, 2, 300, 300, false, false);
            }
        
            addSprite(mSpriteWalking, sSTATE_WALKING);
            addSprite(mSpriteAttacking, sSTATE_ATTACKING);

            changeToSprite(sSTATE_WALKING);

            setCollisionRect(101,187, 374, 155);
            setLocation(origin);

        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            Vector2 playerLocation = getPlayerPosition();


            //PLAYER IS LEFT FROM CRAB
            if (getPlayerCenter() < getCenter())
            {
                //setFlipDirection(FlipDirection.Left);
                if (getPlayerCenter() >= getCenter() - 110)
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
               
                if (getPlayerCenter() <=  getCenter() + 110)
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
                if (getCurrentSprite().getCurrentFrame() == 8)
                {
                    setAttackRectangle(86, 192, 86 + 100, 192 + 80);
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
