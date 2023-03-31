using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MonogameGame;

public class Game1 : Game
{
    public readonly int _mapHeight = 18000;
    public readonly int _mapWidth = 24000;
    private readonly float _scale = 0.1f;
    private readonly float _threshold = 0.4f;

    private Camera _camera;
    public List<Enemy> _enemies;
    public List<Explosion> _explosions;

    private Texture2D _explosionTexture;
    private readonly float _fillProbability = 0.45f;
    private readonly GraphicsDeviceManager _graphics;
    private readonly int _iterations = 2;
    private Color[,] _mapData;

    private int[,] _mapDataInt;
    private readonly MapGenerator _mapGenerator;

    private Texture2D _pixel;

    private Player _player;
    private Random _random;
    private SpriteBatch _spriteBatch;

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

        /*_mapData = new Color[_mapWidth, _mapHeight];

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                _mapData[x, y] = Color.Blue; // Fill the map with water
            }
        }

        */

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _camera = new Camera();
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        Art.Load(Content);

        _explosionTexture = Content.Load<Texture2D>("Default size/Effects/explosion1");

        var playerStartX = _mapWidth * 0.5f; // Multiplied by tile size (64)
        var playerStartY = _mapHeight * 0.5f; // Multiplied by tile size (64)

        _player = new Player(new Vector2(playerStartX, playerStartY), 100, Art.GetPlayerTexture(), 1800, 1200);

        _mapData = new Color[_mapWidth, _mapHeight];

        var generateMapDataTask = Task.Run(() =>
        {
            for (var x = 0; x < _mapWidth; x++)
            for (var y = 0; y < _mapHeight; y++)
                _mapData[x, y] = Color.Blue; // Fill the map with water
        });

        // Wait for the map data generation task to complete
        generateMapDataTask.Wait();

        // Define minimum and maximum distance from player
        float minDistance = 500;
        float maxDistance = 2500;
        var enemyCount = 40;

        var random = new Random();

        _enemies = new List<Enemy>();
        _explosions = new List<Explosion>();

        // Loop to spawn multiple enemies
        for (var i = 0; i < enemyCount; i++)
        {
            // Calculate random angle and distance from player
            var angle = (float)(random.NextDouble() * Math.PI * 2);
            var distance = (float)(random.NextDouble() * (maxDistance - minDistance) + minDistance);

            // Calculate enemy position relative to player
            var enemyX = _player.Position.X + distance * (float)Math.Cos(angle);
            var enemyY = _player.Position.Y + distance * (float)Math.Sin(angle);

            // Create and add new enemy to list
            var newEnemy = new Enemy(new Vector2(enemyX, enemyY), 50, Art.GetEnemyTexture());

            _enemies.Add(newEnemy);
        }

        var song = Content.Load<Song>(
            "Music/The Buccaneer's Haul Royalty Free Pirate Music"); // Put the name of your song here instead of "song_title"
        MediaPlayer.Volume = 0.01f;
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.01f;
        MediaPlayer.IsRepeating = true;
    }


    protected override async void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var cannonBallsToRemove = new List<CannonBall>();
        var enemiesToRemove = new List<Enemy>();

        _player.Update(gameTime);
        _camera.Follow(_player, _mapWidth, _mapHeight);

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            foreach (var cannonBall in _player.cannonBalls)
            foreach (var enemy in _enemies)
                if (cannonBall.BoundingBox.Intersects(enemy.BoundingBox))
                {
                    var explosion = new Explosion(_explosionTexture, enemy.Position);
                    _explosions.Add(explosion);
                    

                    enemiesToRemove.Add(enemy);

                    // Add the cannon ball to the list of cannon balls to remove
                    cannonBallsToRemove.Add(cannonBall);
                    break;
                }
            foreach (var explosion in _explosions)
            {
                explosion.Update(gameTime);
                if (explosion._lifeSpan <= 0)
                {
                    _explosions.Remove(explosion);
                    break;
                }
            }   

            foreach (var enemyToRemove in enemiesToRemove) _enemies.Remove(enemyToRemove);
            foreach (var cannonBallToRemove in cannonBallsToRemove) _player.cannonBalls.Remove(cannonBallToRemove);
        }

        // Create a list to store tasks for each enemy
        var enemyTasks = new List<Task>();

        foreach (var enemy in _enemies)
        {
            enemy.Update(gameTime);
            var enemyTask = Task.Run(() => enemy.PerformAI(_player));
            enemyTasks.Add(enemyTask);

            if (_player.CollidesWith(enemy))
            {
                _player.HandleCollision(enemy);
                enemy.HandleCollision(_player);
            }
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

        foreach (var cannonball in _player.cannonBalls)
        {
            cannonball.Draw(_spriteBatch);
            cannonball.FireCannonBall(_player);
        }

        foreach (var enemy in _enemies) enemy.Draw(_spriteBatch);

        foreach (var explosion in _explosions) explosion.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawMap()
    {
        var tileSize = 64;
        var drawDistance = 3000; // Change this value to adjust the draw distance

        var startX = (int)Math.Max(0, (_player.Position.X - drawDistance) / tileSize);
        var startY = (int)Math.Max(0, (_player.Position.Y - drawDistance) / tileSize);
        var endX = (int)Math.Min(_mapWidth, (_player.Position.X + drawDistance) / tileSize);
        var endY = (int)Math.Min(_mapHeight, (_player.Position.Y + drawDistance) / tileSize);

        for (var x = startX; x < endX; x++)
        for (var y = startY; y < endY; y++)
        {
            var color = _mapGenerator.MapDataInt[x, y] == 1 ? Color.Yellow : Color.Blue;
            _spriteBatch.Draw(_pixel, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), color);
        }
    }
}