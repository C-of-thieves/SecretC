using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Inventory
{
    private Dictionary<string, Item> _items;

    public Inventory()
    {
        _items = new Dictionary<string, Item>();
    }

    public void AddItem(Item item)
    {
        if (_items.ContainsKey(item.Name))
        {
            _items[item.Name].Quantity += item.Quantity;
        }
        else
        {
            _items.Add(item.Name, item);
        }
    }

    public void RemoveItem(string itemName)
    {
        if (_items.ContainsKey(itemName))
        {
            _items[itemName].Quantity--;
            if (_items[itemName].Quantity <= 0)
            {
                _items.Remove(itemName);
            }
        }
    }

    public int GetItemCount(string itemName)
    {
        if (_items.ContainsKey(itemName))
        {
            return _items[itemName].Quantity;
        }
        return 0;
    }

    public IEnumerable<Item> GetItems()
    {
        return _items.Values;
    }
}