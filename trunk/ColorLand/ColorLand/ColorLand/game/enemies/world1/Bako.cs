using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public class Bako : BaseEnemy
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
        private Vector2 oldPosition;
        private Vector2 pos;
        private Vector2 spritePos;
        private float x = 0;
        private double destAngle = 0;

        private double posAngle = 0;


        public Bako(Color color)
            : this(color, new Vector2(0, 0))
        {
            
        }

        public Bako(Color color, Vector2 origin)
            : base(color,origin)
        {

           if(color == Color.Red){
               mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(8, "enemies\\all\\red\\bako"), new int[] { 0,1,2,3,4,5,6,7,6,5,4,3,2,1 }, 3, 200, 200, false, false);
               //mSpriteExploding = new Sprite(imagesDestroyed, new int[] { 0 }, 3, 90, 90, true, true);
           }

           if (color == Color.Green)
           {
               mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(8, "enemies\\all\\green\\bako"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 6, 5, 4, 3, 2, 1 }, 3, 300, 300, false, false);
               //mSpriteExploding = new Sprite(imagesDestroyed, new int[] { 0 }, 3, 90, 90, true, true);
           }

           if (color == Color.Blue)
           {
               mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(8, "enemies\\all\\blau\\bako"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 6, 5, 4, 3, 2, 1 }, 3, 300, 300, false, false);
               //mSpriteExploding = new Sprite(imagesDestroyed, new int[] { 0 }, 3, 90, 90, true, true);
           }
 

           addSprite(mSpriteNormal, sSTATE_NORMAL);
           //addSprite(mSpriteExploding, sSTATE_EXPLODING);

           changeToSprite(sSTATE_NORMAL);

           setCollisionRect(90,90,111,110);

           oldPosition = spritePos = pos = origin;
           //pos = new Vector2(0, 0);
        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            float distance;
            Vector2 playerPosition = getPlayerPosition();
            Vector2.Distance(ref playerPosition, ref spritePos, out distance);
            Console.WriteLine(distance);
            if (distance > 60)
            {
                pos = oldPosition;
                //altere aqui para fazer a onda do seno mais rapidamente/devagarmente :p
                x += 0.1f;

                destAngle = Math.Atan2(getPlayerPosition().Y - pos.Y, getPlayerPosition().X - pos.X);
                if (destAngle < 0){
                    destAngle = posAngle;
                }else{
                    posAngle=destAngle;
                }

                //altere "1.0f" para fazer com que ele se desloque mais rapidamente
                pos.X += 2.0f * (float)Math.Cos(destAngle);
                pos.Y += 2.0f * (float)Math.Sin(destAngle);

                Vector2 direction = getPlayerPosition() - oldPosition;

                Vector2 perpendicular = new Vector2(direction.Y, -direction.X);
                perpendicular.Normalize();

                //faz um seno de "75 pixels"
                float offset = 75.0f * (float)Math.Sin(x);

                spritePos = pos + (offset * perpendicular);
                oldPosition = pos;
            }
            else {
                //Console.WriteLine("colidiu");
            }


            //PLAYER IS LEFT FROM CRAB
            if (getPlayerCenter() < getCenterX())
            {
                getCurrentSprite().setFlip(true);
            }
            else
            {
                getCurrentSprite().setFlip(false);
            }

            setLocation(spritePos);

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


        public Vector2 getPlayerPosition()
        {
            BaseScreen currentScreen = Game1.getInstance().getScreenManager().getCurrentScreen();
            if (currentScreen is GamePlayScreen)
            {
                return ((GamePlayScreen)currentScreen).getPlayerLocation();
            }

            return new Vector2();
        }


        public void setInitXY(int x, int y)
        {
            this.mInitX = x;
            this.mInitY = y;
        }

    }
}
