using System;
using System.Linq;
using ConsoleApp1.Model;

namespace ConsoleApp1.Decorators
{
    public class ItemDecoratorChainLink : IItemDecorator
    {
        private readonly IItemDecorator[] _decorators;

        public ItemDecoratorChainLink(
            IItemDecorator current,
            IItemDecorator next)
        {
            if (current == null)
            {
                throw new ArgumentNullException(nameof(current));
            }

            _decorators = next != null
                ? new[] { current, next }
                : new[] { current };
        }

        public bool CanHandle(Item item) =>
            _decorators.Any(d => d.CanHandle(item));

        public void Decorate(Item item)
        {
            var decorators = _decorators.Where(d => d.CanHandle(item)).ToArray();

            foreach (var decorator in decorators)
            {
                decorator.Decorate(item);
            }
        }
    }
}