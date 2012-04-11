using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;

namespace ColorLand
{
    [Serializable]
	public class ProgressObject
	{
        private int mCurrentStage;
        public enum PlayerColor
        {
            RED,
            BLUE,
            YELLOW
        }

        private PlayerColor playerColor;

        public ProgressObject(int currentStage, PlayerColor color)
        {
            mCurrentStage = currentStage;
            playerColor = color;
        }

        public int getCurrentStage()
        {
            return this.mCurrentStage;
        }

        public void setCurrentStage(int stage)
        {
            mCurrentStage = stage;
        }

        PlayerColor getColor()
        {
            return playerColor;
        }

        void setColor(PlayerColor color)
        {
            playerColor = color;
        }

	}
}
