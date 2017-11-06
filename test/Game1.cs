﻿using Microsoft.Xna.Framework;
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
        int screenWidth = 800;
        int screenHeight = 600;
        int numberOfRocks;
        int numberOfBullets;
        float timeSinceShot;
        List<Projectile> my_bullets;
        List<Planetoids> my_rocks;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            timeSinceShot = 0;
            graphics.ApplyChanges();
            numberOfRocks = 5;
            numberOfBullets = 10000;
            
            // Init sprites
            character = new PlayerCharacter(this.GraphicsDevice);
            my_bullets = new List<Projectile>();
            my_rocks = new List<Planetoids>();
            for(int i=0; i<numberOfRocks; i++)
            {
                my_rocks.Add(new Planetoids(this.GraphicsDevice));
            }
            for(int j=0; j<numberOfBullets; j++)
            {
                my_bullets.Add(new Projectile(this.GraphicsDevice));
            }

            // Init controls



            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            timeSinceShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState state = Keyboard.GetState();
            // TODO: Add your update logic here
            character.checkControls(state);
            for(int j=0; j < numberOfBullets; j++)
            {
                my_bullets[j].UpdateWithShip(character, screenWidth, screenHeight);

                if (!my_bullets[j].getisFlying()) { 
                    if (timeSinceShot > 0.05) {
                        Console.WriteLine(j);
                        my_bullets[j].checkControls(state);
                        timeSinceShot = 0;

                    }
                }

            }
            for (int i = 0; i < numberOfRocks; i++)
            {
                my_rocks[i].Update(gameTime);
                CollsionDetection _charCollision = character.getCollision();
                CollsionDetection _rockCollision = my_rocks[i].getCollision();
                if (_charCollision.DoRectangleCircleOverlap(_rockCollision, _charCollision))
                {
                    //my_rocks[i]
                }


            }
            //planets_3.Update(gameTime);
            
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int j = 0; j < numberOfBullets; j++)
            {
                if(my_bullets[j].getisFlying())
                my_bullets[j].Draw(spriteBatch);
            }
            //Izris karakterja
            character.Draw(spriteBatch);
            //Izris planetov
            for (int i = 0; i < numberOfRocks; i++)
            {
                my_rocks[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}