using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class EnemyArc : BaseEnemy
    {

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
        float theta = (float) -Math.PI;
        


        //sine movement
        private Vector2 pos;

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public EnemyArc(Color color)
            : base(color)
        {

            //this.mType = type;

            if (color == Color.Red)
            {

            }else
            if (color == Color.Blue)
            {

            }
            else
            if (color == Color.Blue)
            {

            }

            switch (mType)
            {
                case BaseEnemy.sTYPE_SIMPLE_FLYING_RED:

                    String[] imagesStopped = new String[4];
                    imagesStopped[0] = "test\\0009";
                    imagesStopped[1] = "test\\0015";
                    imagesStopped[2] = "test\\0019";
                    imagesStopped[3] = "test\\0024";

                    String[] imagesDestroyed = new String[1];
                    imagesDestroyed[0] = "test\\eblue";

                    mSpriteNormal = new Sprite(imagesStopped, new int[] { 0,1,2,3 }, 7, 90, 90, false, false);
                    mSpriteExploding = new Sprite(imagesDestroyed, new int[] { 0 }, 3, 90, 90, true, true);
                    break;
                
            }


            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteExploding, sSTATE_EXPLODING);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(40, 40);

            pos=new Vector2(400, 0);            
        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            float distance;
            Vector2 playerPosition = getPlayerPosition();
            Vector2.Distance(ref playerPosition, ref pos, out distance);
            float radius = 200; //raio

            //if (distance > 20)
            //{
                float max = 0;

                if (theta < max)
                {
                    pos = new Vector2(radius * (float)Math.Cos((double)theta),
                           radius * (float)Math.Sin((double)theta)) + new Vector2(Game1.sSCREEN_RESOLUTION_WIDTH / 2, 440);
                    theta += 0.02f; //velocidade
                }
                else { 
                    //terminou movimento
                    //por enquanto boto para reiniciar so para testar
                    theta = (float)-Math.PI;

                }
            //}


            setLocation(pos);

            base.update(gameTime);//getCurrentSprite().update();
            //LOGICA 
        }

        public void explode()
        {
            changeState(sSTATE_EXPLODING);
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
