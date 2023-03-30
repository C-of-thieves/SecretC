using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Entity
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float HealthPoints { get; set; }
    public Texture2D Texture { get; set; }

    public Entity(Vector2 position, float healthPoints, Texture2D texture)
    {
        Position = position;
        HealthPoints = healthPoints;
        Texture = texture;
    }

    public virtual void Update(GameTime gameTime) 
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

       
        Position += Velocity * deltaTime;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }

    public Rectangle BoundingBox
    {
        get
        {
            int width = Texture.Width;
            int height = Texture.Height;
            return new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }
    }
    public bool CollidesWith(Entity other)
    {
        return this.BoundingBox.Intersects(other.BoundingBox);
    }

    public void HandleCollision(Entity other)
    {
        Vector2 mtv = CalculateMTV(other);

        Position += mtv;
        other.Position -= mtv;

    }

    private Vector2 CalculateMTV(Entity other)
    {
        Rectangle intersection = Rectangle.Intersect(BoundingBox, other.BoundingBox);
        Vector2 direction = Vector2.Normalize(Position - other.Position);
        float distance = intersection.Width > intersection.Height ?
            intersection.Height :
            intersection.Width;
        Vector2 mtv = direction * distance;
        return mtv;
    }
}

