
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Camera2D camera2D;
    private Texture2D texture;
    private KeyboardState kstate;
    private MouseState mstate;
    _GameRunSetup _GameRun;
    bool start = true;

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
        if (start)
        {
            start=false;
            _GameRun = new ChatGPT_game(_graphics,_spriteBatch,camera2D,texture);
        }
        _GameRun.Update();

        
        if (kstate.IsKeyDown(Keys.Escape)){
            Exit();
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _GameRun.Draw();

        _spriteBatch.Begin(transformMatrix:camera2D.get_transformation());
            /*foreach(BasFiender r in background.recs)
            {
                _spriteBatch.Draw(texture,r.rec,Color.LightYellow);
            }*/

            












            
            //_spriteBatch.Draw(texture,DuTillKamera.rec,Color.Gray);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
