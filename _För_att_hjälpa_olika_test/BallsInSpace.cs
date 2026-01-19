using System;
using Lite_olika_test;
using Microsoft.Xna.Framework;


public class BallsInSpace : MinRectangle, QT_Compatible
{
    public Vector2 Vel = new Vector2(0,0);
    protected float Max = 200 ;
    protected float Friction = 0.998f;
    protected float Speed = 1;
    protected float V;
    
    public BallsInSpace(int x, int y, int width, int height) : base(x, y, width, height)
    {}
    public BallsInSpace(Vector2 xy,int width,int height) : base(xy, width, height)
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
        centrum += Vel;
    } 
    public void CalVelocity(Vector2 Tarss)
    {
        float[] XY = {0,0};
        XY[0] = Tarss.X - centrum.X;
        XY[1] = Tarss.Y - centrum.Y;
        double hyp = Math.Pow(Math.Pow(XY[0],2)+Math.Pow(XY[1],2),0.5);
        double V = Math.Atan2(XY[1], XY[0]);
        double Force=0;
        if (hyp > 100)
        {
            return;
        }
        if(Math.Pow(hyp,2)!=0){
            Force = 4*(10 / Math.Pow(hyp,2));
        }
        if(XY[0]!=0||XY[1]!=0){
            // f = g * (m1*m2)/r^2
            Vel += new Vector2((float)(Math.Cos(V)*Force),(float)(Math.Sin(V)*Force));
            
        } 
    }   
}