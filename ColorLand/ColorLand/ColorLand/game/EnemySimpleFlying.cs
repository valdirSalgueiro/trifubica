using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class EnemySimpleFlying : BaseEnemy
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


        //spline
        private List<Vector2> points=new List<Vector2>();
        private Vector2 oldPosition;
        private Vector2 pos;
        private Vector2 spritePos;
        private int pathIter = 0;
        private float x = 0;
        private double destAngle = 0;

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public EnemySimpleFlying(int type)
            : base(type)
        {

            this.mType = type;

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
                case BaseEnemy.sTYPE_SIMPLE_FLYING_GREEN:

                    break;
                case BaseEnemy.sTYPE_SIMPLE_FLYING_BLUE:

                    break;
            }


            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteExploding, sSTATE_EXPLODING);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(40, 40);

            pos=new Vector2(0, 0);

            points.Add(new Vector2(32, 32));
            points.Add(new Vector2(32, -32));    

            
        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            //if (x > 1.0f)
            //{
            //    //x = x-1.0f;
            //    x = 0;
            //    //Console.WriteLine(" " + x);
            //    oldPosition = pos;
            //    pathIter++;
            //    if (pathIter >= points.Count())
            //        pathIter = 0;
            //}
            //else
            //{
            //    Vector2 a;
            //    a.X = points.ElementAt(pathIter).X * (float)Math.Cos(destAngle) - points.ElementAt(pathIter).Y * (float)Math.Sin(destAngle);
            //    a.Y = points.ElementAt(pathIter).X * (float)Math.Sin(destAngle) + points.ElementAt(pathIter).Y * (float)Math.Cos(destAngle);
            //    Vector2 nextPos = oldPosition + a;
            //    pos = Vector2.CatmullRom(oldPosition, oldPosition, nextPos, nextPos, x);
            //}

            ////x += 1*gameTime.ElapsedGameTime.Milliseconds/1000.0f;
            //x += 0.05f;
            //setLocation(pos);

            //descomente para andar em circulos de raio 100
            //x += 0.05f;
            //pos=new Vector2((float)Math.Cos(x) * 100, (float)Math.Sin(x) * 100);
            //pos += new Vector2(100, 100);
            //setLocation(pos);

            //descomente para andar em seno de largura 20 e altura 100
            //x += 0.5f;
            //pos = new Vector2((float)Math.Sin(x) * 100, x * 10);


            pos = oldPosition;
            //altere aqui para fazer a onda do seno mais rapidamente/devagarmente :p
            x += 0.1f;
            
            destAngle = Math.Atan2(getPlayerPosition().Y - pos.Y, getPlayerPosition().X - pos.X);
            //altere "1.0f" para fazer com que ele se desloque mais rapidamente
            pos.X += 1.0f * (float)Math.Cos(destAngle);
            pos.Y += 1.0f * (float)Math.Sin(destAngle);

            Vector2 direction = getPlayerPosition() - oldPosition;

            Vector2 perpendicular = new Vector2(direction.Y, -direction.X);
            perpendicular.Normalize();

            //faz um seno de "75 pixels"
            float offset = 75.0f * (float)Math.Sin(x);
            spritePos = pos + (offset * perpendicular);
            oldPosition = pos;

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
