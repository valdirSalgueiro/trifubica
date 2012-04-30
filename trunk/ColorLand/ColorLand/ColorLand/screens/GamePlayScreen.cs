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
using ColorLand.managers;


namespace ColorLand
{
    public class GamePlayScreen : BaseScreen
    {

        private Effect desaturateEffect;

        private const String cMUSIC_BEGIN = "sound\\music\\begin";
        private const String cMUSIC_STAGE1 = "sound\\music\\stage11";
        private const String cMUSIC_STAGE2 = "sound\\music\\loop2";
        //private const String cMUSIC_BEGIN = "sound\\music\\begin";
        private const String cMUSIC_WIN = "sound\\music\\win";
        private const String cMUSIC_LOSE = "sound\\music\\loose";

        private PauseScreen mPauseScreen;

        private const String cSOUND_COLOR = "sound\\fx\\colorswap8bit";
        public const String cSOUND_EXPLOSION = "sound\\fx\\explosao8bit";
        public const String cSOUND_CHECKPOINT1 = "sound\\fx\\checkpoint1.1-8bit";
        public const String cSOUND_CHECKPOINT2 = "sound\\fx\\checkpoint1.2.2-8bit";
        public const String cSOUND_CHECKPOINT3 = "sound\\fx\\checkpoint1.3.2-8bit";
        public const String cSOUND_WRONG_COLOR = "sound\\fx\\corerrada2.1-8bit";
        public const String cSOUND_MONSTER_APPEAR = "sound\\fx\\monstrosurg1-8bit";

        public const String cSOUND_CHAR_OOPS           = "sound\\fx\\charsounds\\ooops";
        public const String cSOUND_CHAR_YES            = "sound\\fx\\charsounds\\yes";
        public const String cSOUND_CHAR_ALMOST_THERE   = "sound\\fx\\charsounds\\almost_there";
        public const String cSOUND_CHAR_YEAH_WE_DID_IT = "sound\\fx\\charsounds\\yeah_we_did_it";
        public const String cSOUND_CHAR_NICE_COMBO     = "sound\\fx\\charsounds\\nice_combo";
        public const String cSOUND_CHAR_GREAT_COMBO    = "sound\\fx\\charsounds\\great_combo";
        public const String cSOUND_COLORLAND_COMBO     = "sound\\fx\\charsounds\\colorland_combo";
        public const String cSOUND_COLORLAND_OUCH      = "sound\\fx\\charsounds\\OUCH";
        

        /*******************
         * CONSTANTS
         *******************/
        public static int sGROUND_WORLD_1_1 = 500;

        int energy = 1;//100;
        int progress = 0;
        int numberEnemies = 40;
        int pulse = 0;

        private bool mOneThirdDone;
        private bool mSecondThirdDone;


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
        public const int sSTAGE_1 = 1;
        public const int sSTAGE_2 = 2;
        public const int sSTAGE_3 = 3;
        public const int sSTAGE_4 = 4;

        private int mCurrentStage;

        /* ******************
         * COMBO SYSTEM
         * *******************/
        private const int cCOMBO_TIME = 4;
        private MTimer mTimerCombo;
        private int mComboCounter;

        /* ******************
         * CAMERA WALKING
         * *************/
        private const int cSPACE_TO_WALK = 100;
        public static int sCURRENT_STAGE_X = 0; //margem esquerda da tela, que vai andando
        public static float sCURRENT_STAGE_X_PROGRESSIVE = 0; //gradativo. Tem importancia so pra informar a classe Cursor (pra corrigir o bug do mouse bichado apos o scrolling)
        private bool mWalkCamera;

        /* ******************
        * BACKGROUND MOVEMENT
        * *************/
        private MTimer mTimerBackgroundMovement;

        /* ******************
        * FADE PARAMS
        * *************/
        private Texture2D mBlackBackground = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("fades\\blackfade");

        private FADE_PARAM mFadeParam;

        public enum FADE_PARAM
        {
            NEXT_STAGE,
            GAME_OVER,
        }


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

        private bool mMustZoomAtFinish;


        /****
         * INTRO 
         ****/
        private bool mShowBlackBackground = true;
        //private Texture2D mBlackBackground;


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
            GamePlayScreen.sCURRENT_STAGE_X = 0;

            mTimer = null;
           // mGameState = GAME_STATE_PREPARANDO;
            mGameState = GAME_STATE_SUCESSO;

            mCamera = new Camera();
                        
            mCurrentStage = ExtraFunctions.loadProgress().getCurrentStage();
            mCurrentStage = 2; //debug
            loadStage(mCurrentStage);

            mPauseScreen = new PauseScreen(this);

            mShowBlackBackground = false;
            //setGameState(GAME_STATE_EM_JOGO);
            setGameState(GAME_STATE_PREPARANDO);
           
            mKeyboard = KeyboardManager.getInstance();

            mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.SLOW);

            SoundManager.SetMusicVolume(0.8f);
            
            executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);
            //,
        }

        private void loadStage(int stage)
        {

            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();
            //mBlackBackground = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("fades\\blackfade");
            desaturateEffect = Game1.getInstance().getScreenManager().getContent().Load<Effect>("effects\\desaturate");
            
            SoundManager.LoadSound(cSOUND_COLOR);
            SoundManager.LoadSound(cSOUND_EXPLOSION);
            SoundManager.LoadSound(cSOUND_CHECKPOINT1);
            SoundManager.LoadSound(cSOUND_CHECKPOINT2);
            SoundManager.LoadSound(cSOUND_CHECKPOINT3);
            SoundManager.LoadSound(cSOUND_WRONG_COLOR);
            SoundManager.LoadSound(cSOUND_MONSTER_APPEAR);
        
            //char sounds
            SoundManager.LoadSound(cSOUND_CHAR_OOPS);
            SoundManager.LoadSound(cSOUND_CHAR_YES);
            SoundManager.LoadSound(cSOUND_CHAR_NICE_COMBO);
            SoundManager.LoadSound(cSOUND_CHAR_GREAT_COMBO);
            SoundManager.LoadSound(cSOUND_COLORLAND_COMBO);
            
            mFontStageBegin = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("font\\stagebegin_font");
            mTextureReady = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("texts\\ready");
            mTextureGO = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\instruction_3");

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());
            mCursor.changeColor(Color.Green);
            mCursor.setCenter(20, 20);
            mCursor.changeColor(Color.Blue);

            if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.BLUE)
            {
                mMainCharacter = new MainCharacter(Color.Blue);
            }
            if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.GREEN)
            {
                mMainCharacter = new MainCharacter(Color.Green);
            }
            if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.RED)
            {
                mMainCharacter = new MainCharacter(Color.Red);
            }

            mMainCharacter = new MainCharacter(Color.Red);

            mMainCharacter.loadContent(Game1.getInstance().getScreenManager().getContent());
            mMainCharacter.setCenter(Game1.sSCREEN_RESOLUTION_WIDTH / 2, 434);


            //TODO debug
            mCurrentStage = 4;

            switch (mCurrentStage)
            {

                case sSTAGE_1:

                    mBackgroundBack = new Background("gameplay\\backgrounds\\stage1_1\\new\\fase1_bg");
                    mBackgroundFront = new Background();//new Background("gameplay\\backgrounds\\stage1_1\\new\\stage1_1_layer3");
            
                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\new\\fase1_plataforma_02" },1,1000,163,0,600-163); //plataforma
                    
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\new\\fase1_plataforma03" }, 1, 78, 147, 120, 600-147); //madeira esq
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\new\\fase1_plataforma04" }, 1, 65, 126, 730, 600 - 126); //madeira dir
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\new\\fase1_re_06" }, 1, 112, 185, 0, 600 - 185); //pato bottom left
                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\new\\fase1_re_01" }, 1, 424, 221, 0, 0); //folhas top left
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\new\\fase1_re_03" }, 1, 229, 163, 1000-229, 0); //folhas top right
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\new\\fase1_re_09" }, 1, 121, 145, 1000-121, 600-145); //mato bottom down

                    mBackgroundBack.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundBack.setLocation(0, 0);

                    mBackgroundFront.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundFront.setLocation(0, 0);
                   
                    mGroupCollectables.loadContent(Game1.getInstance().getScreenManager().getContent());

                    HUD.getInstance(this).loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosionManager = new ExplosionManager();
                    mExplosionManager.addExplosion(20, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(100, 400));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(100, 400));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(100, 400));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(100, 400));
                    
                    mManager.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mManager.start();
                   
                    break;

                case sSTAGE_2:

                    mBackgroundBack = new Background("gameplay\\backgrounds\\stage1_2\\agua_fase2_cehu");//"gameplay\\backgrounds\\stage1_1\\stage1_1_layer1");
                    mBackgroundFront = new Background();

                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_2\\agua_fase2_nuvem" }, 1, 1000, 465, 0, 0);
                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_2\\agua_fase2_barcoright" }, 1, 207, 204, 860, 215);
                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_2\\agua_fase2_barcoleft" }, 1, 249, 260, 200, 260);
                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_2\\agua_fase2_barcofundo" }, 1, 1000, 600, 0, 0);
                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_2\\agua_fase2_plataforma" }, 1, 1000, 105, 0, 600-105);
                    //mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_1\\stage1_1_layer3" }, 1, 1000, 600, 0, 0);
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_2\\agua_fase2_caixasleft" }, 1, 321, 222, 0, 600-222);
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_2\\agua_fase2_caixasright" }, 1, 247, 222, 1000 - 247, 600 - 222);

                    mBackgroundBack.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundBack.setLocation(0, 0);

                    mBackgroundFront.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundFront.setLocation(0, 0);
                   
                    mGroupCollectables.loadContent(Game1.getInstance().getScreenManager().getContent());

                    HUD.getInstance(this).loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosionManager = new ExplosionManager();

                    mExplosionManager.addExplosion(20, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                    /*mManager.addEnemy(EnemyManager.EnemiesTypes.Kaktos, Color.Red, new Vector2(0, 350));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(150, 300));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(200, 400));

                    mManager.addEnemy(EnemyManager.EnemiesTypes.Rocker, Color.Red, new Vector2(0, 50));
                    //mManager.addEnemy(EnemyManager.EnemiesTypes.Kaktos, Color.Red, new Vector2(0, 350));
                    //mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(150, 300));
                    //mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(200, 400));

                    */
                    
                    //mManager.addEnemy(EnemyManager.EnemiesTypes.Kaktos, Color.Red, new Vector2(100, getPlayerLocation().Y));
                    //mManager.addEnemy(EnemyManager.EnemiesTypes.MongoPirate, Color.Red, new Vector2(200, 0));
                    
                    //mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Green, new Vector2(200, 400));
                    /*mManager.addEnemy(EnemyManager.EnemiesTypes.MongoPirate, Color.Blue, new Vector2(700, 60));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Green, new Vector2(500, 0));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Blue, new Vector2(700, 60));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.MongoPirate, Color.Blue, new Vector2(40, 110));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.MongoPirate, Color.Red, new Vector2(100, 200));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Red, new Vector2(150, 300));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Green, new Vector2(200, 400));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Bako, Color.Blue, new Vector2(700, 60));
                    */
                      
                    //mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Red, new Vector2(-100, getPlayerLocation().Y));                
   
                    mManager.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mManager.start();

                    mTimerBackgroundMovement = new MTimer(true);

                    break;

                //SELVA
                case sSTAGE_3:

                    mBackgroundBack = new Background("gameplay\\backgrounds\\stage1_4\\fase42.0");//"gameplay\\backgrounds\\stage1_1\\stage1_1_layer1");
                    mBackgroundFront = new Background();

                    //mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_3\\fase3_plataforma" }, 1, 1000, 133, 0, 400);
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_4\\fase4_02" }, 1, 211, 367, 0, 600-367); //mato esquerda
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_4\\fase4_05" }, 1, 177, 307, 1000-177, 600 - 307);
                    

                    mBackgroundBack.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundBack.setLocation(0, 0);

                    mBackgroundFront.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundFront.setLocation(0, 0);

                    mGroupCollectables.loadContent(Game1.getInstance().getScreenManager().getContent());

                    HUD.getInstance(this).loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosionManager = new ExplosionManager();

                    mExplosionManager.addExplosion(20, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                    mManager.addEnemy(EnemyManager.EnemiesTypes.Mongo, Color.Green, new Vector2(200, 100));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Mongo, Color.Green, new Vector2(200, 200));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Mongo, Color.Green, new Vector2(200, 300));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Green, new Vector2(200, 400));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Red, new Vector2(400, 400));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Blue, new Vector2(700, 400));

                    mManager.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mManager.start();

                    mTimerBackgroundMovement = new MTimer(true);

                    break;

                //DESERTO
                case sSTAGE_4:

                    mBackgroundBack = new Background("gameplay\\backgrounds\\stage1_3\\fase3_bg");//"gameplay\\backgrounds\\stage1_1\\stage1_1_layer1");
                    mBackgroundFront = new Background();

                    mBackgroundBack.addPart(new String[1] { "gameplay\\backgrounds\\stage1_3\\fase3_plataforma" }, 1, 1000, 133, 0, 400);
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_3\\fase3_02"}, 1, 237, 176, 0, 600 - 176);
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_3\\fase3_05_03"}, 1, 83, 77, 500, 600 - 77);
                    mBackgroundFront.addPart(new String[1] { "gameplay\\backgrounds\\stage1_3\\fase3_06"}, 1, 83, 116, 500, 600 - 116);
                    
                    mBackgroundBack.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundBack.setLocation(0, 0);

                    mBackgroundFront.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mBackgroundFront.setLocation(0, 0);

                    mGroupCollectables.loadContent(Game1.getInstance().getScreenManager().getContent());

                    HUD.getInstance(this).loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosionManager = new ExplosionManager();

                    mExplosionManager.addExplosion(20, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(20, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                    mManager.addEnemy(EnemyManager.EnemiesTypes.Mongo, Color.Green, new Vector2(200, 100));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Mongo, Color.Green, new Vector2(200, 200));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Mongo, Color.Green, new Vector2(200, 300));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Green, new Vector2(200, 400));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Red, new Vector2(400, 400));
                    mManager.addEnemy(EnemyManager.EnemiesTypes.Lizardo, Color.Blue, new Vector2(700, 400));
                    
                    mManager.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mManager.start();

                    mTimerBackgroundMovement = new MTimer(true);

                    break;
            }

            RockManager.getInstance().em = mExplosionManager;
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

            if (mTimerCombo != null)
            {
                mTimerCombo.update(gameTime);

                if (mTimerCombo.getTimeAndLock(cCOMBO_TIME))
                {
                    mTimerCombo.stop();
                    mTimerCombo = null;
                    mComboCounter = 0;
                    Game1.print("----CABOSSE O COMBO----");
                }

            }

            if (mTimerStageFinishExplosions != null)
            {
                mTimerStageFinishExplosions.update(gameTime);
                
                for(double k=0; k<4.2; k+=0.2){
                    if (mTimerStageFinishExplosions.getTimeAndLock(k)) {
                        explodeStageFinish(); 
                    }
                }
                
                if (mTimerStageFinishExplosions.getTimeAndLock(5.5))
                {
                    mMustZoomAtFinish = false;
                    SoundManager.PlayMusic(cMUSIC_WIN, false);
                    mMainCharacter.changeState(MainCharacter.sSTATE_VICTORY);
                    mCursor.setLocation(0, 1000);
                }

                if (mTimerStageFinishExplosions.getTimeAndLock(1.5))
                {
                    //mCamera.zoomIn();
                                        //mCamera.centerCamTo(mMainCharacter);
                    
                }

                if (mMustZoomAtFinish)
                {
                    mCamera.setZoom(mCamera.getZoomLevel() + 0.0005f);
                }
                // mCamera.updateMalucoCenterCam(mMainCharacter.getX(), mMainCharacter.getY());
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

            if (mTimerBackgroundMovement != null)
            {
                mTimerBackgroundMovement.update(gameTime);

                if (mCurrentStage == sSTAGE_2)
                {

                    for (double x = 0; x < 3; x += 0.1)
                    {
                        if (mTimerBackgroundMovement.getTimeAndLock(x))
                        {
                            mBackgroundBack.getPart(2).addY(0.3f); //barco da esquerda
                            mBackgroundBack.getPart(1).addY(0.3f); //barco da direita
                            mBackgroundBack.getPart(3).reduceY(0.8f);//traseira do barco

                            mBackgroundFront.getPart(0).addY(1f); //caixas da esquerda
                            mBackgroundFront.getPart(1).addY(1f); //caixas da direita
                        }
                    }

                    mBackgroundBack.getPart(2).addX(0.1f); //barco esquerda anda pra direita
                    mBackgroundBack.getPart(1).reduceX(0.1f); //barco direita anda pra esquerda

                    for (double x = 3; x < 6; x += 0.1)
                    {
                        if (mTimerBackgroundMovement.getTimeAndLock(x))
                        {
                            mBackgroundBack.getPart(2).reduceY(0.3f); //barco da esquerda
                            mBackgroundBack.getPart(1).reduceY(0.3f); //barco da direita
                            mBackgroundBack.getPart(3).addY(0.8f); //traseira do barco

                            mBackgroundFront.getPart(0).reduceY(1f);  //caixas da esquerda
                            mBackgroundFront.getPart(1).reduceY(1f); //caixas da direita
                        }
                    }

                    if (mTimerBackgroundMovement.getTimeAndLock(7))
                    {
                        mTimerBackgroundMovement = new MTimer(true);
                    }

                }//end stage 2


            }
            
        }


        private void checkCollisions()
        {
            HUD.getInstance(this).checkCollisions(mCursor, mMousePressing);

            if (mManager.checkCollision(mMainCharacter))
            {
                BaseEnemy be = (BaseEnemy)mManager.getGameObjectsGroup().getCollidedObject();

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

            if (!mCursor.isInnofensive() && mManager.checkCollision(mCursor))
            {   
                BaseEnemy be = (BaseEnemy)mManager.getGameObjectsGroup().getCollidedObject();
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
                        
                        //COMBO
                        if (mComboCounter == 0)
                        {
                            mTimerCombo = new MTimer(true);
                        }
                        mComboCounter++;
                        if (mComboCounter == 3)
                        {
                            SoundManager.PlaySound(cSOUND_CHAR_NICE_COMBO);
                        }
                        if (mComboCounter == 6)
                        {
                            SoundManager.PlaySound(cSOUND_CHAR_GREAT_COMBO);
                            if (mTimerCombo != null)
                            {
                                mTimerCombo.start();
                            }
                        }
                        if (mComboCounter == 9)
                        {
                            SoundManager.PlaySound(cSOUND_COLORLAND_COMBO);
                            if (mTimerCombo != null)
                            {
                                mTimerCombo.start();
                            }
                        }

                    }
                    //Collectable c = mGroupCollectables.getNext();
                    //c.appear(x, y);
                }
                else
                {
                    //FUDEU
                    if (!mCursor.isParalyzed())
                    {
                        SoundManager.PlaySound(cSOUND_CHAR_OOPS);
                        mCursor.paralyze();
                    }

                }
                //mGroup.remove(be);

                
                
            }

            if (mManager.checkAttackCollision(mMainCharacter))
            {
                damage();
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
                        //if(mCurrentStage == sSTAGE_1) SoundManager.PlayMusic(cMUSIC_STAGE1, true);
                        //if(mCurrentStage == sSTAGE_2) SoundManager.PlayMusic(cMUSIC_STAGE2, true);
                        break;

                    case GAME_STATE_DERROTA:
                        SoundManager.PlayMusic(cMUSIC_LOSE, false);
                        mMainCharacter.changeState(MainCharacter.sSTATE_LOSE);
                        mCursor.setLocation(0, 1000);
                        mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.SLOW);
                        //fffffffffffffff
                        startManualTimer();
                        break;

                    case GAME_STATE_SUCESSO:
                        SoundManager.PlaySound(cSOUND_CHECKPOINT3);
                        startStageFinishExplosions();
                        mMustZoomAtFinish = true;
                        Game1.progressObject.setCurrentStage(++mCurrentStage);
                        ExtraFunctions.saveProgress(Game1.progressObject);
            



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

            sCURRENT_STAGE_X_PROGRESSIVE = mCamera.getX();

            if (!mPaused)
            {
                // if (mCurrentStage == sSTAGE_1)
                //{
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
                            if (mWalkCamera)
                            {
                                mCamera.centerCamTo(Game1.sHALF_SCREEN_RESOLUTION_WIDTH + sCURRENT_STAGE_X, Game1.sHALF_SCREEN_RESOLUTION_HEIGHT);
                            }
                            
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

                            HUD.getInstance(this).update(gameTime);

                            mExplosionManager.update(gameTime);

                            mManager.update(gameTime);
                            RockManager.getInstance().update(gameTime);
                            break;

                        case GAME_STATE_DERROTA:

                            if (mFade != null)
                            {
                                mFade.update(gameTime);
                            }

                            if (mEndStageTimer != null)
                            {
                                mEndStageTimer.update(gameTime);
                                //Game1.print("#STARTED MANUAL TIMER - $22");
                                if (mEndStageTimer.getTimeAndLock(3))
                                {
                                    //Game1.print("AE");
                                    //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true);
                                    //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true);
                                    executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                                    mFadeParam = FADE_PARAM.GAME_OVER;

                                    Game1.print("#STARTED MANUAL TIMER - $2");
                                }
                            }
                            /*
                             * 
                             * */

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


                //}
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

            //if (mCurrentWorld == sSTAGE_1)
            //{
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
                            mSpriteBatch.Draw(mTextureReady, new Rectangle(300, 280, 265, 64), Color.White);
                        }
                        if (mShowGo)
                        {
                            mSpriteBatch.Draw(mTextureGO, new Rectangle(400, 200, 100, 90), Color.White);
                        }


                        break;

                    case GAME_STATE_DERROTA:
                        if (mFade != null)
                        {
                            mFade.draw(mSpriteBatch);
                        }
                        mMainCharacter.draw(mSpriteBatch);
                        mManager.draw(mSpriteBatch);
                        RockManager.getInstance().draw(mSpriteBatch);
                        mGroupCollectables.draw(mSpriteBatch);
                        mExplosionManager.draw(mSpriteBatch);
                        break;
                    case GAME_STATE_SUCESSO:
                    case GAME_STATE_EM_JOGO:
                        mMainCharacter.draw(mSpriteBatch);
                        RockManager.getInstance().draw(mSpriteBatch);
                        mManager.draw(mSpriteBatch);
                        mGroupCollectables.draw(mSpriteBatch);
                        //mCursor.draw(mSpriteBatch);
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
                    
                    mSpriteBatch.End();

                    mSpriteBatch.Begin(
                        SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        mCamera.get_transformation(Game1.getInstance().GraphicsDevice));
                   // mCursor.draw(mSpriteBatch);

                    HUD.getInstance(this).draw(mSpriteBatch);
                    mCursor.draw(mSpriteBatch);
                    
                    mSpriteBatch.End();

                }
                
            //}

            if (mPaused)
            {
                mPauseScreen.draw(gameTime);
            }
   
        }

        //item do carai de asa - destruicao em massa
        public void instantInk(Color color)
        {
            for (int k = 0; k < mManager.getGameObjectsGroup().getSize(); k++)
            {
                BaseEnemy be = (BaseEnemy)mManager.getGameObjectsGroup().getGameObject(k);

                if (be.getColor() == color)
                {
                    int x = (int)be.getX();
                    int y = (int)be.getY();
                    mExplosionManager.getNextOfColor(be.getColor()).explode(be.getCenter());
                    be.destroy();
                    incrementProgress();
                }
            }
            

        }

        void drawDesaturation(GameTime gameTime, Background background)
        {
            // Begin the sprite batch, using our custom effect.
            if (mCurrentStage == sSTAGE_4)
            {
                desaturateEffect.Parameters["fTimer"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
                desaturateEffect.Parameters["iSeed"].SetValue(1337);
                desaturateEffect.Parameters["fNoiseAmount"].SetValue(0.002f);
                desaturateEffect.Parameters["bHeat"].SetValue(true);
            }
            
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

                int x = rnd.Next(GamePlayScreen.sCURRENT_STAGE_X+100, 700 + GamePlayScreen.sCURRENT_STAGE_X);
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
            if (!mMainCharacter.isHurt())
            {
                energy -= 10;

                if (energy < 0)
                    energy = 0;


                porcentagemRestante = ExtraFunctions.valueToPercent(energy, 100);

                HUD.getInstance(this).setPlayerBarLevel(porcentagemRestante);
                mMainCharacter.hurt();
            }

        }

        public void incrementProgress()
        {
            numberEnemies = mManager.getTotalEnemies();
            if (progress < numberEnemies)
                progress += 1;

            porcentagemRestante = ExtraFunctions.valueToPercent(progress, numberEnemies);

            HUD.getInstance(this).setBarLevel(porcentagemRestante);
            pulse = (int) (porcentagemRestante * 63 / 100.0f);


            if (!mOneThirdDone && oneThird())
            {
                sCURRENT_STAGE_X += cSPACE_TO_WALK;
                mWalkCamera = true;
                mOneThirdDone = true;
                SoundManager.PlaySound(cSOUND_CHECKPOINT1);
            }

            if (!mSecondThirdDone && twoThirds())
            {
                sCURRENT_STAGE_X += cSPACE_TO_WALK;
                mWalkCamera = true;
                mSecondThirdDone = true;
                SoundManager.PlaySound(cSOUND_CHECKPOINT2);
            }
        }

        public bool oneThird() {
            if (ExtraFunctions.valueToPercent(progress, numberEnemies) > 33)
                return true;
            return false;
        }

        public bool twoThirds()
        {
            if (ExtraFunctions.valueToPercent(progress, numberEnemies) > 66)
                return true;
            return false;
        }

        public Cursor getCursor()
        {
            return mCursor;
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

        public MainCharacter getPlayer() {
            return mMainCharacter;
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
                        if (mGameState == GAME_STATE_EM_JOGO)
                        {
                            //SoundManager.PlaySound(cSOUND_COLOR);
                            //mCursor.nextColor();
                        }
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
                        instantInk(mCursor.getColor());

                        //incrementProgress();
                        //damage();
                        /*explodeStageFinish();
                        
                        sCURRENT_STAGE_X += cSPACE_TO_WALK;
                        mWalkCamera = true;*/
                        sCURRENT_STAGE_X += cSPACE_TO_WALK;
                        mWalkCamera = true;
                    }

                }
                if (newState.IsKeyDown(Keys.Escape))
                {
                    if (!oldState.IsKeyDown(Keys.Escape))
                    {
                        togglePauseGame();
                    }
                }

                if (newState.IsKeyDown(Keys.W))
                {
                    if (!oldState.IsKeyDown(Keys.W))
                    {
                        
                    }
                }

                if (newState.IsKeyDown(Keys.S))
                {
                    if (!oldState.IsKeyDown(Keys.S))
                    {
                        
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

                if (mFadeParam == FADE_PARAM.NEXT_STAGE)
                {

                }else
                if (mFadeParam == FADE_PARAM.GAME_OVER)
                {
                    //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_GAMEOVER, false,false);
                }

            }

        }
      
    }
}
