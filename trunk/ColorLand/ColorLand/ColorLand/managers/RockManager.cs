using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ColorLand.game;

namespace ColorLand.managers
{
    class RockManager
    {
        public static RockManager instance;
        private static List<Rock> objects = new List<Rock>();

        public static RockManager getInstance()
        {
            if (instance == null)
                instance = new RockManager();
            return instance;
        }

        public void createObject(Vector2 position)
        {
            Rock rock = new Rock();
            rock.loadContent(Game1.getInstance().getScreenManager().getContent());
            rock.pos = position;
            addObject(rock);
        }

        public void addObject(Rock enemy)
        {
            objects.Add(enemy);
        }

        public void removeObject(Rock enemy)
        {
            objects.Remove(enemy);
        }

        public void cleanUp()
        {
            objects.Clear();
        }

        public void update(GameTime time)
        {
            foreach (Rock rock in objects)
            {
                BaseScreen currentScreen = Game1.getInstance().getScreenManager().getCurrentScreen();

                if (rock.collisionRect.Intersects(((GamePlayScreen)currentScreen).getPlayer().getCollisionRect()))
                {
                    removeObject(rock);
                    continue;
                }

                if (!rock.update(time))
                {
                    removeObject(rock);
                }
            }
        }

        public void draw(SpriteBatch batch)
        {
            foreach (Rock rock in objects)
            {
                rock.draw(batch);
            }
        }
    }
}
