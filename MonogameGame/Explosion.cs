using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Explosion
{
    private readonly Vector2 _position;
    private readonly Texture2D _texture;
    private float _alpha;
    private float _elapsedTime;
    public float _lifeSpan;
    private float _rotation;
    private float _scale;

    public Explosion(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        _position = position;
        _scale = 1f;
        _alpha = 1f;
        _lifeSpan = 1f;
        _rotation = 0f;
    }

    public void Update(GameTime gameTime)
    {
        _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _lifeSpan -= _elapsedTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White * _alpha, _rotation,
            new Vector2(_texture.Width / 2f, _texture.Height / 2f), _scale, SpriteEffects.None, 0f);
    }
}