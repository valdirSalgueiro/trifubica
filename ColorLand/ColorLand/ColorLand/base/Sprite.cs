using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
     //TODO criar qtd de vezes a repetir
    public class Sprite
    {
        public static int sALL_FRAMES_IN_ORDER = 1234;

        private bool mFlip;

        //GAMBIARRA DO CACETO-TEMPORARIA
        private bool mOneFrameMode;
        private int mIndexOneFrameMode;

        public int R = 255;
        public int G = 255;
        public int B = 255;
        public int ALPHA = 255;

        public float rotation;

        private Texture2D mImage;
        private int mTick;
        private int mTickSpeeed;
        private Rectangle mSourceRectangle;

        private int mSourceX;
        private int mSourceY;
        private int mSourceWidth;
        private int mSourceHeight;

        private int mFramesTotal;
        private int mCurrentFrame;

        private float mX;
        private float mY;

        public Vector2 offset=Vector2.Zero;

        private int mWidth;
        private int mHeight;

        private bool mVisible;

        private bool mStopMeAfterEndAnimation;
        private bool mMakeMeInvisibleAfterEndAnimation;

        private bool mAnimationEnded;

        private int mMaxLoops;
        private int mCurrentLoop;

        private String mImagePath;
        private int[] mAnimationSequence;

        private bool   mIndividualImages; //non-sprite sheet
        private Texture2D[] mImages;
        private String[] mImagesPaths;

        /*  public Sprite(int frames, String imagePath, int[] sequence, int tickSpeed) {
              this.mFramesTotal = frames;
              this.mCurrentFrame = 0;
              this.mImagePath = imagePath;
              this.mAnimationSequence = sequence;
              this.mTickSpeeed = tickSpeed;
          } */

        
        public Sprite(String[] imagesPath, int[] sequence, int tickSpeed, int width, int height, bool stopMeAfterEnds, bool makeMeInvisibleAfterEnds)
        {
            mIndividualImages = true;


            //mImages = new Texture2D[sequence.Length];
            mImages = new Texture2D[imagesPath.Length];


            this.mFramesTotal = imagesPath.Length;
            this.mCurrentFrame = 0;
            this.mImagesPaths = imagesPath;

            if (sequence != null)
            {
                if (sequence[0] == sALL_FRAMES_IN_ORDER)
                {
                    //trick 
                    int[] seqAux = new int[sequence[1]];

                    for (int x = 0; x < seqAux.Length; x++)
                    {
                        seqAux[x] = x;
                    }

                    sequence = seqAux;

                }
            }

            this.mAnimationSequence = sequence;
            
            
            this.mTickSpeeed = tickSpeed;
            this.mWidth = width;
            this.mHeight = height;
            this.mVisible = true;
            this.mStopMeAfterEndAnimation = stopMeAfterEnds;
            this.mMakeMeInvisibleAfterEndAnimation = makeMeInvisibleAfterEnds;

            if (mAnimationSequence == null)
            {
                int[] a = new int[imagesPath.Length];

                for (int x = 0; x < a.Length; x++)
                {
                    a[x] = x;
                }

                mAnimationSequence = a;

            }

        }

        public Sprite(String[] imagesPath, int[] sequence, int loops, int tickSpeed, int width, int height, bool stopMeAfterEnds, bool makeMeInvisibleAfterEnds)
        {
            mIndividualImages = true;

            mImages = new Texture2D[sequence.Length];

            this.mFramesTotal = imagesPath.Length;
            this.mCurrentFrame = 0;
            this.mImagesPaths = imagesPath;
            this.mAnimationSequence = sequence;
            this.mTickSpeeed = tickSpeed;
            this.mWidth = width;
            this.mHeight = height;
            this.mVisible = true;
            this.mStopMeAfterEndAnimation = stopMeAfterEnds;
            this.mMakeMeInvisibleAfterEndAnimation = makeMeInvisibleAfterEnds;

            if (mAnimationSequence == null)
            {
                int[] a = new int[imagesPath.Length];

                for (int x = 0; x < a.Length; x++)
                {
                    a[x] = x;
                }

                mAnimationSequence = a;

            }
                
            if (mStopMeAfterEndAnimation)
            {
                mMaxLoops = loops;
                //mCurrentLoop = 0;                
            }
        }

        public Sprite(int frames, String imagePath, int[] sequence, int tickSpeed, int width, int height)
        {
            this.mFramesTotal = frames;
            this.mCurrentFrame = 0;
            this.mImagePath = imagePath;
            this.mAnimationSequence = sequence;
            this.mTickSpeeed = tickSpeed;
            this.mWidth = width;
            this.mHeight = height;
            this.mVisible = true;

           
        }

        public Sprite(int frames, String imagePath, int[] sequence, bool stopMeAfterEnds, bool makeMeInvisibleAfterEnds, int tickSpeed, int width, int height)
        {
            this.mFramesTotal = frames;
            this.mCurrentFrame = 0;
            this.mImagePath = imagePath;
            this.mAnimationSequence = sequence;
            this.mTickSpeeed = tickSpeed;
            this.mWidth = width;
            this.mHeight = height;
            this.mStopMeAfterEndAnimation = stopMeAfterEnds;
            this.mMakeMeInvisibleAfterEndAnimation = makeMeInvisibleAfterEnds;
            this.mVisible = true;

        }

        public Sprite(int frames, String imagePath, int index, int width, int height)
        {
            mOneFrameMode = true;
            this.mFramesTotal = frames;
            this.mCurrentFrame = 0;
            this.mImagePath = imagePath;
            this.mWidth = width;
            this.mHeight = height;
            this.mVisible = true;

        }

        public void loadContent(ContentManager content)
        {

            if (content == null)
            {
                Game1.print("Sprite load content");
            }

            if (mIndividualImages)
            {
                for(int x=0; x < mImagesPaths.Length; x++){
                    mImages[x] = content.Load<Texture2D>(mImagesPaths[x]);
                }
                setSourceValues(0, 0, mImages[0].Width, mImages[0].Height);
                mImage = mImages[0];
            }
            else
            {
                mImage = content.Load<Texture2D>(mImagePath);
                //mSourceRectangle = new Rectangle(0,0,
                mSourceWidth = mImage.Width / mFramesTotal;
                mSourceHeight = mImage.Height;

                if (mOneFrameMode)
                {
                    this.mSourceX = mSourceWidth * mIndexOneFrameMode;
                }

            }
        }

        public void update()
        {
            if (!mOneFrameMode)
            {
                if (mTick < mTickSpeeed)
                {
                    mTick++;
                }
                else
                {

                    if (mIndividualImages)
                    {
                        mTick = 0;
                        if (mCurrentFrame < mAnimationSequence.Length -1/*mFramesTotal-1*/)
                        {
                            mCurrentFrame++;
                            //this.mSourceX = this.mSourceWidth * mAnimationSequence[mCurrentFrame];
                            mAnimationEnded = false;/////////
                        }
                        else
                        {
                            if (!mStopMeAfterEndAnimation)
                            {
                                this.mCurrentFrame = 0;
                                //this.mSourceX = this.mSourceWidth * mAnimationSequence[mCurrentFrame]; //0;
                            }
                            else
                            {
                                mCurrentLoop++;

                                if (mCurrentLoop >= mMaxLoops)
                                {
                                    mAnimationEnded = true;
                                    if (mMakeMeInvisibleAfterEndAnimation)
                                    {
                                        this.mVisible = false;
                                    }

                                }
                                else
                                {
                                    mCurrentFrame = 0;
                                }
                                
                            }

                            //if(mLoopAnim == false) 
                        }
                    }
                    else
                    {

                        mTick = 0;
                        if (mCurrentFrame < mAnimationSequence.Length - 1/*mFramesTotal-1*/)
                        {
                            mCurrentFrame++;
                            this.mSourceX = this.mSourceWidth * mAnimationSequence[mCurrentFrame];
                            mAnimationEnded = false;/////////
                        }
                        else
                        {
                            if (!mStopMeAfterEndAnimation)
                            {
                                this.mCurrentFrame = 0;
                                this.mSourceX = this.mSourceWidth * mAnimationSequence[mCurrentFrame]; //0;
                            }
                            else
                            {
                                mAnimationEnded = true;

                                if (mMakeMeInvisibleAfterEndAnimation)
                                {
                                    this.mVisible = false;
                                }

                            }

                            //if(mLoopAnim == false) 
                        }

                    }
                }
            }
        }

        public void draw(SpriteBatch spritebatch)
        {
            draw(spritebatch, 0);
        }

        public void draw(SpriteBatch spritebatch, double angle)
        {
            if (mVisible)
            {
                //se der um caralho aqui foi culpa da cor, digo logo. Volta pra Color.WHITE
                if (mIndividualImages)
                {
                    //flip effect
                    SpriteEffects flipEffect;
                    if (mFlip)
                    {
                        flipEffect = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        flipEffect = SpriteEffects.None;
                    }

                    int imageIndex = mAnimationSequence[mCurrentFrame];
                    spritebatch.Draw(mImages[imageIndex], destRectangle(), srcRectangle(), new Color(R, G, B), (float)angle, Vector2.Zero, flipEffect, 0);
                }
                else
                {
                    spritebatch.Draw(mImage, destRectangle(), srcRectangle(), new Color(R,G,B), (float)angle, Vector2.Zero, SpriteEffects.None, 0);
                }
            }

        }

        public void draw(SpriteBatch spritebatch, double angle, Vector2 rotationPosition, Vector2 rotationOrigin)
        {
            if (mVisible)
            {
                //se nÃƒÂ£o tiver aparecendo nada ÃƒÂ© pq width e height nao foram setados
                //130,124
                if (mIndividualImages)
                {
                    spritebatch.Draw(mImages[mCurrentFrame], destRectangle(), new Color(R, G, B));
                }
                else
                {

                    //flip?
                    SpriteEffects flipEffect;
                    if(mFlip){
                        flipEffect = SpriteEffects.FlipHorizontally;
                    }else{
                        flipEffect = SpriteEffects.None;
                    }
                    
                    spritebatch.Draw(mImage, rotationPosition, new Rectangle(0, 0, 259, 247), new Color(R, G, B), (float)angle, rotationOrigin, 1, flipEffect, 1);
                }
            }

        }

        public void draw(SpriteBatch spritebatch, float angle, float zoom)
        {
            if (mVisible)
            {
                //se der um caralho aqui foi culpa da cor, digo logo. Volta pra Color.WHITE
                if (mIndividualImages)
                {
                    //flip effect
                    SpriteEffects flipEffect;
                    if (mFlip)
                    {
                        flipEffect = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        flipEffect = SpriteEffects.None;
                    }

                    int imageIndex = mAnimationSequence[mCurrentFrame];

                    spritebatch.Draw(mImages[imageIndex], new Vector2(mX, mY), srcRectangle(), new Color(R, G, B), angle,
                            //new Vector2(mImages[imageIndex].Bounds.X + mImages[imageIndex].Width - mImages[imageIndex].Width / 2, mImages[imageIndex].Bounds.Y + mImages[imageIndex].Height - mImages[imageIndex].Height / 2),  //origin 
                            Vector2.Zero,
                            zoom, flipEffect, 0);
                    
                }
                else
                {
                   Game1.print("ALERTA! ENTROU NUM ELSE FUDIDO!**********************************");
                    //spritebatch.Draw(mImage, destRectangle(), srcRectangle(), new Color(R, G, B), (float)angle, Vector2.Zero, SpriteEffects.None, 0);
                }
            }

        }

        public void draw(SpriteBatch spritebatch, Color c)
        {
            if (mIndividualImages)
            {
                spritebatch.Draw(mImages[mCurrentFrame], destRectangle(), c);//new Color(R, G, B));
            }
            else
            {
                spritebatch.Draw(mImage, destRectangle(), c);
            }
        }

        public void setFlip(bool flip)
        {
            mFlip = flip;
        }

        public void flip()
        {
            mFlip = !mFlip;
        }

        public bool isFlipped()
        {
            return this.mFlip;
        }

        public Rectangle destRectangle()
        {
            return new Rectangle((int)mX + (int)offset.X, (int)mY + (int)offset.Y, mWidth, mHeight);
        }

        public Rectangle srcRectangle()
        {
            return new Rectangle(this.mSourceX, this.mSourceY, mSourceWidth, mSourceHeight);
        }

        public void setSourceValues(int x, int y, int width, int height)
        {
            this.mSourceX = x;
            this.mSourceY = y;
            this.mSourceWidth = width;
            this.mSourceHeight = height;
        }

        public Texture2D getCurrentTexture2D()
        {
            return this.mImage;
        }

        public double getX()
        {
            return this.mX;
        }
        public double getY()
        {
            return this.mY;
        }

        public void addY(float value)
        {
            this.mY += value;
        }

        public void reduceY(float value)
        {
            this.mY -= value;
        }

        public void setX(float x)
        {
            this.mX = x;
        }

        public void setY(float y)
        {
            this.mY = y;
        }

        public void setDimension(int width, int height)
        {
            this.mWidth = width;
            this.mHeight = height;
        }

        public void setLocation(float x, float y)
        {
            this.mX = x;
            this.mY = y;
        }

        public void setRotation(float angle)
        {
            rotation = angle;
        }

        public float getRotation()
        {
            return rotation;
        }


        public int getHeight()
        {
            return this.mHeight;
        }

        public bool getAnimationEnded()
        {
            return this.mAnimationEnded;
        }

        public void resetAnimationFlag()
        {
            this.mCurrentFrame = 0;
            this.mAnimationEnded = false;
            mCurrentLoop = 0;
        }

        public int getCurrentFrame()
        {
            return this.mCurrentFrame;
        }

        public int getWidth()
        {
            return this.mWidth;
        }

        public void resetStatus()
        {
            this.mCurrentFrame = 0;
            this.mSourceX = 0;
            this.mTick = 0;
            this.mAnimationEnded = false;
        }

        public void setAnimationSequence(int[] newAnimationSequence)
        {
            mAnimationSequence = newAnimationSequence;
        }



    }
}