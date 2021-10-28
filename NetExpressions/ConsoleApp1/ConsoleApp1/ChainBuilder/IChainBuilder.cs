using System;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1.ChainBuilder
{
    public interface IChainBuilder<TInterface>
        where TInterface : class
    {
        /// <summary>
        /// Adds a link to the chain of responsibility. The chain itself; however, is not registered until
        /// <see cref="Add(string, LifestyleType?, bool, Action)"/> is called.
        /// </summary>
        /// <param name="implementationType">The type of the chain link implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IChainBuilder<TInterface> Link(Type implementationType);

        /// <summary>
        /// Adds a link to the chain of responsibility. The chain itself; however, is not registered until
        /// <see cref="IChainBuilder{TService}.Add(string, LifestyleType?, bool, Action{ComponentRegistration{object}})"/> is called.
        /// </summary>
        /// <typeparam name="TServiceType">The registered chain link implementation.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IChainBuilder<TInterface> Link<TServiceType>()
            where TServiceType : class, TInterface;

        /// <summary>
        /// Registers the chain of responsibility for service <typeparamref name="TInterface"/>
        /// in the container.
        /// </summary>
        /// <param name="serviceLifetime">The lifetime of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IServiceCollection Build(ServiceLifetime serviceLifetime = ServiceLifetime.Transient);
    }
}