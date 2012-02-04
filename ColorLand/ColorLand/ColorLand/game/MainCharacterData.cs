using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorLand
{
    class MainCharacterData
    {

        private int mEnergy;
        private int mColor;

        //properties
        public int pEnergy
        {
            get
            {
                return mEnergy;
            }
            set
            {
                mEnergy = value;
            }
        }

        public int pColor
        {
            get
            {
                return mColor;
            }
            set
            {
                mColor = value;
            }
        }



    }
}
