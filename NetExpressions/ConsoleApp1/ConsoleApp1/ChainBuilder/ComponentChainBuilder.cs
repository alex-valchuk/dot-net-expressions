using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1.ChainBuilder
{
    public class ComponentChainBuilder<TInterface> : IChainBuilder<TInterface>
        where TInterface : class
    {
        private static readonly Type InterfaceType = typeof(TInterface);

        private readonly List<Type> _chain = new List<Type>();
        private readonly IServiceCollection _container;
        private readonly ConstructorInfo _chainLinkCtor;
        private readonly string _currentImplementationArgName;
        private readonly string _nextImplementationArgName;

        public ComponentChainBuilder(
            IServiceCollection container,
            Type chainLinkType,
            string currentImplementationArgName,
            string nextImplementationArgName)
        {
            _container = container;//.GuardNotNull(nameof(container));
            _chainLinkCtor = chainLinkType.GetConstructors().First();//.GuardNotNull(nameof(chainLinkType));
            _currentImplementationArgName = currentImplementationArgName;//.GuardNeitherNullNorWhitespace(nameof(currentImplementationArgName));
            _nextImplementationArgName = nextImplementationArgName;//.GuardNeitherNullNorWhitespace(nameof(nextImplementationArgName));
        }

        /// <inheritdoc />
        public IChainBuilder<TInterface> Link(Type implementationType)
        {
            _chain.Add(implementationType);
            return this;
        }

        /// <inheritdoc />
        public IChainBuilder<TInterface> Link<TImplementationType>()
          where TImplementationType : class, TInterface
            => Link(typeof(TImplementationType));

        public IServiceCollection Build(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            if (_chain.Count == 0)
            {
                throw new InvalidOperationException("At least one link must be registered.");
            }

            var serviceProviderParameter = Expression.Parameter(typeof(IServiceProvider), "x");
            Expression chainLink = null;

            for (var i = _chain.Count - 1; i > 0; i--)
            {
                var currentLink = CreateLinkExpression(_chain[i - 1], serviceProviderParameter);
                var nextLink = chainLink ?? CreateLinkExpression(_chain[i], serviceProviderParameter);
                chainLink = CreateChainLinkExpression(currentLink, nextLink, serviceProviderParameter);
            }

            if (chainLink == null)
            {
                // only one type is defined so we use it to register dependency
                _container.Add(new ServiceDescriptor(InterfaceType, _chain[0], serviceLifetime));
            }
            else
            {
                // chain is built so we use it to register dependency
                var expressionType = Expression.GetFuncType(typeof(IServiceProvider), InterfaceType);
                var createChainLinkLambda = Expression.Lambda(expressionType, chainLink, serviceProviderParameter);
                var createChainLinkFunction = (Func<IServiceProvider, object>)createChainLinkLambda.Compile();

                _container.Add(new ServiceDescriptor(InterfaceType, createChainLinkFunction, serviceLifetime));
            }

            return _container;
        }

        private NewExpression CreateLinkExpression(Type linkType, ParameterExpression serviceProviderParameter)
        {
            var linkCtor = linkType.GetConstructors().First();
            var linkCtorParameters = linkCtor.GetParameters()
                .Select(p => GetServiceProviderDependenciesExpression(p, serviceProviderParameter))
                .ToArray();
            return Expression.New(linkCtor, linkCtorParameters);
        }

        private Expression CreateChainLinkExpression(
            Expression currentLink,
            Expression nextLink,
            ParameterExpression serviceProviderParameter)
        {
            var chainLinkCtorParameters = _chainLinkCtor.GetParameters().Select(p =>
            {
                if (p.Name == _currentImplementationArgName)
                {
                    return currentLink;
                }

                if (p.Name == _nextImplementationArgName)
                {
                    return nextLink;
                }

                return GetServiceProviderDependenciesExpression(p, serviceProviderParameter);
            }).ToArray();

            return Expression.New(_chainLinkCtor, chainLinkCtorParameters);
        }

        private static Expression GetServiceProviderDependenciesExpression(ParameterInfo parameter, ParameterExpression serviceProviderParameter)
        {
            // this is a parameter we don't care about, so we just ask GetRequiredService to resolve it for us
            return Expression.Call(
                typeof(ServiceProviderServiceExtensions),
                nameof(ServiceProviderServiceExtensions.GetRequiredService),
                new[] { parameter.ParameterType },
                serviceProviderParameter);
        }
    }
}