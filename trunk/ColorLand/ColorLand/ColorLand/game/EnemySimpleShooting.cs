using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class EnemySimpleShooting : BaseEnemy
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


        //sine movement
        private Vector2 pos;
        private double destAngle = 0;
        private Bullet[] bullet=new Bullet[5];
        private int shootingFrame = 0;


        class Bullet {
            public Texture2D texture;
            public bool active;
            public Vector2 pos;
            public Bullet(ContentManager content) {
                texture = content.Load<Texture2D>("test\\egreen");
                active = false;
            }
        }

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public EnemySimpleShooting(int type)
            : base(Color.Black,new Vector2(100,100))
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

            pos=new Vector2(500, 0);            
        }



        public override void loadContent(ContentManager content)
        {
            for (int i = 0; i < 5; i++)
            {
                bullet[i] = new Bullet(content);
            }   
            
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            float distance;
            Vector2 playerPosition = getPlayerPosition();
            Vector2.Distance(ref playerPosition, ref pos, out distance);
            if (distance > 400)
            {
                destAngle = Math.Atan2(getPlayerPosition().Y - pos.Y, getPlayerPosition().X - pos.X);
                //altere "1.0f" para fazer com que ele se desloque mais rapidamente
                pos.X += 1.0f * (float)Math.Cos(destAngle);
                pos.Y += 1.0f * (float)Math.Sin(destAngle);
                shootingFrame = 0;
            }
            else {
                shootingFrame++;
                if (shootingFrame % 20 == 0) {
                    for (int i = 0; i < 5; i++)
                    {
                        if (!bullet[i].active) {
                            bullet[i].active = true;
                            bullet[i].pos.X = pos.X;
                            bullet[i].pos.Y = pos.Y;
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (bullet[i].active)
                {
                    playerPosition = getPlayerPosition();
                    Vector2.Distance(ref playerPosition, ref bullet[i].pos, out distance);
                    if (distance > 20)
                    {
                        destAngle = Math.Atan2(getPlayerPosition().Y - bullet[i].pos.Y, getPlayerPosition().X - bullet[i].pos.X);
                        bullet[i].pos.X += 1.0f * (float)Math.Cos(destAngle);
                        bullet[i].pos.Y += 1.0f * (float)Math.Sin(destAngle);
                    }
                    else
                    {
                        bullet[i].active = false;
                    }
                }
            }


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
            base.draw(spriteBatch);
            for (int i = 0; i < 5; i++)
            {
                if (bullet[i].active)
                    spriteBatch.Draw(bullet[i].texture, new Rectangle((int)bullet[i].pos.X + bullet[i].texture.Width / 2, (int)bullet[i].pos.Y + bullet[i].texture.Height / 2, bullet[i].texture.Width, bullet[i].texture.Height), Color.White);
            }
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
