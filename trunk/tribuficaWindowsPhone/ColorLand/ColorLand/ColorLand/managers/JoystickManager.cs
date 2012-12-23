using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ColorLand {
    
    class JoystickManager {

        private PlayerIndex mPlayerIndex;

        private static JoystickManager instance;

        public static JoystickManager getInstance(PlayerIndex playerIndex) {
            if (instance == null) {
                instance = new JoystickManager(playerIndex);
            }
            return instance;
        }

        private GamePadState state;

        private List<Buttons> pressedButtons;

        private JoystickManager(PlayerIndex playerIndex) {
            mPlayerIndex = playerIndex;
            pressedButtons = new List<Buttons>();
            state = GamePad.GetState(playerIndex);
        }

        public void update() {
            state = GamePad.GetState(mPlayerIndex);
            //state.GetPressedKeys();

            for (int i = 0; i < pressedButtons.Count; i++) {
                if (!state.IsButtonDown(pressedButtons.ElementAt(i))){ //IsKeyDown(pressedKeys.ElementAt(i))) {
                    pressedButtons.RemoveAt(i);
                    i--;
                }
            }
        }

        public bool isPressing(Buttons button) {
            return state.IsButtonDown(button);
        }

        public bool pressed(Buttons button) {
            if (state.IsButtonDown(button) && !pressedButtons.Contains(button)) {
                pressedButtons.Add(button);
                return true;
            }
            return false;
        }
    }
}
