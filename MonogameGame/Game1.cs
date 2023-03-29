using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimplexNoise;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotnetNoise;
using System;

namespace MonogameGame;
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Color[,] _mapData;
    private Random _random;

    private Camera _camera;

    private Player _player;
    private List<Enemy> _enemies;

    private Texture2D _pixel;
    public readonly int _mapWidth = 24000;
    public readonly int _mapHeight = 18000;
    private readonly float _scale = 0.1f;
    private readonly float _threshold = 0.4f;

    private int[,] _mapDataInt;
    private int _iterations = 5;
    private float _fillProbability = 0.45f;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _random = new Random();
        _mapDataInt = new int[_mapWidth, _mapHeight];
        //IsFixedTimeStep = false;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1800;
        _graphics.PreferredBackBufferHeight = 1200;
        _graphics.ApplyChanges();

        _mapData = new Color[_mapWidth, _mapHeight];

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                _mapData[x, y] = Color.Blue; // Fill the map with water
            }
        }

        GenerateIslands();

        base.Initialize();
    }

    protected override void LoadContent()
    {

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _camera = new Camera();
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        


        Texture2D playerTexture = Content.Load<Texture2D>("player");
        _player = new Player(new Vector2(100, 100), 100, playerTexture, _mapWidth, _mapHeight);


        Texture2D shipTexture = Content.Load<Texture2D>("player");
        Texture2D monsterTexture = Content.Load<Texture2D>("player");
        _enemies = new List<Enemy>
        {
            new Enemy(new Vector2(200, 200), 50, shipTexture),
            new Enemy(new Vector2(500, 200), 50, shipTexture),
            new Enemy(new Vector2(300, 300), 30, monsterTexture)
        };
    }


    protected override async void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();



        _player.Update(gameTime);
        _camera.Follow(_player, _mapWidth, _mapHeight);
        // Create a list to store tasks for each enemy
        List<Task> enemyTasks = new List<Task>();

        foreach (Enemy enemy in _enemies)
        {
            enemy.Update(gameTime);
            Task enemyTask = Task.Run(() => enemy.PerformAI(_player));
            enemyTasks.Add(enemyTask);
        }
        await Task.WhenAll(enemyTasks);

        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(transformMatrix: _camera.Transform); //transformMatrix: _camera.Transform
        DrawMap();
        _player.Draw(_spriteBatch);
        

        foreach (Enemy enemy in _enemies)
        {
            enemy.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }


    private void GenerateIslands()
    {
        // Initial random fill
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                if (_random.NextDouble() < _fillProbability)
                {
                    _mapDataInt[x, y] = 1; // Land
                }
                else
                {
                    _mapDataInt[x, y] = 0; // Water
                }
            }
        }

        // Cellular automata iterations
        for (int i = 0; i < _iterations; i++)
        {
            _mapDataInt = RunCellularAutomataStep(_mapDataInt);
        }
    }

    private int[,] RunCellularAutomataStep(int[,] map)
    {
        int[,] newMap = new int[_mapWidth, _mapHeight];

        Parallel.For(0, _mapWidth, x =>
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                int adjacentLandTiles = CountAdjacentLandTiles(x, y, map);

                // Rule 1: If an empty cell has 4 or more filled neighbors, fill it
                // Rule 2: If a filled cell has 3 or fewer filled neighbors, empty it
                if (map[x, y] == 0 && adjacentLandTiles >= 20)
                {
                    newMap[x, y] = 1;
                }
                else if (map[x, y] == 1 && adjacentLandTiles <= 3)
                {
                    newMap[x, y] = 0;
                }
                else
                {
                    newMap[x, y] = map[x, y];
                }
            }
        });

        return newMap;
    }

    private int CountAdjacentLandTiles(int x, int y, int[,] map)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighborX = x + i;
                int neighborY = y + j;

                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (neighborX >= 0 && neighborX < _mapWidth && neighborY >= 0 && neighborY < _mapHeight)
                {
                    count += map[neighborX, neighborY];
                }
            }
        }

        return count;
    }


    private void DrawMap()
    {
        int tileSize = 64;
        int drawDistance = 3000; // Change this value to adjust the draw distance

        int startX = (int)Math.Max(0, (_player.Position.X - drawDistance) / tileSize);
        int startY = (int)Math.Max(0, (_player.Position.Y - drawDistance) / tileSize);
        int endX = (int)Math.Min(_mapWidth, (_player.Position.X + drawDistance) / tileSize);
        int endY = (int)Math.Min(_mapHeight, (_player.Position.Y + drawDistance) / tileSize);

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                Color color = _mapDataInt[x, y] == 1 ? Color.Yellow : Color.Blue;
                _spriteBatch.Draw(_pixel, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), color);
            }
        }
    }



}