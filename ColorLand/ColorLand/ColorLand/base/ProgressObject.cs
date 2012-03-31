using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ColorLand
{
    [Serializable]
	public class ProgressObject
	{
        private int mCurrentStage;

        public ProgressObject(int currentStage)
        {
            mCurrentStage = currentStage;
        }

        public int getCurrentStage()
        {
            return this.mCurrentStage;
        }

        public void setCurrentStage(int stage)
        {
            mCurrentStage = stage;
        }

	}
}
