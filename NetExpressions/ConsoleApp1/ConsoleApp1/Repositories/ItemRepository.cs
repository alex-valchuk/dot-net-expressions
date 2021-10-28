using ConsoleApp1.Model;

namespace ConsoleApp1.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public Item GetItemByType(ItemType itemType)
        {
            return new Item(itemType);
        }
    }
}