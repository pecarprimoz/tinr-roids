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
        private int current_wave;
        private PlayerCharacter player;
        private List<Planetoids> planets;
        public GameLogic(int rcks, int plLife, PlayerCharacter ply, List<Planetoids> plnts)
        {
            current_wave = 1;
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
        public void incrementWave()
        {
            current_wave++;
        }
        public int getWave()
        {
            return current_wave;
        }
    }
}