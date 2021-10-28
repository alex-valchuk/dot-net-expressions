using ConsoleApp1.Model;

namespace ConsoleApp1.Decorators
{
    public class CompetitionItemDecorator : IItemDecorator
    {
        public bool CanHandle(Item item)
        {
            return item.Type == ItemType.Competition;
        }

        public void Decorate(Item item)
        {
            item.CustomFields.Add("channel sign");
        }
    }
}