using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CannonBall : Entity
{
    public CannonBall(Vector2 position, int cannonballSpeed, int cannonballDamage, Texture2D texture) : base(position,
        texture)
    {
        Damage = cannonballDamage;
        Speed = cannonballSpeed;
    }

    public int Damage { get; set; }
    public float Speed { get; set; }

    public override void Update(GameTime gameTime)
    {
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void FireCannonBall(Player player)
    {
        var directionToShoot = new Vector2((float)Math.Cos(MathHelper.ToRadians(90f)),
            (float)Math.Sin(MathHelper.ToRadians(90f)));
        directionToShoot.Normalize();

        Position += directionToShoot * Speed;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}