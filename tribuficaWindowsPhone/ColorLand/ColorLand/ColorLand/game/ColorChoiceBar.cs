using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class ColorChoiceBar : PaperObject {

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        //INDEXES
        public const int sSTATE_R = 0;
        public const int sSTATE_G = 1;
        public const int sSTATE_B = 2;
        

        //SPRITES
        private Sprite mSpriteR;
        private Sprite mSpriteG;
        private Sprite mSpriteB;
        
        //TODO: futuramente gerenciar um alpha

        public ColorChoiceBar()
        {

            String[] imagesR = new String[1];
            imagesR[0] = "test\\rgb1";

            String[] imagesG = new String[1];
            imagesG[0] = "test\\rgb2";

            String[] imagesB = new String[1];
            imagesB[0] = "test\\rgb3";
            
            mSpriteR = new Sprite(imagesR, new int[] { 0 }, 7, 100, 40, false, false);
            mSpriteG = new Sprite(imagesG, new int[] { 0 }, 7, 100, 40, false, false);
            mSpriteB = new Sprite(imagesB, new int[] { 0 }, 7, 100, 40, false, false);
            
            addSprite(mSpriteR, sSTATE_R);
            addSprite(mSpriteG, sSTATE_G);
            addSprite(mSpriteB, sSTATE_B);
           
            changeToSprite(sSTATE_R);

            setCollisionRect(40, 40);
        }

        public override void loadContent(ContentManager content) {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime) {

            base.update(gameTime);//getCurrentSprite().update();
            //LOGICA 
        }

        public override void draw(SpriteBatch spriteBatch) {
            base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
        }

        public void changeState(int state) {

            switch (state) {
                case sSTATE_R:
                    setState(sSTATE_R);
                    changeToSprite(sSTATE_R);
                    break;
                case sSTATE_G:
                    setState(sSTATE_G);
                    changeToSprite(sSTATE_G);
                    break;
                case sSTATE_B:
                    setState(sSTATE_B);
                    changeToSprite(sSTATE_B);
                    break;
            }

        }

    }

}
