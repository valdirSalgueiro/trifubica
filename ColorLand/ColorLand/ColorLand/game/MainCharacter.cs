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

        private bool mWalking;
        private float mCurrentSpeed;
        private bool mJumping;
        private float mYGround;

        private float mDx;
        private float mDy;// = 0.09f;


        //INDEXES
        public const int sSTATE_STOPPED = 0;
        public const int sSTATE_TOP_LEFT = 1;
        public const int sSTATE_TOP_RIGHT = 2;

        //public const int sSTATE_TACKLING = 1;
        public const int sSTATE_MOVING = 1;
        
        //SPRITES
        private Sprite mSpriteStopped;
        private Sprite mSpriteTopLeft;
        private Sprite mSpriteTopRight;
        private Sprite mSpriteDownLeft;
        private Sprite mSpriteDownRight;
        //private Sprite mSpriteTackling;

        

        private float mYbeforeJump;
        private float mJumpSpeed;


        private Texture2D mLeftHandTexture;
        private Texture2D mLeftHandTextureWithBrush;
        private float mLeftHandAngle;

        private Texture2D mRightHandTexture;
        private Texture2D mRightHandTextureWithBrush;
        private float mRightHandAngle;

        KeyboardState oldState;

        private Color mColor;


        class Tracer {
            public float alpha;
            public float angle;
            public Vector2 pos;
            public bool alive;
            public bool leftHand;
        }

        Tracer[] tracers=new Tracer[20];
        int frames=0;
        bool leftHand=true;
        

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
            
            if (mColor == Color.Blue)
            {

                int width = 130;
                int height = 130;

                mSpriteStopped = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\maincharacter\\blue_stopped"), new int[] { 0 }, 7, width, height, false, false);
                mSpriteTopLeft = new Sprite(ExtraFunctions.fillArrayWithImages(5, "gameplay\\maincharacter\\blue_left_top"), new int[] { 0, 1, 2, 3, 4 }, 2, width, height, true, false);
                mSpriteTopRight = new Sprite(ExtraFunctions.fillArrayWithImages(5, "gameplay\\maincharacter\\blue_right_top"), new int[] { 0, 1, 2, 3, 4 }, 2, width, height, true, false);
                
                addSprite(mSpriteStopped, sSTATE_STOPPED);
                addSprite(mSpriteTopLeft, sSTATE_TOP_LEFT);
                addSprite(mSpriteTopRight, sSTATE_TOP_RIGHT);
                
                changeToSprite(sSTATE_STOPPED);

                setCollisionRect(75, 80);

                mFeet = new Feet();

            }

            for (int i = 0; i < tracers.Length; i++)
            {
                tracers[i] = new Tracer();
                tracers[i].alive = false;
            }

        }

        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
            mFeet.loadContent(content);
            mLeftHandTexture = content.Load<Texture2D>("gameplay\\maincharacter\\hands\\HandLeft");
            mRightHandTexture = content.Load<Texture2D>("gameplay\\maincharacter\\hands\\HandRightBrush");
        }

        public override void update(GameTime gameTime) {
            mY += mDy * gameTime.ElapsedGameTime.Milliseconds;
            base.update(gameTime);//getCurrentSprite().update();
            UpdateInput();
            updateJump();
            updateFeet(gameTime);

            //tracers
            int count = 0;
            if (frames % 10 == 0)
            {
                for (int i = 0; i < tracers.Length; i++)
                {
                    if (!tracers[i].alive)
                    {
                        tracers[i].alive = true;
                        tracers[i].alpha = 1.0f;
                        if (leftHand)
                        {
                            tracers[i].angle = mLeftHandAngle - (float)Math.PI;
                            tracers[i].pos = new Vector2(mX + 40, mY + 40);
                        }
                        else
                        {
                            tracers[i].angle = mRightHandAngle;
                            tracers[i].pos = new Vector2(mX + 90, mY + 60);
                        }
                        tracers[i].leftHand = leftHand;
                        leftHand = !leftHand;
                        count++;
                        if (count > 1)
                            break;
                    }
                }
            }
            for (int i = 0; i < tracers.Length; i++)
            {
                if (tracers[i].alive)
                {
                    tracers[i].alpha -= 0.01f;
                    if (tracers[i].alpha <= 0.0f)
                        tracers[i].alive = false;                  
                }
            }
            frames++;
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
            mFeet.mX = mX - 15;
            mFeet.mY = mY - 20;

            /*if (getState() == sSTATE_STOPPED)
            {
                mFeet.changeState(Feet.sSTATE_STOPPED_FEET);
            }
            else
            {*/
                //TODO resolver futuro bug de moving e jumping simultaneos
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

            mFeet.update(gametime);
        }


        public override void draw(SpriteBatch spriteBatch) {
            //getCurrentSprite().draw(spriteBatch);
            mFeet.draw(spriteBatch);

            for (int i = 0; i < tracers.Length; i++)
            {
                if (tracers[i].alive)
                {
                    
                    if (leftHand)
                        spriteBatch.Draw(mLeftHandTexture, tracers[i].pos, null, new Color(1.0f, 1.0f, 1.0f, tracers[i].alpha), tracers[i].angle, new Vector2(800, 331), 0.1f, SpriteEffects.None, 0f);
                    else
                        spriteBatch.Draw(mRightHandTexture, tracers[i].pos, null, new Color(1.0f, 1.0f, 1.0f, tracers[i].alpha), tracers[i].angle, new Vector2(0, 306), 0.1f, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.Draw(mLeftHandTexture, new Vector2(mX + 40, mY + 40), null, Color.White, mLeftHandAngle - (float)Math.PI, new Vector2(800, 331), 0.1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mRightHandTexture, new Vector2(mX + 90, mY + 60), null, Color.White, mRightHandAngle, new Vector2(0, 306), 0.1f, SpriteEffects.None, 0f);


            base.draw(spriteBatch);
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
                    setDirection(Direction.LEFT);
                    mWalking = true;
                    
                }else
                    if (newState.IsKeyDown(Keys.Right))
                    {
                        moveRight(8);
                        setDirection(Direction.RIGHT);
                        mWalking = true;
                    }
                    else
                    {
                        mWalking = false;
                        setDirection(Direction.NONE);
                    }
                
            }
        }


        public void changeState(int state) {

        
            switch (state) {

                case sSTATE_STOPPED:
                    changeToSprite(sSTATE_STOPPED);
                    break;

                case sSTATE_TOP_LEFT:

                    if (getState() != sSTATE_TOP_LEFT)
                    {
                        Game1.print("ETERNAL");
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
            }

            base.setState(state);

           
            
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
            

            public Feet()
            {

                int size = 160;

                mSpriteStoppedFeet = new Sprite(ExtraFunctions.fillArrayWithImages(1, "gameplay\\maincharacter\\feet\\blue_feet_stopped"), new int[] { 0 }, 7, size, size, false, false);
                mSpriteWalkingFeetLeft = new Sprite(ExtraFunctions.fillArrayWithImages(12, "gameplay\\maincharacter\\feet\\blue_feet_walk"), new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, 1, size, size, false, false);
                mSpriteWalkingFeetRight = new Sprite(ExtraFunctions.fillArrayWithImages(12, "gameplay\\maincharacter\\feet\\blue_feet_walk"), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, 1, size, size, false, false);
                               

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