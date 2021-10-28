using ConsoleApp1.Model;

namespace ConsoleApp1.Repositories
{
    public interface IItemRepository
    {
        Item GetItemByType(ItemType itemType);
    }
}