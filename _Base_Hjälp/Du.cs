using System;
using System.Collections.Generic;
using Lite_olika_test;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Lite_olika_test
{
    public class Du : MinRectangle
    {
        protected KeyboardState kstate;
        protected Vector2 Vel = new Vector2(0,0);
        protected float Friction = 0.9f;
        protected int Max = 13;
        public Du(int width, int height) : base(0, 0, width, height){}
        public virtual void Update()
        {
            Move();
        }
        protected virtual void Move()
        {
            kstate = Keyboard.GetState();
            //side to side
            if (kstate.IsKeyUp(Keys.A) && kstate.IsKeyUp(Keys.D)||kstate.IsKeyDown(Keys.A)&&kstate.IsKeyDown(Keys.D)){ 
                Vel.X *= Friction;
                if (Vel.X < 1 && -1 < Vel.X)
                {
                    Vel.X = 0;
                }
            }
            else if (kstate.IsKeyDown(Keys.A))
            {
                Vel.X -= 1;
                if(Vel.X>0){
                    Vel.X*=Friction;
                }
            }
            else if (kstate.IsKeyDown(Keys.D))
            {
                Vel.X += 1;
                if(Vel.X<0){
                    Vel.X*=Friction;
                }
            }
            //up and down
            if (kstate.IsKeyUp(Keys.W) && kstate.IsKeyUp(Keys.S)||kstate.IsKeyDown(Keys.W) && kstate.IsKeyDown(Keys.S)){ 
                Vel.Y *= Friction;
                if (Vel.Y < 1 && -1 < Vel.Y)
                {
                    Vel.Y = 0;
                }
            }
            else if (kstate.IsKeyDown(Keys.W))
            {
                Vel.Y -= 1;
                if(Vel.Y>0){
                    Vel.Y*=Friction;
                }
            }
            else if (kstate.IsKeyDown(Keys.S))
            {
                Vel.Y += 1;
                if(Vel.Y<0){
                    Vel.Y*=Friction;
                }
            }
            double V = Math.Atan2(Vel.Y,Vel.X);
            double Hyp = Math.Pow(Math.Pow(Vel.X,2)+Math.Pow(Vel.Y,2),0.5);
            if(Hyp > Max){
                Vel.X = (float)(Max *Math.Cos(V));
                Vel.Y = (float)(Max *Math.Sin(V));
            }
            centrum += Vel;
        } 
    }
}