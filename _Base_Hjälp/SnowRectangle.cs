using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lite_olika_test._Base_Hjälp
{
    public class SnowRectangle : MinRectangle
    {
        private Vector2 Velosity = new();
        private Random ran = new Random();
        private float försin = 0;
        private int någotnågot;
        private Texture2D texture;
        public SnowRectangle(Texture2D texture, int width, int height) : base(0,0,width,height)
        {
            centrum = new Vector2(ran.Next(0,1801),ran.Next(0,1001));
            this.texture=texture;
            någotnågot = ran.Next(0,11);
            Velosity = new Vector2(ran.Next(1,5),ran.Next(1,10));
        }
        public virtual void Update()
        {
            försin+=0.05f;
            centrum_x+=(float)Math.Sin(försin)*någotnågot;
            centrum+=Velosity;

            if (centrum.Y > 1100)
            {
                centrum_y=0;
                försin=0;
                Velosity=new Vector2(ran.Next(1,5),ran.Next(1,10));
            }
            if (centrum.X > 2000)
            {
                centrum_x=0;
            }
        }
        public void Draw(SpriteBatch _spritebatch)
        {
            _spritebatch.Draw(texture,rec,Color.White);
        }

    }
}