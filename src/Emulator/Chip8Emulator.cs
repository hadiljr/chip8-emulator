using System.Linq;
using System.Threading.Tasks;
using Emulator.Input;
using Hardware;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Emulator;

public class Chip8Emulator : Game
{

    private Chip8 _chip8;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Chip8InputHandler _chip8InputHandler;

    private readonly int Scale = 10;

    private Texture2D _pixel;

    private readonly string _romPath;

    public Chip8Emulator(string romPath)
    {
        _graphics = new GraphicsDeviceManager(this);
        _romPath = romPath;
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferHeight = 32 * Scale;
        _graphics.PreferredBackBufferWidth = 64 * Scale;
        _graphics.ApplyChanges();

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.Black]);

        _chip8InputHandler = new Chip8InputHandler();

        var rom = LoadProgram(_romPath);

        _chip8 = new Chip8();
        _chip8.LoadProgram(rom);

        Window.AllowUserResizing = false;
        Window.IsBorderless = true;

        base.Initialize();


        Task.Run(() => _chip8.Start());

    }

    private byte[] LoadProgram(string path)
    {
        return System.IO.File.ReadAllBytes(path);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _chip8InputHandler.HandleInput(_chip8);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        PrintDisplay();
        _spriteBatch.End();

        base.Draw(gameTime);
    }


    /// <summary>
    /// Prints the display to the screen
    /// </summary>
    private void PrintDisplay()
    {
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                if (_chip8.Display[x, y] == 1)
                {
                    _spriteBatch.Draw(_pixel, new Rectangle(x * Scale, y * Scale, Scale, Scale), Color.White);
                }
            }
        }
    }
}
