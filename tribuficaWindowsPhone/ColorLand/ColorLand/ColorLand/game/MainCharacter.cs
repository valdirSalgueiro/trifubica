using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using ColorLand.game;


namespace ColorLand
{
    public class MainCharacter : GameObject {

        private const int cHURT_TIME = 3; //seconds

        private MainCharacterData mData;
        private Feet mFeet;

        private const float cJUMP_SPEED = -.95f;
        
        private bool mWalking;
        private bool mJumping;
        
        //private float mDx;
        private float mDy;// = 0.09f;

        private bool mHurt;
        private int  mBlinkTotalTime;
        private int  mBlinkSpeed;


        private MTimer mTimerTotalHurtTime;//total blink
        private MTimer mTimerBlinkSpeed;//blink speed

        //INDEXES
        public const int sSTATE_STOPPED = 0;
        public const int sSTATE_TOP_LEFT = 1;
        public const int sSTATE_TOP_RIGHT = 2;
        public const int sSTATE_BOTTOM_LEFT = 3;
        public const int sSTATE_BOTTOM_RIGHT = 4;

        public const int sSTATE_VICTORY = 5;
        public const int sSTATE_LOSE    = 6;

        public const int sSTATE_INVERSE_TOP_LEFT = 7;
        public const int sSTATE_INVERSE_TOP_RIGHT = 8;

        //public const int sSTATE_TACKLING = 1;
        public const int sSTATE_MOVING = 1;
        
        //SPRITES
        private Sprite mSpriteStopped;
        private Sprite mSpriteTopLeft;
        private Sprite mSpriteTopRight;
        private Sprite mSpriteDownLeft;
        private Sprite mSpriteDownRight;
        private Sprite mSpriteVictory;
        private Sprite mSpriteDefeat;
        private Sprite mSpriteInverseTopLeft;
        private Sprite mSpriteInverseTopRight;
        //private Sprite mSpriteTackling;

        

        private float mYbeforeJump;
        private float mJumpSpeed;


        /*private Texture2D mLeftHandTexture;
        private Texture2D mLeftHandTextureWithBrush;
        private float mLeftHandAngle;
        */
          
        private Texture2D mRightHandTexture;
        private Texture2D mRightHandTextureWithBrush;
        private float mRightHandAngle;

        KeyboardState oldState;

        private Color mColor;

        /**SPECIFIC: body state (for cursor) ***/
        private BODYSTATE mBodyState;
        
        public enum BODYSTATE
        {
            UP_LEFT,
            UP_RIGHT,
            DOWN_LEFT,
            DOWN_RIGHT
        }

        public MainCharacter(Color color)
        {

            mColor = color;

            mData = new MainCharacterData();

            String folderColor = "";

            if (mColor == Color.Blue) folderColor = "blue";
            if (mColor == Color.Green) folderColor = "green";
            if (mColor == Color.Red) folderColor = "red";


            int width = 200;
            int height = 200;

            mSpriteStopped = new Sprite(ExtraFunctions.fillArrayWithImages2(1, "gameplay\\maincharacter\\"+ folderColor +"\\body"), new int[] { 0 }, 7, width, height, false, false);
            mSpriteTopLeft = new Sprite(ExtraFunctions.fillArrayWithImages2(50, 4, "gameplay\\maincharacter\\" + folderColor + "\\body"), new int[] { 0, 1, 2, 3 }, 2, width, height, true, false);
            mSpriteTopRight = new Sprite(ExtraFunctions.fillArrayWithImages2(17, 5, "gameplay\\maincharacter\\" + folderColor + "\\body"), new int[] { 0, 1, 2, 3, 4 }, 2, width, height, true, false);
            mSpriteDownLeft = new Sprite(ExtraFunctions.fillArrayWithImages2(54, 4, "gameplay\\maincharacter\\" + folderColor + "\\body"), new int[] { 0, 1, 2, 3 }, 2, width, height, true, false);
            mSpriteDownRight = new Sprite(ExtraFunctions.fillArrayWithImages2(22, 4, "gameplay\\maincharacter\\" + folderColor + "\\body"), new int[] { 0, 1, 2, 3 }, 2, width, height, true, false);

            mSpriteVictory = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 80, "gameplay\\maincharacter\\" + folderColor + "\\win\\" + folderColor + "_win"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 80 }, 1, width, height, true, false);
            mSpriteDefeat = new Sprite(ExtraFunctions.fillArrayWithImages2(1, 89, "gameplay\\maincharacter\\" + folderColor + "\\loose\\lose"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 89 }, 1, width, height, true, false);

            mSpriteInverseTopLeft = new Sprite(ExtraFunctions.fillArrayWithImages2(50, 4, "gameplay\\maincharacter\\" + folderColor + "\\body"), new int[] { 3, 2 }, 2, width, height, true, false);
            mSpriteInverseTopRight = new Sprite(ExtraFunctions.fillArrayWithImages2(17, 5, "gameplay\\maincharacter\\" + folderColor + "\\body"), new int[] { 4, 3 }, 2, width, height, true, false);


            addSprite(mSpriteStopped, sSTATE_STOPPED);
            addSprite(mSpriteTopLeft, sSTATE_TOP_LEFT);
            addSprite(mSpriteTopRight, sSTATE_TOP_RIGHT);
            addSprite(mSpriteDownLeft, sSTATE_BOTTOM_LEFT);
            addSprite(mSpriteDownRight, sSTATE_BOTTOM_RIGHT);
            addSprite(mSpriteVictory, sSTATE_VICTORY);
            addSprite(mSpriteDefeat, sSTATE_LOSE);
            addSprite(mSpriteInverseTopLeft, sSTATE_INVERSE_TOP_LEFT);
            addSprite(mSpriteInverseTopRight, sSTATE_INVERSE_TOP_RIGHT);

            changeToSprite(sSTATE_STOPPED);

            setCollisionRect(70, 75, 70, 90);

            mFeet = new Feet(color);

            //hurt();



        }

        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
            mFeet.loadContent(content);
            //mLeftHandTexture = content.Load<Texture2D>("gameplay\\maincharacter\\hands\\HandLeft");
            mRightHandTexture = content.Load<Texture2D>("gameplay\\maincharacter\\hand\\hand");
        }

        public override void update(GameTime gameTime) {


            //corrige bug do scrolling
            if(mX <= GamePlayScreen.sCURRENT_STAGE_X){
                //moveRight(1f);
                //mX = GamePlayScreen.sCURRENT_STAGE_X;
                //mX = GamePlayScreen.sCURRENT_STAGE_X_PROGRESSIVE;
                mX += gameTime.ElapsedGameTime.Milliseconds * 0.19f;
            }

            mY += mDy * gameTime.ElapsedGameTime.Milliseconds;
            base.update(gameTime);//getCurrentSprite().update();

            if (getState() != sSTATE_VICTORY && getState() != sSTATE_LOSE)
            {
                //UpdateInput();
                UpdateInput(gameTime);
                updateHand();
                updateFeet(gameTime);

                if (mTimerTotalHurtTime != null)
                {
                    mTimerTotalHurtTime.update(gameTime);
                    if (mTimerTotalHurtTime.getTimeAndLock(cHURT_TIME))
                    {
                        mHurt = false;
                    }
                }

                //trata piscadeira do personagem
                if (mHurt)
                {
                    if (mTimerBlinkSpeed != null)
                    {
                        mTimerBlinkSpeed.update(gameTime);

                        if (mTimerBlinkSpeed.getTimeAndLock(0.1))
                        {
                            setVisible(false);
                            mFeet.setVisible(false);
                        }
                        if (mTimerBlinkSpeed.getTimeAndLock(0.3))
                        {
                            setVisible(true);
                            mFeet.setVisible(true);
                            mTimerBlinkSpeed.start();
                        }
                    }
                }
                else
                {
                    setVisible(true);
                    mFeet.setVisible(true);
                    if (mTimerBlinkSpeed != null)
                    {
                        mTimerBlinkSpeed.stop();
                    }
                }
            }
            
        }  
        

        private void updateFeet(GameTime gametime)
        {
            mFeet.mX = mX + 10;
            mFeet.mY = mY + 80;

            if (!Game1.sKINECT_BASED)
            {
                if (mWalking)
                {
                    if (getDirection() == Direction.LEFT)
                    {
                        if (mFeet.getState() != Feet.sSTATE_WALKING_FEET_LEFT)
                        {
                            mFeet.changeState(Feet.sSTATE_WALKING_FEET_LEFT);
                        }
                    }

                    if (getDirection() == Direction.RIGHT)
                    {
                        if (mFeet.getState() != Feet.sSTATE_WALKING_FEET_RIGHT)
                        {
                            mFeet.changeState(Feet.sSTATE_WALKING_FEET_RIGHT);
                        }
                    }
                }
                else
                {

                    if (mFeet.getState() != Feet.sSTATE_STOPPED_FEET)
                    {
                        mFeet.changeState(Feet.sSTATE_STOPPED_FEET);
                    }

                }
                //}
            }
            mFeet.update(gametime);
        }


        public override void draw(SpriteBatch spriteBatch) {

            if (getState() != sSTATE_VICTORY && getState() != sSTATE_LOSE)
            {
                Vector2 vec = new Vector2(mX + 53, mY+ 60);
                float dx= vec.X + (float)Math.Cos(mRightHandAngle) * 70;
                float dy= vec.Y + (float)Math.Sin(mRightHandAngle) * 70;

                spriteBatch.Draw(mRightHandTexture, new Vector2(dx + 10 , dy + 40), null, Color.White, mRightHandAngle + (float)Math.PI / 2, new Vector2(13, 22), 1.2f, SpriteEffects.None, 0f);
                mFeet.draw(spriteBatch);
            }

            base.draw(spriteBatch);
        }

        public void updateHand()
        {
            //cursor.getLocation()
            Vector2 directionRightHand = new Vector2(Mouse.GetState().X,Mouse.GetState().Y) - new Vector2(mX + 100, mY + 100);
            float angleHandCursor = (float)(Math.Atan2(directionRightHand.Y, directionRightHand.X));
            mRightHandAngle = angleHandCursor;

            /*if( !(Mouse.GetState().Y > mY + 90 && (Mouse.GetState().X > mX-125 && Mouse.GetState().X < mX + 230))){
                mRightHandAngle = angleHandCursor;
            }else{
                
            }*/
                        
        }


        //private void UpdateInput()
        private void UpdateInput(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                if (mousePos.X > 0 && mousePos.X < 150 &&
                    mousePos.Y > 150 && mousePos.Y < 400)
                {
                    moveLeft(gameTime);
                }
                else if (mousePos.X > 650 && mousePos.X < 800 &&
                    mousePos.Y > 200 && mousePos.Y < 400)
                {
                    moveRight(gameTime);
                }
                
            }
            else
            {
                mWalking = false;
                setDirection(Direction.NONE);
            }
                
            
        }

        private void moveRight(GameTime gameTime)
        {
            float f = gameTime.ElapsedGameTime.Milliseconds * 0.19f;
            moveRight(f);
            //moveRight(8);
            setDirection(Direction.RIGHT);
            mWalking = true;

            if (mX >= 600 + GamePlayScreen.sCURRENT_STAGE_X)
            {
                mX = 600 + GamePlayScreen.sCURRENT_STAGE_X;
            }
        }

        private void moveLeft(GameTime gameTime)
        {
            if (mX <= GamePlayScreen.sCURRENT_STAGE_X)
            {
                //    mX = GamePlayScreen.sCURRENT_STAGE_X;
            }
            float f = gameTime.ElapsedGameTime.Milliseconds * 0.19f;
            moveLeft(f);
            //moveLeft(8);
            setDirection(Direction.LEFT);
            mWalking = true;
        }


        public void changeState(int state) {

        
            switch (state) {

                case sSTATE_STOPPED:
                    changeToSprite(sSTATE_STOPPED);
                    break;

                case sSTATE_TOP_LEFT:

                    if (getState() != sSTATE_TOP_LEFT)
                    {
                        //Game1.print("ETERNAL");
                        changeToSprite(sSTATE_TOP_LEFT);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;

                
                case sSTATE_TOP_RIGHT:
                    if (getState() != sSTATE_TOP_RIGHT)
                    {
                        changeToSprite(sSTATE_TOP_RIGHT);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;

                case sSTATE_BOTTOM_LEFT:
                    if (getState() != sSTATE_BOTTOM_LEFT)
                    {
                        changeToSprite(sSTATE_BOTTOM_LEFT);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;

                case sSTATE_BOTTOM_RIGHT:
                    if (getState() != sSTATE_BOTTOM_RIGHT)
                    {
                        changeToSprite(sSTATE_BOTTOM_RIGHT);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;

                case sSTATE_VICTORY:
                    if (getState() != sSTATE_VICTORY)
                    {
                        changeToSprite(sSTATE_VICTORY);
                        getCurrentSprite().resetAnimationFlag();
                        mHurt = false;
                    }
                    break;

                case sSTATE_LOSE:
                    if (getState() != sSTATE_LOSE)
                    {
                        changeToSprite(sSTATE_LOSE);
                        getCurrentSprite().resetAnimationFlag();
                        mHurt = false;
                    }
                    break;

                case sSTATE_INVERSE_TOP_LEFT:
                    if (getState() != sSTATE_INVERSE_TOP_LEFT)
                    {
                        changeToSprite(sSTATE_INVERSE_TOP_LEFT);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;

                case sSTATE_INVERSE_TOP_RIGHT:
                    if (getState() != sSTATE_INVERSE_TOP_RIGHT)
                    {
                        changeToSprite(sSTATE_INVERSE_TOP_RIGHT);
                        getCurrentSprite().resetAnimationFlag();
                    }
                    break;
            }

            base.setState(state);

           
            
        }


        
        public void hurt()
        {
            if (!mHurt)
            {
                mHurt = true;
                mTimerTotalHurtTime = new MTimer();
                mTimerTotalHurtTime.start();

                mTimerBlinkSpeed = new MTimer();
                mTimerBlinkSpeed.start();

                mData.removeEnergy();
            }
        }

        public Color getColor()
        {
            return mColor;
        }

        public bool isHurt()
        {
            return mHurt;
        }

        public MainCharacterData getData()
        {
            return this.mData;
        }

        
        public BODYSTATE getBodyState(){
            return mBodyState;
        }

        /**********************************************************************************************

        ////////////INNER CLASSES
         * 
         *********************************************************************************************/
        private class Feet : GameObject
        {
            //INDEXES
            public const int sSTATE_STOPPED_FEET = 0;
            public const int sSTATE_WALKING_FEET_LEFT = 1;
            public const int sSTATE_WALKING_FEET_RIGHT = 2;


            //SPRITES
            private Sprite mSpriteStoppedFeet;
            private Sprite mSpriteWalkingFeetLeft;
            private Sprite mSpriteWalkingFeetRight;
            

            public Feet(Color color)
            {

                int sizeW = 111;
                int sizeH = 40;

                String folderColor = "";

                if (color == Color.Blue) folderColor = "blue";
                if (color == Color.Green) folderColor = "green";
                if (color == Color.Red) folderColor = "red";

                mSpriteStoppedFeet = new Sprite(ExtraFunctions.fillArrayWithImages2(1, "gameplay\\maincharacter\\" + folderColor + "\\feet\\left\\" + folderColor + "_feet_walk_left"), new int[] { 0 }, 7, sizeW, sizeH, false, false);
                mSpriteWalkingFeetLeft = new Sprite(ExtraFunctions.fillArrayWithImages2(17, "gameplay\\maincharacter\\" + folderColor + "\\feet\\left\\" + folderColor + "_feet_walk_left"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 17 }, 1, sizeW, sizeH, false, false);
                mSpriteWalkingFeetRight = new Sprite(ExtraFunctions.fillArrayWithImages2(17, "gameplay\\maincharacter\\" + folderColor + "\\feet\\right\\" + folderColor + "_feet_walk_right"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 17 }, 1, sizeW, sizeH, false, false);
 
                //mSpriteTackling = new Sprite(imagesTackling, new int[] { 0, 1, 2, 3, 4, 5 }, 1, 65, 80, true, false);

                addSprite(mSpriteStoppedFeet, sSTATE_STOPPED_FEET);
                addSprite(mSpriteWalkingFeetLeft, sSTATE_WALKING_FEET_LEFT);
                addSprite(mSpriteWalkingFeetRight, sSTATE_WALKING_FEET_RIGHT);


                changeToSprite(sSTATE_STOPPED_FEET);

                setCollisionRect(75, 80);


            }

            public void changeState(int state)
            {

                setState(state);

                switch (state)
                {
                    case sSTATE_STOPPED_FEET:
                        changeToSprite(sSTATE_STOPPED_FEET);
                        break;
                    case sSTATE_WALKING_FEET_LEFT:
                        changeToSprite(sSTATE_WALKING_FEET_LEFT);
                        getCurrentSprite().resetAnimationFlag();
                        break;
                    case sSTATE_WALKING_FEET_RIGHT:
                        changeToSprite(sSTATE_WALKING_FEET_RIGHT);
                        getCurrentSprite().resetAnimationFlag();
                        break;
        
                }

            }
            
            public override void draw(SpriteBatch spriteBatch)
            {
                base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
                //getCurrentSprite().draw(spriteBatch);
            }

            public override void update(GameTime gameTime)
            {
                base.update(gameTime);//getCurrentSprite().update();
            }

            

        }

    }

}