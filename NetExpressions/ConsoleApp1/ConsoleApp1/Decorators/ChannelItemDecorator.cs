using System;
using ConsoleApp1.Model;
using ConsoleApp1.Repositories;

namespace ConsoleApp1.Decorators
{
    public class ChannelItemDecorator : IItemDecorator
    {
        private readonly IItemRepository _itemRepository;

        public ChannelItemDecorator(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public bool CanHandle(Item item)
        {
            return item.Type == ItemType.Channel;
        }

        public void Decorate(Item item)
        {
            item.CustomFields.Add("channel sign");
        }
    }
}