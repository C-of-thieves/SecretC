using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using MonogameGame;

public class Player : Entity
{
    public Inventory Inventory { get; set; }
    public int gameScreenWidth { get; private set; }
    public int gameScreenHeight { get; private set; }
    public int Ammunition { get; set; }

    public List<CannonBall> cannonBalls = new List<CannonBall>();
    private const int cannonballSpeed = 10;
    private const int cannonballDamage = 10;


    public Player(Vector2 position, float healthPoints, Texture2D texture, int gameScreenWidth, int gameScreenHeight) :
        base(position, texture)
    {
        Inventory = new Inventory();
        this.gameScreenWidth = gameScreenWidth;
        this.gameScreenHeight = gameScreenHeight;
        Ammunition = 100;
        healthPoints = 100;
    }
  /*  public void LoadCannon()
    {
        for (int i = Ammunition; i > 0; i--)
        {
            cannonBalls.Add(new CannonBall(Position, cannonballSpeed, cannonballDamage, Art.GetCannonBallTexture()));
            Ammunition--;
        }
    }*/

    public override void Update(GameTime gameTime)
    {
       // LoadCannon();
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

        if (Keyboard.GetState().IsKeyDown(Keys.Space) && Ammunition > 0)
        {
            
            cannonBalls.Add(new CannonBall(Position, cannonballSpeed, cannonballDamage, Art.GetCannonBallTexture()));
            cannonBalls.Add(new CannonBall(Position, cannonballSpeed, cannonballDamage, Art.GetCannonBallTexture()));
            cannonBalls.Add(new CannonBall(Position, cannonballSpeed, cannonballDamage, Art.GetCannonBallTexture()));
            cannonBalls.Add(new CannonBall(Position, cannonballSpeed, cannonballDamage, Art.GetCannonBallTexture()));
            cannonBalls.Add(new CannonBall(Position, cannonballSpeed, cannonballDamage, Art.GetCannonBallTexture()));
            Ammunition--;
        }

        foreach (CannonBall cannonball in cannonBalls)
        {
            cannonball.Update(gameTime);
        }

        Position += velocity;


        base.Update(gameTime);
    }
}