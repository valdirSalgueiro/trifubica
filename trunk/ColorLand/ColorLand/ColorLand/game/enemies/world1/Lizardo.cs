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

            if (color == Color.Red)
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
            }
            else
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

            base.update(gameTime);//getCurrentSprite().update();
            //LOGICA 
        }

        public void attack()
        {
            changeState(sSTATE_ATTACKING);
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
