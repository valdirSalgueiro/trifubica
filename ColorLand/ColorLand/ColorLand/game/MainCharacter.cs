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
    class MainCharacter : PaperObject {

        private MainCharacterData mData;


        //INDEXES
        public const int sSTATE_STOPPED = 0;
        //public const int sSTATE_TACKLING = 1;
        
        //SPRITES
        private Sprite mSpriteStopped;
        //private Sprite mSpriteTackling;


        public MainCharacter()
        {

            mData = new MainCharacterData();

            String[] imagesStopped = new String[2];
            imagesStopped[0] = "test\\m1";
            imagesStopped[1] = "test\\m2";
            
            mSpriteStopped = new Sprite(imagesStopped, new int[] { 0, 1 }, 7, 65, 80, false, false);
            //mSpriteTackling = new Sprite(imagesTackling, new int[] { 0, 1, 2, 3, 4, 5 }, 1, 65, 80, true, false);
                        
            addSprite(mSpriteStopped, sSTATE_STOPPED);
            //addSprite(mSpriteTackling, sSTATE_TACKLING);

            changeToSprite(sSTATE_STOPPED);

            setCollisionRect(75, 80);

        }

        public override void loadContent(ContentManager content) {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime) {
            base.update(gameTime);//getCurrentSprite().update();

        }

        public override void draw(SpriteBatch spriteBatch) {
            base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
            //getCurrentSprite().draw(spriteBatch);
        }

        public void tackle()
        {
            //changeState(sSTATE_TACKLING);
            //changeState(sSTATE_TACKLING);
        }


        public void changeState(int state) {

            setState(state);

            switch (state) {
                case sSTATE_STOPPED:
                   // if (getCurrentSpriteIndex() != sSTATE1) {
                    changeToSprite(sSTATE_STOPPED);
                    //}
                    break;
                /*case sSTATE_TACKLING:
                    //if (getCurrentSpriteIndex() != sSTATE2) {
                    mTimeToTackleFlag = false;
                    changeToSprite(sSTATE_TACKLING);
                    //}
                    break;*/
            }

        }

        public MainCharacterData getData()
        {
            return this.mData;
        }

    }

}