using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class Kaktos : BaseEnemy
    {

        Kakto up;
        Kakto middle;
        Kakto down;

        private Kakto mCollided;     



        //TODO Construir mecanismo de chamar um delegate method when finish animation


        public Kaktos(Vector2 origin)
            : base(Color.White, origin)
        {
            Random rand=new Random(5);
            switch(rand.Next(3)){
                case 0:
                    up = new Kakto(Color.Blue, origin, Kakto.TYPE.UP);
                    break;
                case 1:
                    up = new Kakto(Color.Red, origin, Kakto.TYPE.UP);
                    break;
                case 2:
                    up = new Kakto(Color.Green, origin, Kakto.TYPE.UP);
                    break;
            }

            switch (rand.Next(3))
            {
                case 0:
                    middle = new Kakto(Color.Blue, origin, Kakto.TYPE.MIDDLE);
                    break;
                case 1:
                    middle = new Kakto(Color.Red, origin, Kakto.TYPE.MIDDLE);
                    break;
                case 2:
                    middle = new Kakto(Color.Green, origin, Kakto.TYPE.MIDDLE);
                    break;
            }

            switch (rand.Next(3))
            {
                case 0:
                    down = new Kakto(Color.Blue, origin, Kakto.TYPE.DOWN);
                    break;
                case 1:
                    down = new Kakto(Color.Red, origin, Kakto.TYPE.DOWN);
                    break;
                case 2:
                    down = new Kakto(Color.Green, origin, Kakto.TYPE.DOWN);
                    break;
            }

            setCollisionRect(0,0,0,0);
        }

        public bool checkCollisionWithMembers(GameObject gameobject)
        {
            bool collideWithAny = false;
            if (down.collidesWith(gameobject))
            {
                mCollided = down;
                collideWithAny = true;
            }
            if (middle.collidesWith(gameobject))
            {
                mCollided = middle;
                collideWithAny = true;
            }
            if (up.collidesWith(gameobject))
            {
                mCollided = up;
                collideWithAny = true;
            }            
            return collideWithAny;
        }

        public Kakto getCollided()
        {
            return mCollided;
        }


        public override void loadContent(ContentManager content)
        {
            up.loadContent(content);
            middle.loadContent(content);
            down.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            int collisions = 0;
            if (!down.isActive())
                collisions++;
            if (!middle.isActive())
                collisions++;

            up.goDown(collisions);
            middle.goDown(collisions);

            up.update(gameTime);
            middle.update(gameTime);
            down.update(gameTime);
            
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            up.draw(spriteBatch);
            middle.draw(spriteBatch);
            down.draw(spriteBatch);
        }

        public bool isEmpty()
        {
            Game1.print("----------> " + (!up.isActive() && !middle.isActive() && !down.isActive()));
            return !up.isActive() && !middle.isActive() && !down.isActive();
        }

    }

}
