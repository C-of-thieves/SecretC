using Microsoft.Xna.Framework.Graphics;

public class Item
{
    public Item(string name, Texture2D icon)
    {
        Name = name;
        Icon = icon;
        Quantity = 1;
    }

    public string Name { get; set; }
    public int Quantity { get; set; }
    public Texture2D Icon { get; set; }
}