using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WongPong {
    public class HUD {

        public int p1score,p2score;
        private string p1string, p2string;
        public Color textColor1,textColor2;
        public float scale1, scale2;
        public Vector2 position1, position2;
        private Texture2D dummyTexture;
        public List<Rectangle> middle_line;
        private SpriteBatch spriteBatch;
        private SpriteFont spritefont;


        //particle constructor
        public HUD() {

            //set primary attributes
            int rec_width = 8;
            int rec_height = 33;
            position1 = new Vector2(Defualt.Default._W * .25f, Defualt.Default._H * .05f);
            position2 = new Vector2(Defualt.Default._W * .75f, Defualt.Default._H * .05f);

            //Set other attributes
            middle_line = new List<Rectangle>();
            scale1 = 1.0f; scale2 = 1.0f;
            textColor1 = Color.White; textColor2 = Color.White;
            p1score = 0; p1string = "0";
            p2score = 0; p1string = "0";
            for (int i = 0; i < Defualt.Default._H; i += rec_height + rec_height / 3) {
                Rectangle r = new Rectangle(Defualt.Default._W / 2 - rec_width / 2, i, rec_width, rec_height);
                middle_line.Add(r);
            }
        }
        
        //Load Content Method
        public void LoadContent(ContentManager content, GraphicsDevice graphicsdevice) {

            spritefont = content.Load<SpriteFont>("SpriteFonts/MyFont1");
            spriteBatch = new SpriteBatch(graphicsdevice);
            dummyTexture = new Texture2D(graphicsdevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

           
        }

        //Update method
        public void Update(GameTime gametime) {
            p1string = "" + p1score;
            p2string = "" + p2score;
          
        }

        //Draw method
        public void Draw() {
            spriteBatch.Begin();
            for (int i = 0; i < middle_line.Count(); i++) spriteBatch.Draw(dummyTexture, middle_line[i], Color.White);
            spriteBatch.DrawString(spritefont, p1string, position1, textColor1, 0f, new Vector2(0,0), scale1, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spritefont, p2string, position2, textColor2, 0f, new Vector2(0, 0), scale2, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

    }
}
