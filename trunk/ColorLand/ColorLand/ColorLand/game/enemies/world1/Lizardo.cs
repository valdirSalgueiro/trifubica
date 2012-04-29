using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class Lizardo : BaseEnemy
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

        //shooting
        private double destAngle = 0;
        private Bullet[] bullet = new Bullet[1];
        private int shootingFrame = 0;


        class Bullet
        {
            public Texture2D texture;
            public bool active;
            public Vector2 pos;
            public Bullet(ContentManager content)
            {
                texture = content.Load<Texture2D>("enemies\\CrabCrab\\blue\\Crab_walk0001");
                active = false;
            }
        }


        //TODO Construir mecanismo de chamar um delegate method when finish animation


        public Lizardo(Color color)
            : this(color, new Vector2(0, 0))
        {

        }

        public Lizardo(Color color, Vector2 origin)
            : base(color, origin)
        {

            String colorString = "";

            if (color == Color.Red)
            {
                colorString = "red";
            }
            if (color == Color.Blue)
            {
                colorString = "blue";
            }
            if (color == Color.Green)
            {
                colorString = "green";
            }

            mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(2, 26, "enemies\\Lizardo\\" + colorString + "\\walk\\lizard_walk"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 26 }, 1, 100, 100, false, false);
            mSpriteAttacking = new Sprite(ExtraFunctions.fillArrayWithImages2(0, 40, "enemies\\Lizardo\\" + colorString + "\\attack\\lizard_attack"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 40 }, 1, 100, 100, false, false);

            addSprite(mSpriteWalking, sSTATE_WALKING);
            addSprite(mSpriteAttacking, sSTATE_ATTACKING);

            changeToSprite(sSTATE_WALKING);

            setCenterHotspot(new Vector2(274, 101));
            setCollisionRect(30, 30, 220, 80);
            setLocation(origin);

        }



        public override void loadContent(ContentManager content)
        {
            for (int i = 0; i < 1; i++)
            {
                bullet[i] = new Bullet(content);
            }

            base.loadContent(content);
            SoundManager.LoadSound(cSOUND_PATADA);
        }

        public override void update(GameTime gameTime)
        {
            float distance;
            Vector2 playerPosition = getPlayerPosition();
            Vector2 pos = getLocation();
            Vector2.Distance(ref playerPosition, ref pos, out distance);
            if (distance > 200)
            {
                destAngle = Math.Atan2(playerPosition.Y - pos.Y, playerPosition.X - pos.X);
                //altere "1.0f" para fazer com que ele se desloque mais rapidamente
                mX += 1.0f * (float)Math.Cos(destAngle);
                mY += 1.0f * (float)Math.Sin(destAngle);

                changeState(sSTATE_WALKING);

            }
            else
            {
                changeState(sSTATE_ATTACKING);
                attack();
            }

            for (int i = 0; i < bullet.Count(); i++)
            {
                if (bullet[i].active)
                {
                    playerPosition = getPlayerPosition();
                    Vector2.Distance(ref playerPosition, ref bullet[i].pos, out distance);
                    if (distance > 20)
                    {
                        destAngle = Math.Atan2(playerPosition.Y - bullet[i].pos.Y, playerPosition.X - bullet[i].pos.X);
                        //altere "2.0f" para fazer com que ele se desloque mais rapidamente
                        bullet[i].pos.X += 2.0f * (float)Math.Cos(destAngle);
                        bullet[i].pos.Y += 2.0f * (float)Math.Sin(destAngle);
                    }
                    else
                    {
                        bullet[i].active = false;
                    }
                }
            }

            ////check flip
            Vector2 playerLocation = getPlayerPosition();


            //PLAYER IS LEFT FROM CRAB
            if (getPlayerCenter() < getCenterX())
            {
                //setFlipDirection(FlipDirection.Left);
                if (getCurrentSprite().isFlipped())
                {
                    setFlip(false);
                    getCurrentSprite().resetAnimationFlag();
                }
            }
            else
            {
                // getCurrentSprite().setFlip(true);
                if (!getCurrentSprite().isFlipped())
                {
                    setFlip(true);
                    getCurrentSprite().resetAnimationFlag();
                }
            }


            base.update(gameTime);//getCurrentSprite().update();
            //LOGICA 
        }

        public void attack()
        {

            Vector2 pos = getLocation();

            if (getState() == sSTATE_ATTACKING)
            {
                if (getCurrentSprite().getCurrentFrame() == 24)
                {
                    for (int i = 0; i < bullet.Count(); i++)
                    {
                        if (!bullet[i].active)
                        {
                            bullet[i].active = true;
                            bullet[i].pos.X = pos.X;
                            bullet[i].pos.Y = pos.Y;
                            break;
                        }
                    }
                }
                
            }
            
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bullet.Count(); i++)
            {
                if (bullet[i].active)
                    //spriteBatch.Draw(bullet[i].texture, new Rectangle((int)bullet[i].pos.X + bullet[i].texture.Width / 2, (int)bullet[i].pos.Y + bullet[i].texture.Height / 2, bullet[i].texture.Width, bullet[i].texture.Height), Color.White);
                    spriteBatch.Draw(bullet[i].texture, new Rectangle((int)bullet[i].pos.X, (int)bullet[i].pos.Y, bullet[i].texture.Width, bullet[i].texture.Height), Color.White);
            }

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
                    if (getState() != sSTATE_WALKING)
                    {
                        setState(sSTATE_WALKING);
                        changeToSprite(sSTATE_WALKING);
                    }
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


    public class LizardoBullet : BaseEnemy
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
        float theta = (float)-Math.PI;
        
        //sine movement
        private Vector2 pos;

        //TODO Construir mecanismo de chamar um delegate method when finish animation

        public LizardoBullet(Color color)
            : base(color)
        {

            //this.mType = type;

            if (color == Color.Red)
            {

            }
            else
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

                    mSpriteNormal = new Sprite(imagesStopped, new int[] { 0, 1, 2, 3 }, 7, 90, 90, false, false);
                    mSpriteExploding = new Sprite(imagesDestroyed, new int[] { 0 }, 3, 90, 90, true, true);
                    break;

            }


            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteExploding, sSTATE_EXPLODING);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(40, 40);

            pos = new Vector2(400, 0);
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
            else
            {
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
