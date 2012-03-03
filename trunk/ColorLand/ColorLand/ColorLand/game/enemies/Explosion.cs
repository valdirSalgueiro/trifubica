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




        public Explosion() {

            String[] imagesStopped = new String[9];

            for (int x = 0; x < imagesStopped.Length; x++)
            {
                imagesStopped[x] = "test\\e" + (x+1);
            }
            
            mSpriteNormal = new Sprite(imagesStopped, new int[] { 0,1,2,3,4,5,6,7,8 }, 7, 90, 90, true, false);
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
            setLocation(x, y);
        }

        public bool isAvailableToExplode()
        {
            return mWaitingAppear;
        }


    }
}
