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
using System.Runtime.InteropServices;


namespace ColorLand
{
    public class MemoryGameScreen : BaseScreen
    {
        public Texture2D background;
        public Texture2D erro;
        public Texture2D acerto;
        public Texture2D cartaverso_select;
        public Texture2D cartaverso_selected;
        public Texture2D cartafrentevermelho;
        public Texture2D cartafrenteverde;
        public Texture2D cartafrentelilas;
        public Texture2D cartafrentelaranja;
        public Texture2D cartafrenteazul;
        public Texture2D cartafrenteamarelo;
        public Texture2D[] cartas;

        private bool mousePressed;

        private SpriteBatch mSpriteBatch;

        int card1;
        int card2;

        public enum TYPE
        {
            VERDE,
            LILAS,
            LARANJA,
            AZUL,
            AMARELO,
            VERMELHO
        }

        public enum CARD_STATE
        {
            SELECTED,
            WRONG,
            CORRECT,
            NONE
        }

        public class Card
        {
            public TYPE type;
            public bool up;
            public CARD_STATE state;
            public float revealStateTimer;
        }

        Card[] cards;
        Random random = new Random();
        bool firstCardSelected;
        CARD_STATE newState;

        public MemoryGameScreen()
        {
            cartas = new Texture2D[12];

            background = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\bg_jogomemoria");
            erro = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\erro");
            acerto = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\acerto");
            cartaverso_select = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\cartaverso_select");
            cartafrentevermelho = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\cartafrentevermelho");
            cartaverso_selected = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\cartaverso_selected");
            cartafrenteverde = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\cartafrenteverde");
            cartafrentelilas = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\cartafrentelilas");
            cartafrentelaranja = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\cartafrentelaranja");
            cartafrenteazul = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\cartafrenteazul");
            cartafrenteamarelo = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\cartafrenteamarelo");
            cartas[0] = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\CARTA06");
            cartas[1] = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\CARTA05");
            cartas[2] = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\CARTA04");
            cartas[3] = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\CARTA03");
            cartas[4] = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\CARTA02");
            cartas[5] = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("memory\\CARTA");
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            cards = new Card[12];
            
            
            //
            for (int i = 0; i < cards.Length; i++)
            {                
                cards[i] = new Card();
                cards[i].type = (TYPE)(i%6);
                cards[i].state = CARD_STATE.NONE;
            }
            for (int i = 0; i < cards.Length; i++)
            {
                int rand = random.Next() % cards.Length;
                Card tmp = cards[rand];
                cards[rand] = cards[i];
                cards[i] = tmp;
            }

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());
        }

        public override void update(GameTime gameTime)
        {
            mCursor.update(gameTime);

            processMouse();

            for (int i = 0; i < cards.Length; i++)
            {
                switch (cards[i].state)
                {

                    case CARD_STATE.WRONG:
                        if (cards[i].revealStateTimer < 1000)
                        {
                            cards[i].revealStateTimer += gameTime.ElapsedGameTime.Milliseconds;
                        }
                        else {
                            mousePressed = false;
                            newState = CARD_STATE.NONE;
                            cards[i].up = false;
                            cards[i].state = newState;
                            cards[i].revealStateTimer = 0;
                        }
                        break;
                }
            }



        }

        private void processMouse()
        {
            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed)
            {
                mousePressed = true;
            }
            else
            {
                if (mousePressed)
                {
                    float x = mCursor.mX;
                    float y = mCursor.mY;

                    if (x > 30 && x < 570 && y > 30 && y < 570 && newState!=CARD_STATE.WRONG)
                    {
                        int cardX = (int)((x - 30) / 135);
                        int cardY = (int)((y - 30) / 180);
                        int cardID = cardX + cardY * 4;
                        Card card = cards[cardID];

                        if (card.state != CARD_STATE.CORRECT)
                        {
                            cards[cardID].up = true;

                            if (card.state == CARD_STATE.NONE)
                            {
                                card.state = CARD_STATE.SELECTED;
                                if (firstCardSelected)
                                {
                                    if (cardID != card1)
                                    {
                                        card2 = cardID;
                                        firstCardSelected = false;

                                        if (cards[card1].type == cards[card2].type)
                                        {
                                            newState = CARD_STATE.CORRECT;
                                        }
                                        else
                                        {
                                            newState = CARD_STATE.WRONG;
                                        }
                                        cards[card1].state = newState;
                                        cards[card2].state = newState;
                                    }
                                }
                                else
                                {
                                    card1 = cardID;
                                    firstCardSelected = true;
                                }
                            }
                            else
                            {
                                firstCardSelected = false;
                                card.state = CARD_STATE.NONE;
                            }
                        }
                        mousePressed = false;
                    }
                }
            }
        }


        public override void draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();
            mSpriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);

            for (int i = 0; i < cards.Length; i++)
            {
                if (!cards[i].up)
                {
                    switch (cards[i].state)
                    {
                        case CARD_STATE.NONE:
                            mSpriteBatch.Draw(cartaverso_select, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                        case CARD_STATE.SELECTED:
                            mSpriteBatch.Draw(cartaverso_selected, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                    }
                }
                else
                {
                    switch (cards[i].type)
                    {
                        case TYPE.VERDE:
                            mSpriteBatch.Draw(cartafrenteverde, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                        case TYPE.LILAS:
                            mSpriteBatch.Draw(cartafrentelilas, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                        case TYPE.LARANJA:
                            mSpriteBatch.Draw(cartafrentelaranja, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                        case TYPE.AZUL:
                            mSpriteBatch.Draw(cartafrenteazul, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                        case TYPE.AMARELO:
                            mSpriteBatch.Draw(cartafrenteamarelo, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                        case TYPE.VERMELHO:
                            mSpriteBatch.Draw(cartafrentevermelho, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                    }

                    mSpriteBatch.Draw(cartas[(int)cards[i].type], new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);

                    switch (cards[i].state)
                    {
                        case CARD_STATE.CORRECT:
                            mSpriteBatch.Draw(acerto, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                        case CARD_STATE.WRONG:
                            mSpriteBatch.Draw(erro, new Rectangle(30 + (i % 4) * 135, 30 + (i / 4) * 180, 135, 180), Color.White);
                            break;
                    }
                }
            }
            mCursor.draw(mSpriteBatch);
            mSpriteBatch.End();

        }


    }




}
