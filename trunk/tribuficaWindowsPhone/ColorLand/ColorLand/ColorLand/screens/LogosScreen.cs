using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
/* ***
 * ESTA CLASSE GERENCIA OS LOGOS INICIAIS QUE DEVEM APARECER ANTES DO MENU PRINCIPAL
 * 
 * */

namespace ColorLand
{
    class LogosScreen : BaseScreen
    {
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundTeamLogo;

        private Background mCurrentBackground;

        private static Fade mFadeIn;

        private static Fade mCurrentFade;

        private List<Background> mList = new List<Background>();

        private const int cMAX_BG_COUNTER = 1;
        private int mBackgroundCounter;

        MTimer mTimer;
        
        

        public LogosScreen()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();


            mBackgroundTeamLogo = new Background("logos\\chihuahuagameslogo");
            mBackgroundTeamLogo.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundTeamLogo);
            
            mCurrentBackground = mList.ElementAt(0);

            mFadeIn = new Fade(this, "fades\\blackfade");
            
            executeFade(mFadeIn,Fade.sFADE_IN_EFFECT_GRADATIVE);

            
        }

        public override void update(GameTime gameTime)
        {
            mCurrentBackground.update();
            mFadeIn.update(gameTime);
            if (mTimer != null)
            {
                mTimer.update(gameTime);
                if (mTimer.getTimeAndLock(2))
                {
                    mTimer.stop();
                    mTimer = null;

                    executeFade(mFadeIn, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                }
            }
           
        }

        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin(SpriteSortMode.Immediate,
        BlendState.AlphaBlend,
        null,
        null,
        null,
        null,
        Game1.globalTransformation);

            mCurrentBackground.draw(mSpriteBatch);

            mFadeIn.draw(mSpriteBatch);
            
            mSpriteBatch.End();
        }

        public override void executeFade(Fade fadeObject, int effect)
        {
            base.executeFade(fadeObject, effect);

            mCurrentFade = fadeObject;
            fadeObject.execute(effect);
        }

        ///////prototipo
        public override void fadeFinished(Fade fadeObject)
        {
            if(fadeObject.getEffect() == Fade.sFADE_IN_EFFECT_GRADATIVE){
                mTimer = new MTimer(true);
            }else    if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
               Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_SPLASHSCREEN, true);
            }

        }



    }
}
