using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;


namespace ColorLand
{
	class Button : PaperObject
	{

        //INDEXES
        public const int sSTATE_NORMAL  = 0;
        public const int sSTATE_PRESSED = 1;

        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpritePressed;


        public Button(String imgNormal, String imgPressed, Rectangle rectArea)
        {

            String[] imagesNormal = new String[1];
            imagesNormal[0] = imgNormal;

            String[] imagesPressed = new String[1];
            imagesPressed[0] = imgPressed;

            mSpriteNormal = new Sprite(imagesNormal, new int[] { 0,0 }, 7, rectArea.Width, rectArea.Height, false, false);
            mSpritePressed = new Sprite(imagesPressed, new int[] { 0,0 }, 7, rectArea.Width, rectArea.Height, false, false);
                        
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpritePressed, sSTATE_PRESSED);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(rectArea.Width,rectArea.Height);

            setLocation(rectArea.X, rectArea.Y);

        }


        public void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }

        public void update(GameTime gameTime)
        {
            base.update(gameTime);//getCurrentSprite().update();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
            //getCurrentSprite().draw(spriteBatch);
        }


        public void changeState(int state)
        {

            setState(state);

            switch (state)
            {
                case sSTATE_NORMAL:
                    changeToSprite(sSTATE_NORMAL);
                    break;
                case sSTATE_PRESSED:
                    changeToSprite(sSTATE_PRESSED);
                    break;
            }

        }

	}
}
