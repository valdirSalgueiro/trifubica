using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class Collectable : GameObject
    {

        //little jump
        private const float cJUMP_SPEED = -.95f;
        private float mDy;// = 0.09f;
        private bool mJumping;
        
        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        private bool mFalling = false;

        //INDEXES
        public const int sSTATE_NORMAL = 0;
        public const int sSTATE_COLLECTED = 1;


        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpriteCollected;

        private CollectableType mType;

        public enum CollectableType{
            StagePiece //necessary to advance progress in the stage
        }


        public Collectable(CollectableType type)
        {

            mType = type;

            switch (mType)
            {
                case CollectableType.StagePiece:

                    mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(10, "gameplay\\collectable\\heart"), new int[] {Sprite.sALL_FRAMES_IN_ORDER,10}, 1, 40, 40, false, false);
                    mSpriteCollected = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\maincharacter\\blue_stopped"), new int[] { 0, 1 }, 1, 45, 45, false, false);
                    break;
                
            }


            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteCollected, sSTATE_COLLECTED);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(40, 40);
            
        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
            
        }

        public override void update(GameTime gameTime)
        {
            mY += mDy * gameTime.ElapsedGameTime.Milliseconds;
            base.update(gameTime);//getCurrentSprite().update();
            updateJump();
            //LOGICA 
        }

        public void explode()
        {
            changeState(sSTATE_COLLECTED);
            enableCollision(false);
            //setLocation(1000, 1000);
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
                case sSTATE_NORMAL:
                    setState(sSTATE_NORMAL);
                    changeToSprite(sSTATE_NORMAL);
                    break;
                case sSTATE_COLLECTED:
                    setState(sSTATE_COLLECTED);
                    changeToSprite(sSTATE_COLLECTED);
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


        public void setType(CollectableType type)
        {
            this.mType = type;
        }

        public CollectableType getType()
        {
            return this.mType;
        }


        private void updateJump()
        {

            if (mJumping)
            {
                //ja tirou o pe do chao
                if (mY != GamePlayScreen.sGROUND_WORLD_1_1)
                {

                    if (mY <= 200)
                    {
                        mDy += 0.11f;
                    }

                }

                if (mDy > 0)
                    if (mY >= GamePlayScreen.sGROUND_WORLD_1_1)
                    {
                        //mDy = -mDy;
                        mJumping = false;
                        mDy = 0;
                        mY = GamePlayScreen.sGROUND_WORLD_1_1;
                    }

            }

        }

        private void Jump()
        {

            if (!mJumping)
            {
                mDy -= 0.55f;
                mJumping = true;

            }

        }

        private void appear(Vector2 position)
        {
            setLocation(position);
            Jump();
        }

        public void appear(float x, float y)
        {
            setLocation(x,y);
            Jump();
        }

    }

}
