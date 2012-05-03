using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ColorLand.managers;

namespace ColorLand
{
    class Lizardo : BaseEnemy
    {

        private const String cSOUND_PATADA = "sound\\fx\\patada8bit";

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_WALKING = 0;
        public const int sSTATE_ATTACKING = 1;

        float dy;
        float ay = 9.8f;


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
        private int shootingFrame = 0;

        private bool shotBullet;


        public Color color_;

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
            color_ = color;

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
            setCollisionRect(20, 20, 56, 56);
            setLocation(origin);

        }



        public override void loadContent(ContentManager content)
        {

            base.loadContent(content);
            SoundManager.LoadSound(cSOUND_PATADA);
        }

        public override void update(GameTime gameTime)
        {
            float distance;
            Vector2 playerPosition = getPlayerPosition();
            Vector2 pos = getLocation();
            Vector2.Distance(ref playerPosition, ref pos, out distance);
            if (distance > 250)
            {
                destAngle = Math.Atan2(playerPosition.Y - pos.Y, playerPosition.X - pos.X);
                //altere "1.0f" para fazer com que ele se desloque mais rapidamente
                mX += 1.0f * (float)Math.Cos(destAngle);

                changeState(sSTATE_WALKING);
            }
            else
            {
                changeState(sSTATE_ATTACKING);
                attack();
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
                if (getCurrentSprite().getCurrentFrame() == 24 && !shotBullet)
                {
                    RockManager.getInstance().createObject(pos + new Vector2(30,20), new Vector2(getCurrentSprite().isFlipped()? 10:-10,-200),color_);
                    shotBullet = true;
                }
                else
                    shotBullet = false;
            }
            
            
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
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

    }



}
