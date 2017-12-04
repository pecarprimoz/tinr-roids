using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class GameLogic
    {
        private long currentScore;
        private int numberOfRocks;
        private int playerLives;
        private PlayerCharacter player;
        private List<Planetoids> planets;
        public GameLogic(int rcks, int plLife, PlayerCharacter ply, List<Planetoids> plnts)
        {
            currentScore = 0;
            numberOfRocks = rcks;
            playerLives = plLife;
            player = ply;
            planets = plnts;
        }

        public void IncrementScore()
        {
            this.currentScore += 10;
        }
        public float getPlayerHP()
        {
            return playerLives;
        }
        public void setPlayerHp(int i)
        {
            playerLives += i;
        }
        public void SetNumberOfRocks(int i)
        {
            this.numberOfRocks += i;
        }
        public long getScore()
        {
            return this.currentScore;
        }
        public long getNumRocks()
        {
            return this.numberOfRocks;
        }
    }
}
