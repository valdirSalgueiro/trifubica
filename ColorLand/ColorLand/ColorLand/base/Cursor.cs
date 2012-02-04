using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ColorLand
{
    class Cursor : PaperObject
    {

        //private bool mKinectBased = false;

          //INDEXES
        public const int sSTATE_NORMAL = 0;

        private Color mCurrentColor;

        public const int sSTATE_BLUE = 1;
        public const int sSTATE_GREEN = 2;
        public const int sSTATE_RED = 3;
        
        //SPRITES
        private Sprite mSpriteNormal;
        private Sprite mSpriteBlue;
        private Sprite mSpriteGreen;
        private Sprite mSpriteRed;

        public Cursor()
        {
            mSpriteNormal = new Sprite(1, "gameplay\\cursors\\cursor", new int[] { 0 }, 10, 32, 32);
            mSpriteBlue   = new Sprite(1, "gameplay\\cursors\\cursorblue", new int[] { 0 }, 10, 32, 32);
            mSpriteGreen  = new Sprite(1, "gameplay\\cursors\\cursorgreen", new int[] { 0 }, 10, 32, 32);
            mSpriteRed    = new Sprite(1, "gameplay\\cursors\\cursorred", new int[] { 0 }, 10, 32, 32);
            
            addSprite(mSpriteNormal, sSTATE_NORMAL);
            addSprite(mSpriteBlue,   sSTATE_BLUE);
            addSprite(mSpriteGreen, sSTATE_GREEN);
            addSprite(mSpriteRed, sSTATE_RED);
            
            changeToSprite(sSTATE_NORMAL);

            setCollisionRect(32, 32);
        }

        public void loadContent(ContentManager content) {
            base.loadContent(content);
        }

        public void update(GameTime gameTime) {
            base.update(gameTime);//getCurrentSprite().update();

            if (!Game1.sKINECT_BASED)
            {
                MouseState mouseState = Mouse.GetState();
                setLocation(mouseState.X, mouseState.Y);
            }
            else
            {
                setLocation(Game1.sMAIN_HAND_COORD.X, Game1.sMAIN_HAND_COORD.Y);
            }
        }

        public void draw(SpriteBatch spriteBatch) {
           base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
        }


        public void changeColor(Color color)
        {

            mCurrentColor = color;
            if (color == Color.Blue) changeToSprite(sSTATE_BLUE);
            if (color == Color.Green) changeToSprite(sSTATE_GREEN);
            if (color == Color.Red) changeToSprite(sSTATE_RED);
            
        }

    }
}
