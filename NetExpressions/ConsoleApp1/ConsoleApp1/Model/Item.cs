using System.Collections.Generic;

namespace ConsoleApp1.Model
{
    public class Item
    {
        public Item(ItemType type)
        {
            Type = type;
        }

        public ItemType Type { get; }
        
        public List<string> CustomFields { get; } = new List<string>();
    }
}