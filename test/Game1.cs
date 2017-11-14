using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerCharacter character;
        int screenWidth = 400;
        int screenHeight = 600;
        int numberOfRocks;
        int numberOfBullets;
        float timeSinceShot;
        List<Projectile> my_bullets;
        List<Planetoids> my_rocks;
        BackgroundLoader my_backs;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {   
            //Nastavimo velikost igralnega zaslona
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
            //Inicializacija kamnov in metkov
            numberOfRocks = 15;
            numberOfBullets = 100;
            //Čas od kadar smo vstrelili
            timeSinceShot = 0;
            // Init sprites
            character = new PlayerCharacter(this.GraphicsDevice);
            my_bullets = new List<Projectile>();
            my_rocks = new List<Planetoids>();
            my_backs = new BackgroundLoader(this.GraphicsDevice);
            for(int i=0; i<numberOfRocks; i++)
            {
                my_rocks.Add(new Planetoids(this.GraphicsDevice));
            }
            for(int j=0; j<numberOfBullets; j++)
            {
                my_bullets.Add(new Projectile(this.GraphicsDevice));
            }

            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            timeSinceShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 debug_direction = character.getDirection();
            //Console.WriteLine(debug_direction);
            //Trenutno stanje tipkovnice
            KeyboardState state = Keyboard.GetState();
            //Preverim kontrole igralca, preverim, ali gre igralec skozi 
            character.checkControls(state, screenWidth, screenHeight);
            
            //Posodabljanje metkov, v primeru da metrki ne letijo jih posodabljam z ladjo
            updateGameBullets(state);
            //Posodabljanje parallax backgrounda
            updateGameParralax(state, gameTime);
            //Posodabljanje kolizij in kamnov
            updateGameRocksCollision(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            
            foreach (Background bg in my_backs.getBackgroundList())
            {
                bg.Draw(spriteBatch);
            }
            //Izris karakterja
            character.Draw(spriteBatch);
            //Izris metkov
            for (int j = 0; j < numberOfBullets; j++)
            {
                if(my_bullets[j].getisFlying())
                    my_bullets[j].Draw(spriteBatch);
            }
            //Izris planetov
            for (int i = 0; i < numberOfRocks; i++)
            {
                my_rocks[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Pomožne funkcije za lepoto update funkcije
        public void updateGameBullets(KeyboardState state)
        {
            for (int j = 0; j < numberOfBullets; j++)
            {
                my_bullets[j].UpdateWithShip(character, screenWidth, screenHeight);
                //V primeru da metek leti, in da nismo streljali že 0.1 sekunde, izstrelimo metek
                if (!my_bullets[j].getisFlying())
                {
                    if (timeSinceShot > 0.1)
                    {
                        // Console.WriteLine(j);
                        my_bullets[j].checkControls(state);
                        timeSinceShot = 0;
                    }
                }
            }
        }
        public void updateGameParralax(KeyboardState state, GameTime gameTime)
        {
            //Posodabljam parallax background svoje igre, če je 6 in 7 slika je automove (space debris)
            Vector2 direction = my_backs.checkDirection(state, character);
            foreach (Background bg in my_backs.getBackgroundList())
            {
                if (bg.autoMove)
                {
                    direction += new Vector2(0, 1);
                }
                bg.Update(gameTime, direction, GraphicsDevice.Viewport);
            }
        }
        
        public void updateGameRocksCollision(GameTime gameTime)
        {
            //Console.WriteLine(my_rocks[0].getPosition());
            for (int i = 0; i < numberOfRocks; i++)
            {
                int rockR = my_rocks[i].getRockR();
                my_rocks[i].checkIfGoingTroughScreenEdges(screenWidth, screenHeight);
                CollsionDetection _charCollision = character.getCollision();
                CollsionDetection _rockCollision = my_rocks[i].getCollision();
                for (int k = 0; k < numberOfRocks; k++)
                {
                    if (my_rocks[i] != my_rocks[k])
                    {
                        CollsionDetection _rockCollsionB = my_rocks[k].getCollision();
                        int _rockRB = my_rocks[k].getRockR();
                        if (_rockCollision.DoCircleCircleOverlap(_rockCollision, _rockCollsionB, rockR, _rockRB))
                        {
                            my_rocks[i].UpdateOnRockCollision(my_rocks[k]);
                            
                        }
                    }
                }
                my_rocks[i].Update(gameTime);

                if (_charCollision.DoRectangleCircleOverlap(_rockCollision, _charCollision,rockR))
                {
                    character.setIsAlive(false);
                    numberOfBullets = 0;
                }
                for (int j = 0; j < numberOfBullets; j++)
                {
                    if (my_bullets[j].getisFlying())
                    {
                        CollsionDetection _bulletCollision = my_bullets[j].getCollision();
                        if (_bulletCollision.DoRectangleCircleOverlap(_rockCollision, _bulletCollision,rockR))
                        {
                            my_rocks[i].setIsHit(true);
                            my_rocks.Remove(my_rocks[i]);
                            numberOfRocks--;

                        }
                    }
                }
                
            }
        }
    }
}