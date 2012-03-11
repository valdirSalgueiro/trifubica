using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public class Explosion : GameObject
    {

        private bool mWaitingAppear = true;

        private Color mColor;

        private const int sSTATE_NORMAL = 0;

        private Sprite mSpriteNormal;

        public Explosion(Color color) {

            mColor = color;

            if (color == Color.Tomato)
            {
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(49, "enemies\\explosion\\explosion"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 49 }, 1, 500, 500, true, false);
            }
            if (color == Color.Red)
            {
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(30, "enemies\\explosion\\red\\explosion_red"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 30 }, 1, 500, 500, true, false);
            }
            if (color == Color.Green)
            {
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(30, "enemies\\explosion\\green\\explosion_green"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 30 }, 1, 500, 500, true, false);
            }
            if (color == Color.Blue)
            {
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(30, "enemies\\explosion\\blue\\explosion_blue"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 30 }, 1, 500, 500, true, false);
            }
                //mSpriteTackling = new Sprite(imagesTackling, new int[] { 0, 1, 2, 3, 4, 5 }, 1, 65, 80, true, false);

            addSprite(mSpriteNormal, sSTATE_NORMAL);
            //addSprite(mSpriteTackling, sSTATE_TACKLING);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(75, 80);
        }

        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
            
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);//getCurrentSprite().update();

            if (mSpriteNormal.getAnimationEnded() == true && !mWaitingAppear)
            {
                mSpriteNormal.resetStatus();
                mSpriteNormal.resetAnimationFlag();
                mSpriteNormal.setLocation(2000, 2000);
                
                mWaitingAppear = true;
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (mWaitingAppear == false)
            {
                base.draw(spriteBatch);
            }
        }

        public void explode(int x, int y)
        {
            mSpriteNormal.resetStatus();
            mSpriteNormal.resetAnimationFlag();                
            mWaitingAppear = false;
            setCenter(x, y);
        }

        public void explode(Vector2 location){
            explode((int)location.X, (int)location.Y);
        }

        public bool isAvailableToExplode()
        {
            return mWaitingAppear;
        }

        public Color getColor()
        {
            return this.mColor;
        }

    }
}
