using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1.ChainBuilder
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///  Creates a chain of components that are linked using a special <typeparamref name="TChainLink"/> objects
        ///  that receive both the current and the next implementations using constructor injection.
        /// </summary>
        /// <typeparam name="TInterface">The type of the service to register.</typeparam>
        /// <typeparam name="TChainLink">The chain link class.</typeparam>
        /// <param name="container">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="currentImplementationArgumentName">The name of the <typeparamref name="TChainLink"/> class constructor argument that receives the current implementation.</param>
        /// <param name="nextImplementationArgumentName">The name of the <typeparamref name="TChainLink"/> class constructor argument that receives the next implementation.</param>
        /// <returns>The component chain builder.</returns>
        public static IChainBuilder<TInterface> Chain<TInterface, TChainLink>(
            this IServiceCollection container,
            string currentImplementationArgumentName = "current",
            string nextImplementationArgumentName = "next")
            where TInterface : class
            where TChainLink : TInterface
            => new ComponentChainBuilder<TInterface>(
                container,
                typeof(TChainLink),
                currentImplementationArgumentName,
                nextImplementationArgumentName);
    }
}