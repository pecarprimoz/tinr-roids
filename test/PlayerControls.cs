using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace test
{
    class PlayerControls
    {
        PlayerCharacter playerCharacter;
        public PlayerControls(PlayerCharacter pc)
        {
            playerCharacter = pc;
        }
        public void checkControls(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                playerCharacter.Y -= 0.5f;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                playerCharacter.Y += 0.5f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                playerCharacter.X += 0.5f;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                playerCharacter.X -= 0.5f;
            }

        }
    }
}
