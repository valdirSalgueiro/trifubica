using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;



namespace ColorLand
{
    public class GamePlayScreen : BaseScreen
    {

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

        private bool mAlertMessageVisible = true;

        private String[] cALERT_MESSAGES  = { "TURNO ", "PRONTO?", "VAI!", "DESCANSO", "OHHHHHHHH" };
        private const int cALERT_MESSAGE_ONDA       = 0;
        private const int cALERT_MESSAGE_PRONTO     = 1;
        private const int cALERT_MESSAGE_VAI        = 2;
        private const int cALERT_MESSAGE_MUITO_BOM  = 3;
        private const int cALERT_MESSAGE_OHHHHH     = 4;

        private int mCurrentAlertMessageIndex;
        
        private int mGameState;

        private const int GAME_STATE_PREPARANDO = 0;
        private const int GAME_STATE_EM_JOGO    = 1;
        private const int GAME_STATE_SUCESSO    = 2;
        private const int GAME_STATE_DERROTA    = 3;

        /*******************
         * GAME
         *******************/
        private Background mBackground;

        private MainCharacter mMainCharacter;

        private EnemySimpleFlying mTestEnemy;

        //private Test mTest;
        //private GameObjectsGroup<Enemy> mGroupEnemies = new GameObjectsGroup<Enemy>();
        // private GameObjectsGroup<Enemy> mGroupEnemiesToCheckCollision = new GameObjectsGroup<Enemy>();
        //private GameObjectsGroup<Fruit> mGroupFruits = new GameObjectsGroup<Fruit>();

        private int mTotalOfActiveEnemies;
        private int mTotalofRemaningFruits;

        /*private Monster mMonster;
        private Projectile[] mProjectiles;
        private Projectile   mCurrentProjectile;
        private Projectile   mProjectileToBeTackled; //projetil la no chao, preparado pra levar lapada
        */
        private int mProjectileIndex;

        //private Enemy mEnemy;

        private Cursor mCursor;

        //WAVES
        private const int cWAVE1_ENEMIES_COUNT = 10;
        private int mCurrentWave = 0;


        public GamePlayScreen()
        {
            
           // mGameState = GAME_STATE_PREPARANDO;
            mGameState = GAME_STATE_SUCESSO;

            mKeyboard = KeyboardManager.getInstance();

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

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());
            mCursor.changeColor(Color.Green);



            /*SoundManager.getInstance().stop();

            SoundManager.getInstance().playMusic(SoundManager.MUSIC_GAME);

            SoundManager.getInstance().stopFX(SoundManager.FX_NARRACAO);
            */
        }

        /*private void manageWaves()
        {
            if (mTotalOfActiveEnemies == 0)
            {
                //vou botar um timer pra melhorar isso
                nextWave();
            }

        }*/


        private void setGameState(int gameState)
        {

            mGameState = gameState;

            switch (gameState)
            {

                case GAME_STATE_PREPARANDO:
                    mAlertMessageVisible = true;
                    mTimerMessages.start();
                    break;
                
                case GAME_STATE_EM_JOGO:
                    mAlertMessageVisible = false;
                    mTimerMessages.stop();
                    //mTimerSucesso.stop();
                    break;

                case GAME_STATE_SUCESSO:
                    mAlertMessageVisible = true;
                    mCurrentAlertMessageIndex = cALERT_MESSAGE_MUITO_BOM;
                    mTimerMessages.start();
                    break;
                
                case GAME_STATE_DERROTA:
                    mAlertMessageVisible = true;
                    mCurrentAlertMessageIndex = cALERT_MESSAGE_OHHHHH;
                    //mTimerDerrota.start();
                    mTimerMessages.start();
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
                    if (!mTimerMessages.isBusyForNumber(0) && mTimerMessages.getTimeInt() == 0)
                    {
                        mTimerMessages.setBusyWithNumber(0);
                        mCurrentAlertMessageIndex = cALERT_MESSAGE_ONDA;
                    }
                    if (!mTimerMessages.isBusyForNumber(3) && mTimerMessages.getTimeInt() == 3)
                    {
                        mTimerMessages.setBusyWithNumber(3);
                        mCurrentAlertMessageIndex = cALERT_MESSAGE_PRONTO;
                    }
                    if (!mTimerMessages.isBusyForNumber(5) && mTimerMessages.getTimeInt() == 5)
                    {
                        mTimerMessages.setBusyWithNumber(5);
                        mCurrentAlertMessageIndex = cALERT_MESSAGE_VAI;
                    }
                    if (!mTimerMessages.isBusyForNumber(6) && mTimerMessages.getTimeInt() == 6)
                    {
                        mTimerMessages.setBusyWithNumber(6);
                        setGameState(GAME_STATE_EM_JOGO);
                        mCurrentAlertMessageIndex = 0;
                    }
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
            if (mCursor.collidesWith(mTestEnemy))
            { 
                //Console.WriteLine("COLIDIU");
            }

           /* if (mCurrentProjectile != null)
            {
                //checa destruicao da pedra apos saida 
                if (mCurrentProjectile.getX() < -30 || mCurrentProjectile.getX() > 640 ||
                    mCurrentProjectile.getY() < -20 || mCurrentProjectile.getY() > 470)
                {
                    mCurrentProjectile.explode();
                }else
                if (mGroupEnemies.checkCollisionWith(mCurrentProjectile))
                {
                    Enemy e = (Enemy)mGroupEnemies.getCollidedObject();

                    if (e.getType() == Enemy.sTYPE_BLUE)
                    {
                        if (mCurrentProjectile.getType() == Projectile.sTYPE_BLUE)
                        {
                            ((Enemy)mGroupEnemies.getCollidedObject()).explode();
                            //TODO ta na beirada
                            //((Enemy)mGroupEnemies.getCollidedObject()).setActive(false);
                        }
                        else
                        {
                            //some action?
                        }

                        mCurrentProjectile.explode();
                    }
                    if (e.getType() == Enemy.sTYPE_RED)
                    {
                        if (mCurrentProjectile.getType() == Projectile.sTYPE_RED)
                        {
                            ((Enemy)mGroupEnemies.getCollidedObject()).explode();
                            //((Enemy)mGroupEnemies.getCollidedObject()).setActive(false);
                        }
                        else
                        {
                            //some action?
                        }

                        mCurrentProjectile.explode();
                    }
                    if (e.getType() == Enemy.sTYPE_GREEN)
                    {
                        if (mCurrentProjectile.getType() == Projectile.sTYPE_GREEN)
                        {
                            ((Enemy)mGroupEnemies.getCollidedObject()).explode();
                           // ((Enemy)mGroupEnemies.getCollidedObject()).setActive(false);
                        }
                        else
                        {
                            //some action?
                        }

                        mCurrentProjectile.explode();
                    }
                    
                }


                mUniversalTEXT = "MGRP: " + mGroupEnemies.getSize() + " Monster: " + mGroupEnemies.getGameObject(0).getCollisionRect();
                mUniversalTEXT2 = "Fruit: " + mGroupFruits.getSize() + " FRUIT: " + mGroupFruits.getGameObject(0).getCollisionRect();

                if (mGroupFruits.checkCollisionWith<Enemy>(mGroupEnemies))
                {
                    
                    Fruit fruit = ((Fruit)mGroupFruits.getCollidedObject());
                    Enemy enemy = ((Enemy)mGroupFruits.getCollidedPassiveObject());

                    enemy.setLocation(enemy.getX(), enemy.getY());

                    if (!enemy.getAlreadyAte())
                    {
                        enemy.eatFruit();
                        enemy.setX(fruit.getX() + 2);
                        enemy.setY(fruit.getY());
                        mGroupEnemies.deactivateGameObject((Enemy)mGroupEnemies.getCollidedObject());

                        //mGroupEnemies.remove();
                        //Se der pau foi aqui, eu juro
                        //mGroupEnemies.remove((Enemy)mGroupEnemiesToCheckCollision.getCollidedObject());
                        fruit.changeState(Fruit.sSTATE_EATEN);
                        fruit.enableCollision(false);
                    }
                }
                
            }*/
        }

        private int nextProjectileIndex()
        {

            int indiceAtual = mProjectileIndex;

            Random r = new Random();
            
            int count = 0;

            do{
            
                if(mCurrentWave == 1) mProjectileIndex = r.Next(0, 2);
                else
                if(mCurrentWave == 2) mProjectileIndex = r.Next(0, 4);
                //else
                //mProjectileIndex = r.Next(mProjectiles.Length);

                count++;

                if(count > 5){
                    break;
                }

            } while( indiceAtual == mProjectileIndex);
            

            //blue01 red23 green45
            if (mProjectileIndex == 0 || mProjectileIndex == 1)
            {
                mCursor.changeColor(Color.Blue);
            }

            if (mProjectileIndex == 2 || mProjectileIndex == 3)
            {
                mCursor.changeColor(Color.Red);
            }

            if (mProjectileIndex == 4 || mProjectileIndex == 5)
            {
                mCursor.changeColor(Color.Green);
            }

            /*mProjectileIndex++;

            if (mProjectileIndex > mProjectiles.Length -1)
            {
                mProjectileIndex = 0;
            }
            */
            //randomize color TODO
            return mProjectileIndex;
        }

        private void checkGameOverCondition()
        {
            //FRUITS
            /*mTotalofRemaningFruits = 0;
            for (int x = 0; x < mGroupFruits.getSize(); x++)
            {
                if (mGroupFruits.getGameObject(x).getState() == Fruit.sSTATE_STOPPED)
                {
                    mTotalofRemaningFruits++;
                }
            }

            if (mTotalofRemaningFruits == 0 && mTotalOfActiveEnemies == 0 && mGameState == GAME_STATE_EM_JOGO)
            {
                setGameState(GAME_STATE_DERROTA);
            }
            */
        }

        private void checkVictoryCondition()
        {
            //mUniversalTEXT = "MEU CACETE VOADOR VICTORY CONDIGITION ACIVATED: " + mTotalOfActiveEnemies + " / " + mTotalofRemaningFruits;
            //FRUITS
           /* if (mTotalOfActiveEnemies == 0 && mTotalofRemaningFruits > 0)
            {
                //mUniversalTEXT = ;
                setGameState(GAME_STATE_SUCESSO);
                //setGameState(GAME_STATE_PREPARANDO);
            }
            */
        }

        public override void update(GameTime gameTime)
        {
            mBackground.update();

            mMainCharacter.update(gameTime);
            mTestEnemy.update(gameTime);

            updateTimers();

            
            if (mGameState == GAME_STATE_EM_JOGO)
            {
            //    mGroupEnemies.update(gameTime);
            }

            mTimerMessages.update(gameTime);

            mTotalOfActiveEnemies = 0;
           

            if (mGameState == GAME_STATE_EM_JOGO)
            {
                checkVictoryCondition();
                checkGameOverCondition();
            }

           
            //mEnemy.update(gameTime);
            checkCollisions();

            mCursor.update(gameTime);
            MouseState mouseState = Mouse.GetState();

            
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

            mSpriteBatch.Begin();
            mBackground.draw(mSpriteBatch);

            mMainCharacter.draw(mSpriteBatch);
            mTestEnemy.draw(mSpriteBatch);

            mCursor.draw(mSpriteBatch);

            int total = 0;

            /*for (int x = 0; x < mGroupEnemies.getSize(); x++)
            {
                if (!mGroupEnemies.getGameObject(x).isActive())
                {
                    total++;
                }
            }*/


            //mSpriteBatch.DrawString(mFontDebug, ""+mUniversalTEXT, new Vector2(10, 100), Color.Red);
            //mSpriteBatch.DrawString(mFontDebug, "" + mUniversalTEXT2, new Vector2(10, 140), Color.Red);
            if (mAlertMessageVisible)
            {
                String text = cALERT_MESSAGES[mCurrentAlertMessageIndex];
                if (mCurrentAlertMessageIndex == cALERT_MESSAGE_ONDA)
                {
                    text += " " + mCurrentWave;
                }
                //mSpriteBatch.DrawString(mFontAlert, text, new Vector2(120, 140), Color.White);
            }
            mSpriteBatch.End();
            //System.Diagnostics.Debug.WriteLine("loca");

        }

        public override void handleInput(InputState input)
        {
            base.handleInput(input);

            if (!Game1.sKINECT_BASED)
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    //mBullet.goToXY(new Vector2(300, 300));
                    
                }
            }
            else
            {
             
            }
        }

       
    }
}
