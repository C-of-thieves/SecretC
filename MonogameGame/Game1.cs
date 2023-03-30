﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimplexNoise;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotnetNoise;
using System;
using Microsoft.Xna.Framework.Media;

namespace MonogameGame;
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Color[,] _mapData;
    private Random _random;

    private Camera _camera;

    private Player _player;
    private MapGenerator _mapGenerator;
    private List<Enemy> _enemies;

    private Texture2D _pixel;
    public readonly int _mapWidth = 24000;
    public readonly int _mapHeight = 18000;
    private readonly float _scale = 0.1f;
    private readonly float _threshold = 0.4f;

    private int[,] _mapDataInt;
    private int _iterations = 2;
    private float _fillProbability = 0.45f;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _random = new Random();
        _mapDataInt = new int[_mapWidth, _mapHeight];
        _mapGenerator = new MapGenerator(_mapWidth, _mapHeight, _fillProbability, _iterations);
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

        

        base.Initialize();
    }

    protected override void LoadContent()
    {

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _camera = new Camera();
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        


        Texture2D playerTexture = Content.Load<Texture2D>("Default size/Ships/ship (6)");
        float playerStartX = (_mapWidth * 0.5f); // Multiplied by tile size (64)
        float playerStartY = (_mapHeight * 0.5f) ; // Multiplied by tile size (64)

        Art.Load(Content);
        _player = new Player(new Vector2(playerStartX, playerStartY), 100, Art.GetPlayerTexture(), 1800, 1200);


        Texture2D shipTexture = Content.Load<Texture2D>("Default size/Ships/ship (6)");
        Texture2D monsterTexture = Content.Load<Texture2D>("Default size/Ships/ship (6)");
        _enemies = new List<Enemy>
        {
            new Enemy(new Vector2(200, 200), 50, Art.GetEnemyTexture()),
            new Enemy(new Vector2(500, 200), 50, Art.GetEnemyTexture()),
            new Enemy(new Vector2(300, 300), 30, Art.GetEnemyTexture())
        };
        Song song = Content.Load<Song>("Music/The Buccaneer's Haul Royalty Free Pirate Music");  // Put the name of your song here instead of "song_title"
        MediaPlayer.Volume = 0.01f;
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.01f;
        MediaPlayer.IsRepeating = true;
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
                Color color = _mapGenerator.MapDataInt[x,y] == 1 ? Color.Yellow : Color.Blue;
                _spriteBatch.Draw(_pixel, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), color);
            }
        }
    }



}