using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lite_olika_test._Base_Hjälp
{
    public class SnowShow
    {
        private List<SnowRectangle> FallingSnow = new();
        public List<SnowRectangle> fallingsnow{get=>FallingSnow;}
        public SnowShow(Texture2D texture)
        {
            for(int i=0;i<100000;i++){
                FallingSnow.Add(new SnowRectangle(texture,2,2));
            }
        }

        public virtual void Update()
        {
            foreach(SnowRectangle sr in FallingSnow)
            {
                sr.Update();
            }
        }
        public void Draw(SpriteBatch _spritebatch)
        {
            _spritebatch.Begin();
            foreach(SnowRectangle sr in FallingSnow)
            {
                sr.Draw(_spritebatch);
            }
            _spritebatch.End();
        }

        private void RemoveSnowRectangle()
        {
            for(int i = 0; i < FallingSnow.Count; i++)
            {
                if(true);
            }
        }
    }
}