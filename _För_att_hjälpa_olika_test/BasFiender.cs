using System;
using Lite_olika_test;
using Microsoft.Xna.Framework;
public class BasFiender : MinRectangle
{
    public Vector2 Vel = new Vector2(0,0);
    protected float Max = 200 ;
    protected float Friction = 0.998f;
    protected float Speed = 1;
    protected float V;
    
    public BasFiender(int x, int y, int width, int height) : base(x, y, width, height)
    {}
    public BasFiender(Vector2 xy,int width,int height) : base(xy, width, height)
    {}
    public virtual void Update(Vector2 Target)
    {
        Move(Target);
    }
    protected virtual void Move(Vector2 Target)
    {
        float[] XY = {0,0};
        XY[0] = Target.X - centrum.X;
        XY[1] = Target.Y - centrum.Y; 
        V = (float)Math.Atan2(XY[1], XY[0]); // räknar ut vinklen som den ska färdas i för att träffa
        Vel.X = Vel.X * Friction;
        Vel.Y = Vel.Y * Friction;
        Vel += new Vector2((float)Math.Cos(V) * Speed,(float)Math.Sin(V) * Speed); // så att den rör sig mot target
                
        double Vink = Math.Atan2(Vel.Y,Vel.X);
        double Hyp = Math.Pow(Math.Pow(Vel.X,2)+Math.Pow(Vel.Y,2),0.5);
        if(Hyp > Max){
            Vel.X = (float)(Max *Math.Cos(Vink));
            Vel.Y = (float)(Max *Math.Sin(Vink));
        }
        //centrum += Vel;
    }   
}
