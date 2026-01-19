
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Quadtree<T> where T : QT_Compatible
{
    protected bool HasSubdivided = false;
    protected Quadtree<T> NorthWest;
    protected Quadtree<T> NorthEast;
    protected Quadtree<T> SouthWest;
    protected Quadtree<T> SouthEast;
    protected int Capacity; // 4
    protected MinRectangle Boundry;
    protected MinRectangle BoundryMinimum = new MinRectangle(0,0,1,1);
    protected List<T> RecList; 
    protected bool Drawb = false;

    public Quadtree(int width, int height, int Capacity)
    {
        Boundry = new MinRectangle(900,500,width,height);
        this.Capacity=Capacity;
    }
    public Quadtree(MinRectangle Boundry, int Capacity)
    {
        this.Boundry=Boundry;
        this.Capacity = Capacity;
        RecList = new List<T>();
    }
    public Quadtree(MinRectangle Boundry, int Capacity, bool start)
    {
        this.Boundry=Boundry;
        this.Capacity = Capacity;
        RecList = new List<T>();
    }
    public virtual List<T> Query(MinRectangle Interaction_range)
    {
        List<T> QL = new List<T>();
        if (!Interaction_range.rec.Intersects(Boundry.rec)){
            return QL;
        }
        if (Interaction_range.rec.Intersects(Boundry.rec)&&!HasSubdivided)
        {
            QL.AddRange(RecList);
            Drawb=true;
            return QL;
        }
        else if (HasSubdivided&&Interaction_range.rec.Intersects(Boundry.rec))
        {
            QL.AddRange(NorthEast.Query(Interaction_range));
            QL.AddRange(NorthWest.Query(Interaction_range));
            QL.AddRange(SouthEast.Query(Interaction_range));
            QL.AddRange(SouthWest.Query(Interaction_range));
            Drawb=true;
            return QL;
        }
        return QL;
    }

    
    
    private bool Contains(T newValue)
    {
        int left = Boundry.rec.Left;
        int right = Boundry.rec.Right;
        int top = Boundry.rec.Top;
        int bottom = Boundry.rec.Bottom;

        int x = (int)newValue.CentrumXY().X;
        int y = (int)newValue.CentrumXY().Y;
        if (x>=left&&x<=right&&y>=top&&y<=bottom)
        {
            return true;
        }
        return false;
    }
    public virtual bool Add(T newValue)
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
            
            foreach(T mr in RecList)
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
    protected virtual void Subdivide()
    {
        NorthEast = new Quadtree<T>(new MinRectangle((int)Boundry.centrum_x-Boundry.rec.Width/4,(int)Boundry.centrum_y-Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        NorthWest = new Quadtree<T>(new MinRectangle((int)Boundry.centrum_x+Boundry.rec.Width/4,(int)Boundry.centrum_y-Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        SouthEast = new Quadtree<T>(new MinRectangle((int)Boundry.centrum_x-Boundry.rec.Width/4,(int)Boundry.centrum_y+Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        SouthWest = new Quadtree<T>(new MinRectangle((int)Boundry.centrum_x+Boundry.rec.Width/4,(int)Boundry.centrum_y+Boundry.rec.Height/4,(int)(Boundry.rec.Width/2+1),(int)(Boundry.rec.Height/2+1)),Capacity);
        //Console.WriteLine(NorthEast.Boundry.centrum+" "+NorthWest.Boundry.centrum+" "+SouthEast.Boundry.centrum+" "+SouthWest.Boundry.centrum);
        HasSubdivided=true;
    }
    public void DrawBoundry(SpriteBatch _spriteBatch, Texture2D texture)
    {
        Color co = new Color((int)(Boundry.centrum_x/3000*255),20,(int)(Boundry.centrum_y/3000*255));
        if(/*Drawb*/RecList.Count>0){
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
    public virtual void Draw(SpriteBatch _spriteBatch, Texture2D texture)
    {
        foreach(T old in RecList)
        {
            _spriteBatch.Draw(texture,old.GetRectangle(),Color.Blue);
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




