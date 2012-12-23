using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public class Kakto : BaseEnemy
    {

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //SPRITES
        private Sprite mSpriteWalking;

        public const int sSTATE_WALKING = 0;

        //Specific
        private int mType;
        private int mInitX;
        private int mInitY;
        private const int cHORIZONTAL_MARGIN = 40;

        float dy;
        float ay=9.8f;

        const int SPACING = 53;

        public enum TYPE { 
            UP,
            MIDDLE,
            DOWN
        }

        int downAnimations=0;


        TYPE type;
        Vector2 origin;
        bool left;
        bool localLeft;
        Vector2 local;


        //TODO Construir mecanismo de chamar um delegate method when finish animation

        
            

        
        public Kakto(Color color, TYPE type) : this (color, new Vector2(0,0), type)
        {
            
        }

        public Kakto(Color color, Vector2 origin_, TYPE type_)
            : base(color, origin_)
        {
            type = type_;
            origin = origin_;
            left = false;
            localLeft = false;
            Random rand = new Random(DateTime.Now.Millisecond);

            if(color == Color.Red)
            {
                switch(rand.Next(0,3)){
                    case 0:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Red\\03\\red_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                        break;
                    case 1:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Red\\01\\red_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                    break;
                    case 2:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Red\\02\\red_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                    break;

                }
            }
            if (color == Color.Blue)
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Blue\\03\\blue_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                        break;
                    case 1:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Blue\\01\\blue_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                        break;
                    case 2:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Blue\\02\\blue_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                        break;
                }
            }
            if (color == Color.Green)
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Green\\03\\green_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                        break;
                    case 1:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Green\\01\\green_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                        break;
                    case 2:
                        mSpriteWalking = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 5, "enemies\\Kaktos\\Green\\02\\green_kaktos"), new int[] { 0, 1, 2, 3, 4 }, 9, 95, 95, false, false);
                        break;
                }
                
            }

            addSprite(mSpriteWalking, sSTATE_WALKING);

            changeToSprite(sSTATE_WALKING);

            setCenterHotspot(new Vector2(274, 101));
            setCollisionRect(0,0,90,90);
            setLocation(origin);

        }

        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
            //SoundManager.LoadSound(cSOUND_PATADA);
        }

        public override void update(GameTime gameTime)
        {
            
            if (!isGrowingUp())
            {
                Vector2 playerLocation = getPlayerPosition();

                if (local.Y < downAnimations * SPACING)
                {
                    dy += ay;
                    local.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * dy;
                }
                else {
                    dy = 0;
                }


                if (!left)
                {
                    if (mX > getCurrentSprite().getWidth())
                        mX--;
                    else
                        left = true;
                }else
                {
                    if (mX < 800 - getCurrentSprite().getWidth())
                        mX++;
                    else
                        left = false;
                }

                if (type == TYPE.UP)
                {
                    if (!localLeft)
                    {
                        if (local.X < 20)
                            local.X += gameTime.ElapsedGameTime.Milliseconds * 0.05f;
                        else
                            localLeft = true;
                    }
                    else
                    {
                        if (local.X > -20)
                            local.X -= gameTime.ElapsedGameTime.Milliseconds * 0.05f;
                        else
                            localLeft = false;
                    }

                    mY = origin.Y + local.Y;
                }
                if (type == TYPE.MIDDLE)
                {
                    mY = origin.Y + SPACING + local.Y;
                }
                if (type == TYPE.DOWN)
                {
                    if (localLeft)
                    {
                        if (local.X < 20)
                            local.X += gameTime.ElapsedGameTime.Milliseconds * 0.05f;
                        else
                            localLeft = false;
                    }
                    else
                    {
                        if (local.X > -20)
                            local.X -= gameTime.ElapsedGameTime.Milliseconds * 0.05f;
                        else
                            localLeft = true;
                    }
                    mY = origin.Y + SPACING*2;
                }

                getCurrentSprite().offset.X = local.X;
               
            }



            base.update(gameTime);//getCurrentSprite().update();
            //LOGICA 
            
        }

        public void goDown(int down) {
            downAnimations = down;
        }

        public void attack()
        {                        
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
