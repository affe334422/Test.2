
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameRun2 : _GameRunSetup
{
    public GameRun2(GraphicsDeviceManager grm, SpriteBatch sb,Camera2D c2d,Texture2D t2d) : base(grm,sb,c2d,t2d)
    {
        
    }

    static MinRectangle Boundry = new MinRectangle(900,500,1800,1000);
    static MinRectangle World = new MinRectangle(900,500,1800,1000);
    static int Capacity = 1;
    Random ran = new Random();
    Color[] Col3 = {Color.Green,Color.Blue,Color.Red};
    Quadtree<FollowColors> quadtree = new Quadtree<FollowColors>(Boundry,Capacity);
    List<FollowColors> Particles = new List<FollowColors>();
    bool Space = true;
    public override void Update()
    {
        mstate = Mouse.GetState();
        kstate = Keyboard.GetState();


        if (kstate.IsKeyDown(Keys.Space) && Space ||kstate.IsKeyDown(Keys.S))
        {
            Space=false;
            Particles.Add(new FollowColors(Col3[ran.Next(0,1)],mstate.Position.ToVector2(),10,10));
        }

        foreach(FollowColors fl in Particles)
        {
            fl.UpdateVelocity(quadtree.Query(fl.vision));            
        }
        Particles.ForEach(fl=>fl.Update());
        quadtree=new Quadtree<FollowColors>(Boundry,Capacity);
        Particles.ForEach(fl=>quadtree.Add(fl));

        if (kstate.IsKeyUp(Keys.S)&&kstate.IsKeyUp(Keys.Space))
        {
            Space=true;
        }
    }


    public override void Draw()
    {
    _spriteBatch.Begin();

        quadtree.DrawBoundry(_spriteBatch,texture);
        //quadtree.Draw(_spriteBatch,texture);
        int a = 0;
        if(Particles.Count>0){
            foreach(FollowColors fl in quadtree.Query(Particles[0]))
            {
                _spriteBatch.Draw(texture,fl.vision.rec,new Color(200,0,10+a));
                a+=10;
            }
        }
        foreach(FollowColors fl in Particles)
        {
            fl.Draw(_spriteBatch,texture);
        }




    _spriteBatch.End();
    }

}
