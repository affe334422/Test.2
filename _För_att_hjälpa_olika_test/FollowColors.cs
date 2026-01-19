
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FollowColors : MinRectangle , QT_Compatible
{
    private Color YourColor;
    private MinRectangle Vision;
    private MinRectangle WorldBoundry = new MinRectangle(900,500,1800,1000);
    public MinRectangle worldboundry{set=>WorldBoundry=value;}
    private int Int_Vis = 100; //radien som den ser
    private Vector2 Vel = new Vector2(0,0);
    private float Friction = 0.9f;
    private float MaxSpeed = 5f;
    public FollowColors(Color YourColor, int x, int y, int width, int height) : base(x, y, width, height)
    {
        this.YourColor=YourColor;
        Vision = new MinRectangle(x,y,width+Int_Vis*2,height+Int_Vis*2);
    }
    public FollowColors(Color YourColor, Vector2 xy,int width,int height) : base(xy, width, height)
    {
        this.YourColor=YourColor;
        Vision = new MinRectangle(xy,width+Int_Vis*2,height+Int_Vis*2);
    }
    public MinRectangle vision{get=>Vision;}

    MinRectangle QT_Compatible.GetMinRectangle()
    {
        return Vision;
    }
    public void Update()
    {
        Move();
    }

    public void Test(FollowColors Target){
        float RepelaraKanske = 1;
        float[] XY = {0,0};
        XY[0] = Target.centrum.X - centrum.X;
        XY[1] = Target.centrum.Y - centrum.Y; 
        float V = (float)Math.Atan2(XY[1], XY[0]); // räknar ut vinklen som den ska färdas i för att träffa
        if(Math.Pow(Math.Pow(XY[0],2)+Math.Pow(XY[1],2),0.5)<90){
            RepelaraKanske = -1f;
        }
        Vel += new Vector2((float)Math.Cos(V) * RepelaraKanske,(float)Math.Sin(V) * RepelaraKanske); // så att den rör sig mot target
        double Vink = Math.Atan2(Vel.Y,Vel.X);
        double Hyp = Math.Pow(Math.Pow(Vel.X,2)+Math.Pow(Vel.Y,2),0.5);
        if(Hyp > MaxSpeed){
            Vel.X = (float)(MaxSpeed *Math.Cos(Vink));
            Vel.Y = (float)(MaxSpeed *Math.Sin(Vink));
        }
    }
    public void UpdateVelocity(List<FollowColors> PotetialTargets){
        foreach(FollowColors fl in PotetialTargets)
        {
            if (fl == this){continue;}
                
            if (!Vision.rec.Contains(fl.rec)){continue;} // only consider targets inside vision

            Test(fl);
        }
    }
    private void Move()
    {
        Vel*=Friction;
        centrum += Vel;
        OutOfWorld();
        Vision.centrum=centrum;
    }
    private void OutOfWorld()
    {
        if (centrum_x > WorldBoundry.rec.Right)
        {
            centrum_x-=WorldBoundry.rec.Width;
        }
        else if (centrum_x < WorldBoundry.rec.Left)
        {
            centrum_x+=WorldBoundry.rec.Width;
        }
        if (centrum_y > WorldBoundry.rec.Bottom)
        {
            centrum_y-=WorldBoundry.rec.Height;
        }
        else if (centrum_y < WorldBoundry.rec.Top)
        {
            centrum_y+=WorldBoundry.rec.Height;
        }
    }



    public void Draw(SpriteBatch _spriteBatch,Texture2D texture)
    {
        //_spriteBatch.Draw(texture,Vision.rec,Color.Blue);
        _spriteBatch.Draw(texture,rec,YourColor);
    }
}
