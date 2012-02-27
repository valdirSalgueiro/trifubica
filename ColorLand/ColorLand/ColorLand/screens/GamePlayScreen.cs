using System;
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


        int energy = 1000;

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


        private MTimer mTimerMessages = new MTimer();
        //private Timer mTimerSucesso = new Timer();
        //private Timer mTimerDerrota= new Timer();

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


        private Background mBackground;

        private MainCharacter mMainCharacter;

        private EnemySimpleFlying mTestEnemy;

        private EnemySimpleWalking mTestEnemy2;
        private EnemySimpleShooting mTestEnemy3;
        private EnemyArc mTestEnemy4;

        private GameObjectsGroup<BaseEnemy> mGroup = new GameObjectsGroup<BaseEnemy>();

        private Fade mFadeIn;
        private Fade mFadeOut;

        private Cursor mCursor;

        private Camera mCamera;

        private ColorChoiceBar mColorChoiceBar;
        private int mColorCount;

        private static Timer mTimer;

        Explosion mExplosion;
        
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
            
           // mGameState = GAME_STATE_PREPARANDO;
            mGameState = GAME_STATE_SUCESSO;

            mCamera = new Camera();

            loadWorld1(sWORLD_1);

            //setGameState(GAME_STATE_PREPARANDO);
            setGameState(GAME_STATE_EM_JOGO);


            mKeyboard = KeyboardManager.getInstance();
        }

        private void loadWorld1(int part)
        {

            mCurrentWorld = 1;
            mCurrentPart  = part;

            SoundManager.LoadSound("test\\iniciar");

            switch (part)
            {

                case 1:

                    mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

                    desaturateEffect = Game1.getInstance().getScreenManager().getContent().Load<Effect>("effects\\desaturate");

                    //mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");
                    //mFontAlert = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("alerts");

                    mBackground = new Background("test\\fase1_4");
                    mBackground.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackground.setLocation(0, 0);

                    mMainCharacter = new MainCharacter();
                    mMainCharacter.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mMainCharacter.setCenter(Game1.sSCREEN_RESOLUTION_WIDTH / 2, 400);

                    mGroup.addGameObject(new EnemyCrabCrab(Color.Red, new Vector2(300, 20)));
                    mGroup.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mCursor = new Cursor();
                    mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mCursor.changeColor(Color.Green);
                    mCursor.setCenter(20, 20);

                    mTestEnemy3 = new EnemySimpleShooting(BaseEnemy.sTYPE_SIMPLE_FLYING_RED);
                    mTestEnemy3.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mTestEnemy3.setCenter(100, 100);

                    mTestEnemy4 = new EnemyArc();
                    mTestEnemy4.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mTestEnemy4.setCenter(100, 100);


                    HUD.getInstance().loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosion = new Explosion();
                    mExplosion.loadContent(Game1.getInstance().getScreenManager().getContent());
                    //mCamera.zoomIn(2.4f);

                    /*
                     * mColorChoiceBar = new ColorChoiceBar();
                        mColorChoiceBar.loadContent(Game1.getInstance().getScreenManager().getContent());
                        mColorChoiceBar.setCenter(200, 200);*/

                    break;

            }


        }


        private void unloadWorld1()
        {
            unload();
            //executeFade(mFadeIn);
        }


        private void setGameState(int gameState)
        {

            mGameState = gameState;

            switch (gameState)
            {

                case GAME_STATE_PREPARANDO:

                    mCamera.setZoom(2.4f);
                    Game1.print("FUCK ME");
                    break;

            }

        }

        private void updateTimers()
        {
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
            }

        }


        private void checkCollisions()
        {
            if (mCursor.collidesWith(mMainCharacter))
            { 
                //Console.WriteLine("COLIDIU");
                //unloadWorld1();
            }
        }

       

        private void checkGameOverCondition()
        {
           
        }

        private void checkVictoryCondition()
        {
          
        }

        public override void executeFade(Fade fadeObject, int effect)
        {
            base.executeFade(fadeObject, effect);

            //fadeObject.activate();
        }


        public override void update(GameTime gameTime)
        {
            if (mCurrentWorld == sWORLD_1)
            {
                mCamera.update();

                switch (mGameState)
                {
                    case GAME_STATE_PREPARANDO:

                        mCamera.zoomOut(0.01f);
                mTestEnemy4.update(gameTime);

                        if (mCamera.getZoomLevel() == 1)
                        {
                            mFlagTimer = FLAG_TIMER_PREPARANDO_WAIT_BEFORE_START;
                            restartTimer(3);
                        }

                        break;

                    case GAME_STATE_EM_JOGO:
                        mBackground.update();

                        mMainCharacter.update(gameTime);

                        mGroup.update(gameTime);

                        checkVictoryCondition();
                        checkGameOverCondition();
                        checkCollisions();
                        updatePlayerBody();
                        mCursor.update(gameTime);
                        MouseState mouseState = Mouse.GetState();

                        HUD.getInstance().update(gameTime);

                        mExplosion.update(gameTime);
                        break;
                }


            }
            
            /*
            if (KeyboardState.pressed(Keys.A))
            {
                Console.WriteLine("Fuck A");
                mMonster.setLocation(100, 100);
            }
            */
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            mTimer.Stop();
            mTimer.Enabled = false;

            switch (mGameState)
            {
                case GAME_STATE_PREPARANDO:

                    if (mFlagTimer == FLAG_TIMER_PREPARANDO_WAIT_BEFORE_START)
                    {
                        this.setGameState(GAME_STATE_EM_JOGO);
                    }

                    break;
            }

            
        }

        private void restartTimer(int seconds)
        {
            mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            mTimer.Interval = seconds * 1000;
            mTimer.Enabled = true;
        }


        public override void draw(GameTime gameTime)
        {

            if (mCurrentWorld == sWORLD_1)
            {


                DrawDesaturate(gameTime);

                //mSpriteBatch.Begin();
                mSpriteBatch.Begin(
                        SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        mCamera.get_transformation(Game1.getInstance().GraphicsDevice));
                

                
                //mSpriteBatch.Begin();

                //mBackground.draw(mSpriteBatch);

                mMainCharacter.draw(mSpriteBatch);
                
                mTestEnemy4.draw(mSpriteBatch);

                mGroup.draw(mSpriteBatch);



                mCursor.draw(mSpriteBatch);

                mExplosion.draw(mSpriteBatch);
                
                //mSpriteBatch.Draw(getFadeImage(), new Rectangle(0, 0, 600, 500), new Color(0, 0, 0, 0.6f));
                //mFadeIn.draw(mSpriteBatch);
                //mFadeOut.draw(mSpriteBatch);

                //mColorChoiceBar.draw(mSpriteBatch);
                
                /*for (int x = 0; x < mGroupEnemies.getSize(); x++)
                {
                    if (!mGroupEnemies.getGameObject(x).isActive())
                    {
                        total++;
                    }
                }*/


                //mSpriteBatch.DrawString(mFontDebug, ""+mUniversalTEXT, new Vector2(10, 100), Color.Red);
                //mSpriteBatch.DrawString(mFontDebug, "" + mUniversalTEXT2, new Vector2(10, 140), Color.Red);

                HUD.getInstance().draw(mSpriteBatch);

                mSpriteBatch.End();
                //System.Diagnostics.Debug.WriteLine("loca");
            }

   
        }

        public void damage()
        {
            //energy -= 10   sobrou 90
            energy -= 13;
            float porcentagemRestante = ExtraFunctions.valueToPercent(energy, 1000);

            int larguraDaBarraDoHud = 200;
            int novoValor = (int)ExtraFunctions.percentToValue((int)porcentagemRestante, larguraDaBarraDoHud);

            HUD.getInstance().setPlayerBarLevel(novoValor);

        }

        public void updatePlayerBody()
        {
            Vector2 directionRightHand = mCursor.getLocation() - new Vector2(mMainCharacter.getX(), mMainCharacter.getY());//mVectorCenterOfScreen;
            float angleHandCursor = (float)(Math.Atan2(directionRightHand.Y, directionRightHand.X));

            mMainCharacter.updateHand(angleHandCursor);

            if (mCursor.getX() < mMainCharacter.getX() && mCursor.getY() < mMainCharacter.getY())
            {
                mMainCharacter.setBodyState(MainCharacter.BODYSTATE.UP_LEFT);
            }else
            if (mCursor.getX() > mMainCharacter.getX() && mCursor.getY() < mMainCharacter.getY())
            {
                mMainCharacter.setBodyState(MainCharacter.BODYSTATE.UP_RIGHT);
            }else
            if (mCursor.getX() < mMainCharacter.getX() && mCursor.getY() > mMainCharacter.getY())
            {
                mMainCharacter.setBodyState(MainCharacter.BODYSTATE.DOWN_LEFT);
            }else
            if (mCursor.getX() > mMainCharacter.getX() && mCursor.getY() > mMainCharacter.getY())
            {
                mMainCharacter.setBodyState(MainCharacter.BODYSTATE.DOWN_RIGHT);
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

        public override void handleInput(InputState input)
        {
            base.handleInput(input);

            if (!Game1.sKINECT_BASED)
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    //damage();
                    //manageColorCount();
                    //mBullet.goToXY(new Vector2(300, 300));
                    //mCamera.centerCamTo(100, 100);

                    mExplosion.explode(140, 140);

                    
                }

                KeyboardState newState = Keyboard.GetState();

                if (newState.IsKeyDown(Keys.Down))
                {
                    pulse--;
                    mCamera.zoomOut(0.1f);
                }

                if (newState.IsKeyDown(Keys.Up))
                {
                    pulse++;
                    mCamera.zoomIn(0.1f);
                }

                if (newState.IsKeyDown(Keys.Space))
                {
                    SoundManager.PlaySound("test\\iniciar");
                }

                //Game1.print("Z:" + mCamera.getZoomLevel());

            }
            else
            {
               
            }
        }//sabe uma notificação que fizeram ontem. Brinco formal demais

     
        ///////////////////////
        void DrawDesaturate(GameTime gameTime)
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

            mSpriteBatch.Draw(mBackground.getTexture(),
                             mBackground.getRectangle(),
                                //new Rectangle(0,0,Game1.sSCREEN_RESOLUTION_WIDTH,Game1.sSCREEN_RESOLUTION_HEIGHT),
                             new Color(255, 255, 255, pulse));

            // End the sprite batch.
            mSpriteBatch.End();
        }

        /// <summary>
        /// Helper computes a value that oscillates over time.
        /// </summary>
        static float Pulsate(GameTime gameTime, float speed, float min, float max)
        {
            double time = gameTime.TotalGameTime.TotalSeconds * speed;

            return min + ((float)Math.Sin(time) + 1) / 2 * (max - min);
        }

      
    }
}
