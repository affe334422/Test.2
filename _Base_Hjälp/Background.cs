using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lite_olika_test.Base_Hjälp
{
    public class Background
    {
        protected List<MinRectangle> Recs = new List<MinRectangle>();
        public List<MinRectangle> recs{get=>Recs;}
        protected Vector2 Centrum = new Vector2(0,0);
        protected Random ran = new Random();
        public Background()
        {
            for(int i = 0; i < 200; i++)
            {
                Recs.Add(new MinRectangle(ran.Next((int)Centrum.X-900,(int)Centrum.X+900),ran.Next((int)Centrum.Y-900,(int)Centrum.Y+900),2,2));
            }
        }
        public void Update(Vector2 centrum)
        {
            Centrum=centrum;
            foreach(MinRectangle r in Recs)
            {
                if (r.centrum.X < Centrum.X - 1000 || r.centrum.X > Centrum.X + 1000)
                {
                    if(r.centrum.X > Centrum.X + 1000)
                    {
                        r.centrum_x=ran.Next((int)Centrum.X-950,(int)Centrum.X-850);
                    }
                    else
                    {
                        r.centrum_x=ran.Next((int)Centrum.X+850,(int)Centrum.X+950);
                    }
                }
                if (r.centrum.Y < Centrum.Y - 1000 || r.centrum.Y > Centrum.Y + 1000)
                {
                    if(r.centrum.Y > Centrum.Y + 1000)
                    {
                        r.centrum_y=ran.Next((int)Centrum.Y-950,(int)Centrum.Y-850);
                    }
                    else
                    {
                        r.centrum_y=ran.Next((int)Centrum.Y+850,(int)Centrum.Y+950);
                    }
                }    
            }
        }
    }
}