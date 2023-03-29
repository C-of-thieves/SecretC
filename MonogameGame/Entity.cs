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
 public Rectangle BoundingBox
    {
        get
        {
            int width = Texture.Width;
            int height = Texture.Height;
            return new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }
    }
    public virtual void Update(GameTime gameTime, List <Entity> entities) {
       
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White); 

    }

    #region Collision
    protected bool IsTouchingLeft(Entity entity)
    {
        return this.BoundingBox.Right + this.Velocity.X > entity.BoundingBox.Left &&
            this.BoundingBox.Left < entity.BoundingBox.Left &&
            this.BoundingBox.Bottom > entity.BoundingBox.Top &&
            this.BoundingBox.Top < entity.BoundingBox.Bottom;
    }

    protected bool IsTouchingRight(Entity entity)
    {
        return this.BoundingBox.Left + this.Velocity.X < entity.BoundingBox.Right &&
            this.BoundingBox.Right > entity.BoundingBox.Right &&
            this.BoundingBox.Bottom > entity.BoundingBox.Top &&
            this.BoundingBox.Top < entity.BoundingBox.Bottom;
    }

    protected bool IsTouchingTop(Entity entity)
    {
        return this.BoundingBox.Bottom + this.Velocity.Y > entity.BoundingBox.Top &&
            this.BoundingBox.Top < entity.BoundingBox.Top &&
            this.BoundingBox.Right > entity.BoundingBox.Left &&
            this.BoundingBox.Left < entity.BoundingBox.Right;
    }

    protected bool IsTouchingBottom(Entity entity)
    {
        return this.BoundingBox.Top + this.Velocity.Y < entity.BoundingBox.Bottom &&
            this.BoundingBox.Bottom > entity.BoundingBox.Bottom &&
            this.BoundingBox.Right > entity.BoundingBox.Left &&
            this.BoundingBox.Left < entity.BoundingBox.Right;
    }

    #endregion

}

