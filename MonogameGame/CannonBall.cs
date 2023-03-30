using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CannonBall : Entity
{

    public int Damage { get; set; }
    public float Speed { get; set; }

    public CannonBall(Vector2 position, int cannonballSpeed, int cannonballDamage, Texture2D texture) : base(position, texture)
    {
        Damage = cannonballDamage;
        Speed = cannonballSpeed;
    }

    public override void Update(GameTime gameTime)
    {
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        base.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}