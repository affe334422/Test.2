using System;
using System.Collections.Generic;
using Lite_olika_test;
using Lite_olika_test.Base_Hjälp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameRun1 : _GameRunSetup
{
    private Du DuTillKamera = new Du(100,100);
    private Random ran = new Random();
    private bool SpasificKeyPressed = true;
    static int Capacity = 1;
    static MinRectangle Boundry = new MinRectangle(900,500,3000,3000);
    private Quadtree<BallsInSpace> quadtree = new Quadtree<BallsInSpace>(Boundry,Capacity,true);
    private List<BallsInSpace> BL = new List<BallsInSpace>();
    private Background background = new Background();
    
    public GameRun1(GraphicsDeviceManager grm, SpriteBatch sb,Camera2D c2d,Texture2D t2d) : base(grm,sb,c2d,t2d)
    {
        
    }

    public override void Update()
    {
        mstate = Mouse.GetState();
        kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.C))
        {
            BL.Clear();
        }
        if (kstate.IsKeyDown(Keys.Space)&&SpasificKeyPressed)
        {
            SpasificKeyPressed=false;
            //quadtree.Add(new MinRectangle(mstate.Position.ToVector2(),3,3));
            BL.Add(new BallsInSpace(mstate.Position.ToVector2(),50,50));
            quadtree = new Quadtree<BallsInSpace>(Boundry,Capacity,true);
            foreach(BallsInSpace b in BL)
            {
                quadtree.Add(b);
            }
        }
        //Gravity();
        BL.ForEach(b=>b.Update(new Vector2(900,500)));
        quadtree=new Quadtree<BallsInSpace>(Boundry,Capacity,true);
        BL.ForEach(b=>quadtree.Add(b));
        colisions();

       





        
        DuTillKamera.Update();
        background.Update(DuTillKamera.centrum);
        if (kstate.IsKeyUp(Keys.Space))
        {
            SpasificKeyPressed=true;
        }
        if (kstate.IsKeyDown(Keys.R))
        {
            DuTillKamera.centrum *=0;
            background=new Background();
        }
    }

    public override void Draw()
    {
        _spriteBatch.Begin();
            
            quadtree.DrawBoundry(_spriteBatch,texture);
            quadtree.Draw(_spriteBatch,texture);
            /*foreach(BallsInSpace mr in quadtree.Query(new MinRectangle(mstate.Position.ToVector2(), 100, 100)))
            {
                _spriteBatch.Draw(texture,mr.rec,Color.Blue);
            }*/
            
        _spriteBatch.End();
    }


    
    void colisions()
    {
        foreach(BallsInSpace mr in BL){
            List<BallsInSpace> Affected = quadtree.Query(mr);
            Affected.Remove(mr);
            foreach(BallsInSpace af in Affected){
                if (mr.rec.Intersects(af.rec)){
                    PuchAppart(mr,af);
                    ChangeVelocity(mr,af);
                }
            }
        }
    }

    void ChangeVelocity(BallsInSpace Original, BallsInSpace Other)
    {
        ElasticCollision(ref Original.Vel,ref Other.Vel,Original.rec.Width,Other.rec.Width);
        
        void ElasticCollision(ref Vector2 vA, ref Vector2 vB, float mA, float mB)
        {
            float Vax =
                (vA.X * (mA - mB) + 2f * mB * vB.X) / (mA + mB);

            float Vbx =
                (vB.X * (mB - mA) + 2f * mA * vA.X) / (mA + mB);

            float Vay =
                (vA.Y * (mA - mB) + 2f * mB * vB.Y) / (mA + mB);

            float Vby =
                (vB.Y * (mB - mA) + 2f * mA * vA.Y) / (mA + mB);

            vA = new Vector2(Vax,Vay);
            vB = new Vector2(Vbx,Vby);
        }
    }
    void PuchAppart(BallsInSpace Original, BallsInSpace Other)
    {
        // Räkna ut överlapp
        int overlapX = Math.Min(Original.rec.Right, Other.rec.Right) - Math.Max(Original.rec.Left, Other.rec.Left);
        int overlapY = Math.Min(Original.rec.Bottom, Other.rec.Bottom) - Math.Max(Original.rec.Top, Other.rec.Top);
        int Gräns = 5;
        // Putta isär i minsta riktningen
        if (overlapX < overlapY)
        {
            // putta horisontellt
            if (Original.centrum.X < Other.centrum.X)
            {
                Original.centrum_x -= overlapX / 2f;
                Other.centrum_x += overlapX / 2f;
            }
            else
            {
                Original.centrum_x += overlapX / 2f;
                Other.centrum_x -= overlapX / 2f;
            }
            if (Original.Vel.X < Gräns &&Original.Vel.X>-Gräns)
            {
                Original.Vel.X*=0;
            }
            if (Other.Vel.X < Gräns && Other.Vel.X > -Gräns)
            {
                Other.Vel.X*=0;
            }
        }
        else
        {
            // putta vertikalt
            if (Original.centrum.Y < Other.centrum.Y)
            {
                Original.centrum_y -= overlapY / 2f;
                Other.centrum_y += overlapY / 2f;
            }
            else
            {
                Original.centrum_y += overlapY / 2f;
                Other.centrum_y -= overlapY / 2f;
            }
            if (Original.Vel.Y < Gräns&&Original.Vel.Y>-Gräns)
            {
                Original.Vel.Y*=0;
            }
            if (Other.Vel.Y < Gräns && Other.Vel.Y > -Gräns)
            {
                Other.Vel.Y*=0;
            }
        }
    }
}
