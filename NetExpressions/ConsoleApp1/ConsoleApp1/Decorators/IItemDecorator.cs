using ConsoleApp1.Model;

namespace ConsoleApp1.Decorators
{
    public interface IItemDecorator
    {
        bool CanHandle(Item item);
        
        void Decorate(Item item);
    }
}