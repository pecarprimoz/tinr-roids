using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.GameGlobal
{
    class GameMain
    {
        AI npc_testing;
        AIUFO ufo_testing;
        SpriteBatch spriteBatch;
        PlayerCharacter character;
        ScreenUI ui_playerHP;
        ScreenUI ui_playerHP_NUMS;
        ScreenUI ui_score_SCORE;
        ScreenUI ui_score_NUMS;
        ScreenUI ui_rocks_REMAIN;
        ScreenUI ui_rocks_NUMOFROCKS;
        ScreenUI ui_wave_NUMS;
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

        GraphicsDeviceManager graphicsMain;
        ContentManager contentMain;
        GraphicsDevice graphicsDeviceMain;

        public GameMain(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice graphicsDevice)
        {
            graphicsMain = graphics;
            contentMain = content;
            graphicsDeviceMain = graphicsDevice;
            //Nastavimo velikost igralnega zaslona
            graphicsMain.PreferredBackBufferWidth = screenWidth;
            graphicsMain.PreferredBackBufferHeight = screenHeight;
            graphicsMain.ApplyChanges();
            LoadContent();
            //Inicializacija kamnov in metkov
            numberOfRocks = 1;
            numberOfBullets = 3;
            playerHP = 3;
            //Čas od kadar smo vstrelili
            timeSinceShot = 0;
            // Init sprites
            npc_testing = new AI(graphicsDeviceMain, 3);
            //ufo_testing = new AIUFO(this.GraphicsDevice);
            character = new PlayerCharacter(graphicsDeviceMain);
            my_bullets = new List<Projectile>();
            my_rocks = new List<Planetoids>();
            gameLogic = new GameLogic(numberOfRocks, playerHP, character, my_rocks);
            ui_rocks_NUMOFROCKS = new ScreenUI(contentMain, 210, 20, "NUM.OF ROCKS");
            ui_rocks_REMAIN = new ScreenUI(contentMain, 210, 40, gameLogic.getNumRocks().ToString());
            ui_score_SCORE = new ScreenUI(contentMain, 55, 20, "SCORE");
            ui_score_NUMS = new ScreenUI(contentMain, 55, 40, gameLogic.getScore().ToString());
            ui_playerHP = new ScreenUI(graphicsDeviceMain, screenWidth - 100, 40, 3);
            ui_playerHP_NUMS = new ScreenUI(contentMain, screenWidth - 60, 40, "x " + playerHP.ToString());
            ui_wave_NUMS = new ScreenUI(contentMain, screenWidth / 2, 40, "WAVE: " + gameLogic.getWave());
            my_backs = new BackgroundLoader(graphicsDeviceMain);
            for (int i = 0; i < numberOfRocks; i++)
            {
                my_rocks.Add(new Planetoids(graphicsDeviceMain));
            }
            for (int j = 0; j < numberOfBullets; j++)
            {
                my_bullets.Add(new Projectile(graphicsDeviceMain, contentMain));
            }
        }
        public void LoadContent()
        {
            graphicsDeviceMain = graphicsMain.GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDeviceMain);
        }
        public void Update(GameTime gameTime)
        {
            if (gameLogic.getNumRocks() <= 0)
            {
                gameLogic.incrementWave();
                ui_wave_NUMS.UpdateValues("WAVE: " + gameLogic.getWave());
                numberOfRocks = 10;
                invulnTimer = 0.0f;
                gameLogic.SetNumberOfRocks(numberOfRocks);
                for (int i = 0; i < numberOfRocks; i++)
                {
                    my_rocks.Add(new Planetoids(graphicsDeviceMain));
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
            if (gameLogic.getPlayerHP() > 0)
            {
                if (!character.getIsAlive())
                {
                    respawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                    ui_playerHP = new ScreenUI(graphicsDeviceMain, screenWidth - 100, 40, (int)gameLogic.getPlayerHP());
                    //Console.WriteLine("beep respawn");
                    character.setPosition(new Vector2(screenWidth / 2, screenHeight / 2));
                    respawnTimer = 0;
                    character.setIsAlive(true);
                    foreach (Projectile p in my_bullets)
                    {
                        p.RefreshProjectile();
                    }
                }
            }
            //Posodabljanje parallax backgrounda
            //Posodabljanje kolizij in kamnov
            npc_testing.UpdateAI(character, gameTime);
            //ufo_testing.UpdateAI(character, gameTime);
            ui_score_NUMS.UpdateValues(gameLogic.getScore().ToString());
            ui_rocks_REMAIN.UpdateValues(gameLogic.getNumRocks().ToString());
            invulnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void Draw(GameTime gameTime)
        {
            graphicsDeviceMain.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            foreach (Background bg in my_backs.getBackgroundList())
            {
                bg.Draw(spriteBatch);
            }
            npc_testing.Draw(spriteBatch);
            //ufo_testing.Draw(spriteBatch);
            //Izris karakterja
            if (gameLogic.getPlayerHP() > 0)
            {
                if (character.getIsAlive())
                {
                    character.Draw(spriteBatch);
                    //Izris metkov
                    foreach (Projectile p in my_bullets)
                    {
                        if (p.getisFlying())
                            p.Draw(spriteBatch);
                    }
                }
            }
            ui_rocks_NUMOFROCKS.DrawFont(spriteBatch);
            ui_rocks_REMAIN.DrawFont(spriteBatch);
            //Izris planetov
            foreach (Planetoids rock in my_rocks)
            {
                rock.Draw(spriteBatch);
            }
            ui_score_SCORE.DrawFont(spriteBatch);
            ui_score_NUMS.DrawFont(spriteBatch);
            ui_playerHP.DrawImage(spriteBatch);
            ui_playerHP_NUMS.DrawFont(spriteBatch);
            ui_wave_NUMS.DrawFont(spriteBatch);
            spriteBatch.End();
        }
        public void updateGameBullets(KeyboardState state)
        {
            foreach (Projectile p in my_bullets)
            {
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
            if (character.getIsAlive())
            {
                Vector2 direction = my_backs.checkDirection(state, character);
                foreach (Background bg in my_backs.getBackgroundList())
                {
                    if (bg.autoMove)
                    {
                        direction += new Vector2(0, 1);
                    }
                    bg.Update(gameTime, direction, graphicsDeviceMain.Viewport);
                }
            }
            else
            {
                foreach (Background bg in my_backs.getBackgroundList())
                {
                    Vector2 direction = new Vector2(0, 1);
                    bg.Update(gameTime, direction, graphicsDeviceMain.Viewport);

                }
            }
        }
        public void updateGameRocksCollision(GameTime gameTime)
        {
            foreach (Planetoids rock in my_rocks.ToArray())
            {
                int rockR = rock.getRockR();
                rock.checkIfGoingTroughScreenEdges(screenWidth, screenHeight);
                CollsionDetection _charCollision = character.getCollision();
                CollsionDetection _rockCollision = rock.getCollision();
                rock.Update(gameTime);
                if (_charCollision.DoRectangleCircleOverlap(_rockCollision, _charCollision, rockR) && invulnTimer > 0.2f)
                {
                    playerHP--;
                    character.setIsAlive(false);
                    //when player dies, set respawn in update, handle ui
                }
                foreach (Projectile p in my_bullets)
                {
                    if (p.getisFlying())
                    {
                        if (p.isOutOfBounds(screenWidth, screenHeight))
                        {   //fuck yeah look at my smart bullets 
                            //Console.WriteLine("im out sry bro");
                            p.RefreshProjectile();
                        }
                        CollsionDetection _bulletCollision = p.getCollision();
                        if (_bulletCollision.DoRectangleCircleOverlap(_rockCollision, _bulletCollision, rockR))
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
