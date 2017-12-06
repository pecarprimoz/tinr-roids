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
        AI npc_testing;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerCharacter character;
        ScreenUI ui_playerHP;
        ScreenUI ui_playerHP_NUMS;
        ScreenUI ui_score_SCORE;
        ScreenUI ui_score_NUMS;
        ScreenUI ui_rocks_REMAIN;
        ScreenUI ui_rocks_NUMOFROCKS;
        int screenWidth = 800;
        int screenHeight = 600;
        int playerHP;
        int numberOfRocks;
        GameLogic gameLogic;
        int numberOfBullets;
        float timeSinceShot;
        float respawnTimer;
        float invulnTimer;
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
            numberOfRocks = 1;
            numberOfBullets = 3;
            playerHP = 3;
            //Čas od kadar smo vstrelili
            timeSinceShot = 0;
            // Init sprites
            npc_testing = new AI(this.GraphicsDevice);
            character = new PlayerCharacter(this.GraphicsDevice);
            my_bullets = new List<Projectile>();
            my_rocks = new List<Planetoids>();
            gameLogic = new GameLogic(numberOfRocks, playerHP,character,my_rocks);
            ui_rocks_NUMOFROCKS = new ScreenUI(Content, 210, 20, "NUM.OF ROCKS");
            ui_rocks_REMAIN = new ScreenUI(Content, 210, 40, gameLogic.getNumRocks().ToString());
            ui_score_SCORE = new ScreenUI(Content,55, 20, "SCORE");
            ui_score_NUMS = new ScreenUI(Content, 55, 40, gameLogic.getScore().ToString());
            ui_playerHP = new ScreenUI(this.GraphicsDevice, screenWidth - 100, 40);
            ui_playerHP_NUMS = new ScreenUI(Content, screenWidth - 60, 40, "x "+playerHP.ToString());

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

            if (gameLogic.getNumRocks() <= 0)
            {
                numberOfRocks = 1;
                invulnTimer = 0.0f;
                gameLogic.SetNumberOfRocks(numberOfRocks);
                for (int i = 0; i < numberOfRocks; i++)
                {
                    my_rocks.Add(new Planetoids(this.GraphicsDevice));
                }
            }
            timeSinceShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 debug_direction = character.getDirection();
            //Console.WriteLine(debug_direction);
            //Trenutno stanje tipkovnice
            KeyboardState state = Keyboard.GetState();
            updateGameParralax(state, gameTime);
            //Preverim kontrole igralca, preverim, ali gre igralec skozi 
            updateGameRocksCollision(gameTime);
            if (gameLogic.getPlayerHP() > 0) { 
                if (!character.getIsAlive())
                {
                    respawnTimer+=(float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    character.checkControls(state, screenWidth, screenHeight, my_rocks);
                    //Posodabljanje metkov, v primeru da metrki ne letijo jih posodabljam z ladjo
                    updateGameBullets(state);
                }
                if (respawnTimer >= 1.0f)
                {
                    invulnTimer = 0.0f;
                    gameLogic.setPlayerHp(-1);
                    ui_playerHP_NUMS.UpdateValues("x " + gameLogic.getPlayerHP());
                    //Console.WriteLine("beep respawn");
                    character.setPosition(new Vector2(screenWidth / 2, screenHeight / 2));
                    respawnTimer = 0;
                    character.setIsAlive(true);
                    foreach(Projectile p in my_bullets)
                    {
                        p.RefreshProjectile();
                    }
                }
            }
            //Posodabljanje parallax backgrounda
            //Posodabljanje kolizij in kamnov
            npc_testing.UpdateAI(screenWidth,screenHeight,character);
            ui_score_NUMS.UpdateValues(gameLogic.getScore().ToString());
            ui_rocks_REMAIN.UpdateValues(gameLogic.getNumRocks().ToString());
            invulnTimer+= (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            npc_testing.Draw(spriteBatch);
            //Izris karakterja
            if (gameLogic.getPlayerHP() > 0) { 
                if (character.getIsAlive()) { 
                    character.Draw(spriteBatch);
                    //Izris metkov
                    foreach(Projectile p in my_bullets)
                    {
                        if (p.getisFlying())
                            p.Draw(spriteBatch);
                    }
                }
            }
            ui_rocks_NUMOFROCKS.DrawFont(spriteBatch);
            ui_rocks_REMAIN.DrawFont(spriteBatch);
            //Izris planetov
            foreach(Planetoids rock in my_rocks)
            {
                rock.Draw(spriteBatch);
            }
            ui_score_SCORE.DrawFont(spriteBatch);
            ui_score_NUMS.DrawFont(spriteBatch);
            ui_playerHP.DrawImage(spriteBatch);
            ui_playerHP_NUMS.DrawFont(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Pomožne funkcije za lepoto update funkcije
        public void updateGameBullets(KeyboardState state)
        {
            foreach(Projectile p in my_bullets) {
                p.UpdateWithShip(character, screenWidth, screenHeight);
                //V primeru da metek leti, in da nismo streljali že 0.1 sekunde, izstrelimo metek
                if (!p.getisFlying())
                {
                    if (timeSinceShot > 0.5)
                    {
                        p.checkControls(state);
                        timeSinceShot = 0;
                    }
                }
            }
        }
        public void updateGameParralax(KeyboardState state, GameTime gameTime)
        {
            //Posodabljam parallax background svoje igre, če je 6 in 7 slika je automove (space debris)
            if (character.getIsAlive()) { 
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
            else
            {
                foreach(Background bg in my_backs.getBackgroundList())
                {
                    Vector2 direction = new Vector2(0, 1);
                    bg.Update(gameTime, direction, GraphicsDevice.Viewport);

                }
            }
        }
        
        public void updateGameRocksCollision(GameTime gameTime)
        {
            //Console.WriteLine(my_rocks[0].getPosition());
            foreach(Planetoids rock in my_rocks.ToArray()) { 
                int rockR = rock.getRockR();
                rock.checkIfGoingTroughScreenEdges(screenWidth, screenHeight);
                CollsionDetection _charCollision = character.getCollision();
                CollsionDetection _rockCollision = rock.getCollision();
                /*
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
                */
                rock.Update(gameTime);
                if (_charCollision.DoRectangleCircleOverlap(_rockCollision, _charCollision,rockR) && invulnTimer>0.2f)
                {
                    playerHP--;
                    character.setIsAlive(false);
                    //when player dies, set respawn in update, handle ui
                }
                foreach(Projectile p in my_bullets)
                {
                    if (p.getisFlying())
                    {
                        if (p.isOutOfBounds(screenWidth,screenHeight))
                        {   //fuck yeah look at my smart bullets 
                            //Console.WriteLine("im out sry bro");
                            p.RefreshProjectile();
                        }
                        CollsionDetection _bulletCollision = p.getCollision();
                        if (_bulletCollision.DoRectangleCircleOverlap(_rockCollision, _bulletCollision,rockR))
                        {
                            p.RefreshProjectile();
                            //Re workaj da se kamni splittajo TESTING
                            rock.setIsHit(true);
                            my_rocks.Remove(rock);
                            //DODAJ KASNEJ CE TE ZADANEJO
                            /*
                             * SEDAJ BOS MOGU NARDIT SLEDECE
                             * KO SE JE KLE ZGODL DA BOS HOTU PONOVNO DODAJAT NA SCENO, BOS TO DODAL V EN QUEUE, 
                             * MAGAR NOV CLASS, POL PO UPDATE PA BOS DODAJAL PONOVNO NA SCENO,
                             * TOREJ KLE ZAZNAS D SE JE ZADEL V KAMEN METEK
                             * ZBRISES TA KAMEN, ZBRISES METEK DELA OK
                             * SEDAJ BOS PA MOGU NA QUEUE DODAT DA SE NAJ SPAWNA NOV KAMEN OZ VEC KAMNOV
                             * ODVISNO KAKO SE BOM ODLOCU, SAM MORS DODAJAT PO UPDATE,
                             * NIKOLI NE POSODABLJAJ TRENUTNE ZANKE!!!!!!!!!
                             * */
                            //my_rocks.Add(new Planetoids(this.GraphicsDevice));
                            //Zmanjsamo kamne
                            gameLogic.SetNumberOfRocks(-1);
                            gameLogic.IncrementScore();
                        }
                    }
                }
                
            }
        }
    }
}