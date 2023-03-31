using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Enemy : Entity
{
    public enum EnemyState
    {
        Idle,
        Wander,
        Chase
    }

    private readonly Random random = new();
    private Vector2 _randomDirection;
    private float _timer;
    private EnemyState CurrentState;


    public Enemy(Vector2 position, float healthPoints, Texture2D texture) : base(position, texture)
    {
        DroppedItems = new List<Item>();
        _timer = 0;
        _randomDirection = Vector2.Zero;
    }

    public List<Item> DroppedItems { get; set; }

    public void PerformAI(Player player)
    {
        _timer += 1;

        // Calculate the distance between the enemy and the player
        var distanceToPlayer = Vector2.Distance(Position, player.Position);

        // Define thresholds for changing states
        var chaseDistanceThreshold = 800f;
        var idleDistanceThreshold = 1200f;
        float idleTime = 180; // 3 seconds at 60 FPS
        float wanderTime = 240; // 4 seconds at 60 FPS

        // Adjust the speed value to control how fast monsters move
        var speed = 0.8f;

        // Update enemy state based on the distance to the player
        if (distanceToPlayer < chaseDistanceThreshold)
            CurrentState = EnemyState.Chase;
        else if (distanceToPlayer < idleDistanceThreshold)
            CurrentState = EnemyState.Wander;
        else
            CurrentState = EnemyState.Idle;

        // Perform actions based on the current state
        switch (CurrentState)
        {
            case EnemyState.Idle:
                if (_timer >= idleTime) _timer = 0;

                break;

            case EnemyState.Wander:
                if (_timer >= wanderTime)
                {
                    _randomDirection = new Vector2((float)(2 * random.NextDouble() - 1),
                        (float)(2 * random.NextDouble() - 1));
                    _randomDirection.Normalize();
                    _timer = 0;
                }

                Position += _randomDirection * speed;
                break;

            case EnemyState.Chase:
                // Calculate the direction to the player
                var directionToPlayer = player.Position - Position;
                directionToPlayer.Normalize();

                // Move towards the player
                Position += directionToPlayer * speed;
                break;
        }
    }
}