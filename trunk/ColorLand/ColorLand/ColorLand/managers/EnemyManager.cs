using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;


namespace ColorLand
{
    public class EnemyManager
    {
        private int mMaxEnemiesPerScreen = 4;
        private const int cTIME_BETWEEN_ENEMIES = 4;
        private List<BaseEnemy> mList;
        private GameObjectsGroup<BaseEnemy> mGroup;
        private int mCurrentIndex = -1;
        private MTimer mTimer;

        public EnemyManager()
        {
            mList = new List<BaseEnemy>();
            mGroup = new GameObjectsGroup<BaseEnemy>();
        }

        public void start()
        {
            mTimer = new MTimer();
            mTimer.start();
        }

        public void addEnemy(BaseEnemy enemy, Vector2 location)
        {
            enemy.setLocation(location);
            mList.Add(enemy);
        }

        public void loadContent(ContentManager content)
        {
            foreach (BaseEnemy e in mList)
            {
                e.loadContent(content);
            }
        }

        private void manageEnemies()
        {

            if (canNextAppear())
            {
                BaseEnemy b = next();
                if (b != null)
                {
                    mGroup.addGameObject(b);
                    startTimer();
                }
            }

        }

        private void startTimer()
        {
            mTimer = new MTimer();
            mTimer.start();
        }

        private BaseEnemy next()
        {

            if (mCurrentIndex < mList.Count)
            {
                return mList.ElementAt(++mCurrentIndex);
            }

            return null;
        }

        private BaseEnemy checkNext()
        {

            if (mCurrentIndex + 1 < mList.Count)
            {
                return mList.ElementAt(mCurrentIndex + 1);
            }

            return null;


        }

        private bool canNextAppear()
        {

            if (mGroup.getSize() < mMaxEnemiesPerScreen)
            {
                if (checkNext() != null && checkNext().isReady())
                {
                    return true;
                }
            }

            return false;
        }

        public void update(GameTime gameTime)
        {

            manageEnemies();

            if (mTimer != null)
            {

                mTimer.update(gameTime);

                if (mTimer.getTimeAndLock(cTIME_BETWEEN_ENEMIES))
                {
                    if (checkNext() != null)
                    {
                        checkNext().setReady(true);
                    }
                    mTimer.stop();
                }

            }

            for (int x = 0; x < mGroup.getSize(); x++ )
            {
                mGroup.getGameObject(x).update(gameTime);
            }

            garbageCollection();

        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < mGroup.getSize(); x++)
            {
                mGroup.getGameObject(x).draw(spriteBatch);
            }
        }

        public void garbageCollection()
        {

            int indexCondenado = -1;
            for (int x = 0; x < mGroup.getSize(); x++)
            {
                if (mGroup.getGameObject(x).isActive() == false)
                {
                    indexCondenado = x;
                    break;
                }
            }

            if (indexCondenado != -1)
            {
                mGroup.remove(indexCondenado);
            }
        }

        public void setMaxEnemiesPerScreen(int maxEnemies)
        {
            mMaxEnemiesPerScreen = maxEnemies;
        }

    }
}
