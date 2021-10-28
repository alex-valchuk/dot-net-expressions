using ConsoleApp1.ChainBuilder;
using ConsoleApp1.Decorators;
using ConsoleApp1.Model;
using ConsoleApp1.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1
{
    static class Program
    {
        static void Main()
        {
            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IItemRepository, ItemRepository>();
            
            serviceProvider.Chain<IItemDecorator, ItemDecoratorChainLink>()
                .Link<ChannelItemDecorator>()
                .Link<CompetitionItemDecorator>()
                .Link<ProgramItemDecorator>()
                .Build(ServiceLifetime.Singleton);
            
            var services = serviceProvider.BuildServiceProvider();

            var itemDecorator = services.GetRequiredService<IItemDecorator>();
            var item = new Item(ItemType.Program);
            
            itemDecorator.Decorate(item);
        }
    }
}