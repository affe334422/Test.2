

using System;
using System.Collections.Generic;
using Lite_olika_test;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Quadtree
{
    private bool HasSubdivided = false;
    Quadtree NorthWest;
    Quadtree NorthEast;
    Quadtree SouthWest;
    Quadtree SouthEast;
    private int Capacity;
    MinRectangle Boundry;
    List<MinRectangle> RecArray;
    public Quadtree(MinRectangle Boundry, int Capacity)
    {
        this.Boundry=Boundry;
        this.Capacity = Capacity;
        RecArray = new List<MinRectangle>();
    }

    private bool Contains(MinRectangle newValue)
    {
        int left = Boundry.rec.Left;
        int right = Boundry.rec.Right;
        int top = Boundry.rec.Top;
        int bottom = Boundry.rec.Bottom;

        int x = (int)newValue.centrum_x;
        int y = (int)newValue.centrum_y;
        if (x>=left&&x<=right&&y>=top&&y<=bottom)
        {
            return true;
        }
        return false;
    }
    public bool Add(MinRectangle newValue)
    {
        if (Contains(newValue))
        {
            //Console.WriteLine(RecArray.Count+" "+Capacity);
            if (RecArray.Count < Capacity)
            {
                RecArray.Add(newValue);
                return true;
            }
            else
            {
                if(!HasSubdivided){
                    Subdivide();
                }
                if (NorthEast.Add(newValue))
                {
                    return true;
                }else if (NorthWest.Add(newValue))
                {
                    return true;
                }else if (SouthEast.Add(newValue))
                {
                    return true;
                }else if(SouthWest.Add(newValue)){
                    return true;
                }
                return false;
            }
        }
        return false;
    }
    private void Subdivide()
    {
        NorthEast = new Quadtree(new MinRectangle((int)Boundry.centrum_x-Boundry.rec.Width/4,(int)Boundry.centrum_y-Boundry.rec.Height/4,Boundry.rec.Width/2,Boundry.rec.Height/2),Capacity);
        NorthWest = new Quadtree(new MinRectangle((int)Boundry.centrum_x+Boundry.rec.Width/4,(int)Boundry.centrum_y-Boundry.rec.Height/4,Boundry.rec.Width/2,Boundry.rec.Height/2),Capacity);
        SouthEast = new Quadtree(new MinRectangle((int)Boundry.centrum_x-Boundry.rec.Width/4,(int)Boundry.centrum_y+Boundry.rec.Height/4,Boundry.rec.Width/2,Boundry.rec.Height/2),Capacity);
        SouthWest = new Quadtree(new MinRectangle((int)Boundry.centrum_x+Boundry.rec.Width/4,(int)Boundry.centrum_y+Boundry.rec.Height/4,Boundry.rec.Width/2,Boundry.rec.Height/2),Capacity);
        //Console.WriteLine(NorthEast.Boundry.centrum+" "+NorthWest.Boundry.centrum+" "+SouthEast.Boundry.centrum+" "+SouthWest.Boundry.centrum);
        HasSubdivided=true;
    }

    public void DrawBoundry(SpriteBatch _spriteBatch, Texture2D texture)
    {
        Color co = new Color((int)(Boundry.centrum_x/1800*255),20,(int)(Boundry.centrum_y/1800*255));
        
        _spriteBatch.Draw(texture,new MinRectangle(Boundry.centrum,Boundry.rec.Width,Boundry.rec.Height).rec,co);
        if (HasSubdivided)
        {
            NorthEast.DrawBoundry(_spriteBatch,texture);
            NorthWest.DrawBoundry(_spriteBatch,texture);
            SouthEast.DrawBoundry(_spriteBatch,texture);
            SouthWest.DrawBoundry(_spriteBatch,texture);
        }
    }
    public void Draw(SpriteBatch _spriteBatch, Texture2D texture)
    {
        foreach(MinRectangle old in RecArray)
        {
            _spriteBatch.Draw(texture,old.rec,Color.Blue);
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




