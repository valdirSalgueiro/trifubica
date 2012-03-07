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


        /*******************
         * CONSTANTS
         *******************/
        public static int sGROUND_WORLD_1_1 = 500;

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


        private Background mBackgroundBack;
        private Background mBackgroundFront;

        private MainCharacter mMainCharacter;

        //private GameObjectsGroup<BaseEnemy> mGroup = new GameObjectsGroup<BaseEnemy>();
        private GameObjectsGroup<Collectable> mGroupCollectables = new GameObjectsGroup<Collectable>();

        private Fade mFadeIn;
        private Fade mFadeOut;

        private Cursor mCursor;

        private Camera mCamera;

        private ColorChoiceBar mColorChoiceBar;
        private int mColorCount;

        private static Timer mTimer;

        Explosion mExplosion;
        private ExplosionManager mExplosionManager;


        private EnemyManager mManager = new EnemyManager();


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

                    mBackgroundBack  = new Background("gameplay\\backgrounds\\stage1_1\\stage1_1_layer1");
                    mBackgroundFront = new Background("gameplay\\backgrounds\\stage1_1\\stage1_1_layer3");
                    
                    //String[] imagesBG = new String[9];
                    /*for (int x = 0; x < imagesBG.Length; x++)
                    {
                        imagesBG[x] = "test\\e" + (x+1);
                    }
                    mBackground.addPart(imagesBG, 2, 200, 200, 40, 500);
                    */

                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\stage1_1_layer2" },1,1000,600,0,0);
                    //mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\stage1_1_layer3" }, 1, 1000, 600, 0, 0);
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\stage1_1_layer4" }, 1, 1000, 600, 0, 0);

                    mBackgroundBack.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundBack.setLocation(0, 0);

                    mBackgroundFront.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundFront.setLocation(0, 0);


                    mMainCharacter = new MainCharacter(Color.Blue);
                    mMainCharacter.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mMainCharacter.setCenter(Game1.sSCREEN_RESOLUTION_WIDTH / 2, 440);

                   /* mGroup.addGameObject(new EnemyCrabCrab(Color.Green, new Vector2(300, 320)));
                    mGroup.addGameObject(new EnemyArc(Color.Blue));
                    mGroup.loadContent(Game1.getInstance().getScreenManager().getContent());
                    */
                    /*for (int x = 0; x < mGroup.getSize(); x++)
                    {
                        mGroupCollectables.addGameObject(new Collectable(Collectable.CollectableType.StagePiece));
                    }*/

                    mGroupCollectables.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mCursor = new Cursor();
                    mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mCursor.changeColor(Color.Green);
                    mCursor.setCenter(20, 20);

                    EnemySimpleShooting mTestEnemy3;
                    EnemyArc mTestEnemy4;

                    mTestEnemy3 = new EnemySimpleShooting(BaseEnemy.sTYPE_SIMPLE_FLYING_RED);
                    mTestEnemy3.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mTestEnemy3.setCenter(100, 100);

                    mTestEnemy4 = new EnemyArc(Color.Blue);
                    mTestEnemy4.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mTestEnemy4.setCenter(100, 100);

                    
                    HUD.getInstance().loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosion = new Explosion();
                    mExplosion.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosionManager = new ExplosionManager();
                    mExplosionManager.add(new Explosion(), Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.add(new Explosion(), Game1.getInstance().getScreenManager().getContent());

                    mManager.addEnemy(new EnemyCrabCrab(Color.Green),new Vector2(300, 320));
                    mManager.addEnemy(new EnemyCrabCrab(Color.Red), new Vector2(500, 320));
                    mManager.addEnemy(new EnemyCrabCrab(Color.Red), new Vector2(-100, 320));
                    mManager.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mManager.start();
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
                    //Game1.print("FUCK ME");
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

            /*if (mGroup.checkCollisionWith(mCursor))
            {

                BaseEnemy be = mGroup.getCollidedObject();
                //mExplosionManager.getNextOfColor().explode((int)be.getX(), (int)be.getY());
                int x = (int)be.getX();
                int y = (int)be.getY();
                mExplosionManager.getNextOfColor().explode(x,y);
                be.destroy();
                mGroup.remove(be);

                Collectable c = mGroupCollectables.getNext();
                c.appear(x, y);
                
            }

            if (mGroup.checkAttackCollisionWith(mMainCharacter))
            {
                Game1.print("AFE... bateu ````````````````````````````````````````````````````````````````````````````````````");
                //if(!player.damaged) damageit

            }*/

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
                        
                        if (mCamera.getZoomLevel() == 1)
                        {
                            mFlagTimer = FLAG_TIMER_PREPARANDO_WAIT_BEFORE_START;
                            restartTimer(3);
                        }

                        break;

                    case GAME_STATE_EM_JOGO:
                        mBackgroundBack.update();
                        mBackgroundFront.update();

                        mMainCharacter.update(gameTime);

                        //mGroup.update(gameTime);
                        mGroupCollectables.update(gameTime);

                        checkVictoryCondition();
                        checkGameOverCondition();
                        checkCollisions();
                        updatePlayerBody();
                        mCursor.update(gameTime);
                        MouseState mouseState = Mouse.GetState();

                        HUD.getInstance().update(gameTime);

                        mExplosionManager.update(gameTime);

                        mManager.update(gameTime);

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
                
                mMainCharacter.draw(mSpriteBatch);

                mManager.draw(mSpriteBatch);

                //mGroup.draw(mSpriteBatch);

                mGroupCollectables.draw(mSpriteBatch);

                mCursor.draw(mSpriteBatch);

                mExplosionManager.draw(mSpriteBatch);
                
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

               

                mSpriteBatch.End();

                drawDesaturation(gameTime, mBackgroundFront);

                mSpriteBatch.Begin();
                HUD.getInstance().draw(mSpriteBatch);
                mCursor.draw(mSpriteBatch);
                mSpriteBatch.End();

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

        public void damage()
        {
            //energy -= 10   sobrou 90
            Console.WriteLine("*******");
            Console.WriteLine(energy);
            energy -= 13;
            Console.WriteLine(energy);
            float porcentagemRestante = ExtraFunctions.valueToPercent(energy, 1000);
            Console.WriteLine(porcentagemRestante);

            int larguraDaBarraDoHud = 200;
            int novoValor = (int)ExtraFunctions.percentToValue((int)porcentagemRestante, larguraDaBarraDoHud);
            Console.WriteLine(novoValor);

            HUD.getInstance().setPlayerBarLevel(novoValor);

        }

        public void updatePlayerBody()
        {
            Vector2 directionRightHand = mCursor.getLocation() - new Vector2(mMainCharacter.getX(), mMainCharacter.getY());//mVectorCenterOfScreen;
            float angleHandCursor = (float)(Math.Atan2(directionRightHand.Y, directionRightHand.X));

            mMainCharacter.updateHand(angleHandCursor);

            if (mMainCharacter.getState() != MainCharacter.sSTATE_TOP_LEFT && mCursor.getX() < mMainCharacter.getX() && mCursor.getY() < mMainCharacter.getY())
            {
                mMainCharacter.changeState(MainCharacter.sSTATE_TOP_LEFT);
            }else
            if (mMainCharacter.getState() != MainCharacter.sSTATE_TOP_RIGHT && mCursor.getX() > mMainCharacter.getX() && mCursor.getY() < mMainCharacter.getY())
            {
                mMainCharacter.changeState(MainCharacter.sSTATE_TOP_RIGHT);
            }else
            if (mCursor.getX() < mMainCharacter.getX() && mCursor.getY() > mMainCharacter.getY())
            {
                //mMainCharacter..changeState(MainCharacter.sSTATE_dow;
            }else
            if (mCursor.getX() > mMainCharacter.getX() && mCursor.getY() > mMainCharacter.getY())
            {
                //mMainCharacter.setBodyState(MainCharacter.BODYSTATE.DOWN_RIGHT);
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
                return mMainCharacter.getCenter();
            }

            return 0f;
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

                    Explosion e = mExplosionManager.getNextOfColor();
                    if (e != null)
                    {
                        mExplosionManager.getNextOfColor().explode(mouseState.X, mouseState.Y);
                    }
                    //Game1.print("oxe");
                    mCursor.nextColor();

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
                    damage();
                    //SoundManager.PlaySound("test\\iniciar");
                }

                //Game1.print("Z:" + mCamera.getZoomLevel());

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

      
    }
}
