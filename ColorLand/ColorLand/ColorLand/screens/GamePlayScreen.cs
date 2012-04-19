﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using System.Timers;


namespace ColorLand
{
    public class GamePlayScreen : BaseScreen
    {

        private Effect desaturateEffect;

        private const String cMUSIC_BEGIN = "sound\\music\\begin";
        private const String cMUSIC_STAGE11 = "sound\\music\\stage11";
        //private const String cMUSIC_BEGIN = "sound\\music\\begin";
        private const String cMUSIC_WIN = "sound\\music\\win";
        private const String cMUSIC_LOSE = "sound\\music\\loose";

        private PauseScreen mPauseScreen;

        public const String cSOUND_EXPLOSION = "sound\\fx\\explosao8bit";

        /*******************
         * CONSTANTS
         *******************/
        public static int sGROUND_WORLD_1_1 = 500;

        int energy = 100;
        int progress = 0;
        int numberEnemies = 40;
        int pulse = 0;

        //debug
        public bool DEBUG = true;
        //private PaperObject mCurrentDebugPaperObject;

        //SpriteFont mFontDebug;
        //SpriteFont mFontAlert;

        /*******************
         * INPUT
         *******************/
        KeyboardManager mKeyboard;

        KeyboardState oldState;

        MouseState oldStateMouse;

        /*******************
         * BASIC
         *******************/
        private SpriteBatch mSpriteBatch;

        private int mTouchX, mTouchY;

        /*******************
         * ALERTS MESSAGES AND ATTRIBUTES
         *******************/
                
        String mUniversalTEXT;
        String mUniversalTEXT2;

        private bool mMousePressing;

        private bool mPaused;

        private MTimer mTimerMessages = new MTimer();
        //private Timer mTimerSucesso = new Timer();
        //private Timer mTimerDerrota= new Timer();

        private MTimer mTimerStageFinishExplosions;

        private int mGameState;

        private const int GAME_STATE_PREPARANDO = 0;
        private const int GAME_STATE_EM_JOGO    = 1;
        private const int GAME_STATE_SUCESSO    = 2;
        private const int GAME_STATE_DERROTA    = 3;

        private int mFlagTimer;
        private const int FLAG_TIMER_PREPARANDO_WAIT_BEFORE_START = 0;

        /*******************
         * GAME
         *******************/
        public const int sWORLD_1 = 1;
        public const int sWORLD_2 = 2;

        public const int sPART_1 = 10;
        public const int sPART_2 = 11;
        

        private int mCurrentWorld;
        private int mCurrentPart;


        private Background mBackgroundBack;
        private Background mBackgroundFront;

        private MainCharacter mMainCharacter;

        //private GameObjectsGroup<BaseEnemy> mGroup = new GameObjectsGroup<BaseEnemy>();
        private GameObjectsGroup<Collectable> mGroupCollectables = new GameObjectsGroup<Collectable>();

        private Camera mCamera;

        private ColorChoiceBar mColorChoiceBar;
        private int mColorCount;

        private static Timer mTimer;

        private MTimer mTimerStageBegin;
        private MTimer mEndStageTimer;

        Explosion mExplosion;
        private ExplosionManager mExplosionManager;


        private EnemyManager mManager = new EnemyManager();

        private float porcentagemRestante=0.0f;

        private Fade mFade;
        private Fade mCurrentFade;
        private float mAlphaBackground = 1.0f;
        private bool mReduceAlpha;
        private SpriteFont mFontStageBegin;
        private float mAlphaFontBegin = 0f;
        private Texture2D mTextureReady;
        private Texture2D mTextureGO;
        private bool mShowReady;
        private bool mShowGo;


        /****
         * INTRO 
         ****/
        private bool mShowBlackBackground = true;
        private Texture2D mBlackBackground;


        public void manageColorCount()
        {
            mColorCount++;

            if (mColorCount > 2)
            {
                mColorCount = 0;
            }

            mColorChoiceBar.changeState(mColorCount);

            if (mColorCount == 0) mCursor.changeColor(Color.Red);
            if (mColorCount == 1) mCursor.changeColor(Color.Green);
            if (mColorCount == 2) mCursor.changeColor(Color.Blue);

        }

        public GamePlayScreen()
        {
            mTimer = null;
           // mGameState = GAME_STATE_PREPARANDO;
            mGameState = GAME_STATE_SUCESSO;

            mCamera = new Camera();

            loadWorld1(sWORLD_1);

            mPauseScreen = new PauseScreen(this);

            mShowBlackBackground = false;
            setGameState(GAME_STATE_EM_JOGO);
            //setGameState(GAME_STATE_PREPARANDO);
           

            mKeyboard = KeyboardManager.getInstance();

            mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.SLOW);

            executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);
            //,
        }

        private void loadWorld1(int part)
        {

            mCurrentWorld = 1;
            mCurrentPart  = part;

            switch (part)
            {

                case 1:

                    mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

                    mBlackBackground = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("fades\\blackfade");

                    desaturateEffect = Game1.getInstance().getScreenManager().getContent().Load<Effect>("effects\\desaturate");

                    //mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");
                    //mFontAlert = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("alerts");

                    mBackgroundBack  = new Background("gameplay\\backgrounds\\stage1_1\\stage1_1_layer1");
                    mBackgroundFront = new Background("gameplay\\backgrounds\\stage1_1\\stage1_1_layer3");
                    
                    //String[] imagesBG = new String[9];
                    /*for (int x = 0; x < imagesBG.Length; x++)
                    {
                        imagesBG[x] = "test\\e" + (x+1);
                    }
                    mBackground.addPart(imagesBG, 2, 200, 200, 40, 500);
                    */

                    SoundManager.LoadSound(cSOUND_EXPLOSION);

                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\stage1_1_layer2" },1,1000,600,0,0);
                    //mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\stage1_1_layer3" }, 1, 1000, 600, 0, 0);
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\stage1_1_layer4" }, 1, 1000, 600, 0, 0);

                    mBackgroundBack.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundBack.setLocation(0, 0);

                    mBackgroundFront.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundFront.setLocation(0, 0);


                    mMainCharacter = new MainCharacter(Color.Green);
                    mMainCharacter.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mMainCharacter.setCenter(Game1.sSCREEN_RESOLUTION_WIDTH / 2, 434);

                   /* mGroup.addGameObject(new EnemyCrabCrab(Color.Green, new Vector2(300, 320)));
                    mGroup.addGameObject(new EnemyArc(Color.Blue));
                    mGroup.loadContent(Game1.getInstance().getScreenManager().getContent());
                    */

                    mFontStageBegin = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("font\\stagebegin_font");
                    mTextureReady = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\instruction_1");
                    mTextureGO = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\instruction_3");
                    
                    mGroupCollectables.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mCursor = new Cursor();
                    mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mCursor.changeColor(Color.Green);
                    mCursor.setCenter(20, 20);

                    HUD.getInstance().loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosionManager = new ExplosionManager();
                    mExplosionManager.addExplosion(20, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                   // mManager.addEnemy(new EnemySimpleFlying(BaseEnemy.sTYPE_SIMPLE_FLYING_RED), new Vector2(300, 320));
                    //mManager.addEnemy(EnemyManager.EnemiesTypes.Mongo, Color.Red, new Vector2(300, 190));

                    mManager.addEnemy(EnemyManager.EnemiesTypes.CrabCrab, Color.Red, new Vector2(200, 0));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.CrabCrab, Color.Red, new Vector2(500, 0));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.CrabCrab, Color.Red, new Vector2(700, 0));
                   

                    //mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Red, new Vector2(0, getPlayerCenterVector().Y));                
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Red, new Vector2(-100, getPlayerLocation().Y));                
   

                    mManager.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mManager.start();

                    /*for (int x = 0; x < mManager.getTotalEnemies(); x++)
                    {
                        mGroupCollectables.addGameObject(new Collectable(Collectable.CollectableType.StagePiece));
                    }*/
                    //mCamera.zoomIn(2.4f);

                    /*
                     * mColorChoiceBar = new ColorChoiceBar();
                        mColorChoiceBar.loadContent(Game1.getInstance().getScreenManager().getContent());
                        mColorChoiceBar.setCenter(200, 200);*/

                    mCursor.changeColor(Color.Blue);

                    break;

            }


        }


        private void unloadWorld1()
        {
            unload();
            //executeFade(mFadeIn);
        }


        private void startManualTimer()
        {
            mEndStageTimer = new MTimer();
            mEndStageTimer.start();
        }

        private void updateTimers(GameTime gameTime)
        {

            if (mTimerStageFinishExplosions != null)
            {
                mTimerStageFinishExplosions.update(gameTime);
                
                for(double k=0; k<10; k+=0.2){
                    if (mTimerStageFinishExplosions.getTimeAndLock(k)) { explodeStageFinish(); };
                }
                /*if (mTimerStageFinishExplosions.getTimeAndLock(0.2)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(0.5)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(0.7)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(1)) explodeStageFinish();
                //if (mTimerStageFinishExplosions.getTimeAndLock(1.3)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(1.7)) explodeStageFinish();
                //if (mTimerStageFinishExplosions.getTimeAndLock(1.9)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(2.5)) explodeStageFinish();
                //if (mTimerStageFinishExplosions.getTimeAndLock(2.9)) explodeStageFinish();
                //if (mTimerStageFinishExplosions.getTimeAndLock(2.9)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(3.1)) explodeStageFinish();
                //if (mTimerStageFinishExplosions.getTimeAndLock(3.4)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(3.7)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(4)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(4.5)) explodeStageFinish();
                */
                /*if (mTimerStageFinishExplosions.getTimeAndLock(1)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(1.3)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(1.6)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(2)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(2.5)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(3)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(3.5)) explodeStageFinish();
                if (mTimerStageFinishExplosions.getTimeAndLock(4)) explodeStageFinish();*/
                if (mTimerStageFinishExplosions.getTimeAndLock(5.5))
                {
                    SoundManager.PlayMusic(cMUSIC_WIN, false);
                    mMainCharacter.changeState(MainCharacter.sSTATE_VICTORY);
                    mCursor.setLocation(0, 1000);
                }

                
            }

            if (mTimerStageBegin != null)
            {

                mTimerStageBegin.update(gameTime);

                if (mTimerStageBegin.getTimeAndLock(5))
                {
                    mReduceAlpha = true;
                }

                if (mTimerStageBegin.getTimeAndLock(7))
                {
                    mShowReady = true;
                }

                if (mTimerStageBegin.getTimeAndLock(9))
                {
                    mShowReady = false;
                    mShowGo = true;
                }

                if (mTimerStageBegin.getTimeAndLock(10))
                {
                    mShowGo = false;
                    setGameState(GAME_STATE_EM_JOGO);
                }
                if (mTimerStageBegin.getTimeAndLock(11))
                {
                    mTimerStageBegin.stop();
                    mTimerStageBegin = null;
                }

                

            }
            /*
            //mTimerMessages alerts
            if (mTimerMessages.isActive())
            {
                //mUniversalTEXT = "AE PORRA: " + mTimerMessages.getTimeInt();

                if (mGameState == GAME_STATE_PREPARANDO)
                {
                    
                }else
                if (mGameState == GAME_STATE_SUCESSO)
                {
                    if (!mTimerMessages.isBusyForNumber(3) && mTimerMessages.getTimeInt() == 3)
                    {
                        //mUniversalTEXT = "AE PORRA: " + mTimerMessages.getTimeInt();
                        mTimerMessages.setBusyWithNumber(3);
                        //setGameState(GAME_STATE_PREPARANDO);
                        //mTimerSucesso.stop();
                        //nextWave();
                    }
                }else
                if (mGameState == GAME_STATE_DERROTA)
                {
                    if (!mTimerMessages.isBusyForNumber(3) && mTimerMessages.getTimeInt() == 3)
                    {
                        //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, false);
                    }
                }
            }*/

        }


        private void checkCollisions()
        {
            HUD.getInstance().checkCollisions(mCursor, mMousePressing);

            if (mManager.checkCollision(mMainCharacter))
            {
                BaseEnemy be = mManager.getGameObjectsGroup().getCollidedObject();

                if (be is Bako)
                {
                    int x = (int)be.getX();
                    int y = (int)be.getY();
                    mExplosionManager.getNextOfColor(be.getColor()).explode(be.getCenter());
                    be.destroy();
                    incrementProgress();
                    damage();
                }
            }

            if (mManager.checkCollision(mCursor))
            {   
                BaseEnemy be = mManager.getGameObjectsGroup().getCollidedObject();
                //mExplosionManager.getNextOfColor().explode((int)be.getX(), (int)be.getY());

                if (be.getColor() == mCursor.getColor())
                {
                    if (!mCursor.isParalyzed())
                    {
                        int x = (int)be.getX();
                        int y = (int)be.getY();
                        mExplosionManager.getNextOfColor(be.getColor()).explode(be.getCenter());
                        be.destroy();
                        incrementProgress();
                    }
                    //Collectable c = mGroupCollectables.getNext();
                    //c.appear(x, y);
                }
                else
                {
                    //FUDEU
                    mCursor.paralyze();
                }
                //mGroup.remove(be);

                
                
            }

            if (mManager.checkAttackCollision(mMainCharacter))
            {
                if (!mMainCharacter.isHurt())
                {
                    mMainCharacter.hurt();
                    damage();
                }

            }

        }

       

        private void checkGameOverCondition()
        {
            //if (mMainCharacter.getData().getEnergy() == 0)
            if (energy == 0)
            {
                setGameState(GAME_STATE_DERROTA);

                //mGameState = GAME_STATE_DERROTA;
            }

        }

        private void checkVictoryCondition()
        {
            if (mManager.enemiesAreOver())
            {
                //Game1.print("FINISH. YOU WIN");
                setGameState(GAME_STATE_SUCESSO);
            }
        }

        private void setGameState(int gameState)
        {

            //if (mGameState != gameState)
           // {
                mGameState = gameState;

                switch (gameState)
                {

                    case GAME_STATE_PREPARANDO:
                        SoundManager.PlayMusic(cMUSIC_BEGIN, false);
                        //mCamera.setZoom(1.4f);
                        mFlagTimer = FLAG_TIMER_PREPARANDO_WAIT_BEFORE_START;
                        //restartTimer(5);
                        mTimerStageBegin = new MTimer(true);
                        break;

                    case GAME_STATE_EM_JOGO:
                        SoundManager.PlayMusic(cMUSIC_STAGE11, true);
                        break;

                    case GAME_STATE_DERROTA:
                        SoundManager.PlayMusic(cMUSIC_LOSE, false);
                        mMainCharacter.changeState(MainCharacter.sSTATE_LOSE);
                        mCursor.setLocation(0, 1000);
                        startManualTimer();
                        break;

                    case GAME_STATE_SUCESSO:
                        startStageFinishExplosions();
                        //SoundManager.PlayMusic(cMUSIC_WIN, false);
                        //mMainCharacter.changeState(MainCharacter.sSTATE_VICTORY);
                        //mCursor.setLocation(0, 1000);
                        ///startManualTimer();
                        break;
                }
           // }


        }
        public override void update(GameTime gameTime)
        {
            if (!mPaused)
            {
                if (mCurrentWorld == sWORLD_1)
                {
                    mCamera.update();

                    updateTimers(gameTime);

                    switch (mGameState)
                    {
                        case GAME_STATE_PREPARANDO:

                            if (mFade != null)
                            {
                                mFade.update(gameTime);
                            }

                            //mCamera.zoomOut(0.004f);


                            // mBackgroundBack.update();
                            // mBackgroundFront.update();

                            mMainCharacter.update(gameTime);
                            updatePlayerBody();
                            break;

                        case GAME_STATE_EM_JOGO:
                            mBackgroundBack.update();
                            mBackgroundFront.update();

                            mMainCharacter.update(gameTime);

                            //mGroup.update(gameTime);
                            mGroupCollectables.update(gameTime);

                            updatePlayerBody();
                            checkVictoryCondition();
                            checkGameOverCondition();
                            checkCollisions();
                            
                            mCursor.update(gameTime);
                            MouseState mouseState = Mouse.GetState();

                            HUD.getInstance().update(gameTime);

                            mExplosionManager.update(gameTime);

                            mManager.update(gameTime);

                            break;

                        case GAME_STATE_DERROTA:

                            if (mEndStageTimer != null)
                            {
                                mEndStageTimer.update(gameTime);

                                if (mEndStageTimer.getTimeAndLock(5))
                                {
                                    //Game1.print("AE");
                                    //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true);
                                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true);
                                }
                            }

                            mBackgroundBack.update();
                            mBackgroundFront.update();
                            mGroupCollectables.update(gameTime);
                            mMainCharacter.update(gameTime);
                            mMainCharacter.setVisible(true);
                            mExplosionManager.update(gameTime);
                            mCursor.update(gameTime);
                            //mManager.update(gameTime);
                            mMainCharacter.update(gameTime);
                            mMainCharacter.setCollisionRect(0, 0, 0, 0);
                            mMainCharacter.setVisible(true);
                            break;

                        case GAME_STATE_SUCESSO:


                            if (mEndStageTimer != null)
                            {
                                mEndStageTimer.update(gameTime);

                                if (mEndStageTimer.getTimeAndLock(5))
                                {
                                    //Game1.print("CABOSSE");
                                    //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true);
                                }
                                else
                                {
                                    //mCamera.zoomIn(0.002f);
                                }
                            }
                            
                            mBackgroundBack.update();
                            mBackgroundFront.update();
                            mGroupCollectables.update(gameTime);
                            mMainCharacter.update(gameTime);
                            mMainCharacter.setVisible(true);
                            mExplosionManager.update(gameTime);
                            mCursor.update(gameTime);
                            break;
                    }


                }
            }
            else
            {
                mPauseScreen.update(gameTime);
            }
            /*
            if (KeyboardState.pressed(Keys.A))
            {
                Console.WriteLine("Fuck A");
                mMonster.setLocation(100, 100);
            }
            */
        }


        public override void draw(GameTime gameTime)
        {

            if (mCurrentWorld == sWORLD_1)
            {
                drawDesaturation(gameTime,mBackgroundBack);

                //mSpriteBatch.Begin();
                mSpriteBatch.Begin(
                        SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        mCamera.get_transformation(Game1.getInstance().GraphicsDevice));

                //mBackgroundBack.draw(mSpriteBatch);
                switch (mGameState)
                {
                    case GAME_STATE_PREPARANDO:

                        if (mReduceAlpha)
                        {
                            if (mAlphaBackground > 0)
                            {
                                mAlphaBackground -= 0.05f;
                            }
                            else
                            {
                                mAlphaBackground = 0;
                                mShowBlackBackground = false;
                                mReduceAlpha = false;
                            }
                            
                        }
                        if (mShowBlackBackground)
                        {
                            mSpriteBatch.Draw(mBlackBackground, new Rectangle(0, 0, 800, 600), Color.Black * mAlphaBackground);

                            if (mAlphaFontBegin < 1.0f) mAlphaFontBegin += 0.05f;
                            else mAlphaFontBegin = 1.0f;
                        }

                        mMainCharacter.draw(mSpriteBatch);
                        mFade.draw(mSpriteBatch);

                        if (mShowReady)
                        {
                            mSpriteBatch.Draw(mTextureReady, new Rectangle(400, 200, 100, 90), Color.White);
                        }
                        if (mShowGo)
                        {
                            mSpriteBatch.Draw(mTextureGO, new Rectangle(400, 200, 100, 90), Color.White);
                        }


                        break;

                    case GAME_STATE_SUCESSO:
                    case GAME_STATE_EM_JOGO:
                        mMainCharacter.draw(mSpriteBatch);
                        mManager.draw(mSpriteBatch);
                        mGroupCollectables.draw(mSpriteBatch);
                        mCursor.draw(mSpriteBatch);
                        mExplosionManager.draw(mSpriteBatch);
                        break;
                }
                
    
                //mSpriteBatch.DrawString(mFontDebug, ""+mUniversalTEXT, new Vector2(10, 100), Color.Red);
                mSpriteBatch.End();

                
                if (mShowBlackBackground)
                {
                    //FRONT BACKGROUND
                    drawDesaturation(gameTime, mBackgroundFront);
                    mSpriteBatch.Begin();
                    mSpriteBatch.Draw(mBlackBackground, new Rectangle(0, 0, 800, 600), Color.Black * mAlphaBackground);
                    mMainCharacter.draw(mSpriteBatch);
                    mFade.draw(mSpriteBatch);
                    mSpriteBatch.DrawString(mFontStageBegin, "SEACOAST", new Vector2(300, 100), Color.White * mAlphaFontBegin);
                    mSpriteBatch.End();
                }
                else
                {
                    //FRONT BACKGROUND
                    drawDesaturation(gameTime, mBackgroundFront);
                }
                
                if (mGameState == GAME_STATE_EM_JOGO)
                {
                    mSpriteBatch.Begin();
                    HUD.getInstance().draw(mSpriteBatch);
                    mCursor.draw(mSpriteBatch);
                    mSpriteBatch.End();
                }
                
            }

            if (mPaused)
            {
                mPauseScreen.draw(gameTime);
            }
   
        }

        void drawDesaturation(GameTime gameTime, Background background)
        {
            // Begin the sprite batch, using our custom effect.
            mSpriteBatch.Begin(0, null, null, null, null, desaturateEffect, mCamera.get_transformation(Game1.getInstance().GraphicsDevice));

            // Draw four copies of the same sprite with different saturation levels.
            // The saturation amount is passed into the effect using the alpha of the
            // SpriteBatch.Draw color parameter. This isn't as flexible as using a
            // regular effect parameter, but makes it easy to draw many sprites with
            // a different saturation amount for each. If we had used an effect
            // parameter for this, we would have to end the sprite batch, then begin
            // a new one, each time we wanted to change the saturation setting.

            byte pulsate = (byte)Pulsate(gameTime, 4, 0, 255);

            /*mSpriteBatch.Draw(mBackgroundBack.getTexture(),
                             mBackgroundBack.getRectangle(),
                //new Rectangle(0,0,Game1.sSCREEN_RESOLUTION_WIDTH,Game1.sSCREEN_RESOLUTION_HEIGHT),
                             new Color(255, 255, 255, pulse));
            */
            if (background == mBackgroundBack)
            {
                mBackgroundBack.draw(mSpriteBatch, new Color(255, 255, 255, pulse));
            }else
            if (background == mBackgroundFront)
            {
                mBackgroundFront.draw(mSpriteBatch, new Color(255, 255, 255, pulse));
            }

           /* mSpriteBatch.Draw(mBackgroundBack.getTexture(),
                             mBackgroundBack.getRectangle(),
                //new Rectangle(0,0,Game1.sSCREEN_RESOLUTION_WIDTH,Game1.sSCREEN_RESOLUTION_HEIGHT),
                             new Color(255, 255, 255, pulse));
            */
            // End the sprite batch.
            mSpriteBatch.End();
        }

        private void startStageFinishExplosions()
        {
            mTimerStageFinishExplosions = new MTimer(true);
        }

        private void explodeStageFinish()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            for(int k = 0; k < 2; k++){

                int x = rnd.Next(100, 700);
                int y = rnd.Next(10, 500);
                Color c;

                if(x%2 == 0){
                    c = Color.Red;
                }else{
                    if(y%2 == 0){
                        c = Color.Green;
                    }else{
                        c = Color.Blue;
                    }
                }

                mExplosionManager.getNextOfColor(c).explode(x, y);
                SoundManager.PlaySound(cSOUND_EXPLOSION);
            }
        }


        public void togglePauseGame()
        {
            mPaused = !mPaused;
        }

        public void setPauseGame(bool paused)
        {
            mPaused = paused;
        }

        public void damage()
        {
            energy -= 10;

            if (energy < 0)
                energy = 0;
         

            porcentagemRestante = ExtraFunctions.valueToPercent(energy, 100);

            int larguraDaBarraDoHud = 181;
            int novoValor = (int)ExtraFunctions.percentToValue((int)porcentagemRestante, larguraDaBarraDoHud);

            HUD.getInstance().setPlayerBarLevel(novoValor);
        }

        public void incrementProgress()
        {
            numberEnemies = mManager.getTotalEnemies();
            if (progress < numberEnemies)
                progress += 1;

            porcentagemRestante = ExtraFunctions.valueToPercent(progress, numberEnemies);

            int larguraDaBarraDoHud = 182;
            int novoValor = (int)ExtraFunctions.percentToValue((int)porcentagemRestante, larguraDaBarraDoHud);

            HUD.getInstance().setBarLevel(novoValor);
            pulse = (int) (porcentagemRestante * 63 / 100.0f);

        }

        public void updatePlayerBody()
        {
            if (mMainCharacter.getState() != MainCharacter.sSTATE_TOP_LEFT && mCursor.getX() < mMainCharacter.getX() && mCursor.getY() <= Game1.sHALF_SCREEN_RESOLUTION_HEIGHT)
            {
                if (mMainCharacter.getState() != MainCharacter.sSTATE_BOTTOM_LEFT && mMainCharacter.getState() != MainCharacter.sSTATE_INVERSE_TOP_LEFT)
                {
                    mMainCharacter.changeState(MainCharacter.sSTATE_TOP_LEFT);
                }
                else
                {
                    mMainCharacter.changeState(MainCharacter.sSTATE_INVERSE_TOP_LEFT);
                }

            }else
            if (mMainCharacter.getState() != MainCharacter.sSTATE_TOP_RIGHT && mCursor.getX() > mMainCharacter.getX() && mCursor.getY() <= Game1.sHALF_SCREEN_RESOLUTION_HEIGHT)
            {
                if (mMainCharacter.getState() != MainCharacter.sSTATE_BOTTOM_RIGHT && mMainCharacter.getState() != MainCharacter.sSTATE_INVERSE_TOP_RIGHT)
                {
                    mMainCharacter.changeState(MainCharacter.sSTATE_TOP_RIGHT);
                }
                else
                {
                    mMainCharacter.changeState(MainCharacter.sSTATE_INVERSE_TOP_RIGHT);
                }
                
            }else
            if (mMainCharacter.getState() != MainCharacter.sSTATE_BOTTOM_LEFT && mCursor.getX() < mMainCharacter.getX() && mCursor.getY() > Game1.sHALF_SCREEN_RESOLUTION_HEIGHT)
            {
                mMainCharacter.changeState(MainCharacter.sSTATE_BOTTOM_LEFT);
            }
            else
            if (mMainCharacter.getState() != MainCharacter.sSTATE_BOTTOM_RIGHT && mCursor.getX() > mMainCharacter.getX() && mCursor.getY() > Game1.sHALF_SCREEN_RESOLUTION_HEIGHT)
            {
                mMainCharacter.changeState(MainCharacter.sSTATE_BOTTOM_RIGHT);
            }
            
            

        }

        public Vector2 getPlayerLocation()
        {
            if (mMainCharacter != null)
            {
                return mMainCharacter.getLocation();
            }

            return new Vector2(0, 0);
        }

        public float getPlayerCenter()
        {
            if (mMainCharacter != null)
            {
                return mMainCharacter.getCenterX();
            }

            return 0f;
        }

        public Vector2 getPlayerCenterVector()
        {
            if (mMainCharacter != null)
            {
                return mMainCharacter.getCenter();
            }

            return Vector2.Zero;
        }

        public override void handleInput(InputState input)
        {
            base.handleInput(input);

            if (!Game1.sKINECT_BASED)
            {
                MouseState mouseState = Mouse.GetState();


                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (oldStateMouse.LeftButton != ButtonState.Pressed)
                    {
                        mCursor.nextColor();
                    }
                }
                

                oldStateMouse = mouseState;

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    mMousePressing = true;
                }
                else
                {
                    mMousePressing = false;
                }

                KeyboardState newState = Keyboard.GetState();
                /*
                if (newState.IsKeyDown(Keys.Down))
                {
                    pulse--;
                    mCamera.zoomOut(0.1f);
                }

                if (newState.IsKeyDown(Keys.Up))
                {
                    pulse++;
                    mCamera.zoomIn(0.1f);
                }*/
                if (newState.IsKeyDown(Keys.Space))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldState.IsKeyDown(Keys.Space))
                    {
                        //incrementProgress();
                        //damage();
                        explodeStageFinish();
                    }
                }
                if (newState.IsKeyDown(Keys.Escape))
                {
                    if (!oldState.IsKeyDown(Keys.Escape))
                    {
                        togglePauseGame();
                    }
                }


                oldState = newState;

            }
            else
            {
               
            }
        }//sabe uma notificação que fizeram ontem. Brinco formal demais


        /// <summary>
        /// Helper computes a value that oscillates over time.
        /// </summary>
        static float Pulsate(GameTime gameTime, float speed, float min, float max)
        {
            double time = gameTime.TotalGameTime.TotalSeconds * speed;

            return min + ((float)Math.Sin(time) + 1) / 2 * (max - min);
        }



        public override void executeFade(Fade fadeObject, int effect)
        {
            base.executeFade(fadeObject, effect);

            mCurrentFade = fadeObject;
            fadeObject.execute(effect);
        }

        ///////prototipo
        public override void fadeFinished(Fade fadeObject)
        {
            if (fadeObject.getEffect() == Fade.sFADE_IN_EFFECT_GRADATIVE)
            {

            }
            else
                if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
                {
                    //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true);
                }

        }
      
    }
}
