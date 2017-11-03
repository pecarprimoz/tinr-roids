using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        Planetoids planets_0;
        Planetoids planets_1;
        Planetoids planets_2;
        Planetoids planets_3;
        Planetoids planets_4;
        PlayerControls pc;

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
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            // Init sprites
            character = new PlayerCharacter(this.GraphicsDevice);
            planets_0 = new Planetoids(this.GraphicsDevice,0);
            planets_1 = new Planetoids(this.GraphicsDevice, 1);
            planets_2 = new Planetoids(this.GraphicsDevice, 2);
            planets_3 = new Planetoids(this.GraphicsDevice, 3);
            planets_4 = new Planetoids(this.GraphicsDevice, 4);

            // Init controls

            pc = new PlayerControls(character);


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
            KeyboardState state = Keyboard.GetState();
            pc.checkControls(state);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            planets_0.Update(gameTime);
            planets_1.Update(gameTime);
            planets_2.Update(gameTime);
            planets_3.Update(gameTime);
            planets_4.Update(gameTime);
            
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

            //Izris karakterja
            character.Draw(spriteBatch);
            //Izris planetov
            planets_0.Draw(spriteBatch);
            planets_1.Draw(spriteBatch);
            planets_2.Draw(spriteBatch);
            planets_3.Draw(spriteBatch);
            planets_4.Draw(spriteBatch);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}