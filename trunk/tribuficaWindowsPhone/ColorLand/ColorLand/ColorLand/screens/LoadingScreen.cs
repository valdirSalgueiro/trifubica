using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;


namespace ColorLand
{
    class LoadingScreen : BaseScreen
    {
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundLoading;

        private Background mCurrentBackground;

        private static Fade mFadeIn;

        private static Fade mCurrentFade;

        private List<Background> mList = new List<Background>();

        private const int cMAX_BG_COUNTER = 1;
        private int mBackgroundCounter;

        private LoadingLogo mLoadingLogo;


        public LoadingScreen()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundLoading = new Background("fades\\blackfade");
            mBackgroundLoading.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundLoading);
            
            mCurrentBackground = mList.ElementAt(0);

            mFadeIn = new Fade(this, "fades\\blackfade");

            mLoadingLogo = new LoadingLogo(Color.Blue); //TODO fuck this color
            mLoadingLogo.loadContent(Game1.getInstance().getScreenManager().getContent());
            mLoadingLogo.setLocation(610, 450);

            executeFade(mFadeIn,Fade.sFADE_IN_EFFECT_GRADATIVE);
            
        }

        

        private void nextBackground()
        {
            //Console.WriteLine("FUNCIONOU!!!!");
        }

        public override void update(GameTime gameTime)
        {

            mCurrentBackground.update();

            mLoadingLogo.update(gameTime);

            mFadeIn.update(gameTime);
           
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

            mLoadingLogo.draw(mSpriteBatch);
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
            }else
            if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
               
            }

        }

        

        //apenas limpar quando for para o menu principal
        private void clearAllBackgrounds()
        {

        }

    }

    public class LoadingLogo : GameObject
    {
        //INDEXES
        public const int sSTATE_NORMAL = 0;
        
        //SPRITES
        private Sprite mSpriteNormal;

        public LoadingLogo(Color color)
        {
            /*color = Color.Blue;
            if (color == Color.Red)
            {
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages2(10, "loading\\loading"), new int[] { 0,1,2,3,4}, 1, 200, 200, false, false);
            }
            else
            if (color == Color.Green)
            {
                mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages2(10, "loading\\loading"), new int[] { 0,1,2,3,4,5,6 }, 1, 107, 107, false, false);
            }
            else
            if (color == Color.Blue)
            {
                
            }*/
            mSpriteNormal = new Sprite(ExtraFunctions.fillArrayWithImages2(117, "loading\\loading"), new int[] { Sprite.sALL_FRAMES_IN_ORDER, 117/**/}, 1, 117, 117, false, false);

            addSprite(mSpriteNormal, sSTATE_NORMAL);
            
            changeToSprite(sSTATE_NORMAL);

        }

        public override void loadContent(ContentManager content)
        {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);//getCurrentSprite().update();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
        }


    }

}
