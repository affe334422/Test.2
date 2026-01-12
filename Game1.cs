using System;
using System.Collections.Generic;
using Lek2;
using Lite_olika_test._Base_Hjälp;
using Lite_olika_test.Base_Hjälp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lite_olika_test;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Camera2D camera2D;
    private Texture2D texture;
    private KeyboardState kstate;
    private MouseState mstate;
    private Du DuTillKamera = new Du(100,100);
    private Random ran = new Random();
    private bool SpasificKeyPressed = true;
    static int Capacity = 1;
    static MinRectangle Boundry = new MinRectangle(900,500,3000,3000);
    private Quadtree quadtree = new Quadtree(Boundry,Capacity);
    private List<BasFiender> BL = new List<BasFiender>();
    private Background background = new Background();
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferHeight=1000;
        _graphics.PreferredBackBufferWidth=1800;
    }
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        camera2D = new Camera2D(GraphicsDevice);
        base.Initialize();
    }
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        texture = new Texture2D(GraphicsDevice, 1, 1);
        texture.SetData(new[] {Color.White});
        // TODO: use this.Content to load your game content here
    }
    protected override void Update(GameTime gameTime)
    {
        mstate = Mouse.GetState();
        kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.C))
        {
            BL.Clear();
        }
        if (kstate.IsKeyDown(Keys.Space)&&SpasificKeyPressed)
        {
            //SpasificKeyPressed=false;
            //quadtree.Add(new MinRectangle(mstate.Position.ToVector2(),3,3));
            BL.Add(new BasFiender(mstate.Position.ToVector2(),5,5));
            quadtree = new Quadtree(Boundry,Capacity);
            foreach(BasFiender b in BL)
            {
                quadtree.Add(b);
            }
        }
        BL.ForEach(b=>b.Update(mstate.Position.ToVector2()));
        quadtree=new Quadtree(Boundry,Capacity);
        BL.ForEach(b=>quadtree.Add(b));
        colisions();






        
        DuTillKamera.Update();
        background.Update(DuTillKamera.centrum);
        camera2D.Pos=DuTillKamera.centrum;
        if (kstate.IsKeyUp(Keys.Space))
        {
            SpasificKeyPressed=true;
        }
        if (kstate.IsKeyDown(Keys.R))
        {
            DuTillKamera.centrum *=0;
            background=new Background();
        }
        if (kstate.IsKeyDown(Keys.Escape)){
            Exit();
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();
            
            quadtree.DrawBoundry(_spriteBatch,texture);
            quadtree.Draw(_spriteBatch,texture);
            /*foreach(MinRectangle mr in quadtree.Query(new MinRectangle(mstate.Position.ToVector2(), 100, 100)))
            {
                _spriteBatch.Draw(texture,mr.rec,Color.Blue);
            }*/

        _spriteBatch.End();
        _spriteBatch.Begin(transformMatrix:camera2D.get_transformation());
            /*foreach(MinRectangle r in background.recs)
            {
                _spriteBatch.Draw(texture,r.rec,Color.LightYellow);
            }*/

            












            
            //_spriteBatch.Draw(texture,DuTillKamera.rec,Color.Gray);
        _spriteBatch.End();
        base.Draw(gameTime);
    }



    void colisions()
    {
        foreach(MinRectangle mr in BL){
            Console.WriteLine("1");
            List<MinRectangle> Fiender = quadtree.Query(mr);
            for(int j=0;j<Fiender.Count;j++){
                if (mr.rec.Intersects(Fiender[j].rec)){
                    // Räkna ut överlapp
                    int overlapX = Math.Min(mr.rec.Right, Fiender[j].rec.Right) - Math.Max(mr.rec.Left, Fiender[j].rec.Left);
                    int overlapY = Math.Min(mr.rec.Bottom, Fiender[j].rec.Bottom) - Math.Max(mr.rec.Top, Fiender[j].rec.Top);

                    // Putta isär i minsta riktningen
                    if (overlapX < overlapY)
                    {
                        // putta horisontellt
                        if (mr.centrum.X < Fiender[j].centrum.X)
                        {
                            mr.centrum_x -= overlapX / 2f;
                            Fiender[j].centrum_x += overlapX / 2f;
                        }
                        else
                        {
                            mr.centrum_x += overlapX / 2f;
                            Fiender[j].centrum_x -= overlapX / 2f;
                        }
                    }
                    else
                    {
                        // putta vertikalt
                        if (mr.centrum.Y < Fiender[j].centrum.Y)
                        {
                            mr.centrum_y -= overlapY / 2f;
                            Fiender[j].centrum_y += overlapY / 2f;
                        }
                        else
                        {
                            mr.centrum_y += overlapY / 2f;
                            Fiender[j].centrum_y -= overlapY / 2f;
                        }
                    }
                }
            }
        }
        Console.WriteLine(BL.Count);
    }
}
