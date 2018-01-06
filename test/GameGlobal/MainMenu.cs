﻿using Microsoft.Xna.Framework;
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
    class MainMenu
    {
        MouseState _currentMouseState;
        MouseState _previousMouseState;
        int screenWidth = 800;
        int screenHeight = 600;
        Texture2D buttonPlay;
        Texture2D buttonSettings;

        Vector2 buttonPlayPosition;
        Vector2 buttonPlayDims;
        bool isButtonPlayPressed;

        Vector2 buttonSettingsPosition;
        Vector2 buttonSettingsDims;
        bool isButtonSettingsPressed;


        GraphicsDeviceManager graphicsMain;
        ContentManager contentMain;
        GraphicsDevice graphicsDeviceMain;
        SpriteBatch spriteBatch;
        public bool getIsButtonPlayPressed()
        {
            return isButtonPlayPressed;
        }
        public void setIsButtonPlayPressed(bool b)
        {
            isButtonPlayPressed = b;
        }
        public bool getIsButtonSettingsPressed()
        {
            return isButtonSettingsPressed;
        }
        public void setIsButtonSettingsPressed(bool b)
        {
            isButtonSettingsPressed = b;
        }


        public MainMenu(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice graphicsDevice)
        {
            graphicsMain = graphics;
            contentMain = content;
            graphicsDeviceMain = graphicsDevice;
            //Nastavimo velikost igralnega zaslona
            graphicsMain.PreferredBackBufferWidth = screenWidth;
            graphicsMain.PreferredBackBufferHeight = screenHeight;
            graphicsMain.ApplyChanges();
            LoadContent();

            if (buttonPlay == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/Buttons/button_play-game.png"))
                {
                    buttonPlay = Texture2D.FromStream(graphicsDeviceMain, stream);
                    buttonPlayDims = new Vector2(buttonPlay.Width, buttonPlay.Height);
                    buttonPlayPosition = new Vector2(screenWidth / 2, screenHeight / 2);
                   //_width = spaceShipsSheet.Width;
                   // _height = spaceShipsSheet.Height;
                }
            }
            if (buttonSettings == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/Buttons/button_settings.png"))
                {
                    buttonSettings = Texture2D.FromStream(graphicsDeviceMain, stream);
                    buttonSettingsDims = new Vector2(buttonPlay.Width, buttonPlay.Height);
                    buttonSettingsPosition = new Vector2(screenWidth / 2 + 22, screenHeight / 2 + 50);
                    //_width = spaceShipsSheet.Width;
                    // _height = spaceShipsSheet.Height;
                }
            }


        }
        public void LoadContent()
        {
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
            graphicsDeviceMain = graphicsMain.GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDeviceMain);
        }
        public void Draw(GameTime gameTime)
        {

            graphicsDeviceMain.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            Vector2 imageMiddlePoint = buttonPlayDims / 2;
            spriteBatch.Draw(buttonPlay, buttonPlayPosition, null, Color.White, 0.0f, imageMiddlePoint, 1f, SpriteEffects.None, 0f);

            imageMiddlePoint = buttonSettingsDims / 2;
            spriteBatch.Draw(buttonSettings, buttonSettingsPosition, null, Color.White, 0.0f, imageMiddlePoint, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
        public void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            if (_previousMouseState.LeftButton == ButtonState.Released
                 && _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePos = new Vector2(_currentMouseState.X, _currentMouseState.Y);
                isButtonPlayPressed = checkIfButtonClicked(mousePos, buttonPlayPosition, buttonPlayDims);
                isButtonSettingsPressed = checkIfButtonClicked(mousePos, buttonSettingsPosition, buttonSettingsDims);
            }
        }
        public bool checkIfButtonClicked(Vector2 mousePos, Vector2 pos, Vector2 dims)
        {
            return (mousePos.X < pos.X + (dims.X / 2) && mousePos.X > pos.X - (dims.X / 2)
                && mousePos.Y < pos.Y + (dims.Y / 2) && mousePos.Y > pos.Y - (dims.Y / 2));
            
        }
    }
}
