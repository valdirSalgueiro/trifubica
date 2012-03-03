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
            mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(1,imgNormal), new int[] { 0 }, 7, rectArea.Width, rectArea.Height, false, false);
            mSpritePressed = new Sprite(ExtraFunctions.fillArrayWithImages(1, imgPressed), new int[] { 0 }, 7, rectArea.Width, rectArea.Height, false, false);
                        
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpritePressed, sSTATE_PRESSED);

            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(rectArea.Width,rectArea.Height);

            setLocation(rectArea.X, rectArea.Y);

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
