using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Entity
{
    public Entity(Vector2 position, float healthPoints, Texture2D texture)
    {
        Position = position;
        Texture = texture;
        HealthPoints = healthPoints;
    }

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Texture2D Texture { get; set; }
    public float HealthPoints { get; set; }

    public Rectangle BoundingBox
    {
        get
        {
            var width = Texture.Width;
            var height = Texture.Height;
            return new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }
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

    public bool CollidesWith(Entity other)
    {
        return BoundingBox.Intersects(other.BoundingBox);
    }

    public void HandleCollision(Entity other)
    {
        var mtv = CalculateMtv(other);

        Position += mtv;
        other.Position -= mtv;
    }

    private Vector2 CalculateMtv(Entity other)
    {
        var intersection = Rectangle.Intersect(BoundingBox, other.BoundingBox);
        var direction = Vector2.Normalize(Position - other.Position);
        float distance = intersection.Width > intersection.Height ? intersection.Height : intersection.Width;
        var mtv = direction * distance;
        return mtv;
    }
}