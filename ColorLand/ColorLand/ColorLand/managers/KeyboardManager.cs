using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ColorLand {
    
    class KeyboardManager {

        private static KeyboardManager instance;

        public static KeyboardManager getInstance() {
            if (instance == null) {
                instance = new KeyboardManager();
            }
            return instance;
        }

        private KeyboardState state;

        private List<Keys> pressedKeys;

        private KeyboardManager() {
            pressedKeys = new List<Keys>();
            state = Keyboard.GetState();
        }

        public void update() {
            state = Keyboard.GetState();
            //state.GetPressedKeys();

            for (int i = 0; i < pressedKeys.Count; i++) {
                if (!state.IsKeyDown(pressedKeys.ElementAt(i))) {
                    pressedKeys.RemoveAt(i);
                    i--;
                }
            }
        }

        public Boolean isPressing(Keys key) {
            //Console.WriteLine(state.IsKeyDown(key));
            return state.IsKeyDown(key);
        }

        public Boolean pressed(Keys key) {

            if (state.IsKeyDown(key))
            {
                Game1.print("SO TRUE!");
            }

            if (state.IsKeyDown(key) && !pressedKeys.Contains(key)) {
                pressedKeys.Add(key);
                Game1.print("AFE1!");
                return true;
            }
            //Game1.print("AFE2!");
            return false;
        }
    }
}
