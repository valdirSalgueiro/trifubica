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
            GREEN
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

        public ProgressObject setCurrentStage(int stage)
        {
            mCurrentStage = stage;
            return this;
        }

        public PlayerColor getColor()
        {
            return playerColor;
        }

        public ProgressObject setColor(PlayerColor color)
        {
            playerColor = color;
            return this;
        }

        public ProgressObject setStageAndColor(int stage, PlayerColor color)
        {
            mCurrentStage = stage;
            playerColor = color;
            return this;
        }

	}
}
