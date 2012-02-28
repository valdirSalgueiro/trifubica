using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ColorLand.managers
{
    public static class EnemyManager
    {
        private static List<BaseEnemy> enemies=new List<BaseEnemy>();
        
        public static void addEnemy(BaseEnemy enemy){
            enemies.Add(enemy);
        }

        public static void removeEnemy(BaseEnemy enemy)
        {
            enemies.Add(enemy);
        }

        public static void cleanUp() {
            enemies.Clear();
        }

        public static void update(GameTime time)
        { 
            foreach(BaseEnemy enemy in enemies){
                enemy.update(time);
            }
        }

        public static void draw(SpriteBatch batch)
        {
            foreach (BaseEnemy enemy in enemies)
            {
                enemy.draw(batch);
            }
        }

    }
}
