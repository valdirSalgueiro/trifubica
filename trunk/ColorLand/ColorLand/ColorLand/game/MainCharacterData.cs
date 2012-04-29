using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorLand
{
    public class MainCharacterData
    {

        private int mEnergy;
        
        public MainCharacterData(){
            setEnergy(4);
        }

        public void addEnergy()
        {
            mEnergy++;
        }

        public void removeEnergy()
        {
            mEnergy--;
        }

        public void setEnergy(int total)
        {
            mEnergy = total;
        }

        public int getEnergy()
        {
            return mEnergy;
        }

    }
}
