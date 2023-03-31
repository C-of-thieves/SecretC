using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class GameState
{
    /**
     * This function is to store the gamestate data in a class and used to serialize and deserialize the data to and from XML. 
     */
    public Vector2 playerPosition { get; set; }
    public float playerHealth { get; set; }
    public int playerCannons { get; set; }
    public int playerCrew { get; set; }
    // public Player Player { get; set; }
    public List<Vector2> enemyPositions { get; set; }

    public GameState()
    {
        enemyPositions = new List<Vector2>();
    }
}