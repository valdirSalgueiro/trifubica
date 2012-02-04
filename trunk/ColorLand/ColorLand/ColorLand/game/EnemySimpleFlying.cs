using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class EnemySimpleFlying : PaperObject {

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_NORMAL = 0;
        public const int sSTATE_EXPLODING = 1;
        

        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpriteExploding;
        
        //Specific
        private int mType;
        private int mInitX;
        private int mInitY;
        private const int cHORIZONTAL_MARGIN = 40;
        private Boolean mMovingRight = true;

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public EnemySimpleFlying(int type)
        {

            this.mType = type;

            switch(mType){
                case BaseEnemy.sTYPE_SIMPLE_FLYING_RED:
            
                    String[] imagesStopped = new String[1];
                    imagesStopped[0] = "test\\ered";
                
                    String[] imagesDestroyed = new String[1];
                    imagesDestroyed[0] = "test\\ered";
                
                    mSpriteNormal    = new Sprite(imagesStopped, new int[] { 0 }, 7, 40, 40, false, false);
                    mSpriteExploding = new Sprite(imagesDestroyed, new int[] { 0 }, 3, 40, 40, true, true);
                    break;
                case BaseEnemy.sTYPE_SIMPLE_FLYING_GREEN:

                    break;
                case BaseEnemy.sTYPE_SIMPLE_FLYING_BLUE:

                    break;
            }
        
            
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteExploding, sSTATE_EXPLODING);
           
            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(40, 40);
        }

        public override void loadContent(ContentManager content) {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime) {

            base.update(gameTime);//getCurrentSprite().update();

        }

        public void explode()
        {
            changeState(sSTATE_EXPLODING);
            enableCollision(false);
            //setLocation(1000, 1000);
        }

        public override void draw(SpriteBatch spriteBatch) {
            if (isActive())
            {
                base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);

               // spriteBatch.DrawString(mFontDebug, /*" ATE: " + mAlreadyAte + " ColEnabled: " + collisionEnabled() +*/" Rect: " + getCollisionRect(), new Vector2(0, 150), Color.Yellow);
            }
            //getCurrentSprite().draw(spriteBatch);
        }

        public void changeState(int state) {

            switch (state) {
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
