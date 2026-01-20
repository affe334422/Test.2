
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Text.Json;

public class ChatGPT_game : _GameRunSetup
{
    public ChatGPT_game(GraphicsDeviceManager grm, SpriteBatch sb,Camera2D c2d,Texture2D t2d) : base(grm,sb,c2d,t2d)
    {
        ForceMatrix.Generate(particleColors); // 👈 VIKTIG RAD
    }
    Color[] particleColors =
    {
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Purple,
        Color.Yellow,
        /*Color.Orange,
        Color.Cyan,*/
        // lägg till hur många du vill
    };


    List<Particle> particles = new();
    Quadtree quadtree = new Quadtree(new Rectangle(0,0,1800,1000),4);
    public const int WorldWidth = 1800;
    public const int WorldHeight = 1000;

    Random rng = new();

    void SpawnParticle(Vector2 pos)
    {
        particles.Add(
            new Particle(
                pos,
                particleColors[rng.Next(particleColors.Length)]
            )
        );
    }


    public override void Update()
    {
        mstate = Mouse.GetState();
        kstate = Keyboard.GetState();
        if (kstate.IsKeyDown(Keys.Space))
        {
            particles.Clear();
            ForceMatrix.Generate(particleColors); // 👈 VIKTIG RAD
            for (int i = 0; i < 1000; i++)
            {
                SpawnParticle(new Vector2(rng.Next(WorldWidth), rng.Next(WorldHeight)));
            }
        }
        if (kstate.IsKeyDown(Keys.S))
        {
            ForceMatrix.Save("forces.json");
        }
        if (kstate.IsKeyDown(Keys.L))
        {
            ForceMatrix.Load("forces.json");
        }

        quadtree.Clear();

        foreach (var p in particles){
            quadtree.Insert(p);
        }

        foreach (var p in particles)
        {
            Rectangle vision = new Rectangle(
                (int)(p.Position.X - 100),
                (int)(p.Position.Y - 100),
                200, 200);

            List<Particle> nearby = new();

            // NORMAL QUERY
            quadtree.Query(vision, nearby);

            // WRAP-QUERIES
            if (vision.Left < 0)
                quadtree.Query(
                    new Rectangle(vision.Left + WorldWidth, vision.Y, vision.Width, vision.Height),
                    nearby);

            if (vision.Right > WorldWidth)
                quadtree.Query(
                    new Rectangle(vision.Left - WorldWidth, vision.Y, vision.Width, vision.Height),
                    nearby);

            if (vision.Top < 0)
                quadtree.Query(
                    new Rectangle(vision.X, vision.Top + WorldHeight, vision.Width, vision.Height),
                    nearby);

            if (vision.Bottom > WorldHeight)
                quadtree.Query(
                    new Rectangle(vision.X, vision.Top - WorldHeight, vision.Width, vision.Height),
                    nearby);

            foreach (var other in nearby)
            {
                if (other == p) continue;
                p.ApplyForceFrom(other);
            }
        }

        if(kstate.IsKeyDown(Keys.A))
        {
            foreach (var p in particles){
                p.ApplyForceFromMouse(mstate.Position.ToVector2());
            }
        }
        foreach (var p in particles){
            p.Update();
        }
    }
    public override void Draw()
    {
    _spriteBatch.Begin();
        foreach (var p in particles){
            p.Draw(_spriteBatch, texture);
        }



    _spriteBatch.End();
    }
}
public class Particle
{
    const float MinDistance = 10f;          // ungefär partikelstorlek
    const float CollisionRepelStrength = 10f;
    public Vector2 Position;
    public Vector2 Velocity;
    public Color Color;

    const float VisionRadius = 100f;
    const float MaxSpeed = 5f;
    const float Friction = 0.8f;

    public Particle(Vector2 position, Color color)
    {
        Position = position;
        Color = color;
        Velocity = Vector2.Zero;
    }

    public void ApplyForceFromMouse(Vector2 Mousepos)
    {
        if(Color!= Color.Red){return;} // bara röda påverkas av musen

        Vector2 delta = Mousepos - Position;

        // WRAP-AWARE DELTA
       
        float distance = delta.Length();
        if (distance <= 0)
            return;

        Vector2 direction = delta / distance;

        // 1️⃣ COLLISION / ANTI-OVERLAP FORCE

        Velocity += direction * CollisionRepelStrength;
        return; // viktig: inga färgkrafter när de är "i varandra"
        
    }
    public void ApplyForceFrom(Particle other)
    {
        Vector2 delta = other.Position - Position;

        // WRAP-AWARE DELTA
        if (delta.X > ChatGPT_game.WorldWidth / 2) delta.X -= ChatGPT_game.WorldWidth;
        if (delta.X < -ChatGPT_game.WorldWidth / 2) delta.X += ChatGPT_game.WorldWidth;
        if (delta.Y > ChatGPT_game.WorldHeight / 2) delta.Y -= ChatGPT_game.WorldHeight;
        if (delta.Y < -ChatGPT_game.WorldHeight / 2) delta.Y += ChatGPT_game.WorldHeight;

        float distance = delta.Length();
        if (distance <= 0)
            return;

        Vector2 direction = delta / distance;

        // 1️⃣ COLLISION / ANTI-OVERLAP FORCE
        if (distance < MinDistance)
        {
            float push = (MinDistance - distance) / MinDistance;
            Velocity -= direction * push * CollisionRepelStrength;
            return; // viktig: inga färgkrafter när de är "i varandra"
        }

        // 2️⃣ NORMAL COLOR FORCE
        if (distance > VisionRadius)
            return;

        float strength = ForceMatrix.GetForce(Color, other.Color);
        Velocity += direction * strength;
    }
    public void Update()
    {
        Velocity *= Friction;

        if (Velocity.Length() > MaxSpeed)
            Velocity = Vector2.Normalize(Velocity) * MaxSpeed;

        Position += Velocity;

        // WRAP AROUND
        if (Position.X < 0) Position.X += ChatGPT_game.WorldWidth;
        if (Position.X > ChatGPT_game.WorldWidth) Position.X -= ChatGPT_game.WorldWidth;
        if (Position.Y < 0) Position.Y += ChatGPT_game.WorldHeight;
        if (Position.Y > ChatGPT_game.WorldHeight) Position.Y -= ChatGPT_game.WorldHeight;
    }


    public Rectangle Bounds =>
        new Rectangle((int)Position.X, (int)Position.Y, 4, 4);

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        spriteBatch.Draw(pixel, Bounds, Color);
    }
}
public class Quadtree
{
    Rectangle bounds;
    int capacity;
    List<Particle> particles = new();
    bool divided = false;

    Quadtree ne, nw, se, sw;

    public Quadtree(Rectangle bounds, int capacity)
    {
        this.bounds = bounds;
        this.capacity = capacity;
    }

    public void Clear()
    {
        particles.Clear();
        divided = false;
        ne = nw = se = sw = null;
    }

    public bool Insert(Particle p)
    {
        if (!bounds.Contains(p.Position))
            return false;

        if (particles.Count < capacity && !divided)
        {
            particles.Add(p);
            return true;
        }

        if (!divided)
            Subdivide();

        return
            ne.Insert(p) || nw.Insert(p) ||
            se.Insert(p) || sw.Insert(p);
    }

    void Subdivide()
    {
        int x = bounds.X;
        int y = bounds.Y;
        int w = bounds.Width / 2;
        int h = bounds.Height / 2;

        ne = new Quadtree(new Rectangle(x + w, y, w, h), capacity);
        nw = new Quadtree(new Rectangle(x, y, w, h), capacity);
        se = new Quadtree(new Rectangle(x + w, y + h, w, h), capacity);
        sw = new Quadtree(new Rectangle(x, y + h, w, h), capacity);

        divided = true;

        foreach (var p in particles)
            Insert(p);

        particles.Clear();
    }

    public void Query(Rectangle range, List<Particle> found)
    {
        if (!bounds.Intersects(range))
            return;

        if (!divided)
        {
            found.AddRange(particles);
            return;
        }

        ne.Query(range, found);
        nw.Query(range, found);
        se.Query(range, found);
        sw.Query(range, found);
    }
}
public static class ForceMatrix
{
    static Dictionary<(int, int), float> forces = new();
    static Random rng = new();

    static int colorCount;

    const float MinForce = -5f;
    const float MaxForce = 2f;

    public static void Generate(Color[] colors)
    {
        colorCount = colors.Length;
        forces.Clear();

        for (int a = 0; a < colorCount; a++)
        {
            for (int b = 0; b < colorCount; b++)
            {
                forces[(a, b)] = RandomRange(MinForce, MaxForce);
            }
        }
    }

    public static float GetForce(Color a, Color b)
    {
        int ia = ColorIndex(a);
        int ib = ColorIndex(b);

        if (forces.TryGetValue((ia, ib), out float f))
            return f;

        return 0f;
    }

    static int ColorIndex(Color c)
    {
        if (c == Color.Red) return 0;
        if (c == Color.Green) return 1;
        if (c == Color.Blue) return 2;
        if (c == Color.Yellow) return 3;
        if (c == Color.Purple) return 4;
        if (c == Color.Orange) return 5;
        if (c == Color.Cyan) return 6;

        return 0;
    }

    static float RandomRange(float min, float max)
    {
        return (float)(rng.NextDouble() * (max - min) + min);
    }

    // ===============================
    // SAVE / LOAD
    // ===============================

    public static void Save(string file)
    {
        ForcePreset preset = new ForcePreset
        {
            ColorCount = colorCount,
            Forces = new float[colorCount * colorCount]
        };

        int i = 0;
        for (int a = 0; a < colorCount; a++)
            for (int b = 0; b < colorCount; b++)
                preset.Forces[i++] = forces[(a, b)];

        string json = JsonSerializer.Serialize(preset, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(file, json);
    }

    public static void Load(string file)
    {
        if (!File.Exists(file))
            return;

        string json = File.ReadAllText(file);
        ForcePreset preset = JsonSerializer.Deserialize<ForcePreset>(json);

        forces.Clear();
        colorCount = preset.ColorCount;

        int i = 0;
        for (int a = 0; a < colorCount; a++)
            for (int b = 0; b < colorCount; b++)
                forces[(a, b)] = preset.Forces[i++];
    }
}


public class ForcePreset
{
    public int ColorCount;
    public float[] Forces;
}