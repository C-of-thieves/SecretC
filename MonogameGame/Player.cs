using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player : Entity
{
    public Inventory Inventory { get; set; }
    public int Cannons { get; set; }
    public int Crew { get; set; }

    public Player(Vector2 position, float healthPoints, Texture2D texture) : base(position, healthPoints, texture)
    {
        Inventory = new Inventory();
        Cannons = 0;
        Crew = 0;
    }
}