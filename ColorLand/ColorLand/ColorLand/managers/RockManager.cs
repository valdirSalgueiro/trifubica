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
        public ExplosionManager em;

        public static RockManager getInstance()
        {
            if (instance == null)
                instance = new RockManager();
            return instance;
        }

        public void restart()
        {
            instance = null;
        }

        public void createObject(Vector2 position, Vector2 vel, Color rockType, Rock.ITEM item)
        {
            Rock rock = new Rock(vel, rockType, item);
            rock.loadContent(Game1.getInstance().getScreenManager().getContent());
            rock.pos = position;
            rock.type = rockType;
            addObject(rock);
        }

        public void createObject(Vector2 position)
        {
            createObject(position,new Vector2(0,0), Color.White, Rock.ITEM.NONE);
        }

        public void createObject(Vector2 position, Vector2 vel, Color rockType)
        {
            createObject(position, vel, rockType, Rock.ITEM.NONE);
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
            for (int i = 0; i < objects.Count;i++ )
            {
                Rock rock = objects[i];
                BaseScreen currentScreen = Game1.getInstance().getScreenManager().getCurrentScreen();

                if (rock.collisionRect.Intersects(((GamePlayScreen)currentScreen).getPlayer().getCollisionRect()))
                {
                    if (rock.item == Rock.ITEM.NONE)
                    {
                        ((GamePlayScreen)currentScreen).damage();
                    }
                    else
                    {
                        ((GamePlayScreen)currentScreen).heal();
                    }
                    rock.notifyCollision();
                }

                Cursor cursor=((GamePlayScreen)currentScreen).getCursor();
                if (!cursor.isInnofensive() && rock.item == Rock.ITEM.NONE && rock.type==cursor.getColor() && rock.collisionRect.Intersects(cursor.getCollisionRect()))
                {
                    em.getNextOfColor(rock.type).explode(rock.pos);
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
