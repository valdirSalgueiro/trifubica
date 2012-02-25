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



namespace ColorLand
{
    public class GamePlayScreen : BaseScreen
    {

        int energy = 1000;

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


        private Timer mTimerMessages = new Timer();
        //private Timer mTimerSucesso = new Timer();
        //private Timer mTimerDerrota= new Timer();

        private int mGameState;

        private const int GAME_STATE_PREPARANDO = 0;
        private const int GAME_STATE_EM_JOGO    = 1;
        private const int GAME_STATE_SUCESSO    = 2;
        private const int GAME_STATE_DERROTA    = 3;

        /*******************
         * GAME
         *******************/
        public const int sWORLD_1 = 0;
        public const int sWORLD_2 = 1;
        
        private int mCurrentWorld;



        private Background mBackground;

        private MainCharacter mMainCharacter;

        private EnemySimpleFlying mTestEnemy;

        private EnemySimpleWalking mTestEnemy2;
        private EnemySimpleShooting mTestEnemy3;
        private EnemyArc mTestEnemy4;

        private Fade mFadeIn;
        private Fade mFadeOut;

        private Cursor mCursor;

        private Camera mCamera;

        private ColorChoiceBar mColorChoiceBar;
        private int mColorCount;


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

            loadWorld1();

            mKeyboard = KeyboardManager.getInstance();
        }

        private void loadWorld1()
        {

            mCurrentWorld = sWORLD_1;

            mFadeIn  = new Fade(Fade.sFADE_IN_EFFECT_GRADATIVE, "fades\\blackfade");
            mFadeOut = new Fade(Fade.sFADE_OUT_EFFECT_GRADATIVE,"fades\\blackfade");

            //setAndLoadFadeImage("fades\\blackfade", Game1.getInstance().getScreenManager().getContent());

            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            //mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");
            //mFontAlert = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("alerts");

            mBackground = new Background("gameplay\\backgrounds\\bgteste");
            mBackground.loadContent(Game1.getInstance().getScreenManager().getContent());

            mMainCharacter = new MainCharacter();
            mMainCharacter.loadContent(Game1.getInstance().getScreenManager().getContent());
            mMainCharacter.setCenter(Game1.sSCREEN_RESOLUTION_WIDTH / 2, 400);

            mTestEnemy = new EnemySimpleFlying(BaseEnemy.sTYPE_SIMPLE_FLYING_RED);
            mTestEnemy.loadContent(Game1.getInstance().getScreenManager().getContent());
            mTestEnemy.setCenter(100, 100);

            mTestEnemy2 = new EnemySimpleWalking(BaseEnemy.sTYPE_SIMPLE_FLYING_RED);
            mTestEnemy2.loadContent(Game1.getInstance().getScreenManager().getContent());
            mTestEnemy2.setCenter(100, 100);

            mTestEnemy3 = new EnemySimpleShooting(BaseEnemy.sTYPE_SIMPLE_FLYING_RED);
            mTestEnemy3.loadContent(Game1.getInstance().getScreenManager().getContent());
            mTestEnemy3.setCenter(100, 100);

            mTestEnemy4 = new EnemyArc(BaseEnemy.sTYPE_SIMPLE_FLYING_RED);
            mTestEnemy4.loadContent(Game1.getInstance().getScreenManager().getContent());
            mTestEnemy4.setCenter(100, 100);
            
            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());
            mCursor.changeColor(Color.Green);
            mCursor.setCenter(20, 20);

            mColorChoiceBar = new ColorChoiceBar();
            mColorChoiceBar.loadContent(Game1.getInstance().getScreenManager().getContent());
            mColorChoiceBar.setCenter(200, 200);
            //executeFade(mFadeOut);

            HUD.getInstance().loadContent(Game1.getInstance().getScreenManager().getContent());
        }

        private void unloadWorld1()
        {
            unload();
            loadWorld1();
            //executeFade(mFadeIn);
        }


        private void setGameState(int gameState)
        {

            mGameState = gameState;

            switch (gameState)
            {

                case GAME_STATE_PREPARANDO:
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
                unloadWorld1();
            }
        }

       

        private void checkGameOverCondition()
        {
           
        }

        private void checkVictoryCondition()
        {
          
        }

        public override void executeFade(Fade fadeObject)
        {
            base.executeFade(fadeObject);

            fadeObject.activate();
        }


        public override void update(GameTime gameTime)
        {
            if (mCurrentWorld == sWORLD_1)
            {
                mCamera.update();

                mBackground.update();

                mMainCharacter.update(gameTime);
                mTestEnemy.update(gameTime);
                mTestEnemy2.update(gameTime);
                mTestEnemy3.update(gameTime);
                mTestEnemy4.update(gameTime);

                updateTimers();

                mColorChoiceBar.update(gameTime);

                if (mGameState == GAME_STATE_EM_JOGO)
                {
                    //    mGroupEnemies.update(gameTime);
                }

                mTimerMessages.update(gameTime);



                if (mGameState == GAME_STATE_EM_JOGO)
                {
                    checkVictoryCondition();
                    checkGameOverCondition();
                }

                //mEnemy.update(gameTime);
                checkCollisions();

                mCursor.update(gameTime);
                MouseState mouseState = Mouse.GetState();

                mFadeIn.update(gameTime);
                mFadeOut.update(gameTime);

               // if(mFade.isFadeComplete()
                HUD.getInstance().update(gameTime);
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

                //mSpriteBatch.Begin();
                mSpriteBatch.Begin(
                        SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        mCamera.get_transformation(Game1.getInstance().GraphicsDevice));


                mBackground.draw(mSpriteBatch);

                mMainCharacter.draw(mSpriteBatch);

                mTestEnemy.draw(mSpriteBatch);
                mTestEnemy2.draw(mSpriteBatch);
                mTestEnemy3.draw(mSpriteBatch);
                mTestEnemy4.draw(mSpriteBatch);

                mCursor.draw(mSpriteBatch);

                //mSpriteBatch.Draw(getFadeImage(), new Rectangle(0, 0, 600, 500), new Color(0, 0, 0, 0.6f));
                mFadeIn.draw(mSpriteBatch);
                mFadeOut.draw(mSpriteBatch);

                mColorChoiceBar.draw(mSpriteBatch);
                
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
                    damage();
                    manageColorCount();
                    //mBullet.goToXY(new Vector2(300, 300));
                    //mCamera.centerCamTo(100, 100);
                    
                }
            }
            else
            {
             
            }
        }

       
    }
}
