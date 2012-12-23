using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace ColorLand
{
    [DataContractAttribute]
	public class ProgressObject
	{
        [DataMember]
        public int mCurrentStage;
        public enum PlayerColor
        {
            RED,
            BLUE,
            GREEN
        }

        [DataMember]
        public PlayerColor playerColor;

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
