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
	class Button : GameObject
	{

        //INDEXES
        public const int sSTATE_NORMAL   = 0;
        public const int sSTATE_HIGHLIGH = 1;
        public const int sSTATE_PRESSED = 2;

        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpriteHighlight;
        private Sprite mSpritePressed;

        private Rectangle mRectArea;

        public Button(String imgNormal, String imgPressed, Rectangle rectArea)
        {
            mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages(1,imgNormal), new int[] { 0 }, 7, rectArea.Width, rectArea.Height, false, false);
            mSpritePressed = new Sprite(ExtraFunctions.fillArrayWithImages(1, imgPressed), new int[] { 0 }, 7, rectArea.Width, rectArea.Height, false, false);
                        
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpritePressed, sSTATE_PRESSED);

            changeToSprite(sSTATE_NORMAL);

            //setCollisionRect(rectArea.Width,rectArea.Height);

            setCollisionRect(0, 0, rectArea.Width, rectArea.Height);

            setLocation(rectArea.X, rectArea.Y);


            mRectArea = rectArea;
        }

        public Button(String imgNormal, String imgHighlight, String imgPressed, Rectangle rectArea)
        {
            mSpriteNormal = new Sprite(new String[]{imgNormal}, new int[] { 0 }, 7, rectArea.Width, rectArea.Height, false, false);
            mSpriteHighlight = new Sprite(new String[]{imgHighlight}, new int[] { 0 }, 7, rectArea.Width, rectArea.Height, false, false);
            mSpritePressed = new Sprite(new String[]{imgPressed}, new int[] { 0 }, 7, rectArea.Width, rectArea.Height, false, false);

            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteHighlight, sSTATE_HIGHLIGH);
            addSprite(mSpritePressed, sSTATE_PRESSED);

            changeToSprite(sSTATE_NORMAL);

            //setCollisionRect(rectArea.Width,rectArea.Height);

            setCollisionRect(0, 0, rectArea.Width, rectArea.Height);
            //setCollisionRect(300, 300);
            setLocation(rectArea.X, rectArea.Y);


            mRectArea = rectArea;
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
                case sSTATE_HIGHLIGH:
                    changeToSprite(sSTATE_HIGHLIGH);
                    break;
                case sSTATE_PRESSED:
                    changeToSprite(sSTATE_PRESSED);
                    break;
            }

        }

        public Rectangle getRectangle()
        {
            return this.mRectArea;
        }

	}
}
