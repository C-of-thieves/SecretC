using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

public class Player : Entity
{
    public Inventory Inventory { get; set; }
    public int Cannons { get; set; }
    public int Crew { get; set; }

    public int gameScreenWidth { get; private set; }
    public int gameScreenHeight { get; private set; }

    public Player(Vector2 position, float healthPoints, Texture2D texture,int gameScreenWidth, int gameScreenHeight) : base(position, healthPoints, texture)
    {
        Inventory = new Inventory();
        Cannons = 0;
        Crew = 0;
        this.gameScreenWidth = gameScreenWidth;
        this.gameScreenHeight = gameScreenHeight;
    }

    public override void Update(GameTime gameTime, List<Entity> entities)
    {
        var velocity = Velocity;

        foreach (var entity in entities)
        {
            if (entity == this)
            continue;

            if ((this.Velocity.X > 0 && this.IsTouchingLeft(entity)) ||
                (this.Velocity.X < 0 && this.IsTouchingRight(entity)))
                velocity.X = 0;

            if ((this.Velocity.Y > 0 && this.IsTouchingLeft(entity)) ||
                (this.Velocity.Y < 0 && this.IsTouchingRight(entity)))
                velocity.Y = 0;
        }

        Position += Velocity;
        Velocity = Vector2.Zero;
       
        var speed = 3f;

        if (Keyboard.GetState().IsKeyDown(Keys.W))
            velocity.Y = -speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
            velocity.Y = speed;
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            velocity.X = -speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
            velocity.X = speed;

        Position += velocity;


    }
}