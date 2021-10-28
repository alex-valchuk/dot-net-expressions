using ConsoleApp1.Model;

namespace ConsoleApp1.Decorators
{
    public class ProgramItemDecorator : IItemDecorator
    {
        public bool CanHandle(Item item) => item.Type == ItemType.Program;

        public void Decorate(Item item)
        {
            item.CustomFields.Add("program sign");
        }
    }
}