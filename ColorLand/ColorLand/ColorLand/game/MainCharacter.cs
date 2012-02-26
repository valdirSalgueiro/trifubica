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
    class MainCharacter : GameObject {

        private MainCharacterData mData;
        private Feet mFeet;

        private const float cJUMP_SPEED = -.95f;
        private bool mOnGround;

        private float mCurrentSpeed;
        private bool mJumping;
        private float mYGround;

        private float mDx;
        private float mDy;// = 0.09f;


        //INDEXES
        public const int sSTATE_STOPPED = 0;
        //public const int sSTATE_TACKLING = 1;
        public const int sSTATE_MOVING = 1;
        
        //SPRITES
        private Sprite mSpriteStopped;
        //private Sprite mSpriteTackling;

        
        private float mYbeforeJump;
        private float mJumpSpeed;


        private Texture2D mLeftHandTexture;
        private float mLeftHandAngle;

        private Texture2D mRightHandTexture;
        private float mRightHandAngle;

        KeyboardState oldState;


        /**SPECIFIC: body state (for cursor) ***/
        private BODYSTATE mBodyState;
        
        public enum BODYSTATE
        {
            UP_LEFT,
            UP_RIGHT,
            DOWN_LEFT,
            DOWN_RIGHT
        }


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


            mFeet = new Feet();

        }

        public override void loadContent(ContentManager content) {
            base.loadContent(content);
            mFeet.loadContent(content);
            mLeftHandTexture = content.Load<Texture2D>("test\\glooveleft");
            mRightHandTexture = content.Load<Texture2D>("test\\gloove");
        }

        public override void update(GameTime gameTime) {
            mY += mDy * gameTime.ElapsedGameTime.Milliseconds;
            base.update(gameTime);//getCurrentSprite().update();
            UpdateInput();
            updateJump();
            updateFeet(gameTime);
        }
        
        private void updateJump(){

            if (mJumping)
            {
                //ja tirou o pe do chao
                if (mY != mYbeforeJump)
                {

                    if (mY <= 200)
                    {
                        mDy += 0.11f;
                    }
                    
                }

                if(mDy > 0)
                if (mY >= mYbeforeJump)
                {
                    //mDy = -mDy;
                    mJumping = false;
                    mDy = 0;
                    mY = mYbeforeJump;
                }

            }

        }

        private void Jump()
        {
            
            if (!mJumping)
            {
                mDy -= 0.55f;
                mJumping = true;

               mYbeforeJump = getY();

            }

        }

        private void updateFeet(GameTime gametime)
        {
            mFeet.mX = mX;
            mFeet.mY = mY + 50;

            if (getState() == sSTATE_STOPPED)
            {
                mFeet.changeState(Feet.sSTATE_STOPPED_FEET);
            }
            else
            {
                //TODO resolver futuro bug de moving e jumping simultaneos
                if (getState() == sSTATE_MOVING)
                {
                    if (mFeet.getState() != Feet.sSTATE_WALKING_FEET)
                    {
                        mFeet.changeState(Feet.sSTATE_WALKING_FEET);
                    }
                }
            }

            mFeet.update(gametime);
        }


        public override void draw(SpriteBatch spriteBatch) {
            base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
            //getCurrentSprite().draw(spriteBatch);
            mFeet.draw(spriteBatch);
            spriteBatch.Draw(mLeftHandTexture, new Vector2(mX, mY), null, Color.White, mLeftHandAngle, new Vector2(50, 50), 1.0f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mRightHandTexture, new Vector2(mX + 30, mY + 20), null, Color.White, mRightHandAngle, new Vector2(0, 25), 1.0f, SpriteEffects.None, 0f);
        }

        public void updateHand(float angle)
        {
            //TODO corrigir esta porra
            mLeftHandAngle = angle;
            mRightHandAngle = angle;
        }


        private void UpdateInput()
        {

            if (Game1.sKINECT_BASED == true)
            {
                mX = KinectManager.getInstance().getHipCenterPosition().X;
                //fazer esquema de old x = kinect.x
                //if(mx != old x) entao moving

                if (mJumping == false)
                {
                    if (KinectManager.getInstance().getHipCenterPosition().Y <= 250)
                    {
                     //   Jump();
                    }
                    //Game1.print(KinectManager.getInstance().getHipCenterPosition().Y + "");
                }
            }
            else
            {

                KeyboardState newState = Keyboard.GetState();

                // Is the SPACE key down?
                if (newState.IsKeyDown(Keys.Space))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.Space))
                    {
                        Jump();
                    }
                }
                else if (oldState.IsKeyDown(Keys.Space))
                {
                    // Key was down last update, but not down now, so
                    // it has just been released.
                }

                // Update saved state.
                oldState = newState;

                if (newState.IsKeyDown(Keys.Left))
                {
                    moveLeft(8);
                    if (getState() != sSTATE_MOVING)
                    {
                        changeState(sSTATE_MOVING);
                    }

                }else
                    if (newState.IsKeyDown(Keys.Right))
                    {
                        moveRight(8);
                        if (getState() != sSTATE_MOVING)
                        {
                            changeState(sSTATE_MOVING);
                        }
                    }
                    else
                    {
                        changeState(sSTATE_STOPPED);
                    }
                
            }
        }


        public void changeState(int state) {

            setState(state);

            switch (state) {
                case sSTATE_STOPPED:
                    changeToSprite(sSTATE_STOPPED);
                    break;

                case sSTATE_MOVING:
                    //nothing occurs
                    break;
            }

        }

        public MainCharacterData getData()
        {
            return this.mData;
        }

        public void setBodyState(BODYSTATE bodyState)
        {
            this.mBodyState = bodyState;
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
            public const int sSTATE_WALKING_FEET = 1;


            //SPRITES
            private Sprite mSpriteStoppedFeet;
            private Sprite mSpriteWalkingFeet;
            

            public Feet()
            {

                String[] imagesStoppedFeet = new String[1];
                imagesStoppedFeet[0] = "test\\foot1";

                String[] imagesWalkingFeet = new String[2];
                imagesWalkingFeet[0] = "test\\foot2";
                imagesWalkingFeet[1] = "test\\foot3";


                mSpriteStoppedFeet = new Sprite(imagesStoppedFeet, new int[] { 0 }, 7, 65, 80, false, false);
                mSpriteWalkingFeet = new Sprite(imagesWalkingFeet, new int[] { 0, 1 }, 2, 65, 80, false, false);
                
                //mSpriteTackling = new Sprite(imagesTackling, new int[] { 0, 1, 2, 3, 4, 5 }, 1, 65, 80, true, false);

                addSprite(mSpriteStoppedFeet, sSTATE_STOPPED_FEET);
                addSprite(mSpriteWalkingFeet, sSTATE_WALKING_FEET);
                //addSprite(mSpriteTackling, sSTATE_TACKLING);

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
                    case sSTATE_WALKING_FEET:
                        changeToSprite(sSTATE_WALKING_FEET);
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