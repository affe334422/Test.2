
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class WarppedQuadTree<T> : Quadtree<T> where T : QT_Compatible
{
    MinRectangle World;
    public WarppedQuadTree(MinRectangle World,MinRectangle Boundry, int Capacity) : base(Boundry,Capacity)
    {
        this.World=World;
    }


    public List<T> WrappedQuery(MinRectangle Interaction_range)
    {
        List<T> WQR = new List<T>();
        if (IsItOutOfboundry(Interaction_range)!=null)
        {
            WQR.AddRange(Query(IsItOutOfboundry(Interaction_range)));
        }
        WQR.AddRange(Query(Interaction_range));
        return WQR;
    }
    
    protected override void Subdivide()
    {
        NorthEast = new WarppedQuadTree<T>(World,new MinRectangle((int)Boundry.centrum_x-Boundry.rec.Width/4,(int)Boundry.centrum_y-Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        NorthWest = new WarppedQuadTree<T>(World,new MinRectangle((int)Boundry.centrum_x+Boundry.rec.Width/4,(int)Boundry.centrum_y-Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        SouthEast = new WarppedQuadTree<T>(World,new MinRectangle((int)Boundry.centrum_x-Boundry.rec.Width/4,(int)Boundry.centrum_y+Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        SouthWest = new WarppedQuadTree<T>(World,new MinRectangle((int)Boundry.centrum_x+Boundry.rec.Width/4,(int)Boundry.centrum_y+Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        //Console.WriteLine(NorthEast.Boundry.centrum+" "+NorthWest.Boundry.centrum+" "+SouthEast.Boundry.centrum+" "+SouthWest.Boundry.centrum);
        HasSubdivided=true;
    }


    private MinRectangle IsItOutOfboundry(MinRectangle Interaction_range)
    {
        float cx = Interaction_range.centrum_x;
        float cy = Interaction_range.centrum_y;
        int w = Interaction_range.rec.Width;
        int h = Interaction_range.rec.Height;

        // Vertikal wrap
        if(Interaction_range.rec.Top < World.rec.Top||Interaction_range.rec.Bottom > World.rec.Bottom||Interaction_range.rec.Left < World.rec.Left||Interaction_range.rec.Right > World.rec.Right){
            if (Interaction_range.rec.Top < World.rec.Top)
            {
                cy += World.rec.Height;
            }
            else if (Interaction_range.rec.Bottom > World.rec.Bottom)
            {
                cy -= World.rec.Height;
            }

            // Horisontell wrap
            if (Interaction_range.rec.Left < World.rec.Left)
            {
                cx += World.rec.Width;
            }
            else if (Interaction_range.rec.Right > World.rec.Right)
            {
                cx -= World.rec.Width;
            }
            return new MinRectangle(new Vector2(cx, cy), w, h);
        }
        return null;
        
    }

    public override void Draw(SpriteBatch _spriteBatch, Texture2D texture)
    {
        foreach(T old in RecList)
        {
            _spriteBatch.Draw(texture,IsItOutOfboundry(old.GetMinRectangle()).rec,Color.Green);
            _spriteBatch.Draw(texture,old.GetMinRectangle().rec,Color.Green);
        }
        if (HasSubdivided)
        {
            NorthEast.Draw(_spriteBatch,texture);
            NorthWest.Draw(_spriteBatch,texture);
            SouthEast.Draw(_spriteBatch,texture);
            SouthWest.Draw(_spriteBatch,texture);
        }
    }
}
