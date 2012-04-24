using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class SoundIcon : GameObject
    {

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_NORMAL = 0;
        public const int sSTATE_SHAKING = 1;
        public const int sSTATE_OFF = 2;


        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpriteShaking;
        private Sprite mSpriteOff;

        public SoundIcon(Vector2 position)
        {

            mSpriteNormal =  new Sprite(ExtraFunctions.fillArrayWithImages2(1, "mainmenu\\sound_on_on"), new int[] { 0 }, 9, 100, 65, false, false);
            mSpriteShaking = new Sprite(ExtraFunctions.fillArrayWithImages2(8, "mainmenu\\sound_on_on"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 8 }, 1, 100, 65, false, false);
            mSpriteOff =     new Sprite(ExtraFunctions.fillArrayWithImages2(1, "mainmenu\\off_sound"), new int[] { 0 }, 9, 100, 65, false, false);
                        
            addSprite(mSpriteNormal,  sSTATE_NORMAL);
            addSprite(mSpriteShaking, sSTATE_SHAKING);
            addSprite(mSpriteOff, sSTATE_OFF);
            //addSprite(mSpriteExploding, sSTATE_EXPLODING);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(25, 8, 56,60);

            setLocation(position);
            setVisible(true);
            
        }



        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }



        public override void update(GameTime gameTime)
        {
            base.update(gameTime);//getCurrentSprite().update();
        }

        public override void draw(SpriteBatch spriteBatch)
        {

            if (isVisible())
           {
               base.draw(spriteBatch);
           }
        }

        public void changeState(int state)
        {

            switch (state)
            {
                case sSTATE_NORMAL:
                    if (getState() != sSTATE_NORMAL)
                    {
                        setState(sSTATE_NORMAL);
                        changeToSprite(sSTATE_NORMAL);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;
                case sSTATE_SHAKING:
                    if (getState() != sSTATE_SHAKING)
                    {
                        setState(sSTATE_SHAKING);
                        changeToSprite(sSTATE_SHAKING);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;
                case sSTATE_OFF:
                    if (getState() != sSTATE_OFF)
                    {
                        setState(sSTATE_OFF);
                        changeToSprite(sSTATE_OFF);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;                

            }

        }

    }

}
