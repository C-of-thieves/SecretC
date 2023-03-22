using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Enemy : Entity
{
    public List<Item> DroppedItems { get; set; }
    private float _timer;
    private Vector2 _randomDirection;
    private Random random= new Random();

    public Enemy(Vector2 position, float healthPoints, Texture2D texture) : base(position, healthPoints, texture)
    {
        DroppedItems = new List<Item>();
        _timer = 0;
        _randomDirection = Vector2.Zero;
    }

    public void PerformAI(Player player)
    {
        _timer += 1;

        // Change direction every 60 frames (1 second at 60 FPS)
        if (_timer >= 180)
        {
            _randomDirection = new Vector2((float)(2 * random.NextDouble() - 1), (float)(2 * random.NextDouble() - 1));
            _randomDirection.Normalize();
            _timer = 0;
        }

        // Adjust the speed value to control how fast monsters move
        float speed = 0.5f;
        Position += _randomDirection * speed;
    }
}