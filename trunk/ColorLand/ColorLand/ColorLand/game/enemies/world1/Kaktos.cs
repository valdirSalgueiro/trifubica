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
            if (up.collidesWith(gameobject))
            {
                mCollided = up;
                collideWithAny = true;
            }
            if (middle.collidesWith(gameobject))
            {
                mCollided = middle;
                collideWithAny = true;
            }
            if (down.collidesWith(gameobject))
            {
                mCollided = down;
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

    }

}
