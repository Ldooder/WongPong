using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WongPong {
    public class Particle {

        public Texture2D texture;     //Particle's texture
        public Vector2 position,origin;      //position of Player in the game world
        public Vector2 velocity;      //Particle's velocity
        public int life;              //Particle's life
        public float rotationAngle;  //particle's rotation
        public Color color;         //particle's defualt color
        public int type;            //particles stype

        //particle constructor
        public Particle(Texture2D newTexture, int x, int y, Vector2 newVelocity,Color newColor, int newtype = 0) {
            position = new Vector2(x, y);
            velocity = newVelocity;
            texture = newTexture;
            color = newColor;
            type = newtype;

            //set origin (center)
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            //set rotation angle
            Random rand = new Random();
            rotationAngle = rand.Next(0, 360);
            life = 0;
        }

        //Update method
        public void Update(GameTime gametime) {
            
            //Update position
            position.X += velocity.X;
            position.Y += velocity.Y;

            //rotate particle
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
            rotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;
            life++;
        }

        //Draw method
        public void Draw(SpriteBatch spritebatch, Color c, bool usecolor = false, float newscale = 1f) {
            if(!usecolor)
                spritebatch.Draw(texture, position, null, color, rotationAngle, origin, newscale, SpriteEffects.None, 0f);
            else
                spritebatch.Draw(texture, position, null, c, rotationAngle, origin, newscale, SpriteEffects.None, 0f);
        }

    }
}
