using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Player : Entity
{
    private const int cannonballSpeed = 7;
    private const int cannonballDamage = 10;

    public List<CannonBall> cannonBalls = new();

    private float fireCooldown;
    private readonly float fireDelay = 0.5f; // 0.5 seconds between each shot


    public Player(Vector2 position, float healthPoints, Texture2D texture, int gameScreenWidth, int gameScreenHeight) :
        base(position, texture)
    {
        Inventory = new Inventory();
        this.gameScreenWidth = gameScreenWidth;
        this.gameScreenHeight = gameScreenHeight;
        Ammunition = 10;
        healthPoints = 100;
    }

    public Inventory Inventory { get; set; }
    public int gameScreenWidth { get; }
    public int gameScreenHeight { get; }
    public int Ammunition { get; set; }


    public override void Update(GameTime gameTime)
    {
        var velocity = new Vector2();
        var speed = 3f;

        if (Keyboard.GetState().IsKeyDown(Keys.W))
            velocity.Y = -speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
            velocity.Y = speed;
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            velocity.X = -speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
            velocity.X = speed;

        if (fireCooldown > 0) fireCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Keyboard.GetState().IsKeyDown(Keys.Space) && fireCooldown <= 0)
        {
            fireCooldown = fireDelay;
            var origin = new Vector2(
                Position.X + Texture.Width / 2,
                Position.Y + Texture.Height / 2
            );

            for (var i = Ammunition; i > 0; i--)
                cannonBalls.Add(new CannonBall(origin, cannonballSpeed, cannonballDamage, Art.GetCannonBallTexture()));
        }

        foreach (var cannonball in cannonBalls) cannonball.Update(gameTime);

        Position += velocity;


        base.Update(gameTime);
    }
}