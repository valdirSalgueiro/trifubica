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
                
                  String[] imagesWalking = new String[4];
                  imagesWalking[0] = "test\\0009";
                  imagesWalking[1] = "test\\0015";
                  imagesWalking[2] = "test\\0019";
                  imagesWalking[3] = "test\\0024";

                  String[] imagesAttacking = new String[1];
                  imagesAttacking[0] = "test\\eblue";

                  mSpriteWalking   = new Sprite(imagesWalking, new int[] { 0,1,2,3 }, 7, 90, 90, false, false);
                  mSpriteAttacking = new Sprite(imagesAttacking, new int[] { 0 }, 3, 90, 90, false, false);
            }
        
            addSprite(mSpriteWalking, sSTATE_WALKING);
            addSprite(mSpriteAttacking, sSTATE_ATTACKING);

            changeToSprite(sSTATE_WALKING);

            setCollisionRect(40, 40);
            setLocation(origin);

        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            Vector2 playerLocation = getPlayerPosition();


            if (playerLocation.X > mX)
            {

                if (mX > playerLocation.X - 50)
                {
                    if(getState() != sSTATE_ATTACKING)
                        changeState(sSTATE_ATTACKING);
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
            }
            else
            {

                if (mX < playerLocation.X + 50)
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
                    setState(sSTATE_ATTACKING);
                    changeToSprite(sSTATE_ATTACKING);
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
