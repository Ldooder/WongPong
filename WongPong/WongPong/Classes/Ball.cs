using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WongPong {
    public class Ball {

        public Texture2D texture; //ball's texture
        private Texture2D particleTexture; //particles' texture
        public Vector2 position,origin; //position of ball in the game world
        public Rectangle boundingBox; //bounding box used for collision
        public Vector2 velocity; //velocity of ball (x/y movement)
        public int directionRight; //if ball is suppose to move right
        public bool isVisible;    //if the ball is visible and to be rendered
        private float rotationAngle; // rotation of the ball
        private Vector2 maxVelocity; //max velocity of the ball (x/y movement)
        public int maxParticleLife; //life of each generated particles
        public float scale;         //object's scale
        public List<Particle> particles;

        //Constructor
        public Ball() {
            Random rand = new Random();  //Seed random number

            //Set important attributes
            maxParticleLife = 20;
            position = new Vector2(Defualt.Default._W / 10, Defualt.Default._H / 2);
            velocity = new Vector2(3.5f, 0);
            maxVelocity = new Vector2(22, 22);
            
            //set other stuff
            isVisible = true;
            particles = new List<Particle>();
            scale = 1.0f;
            rotationAngle = 0f;
            
        }

        //Load Content Method
        public void LoadContent(ContentManager content){

            //set the texture & bounding box
            texture = content.Load<Texture2D>("Artwork/ball");
            particleTexture = content.Load<Texture2D>("Artwork/ball_particle");
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            origin.X = (texture.Width / 2); origin.Y = (texture.Height / 2);
        }

        //update method
        public void Update(GameTime gametime,ContentManager Content, bool pause) {

            //Get out if game is paused
            if (pause) return;

            //Update curent particles
            for (int i = 0; i < particles.Count(); i++) {
                particles[i].Update(gametime);
                if (particles[i].life > maxParticleLife) particles.RemoveAt(i);
            }

            //Dont update if not visible
            if (!isVisible) return;

            //Make particle trail
            Random rand = new Random();
            particles.Add(new Particle(particleTexture,
                (int)position.X + texture.Width / 2 + rand.Next(-4, 4),
                (int)position.Y + texture.Width / 2 + rand.Next(-4, 4),
                new Vector2(rand.Next(-150,150)/100f, rand.Next(-150, 150)/100f),
                Color.Green ));

            //Move the ball && update bounding box, degrade X velocity
            position.X += velocity.X; position.Y += velocity.Y;
            boundingBox.X = (int)position.X; boundingBox.Y = (int)position.Y;

            //set limits on the velocity
            if (velocity.X > maxVelocity.X) velocity.X = maxVelocity.X;
            else if (velocity.X < -1 * maxVelocity.X) velocity.X = -1 * maxVelocity.X;
            if (velocity.Y > maxVelocity.Y) velocity.Y = maxVelocity.Y;
            else if (velocity.Y < -1 * maxVelocity.Y) velocity.Y = -1 * maxVelocity.Y;

            //rotate particle
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
            rotationAngle += elapsed ;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;

        }

        //draw method
        public void Draw(SpriteBatch spritebatch,bool hit) {

            //draw the ball & particles
            Color c = Color.Green;
            if ( Math.Abs(velocity.X) > 10) c = Color.Gold;
            if (Math.Abs(velocity.Y) > 4) c = Color.YellowGreen;
            if (hit) c = Color.White;
            if(isVisible) spritebatch.Draw(texture, position+origin, null, c,rotationAngle,origin, scale, SpriteEffects.None, 0f);
            for (int i = 0; i < particles.Count(); i++) {
                if (particles[i].type != 0) particles[i].Draw(spritebatch, Color.Black);
                else if (Math.Abs(velocity.X) > 10) particles[i].Draw(spritebatch, c, true, 2f);
                if (Math.Abs(velocity.Y) > 4) particles[i].Draw(spritebatch, c, true, 1.3f);
                else if (hit) particles[i].Draw(spritebatch, c, true);
                else particles[i].Draw(spritebatch, Color.Black);
            }
            
        }

        //Paddle hit, make particle effect
        public void PaddleHit(Color newcolor) {

            //Figure out if it was right or left paddle
            int xMax, xMin;
            if (position.X > Defualt.Default._W / 2) { xMax = 0; xMin = -10; }
            else { xMax = 10; xMin = 0; }

            //make directional small explision
            Random rand = new Random();
            for (int i = 0; i < 8; i++) {
                Particle p = new Particle(particleTexture,
                    (int)position.X, (int)position.Y,
                    new Vector2(rand.Next(xMin,xMax) / 10f, rand.Next(-5, 5) / 10f),
                    newcolor,1);
                p.life = -100;
                particles.Add(p);
            }
        }


        //destroy the ball at current location, make particle effect
        public void Kill() {
            
            //Make explision
            Random rand = new Random();
            for (int i = 0; i < 100; i++) {
                Particle p = new Particle(particleTexture,
                    (int)position.X,(int)position.Y,
                    new Vector2(rand.Next(-1000,1000)/100f,rand.Next(-1000,1000)/100f),
                    Color.Orange);
                p.life = - 200;
                particles.Add(p);
            }

            //set attributes
            isVisible = false;
            velocity = new Vector2(0, 0);
            position = new Vector2(Defualt.Default._W / 2, Defualt.Default._H / 2);
            boundingBox.X = (int)position.X; boundingBox.Y = (int)position.Y;
        }

        //reset the ball
        public void Reset() {
            particles.Clear();
            isVisible = true;
            position = new Vector2(Defualt.Default._W / 10, Defualt.Default._H / 2);
            velocity = new Vector2(3.5f, 0);
        }
    }

  

    
}
