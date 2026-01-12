

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
    private int Capacity; // 4
    MinRectangle Boundry;
    MinRectangle BoundryMinimum = new MinRectangle(0,0,1,1);
    List<MinRectangle> RecList; 
    int qurytimes = 0;
    bool first = false;
    public Quadtree(MinRectangle Boundry, int Capacity)
    {
        this.Boundry=Boundry;
        this.Capacity = Capacity;
        RecList = new List<MinRectangle>();
    }
    public Quadtree(MinRectangle Boundry, int Capacity, bool start)
    {
        this.Boundry=Boundry;
        this.Capacity = Capacity;
        RecList = new List<MinRectangle>();
        first = start;
    }
    public List<MinRectangle> Query(MinRectangle Interaction_range)
    {
        qurytimes++;
        List<MinRectangle> QL = new List<MinRectangle>();
        if (Interaction_range.rec.Intersects(Boundry.rec))
        {
            QL.AddRange(RecList);
        }
        if (HasSubdivided)
        {
            QL.AddRange(NorthEast.Query(Interaction_range));
            qurytimes+=NorthEast.qurytimes;
            QL.AddRange(NorthWest.Query(Interaction_range));
            qurytimes+=NorthWest.qurytimes;
            QL.AddRange(SouthEast.Query(Interaction_range));
            qurytimes+=SouthEast.qurytimes;
            QL.AddRange(SouthWest.Query(Interaction_range));
            qurytimes+=SouthWest.qurytimes;
        }
        if (first)
        {
            Console.WriteLine(qurytimes);
        }
        return QL;
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
        if (MinnimumBoundryReached())
        {
            RecList.Add(newValue);
            return true;
        }
        if (HasSubdivided)
        {
            //Console.WriteLine("n");
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
        if (!Contains(newValue))
        {
            return false;
        }
        //Console.WriteLine(RecArray.Count+" "+Capacity);
        if (RecList.Count < Capacity)
        {
            RecList.Add(newValue);
            return true;
        }
        else
        {
            if(!HasSubdivided){
                Subdivide();
            }
            
            foreach(MinRectangle mr in RecList)
            {
                if (NorthEast.Add(mr)){}
                else if (NorthWest.Add(mr)){}
                else if (SouthEast.Add(mr)){}
                else if(SouthWest.Add(mr)){}
            }
            RecList.Clear();

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

    private bool MinnimumBoundryReached()
    {
        if (Boundry.rec.Width/4 < BoundryMinimum.rec.Width||Boundry.rec.Height/4<BoundryMinimum.rec.Height)
        {
            return true;
        }
        return false;
    }

    private void Subdivide()
    {
        NorthEast = new Quadtree(new MinRectangle((int)Boundry.centrum_x-Boundry.rec.Width/4,(int)Boundry.centrum_y-Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        NorthWest = new Quadtree(new MinRectangle((int)Boundry.centrum_x+Boundry.rec.Width/4,(int)Boundry.centrum_y-Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        SouthEast = new Quadtree(new MinRectangle((int)Boundry.centrum_x-Boundry.rec.Width/4,(int)Boundry.centrum_y+Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        SouthWest = new Quadtree(new MinRectangle((int)Boundry.centrum_x+Boundry.rec.Width/4,(int)Boundry.centrum_y+Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        //Console.WriteLine(NorthEast.Boundry.centrum+" "+NorthWest.Boundry.centrum+" "+SouthEast.Boundry.centrum+" "+SouthWest.Boundry.centrum);
        HasSubdivided=true;
    }

    public void DrawBoundry(SpriteBatch _spriteBatch, Texture2D texture)
    {
        Color co = new Color((int)(Boundry.centrum_x/3000*255),20,(int)(Boundry.centrum_y/3000*255));
        if(RecList.Count>0){
            _spriteBatch.Draw(texture,new MinRectangle(Boundry.centrum,Boundry.rec.Width,Boundry.rec.Height).rec,co);
        }
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
        foreach(MinRectangle old in RecList)
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




