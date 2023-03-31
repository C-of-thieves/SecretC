using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Item
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public Texture2D Icon { get; set; }

    public Item(string name, Texture2D icon)
    {
        Name = name;
        Icon = icon;
        Quantity = 1;
    }
}