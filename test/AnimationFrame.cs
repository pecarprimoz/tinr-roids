using System;
using Microsoft.Xna.Framework;

namespace test
{
    public class AnimationFrame
    {
        //definicija kje se bo nahajal naš image
        public Rectangle SourceRectangle
        {
            get;
            set;
        }
        //definicija ki določa koliko časa bo določen frame na ekranu
        public TimeSpan Duration
        {
            get;
            set;
        }

    }
}
