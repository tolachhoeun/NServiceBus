﻿namespace NServiceBus.Core.Tests.Routing.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus.Extensibility;
    using NServiceBus.Routing;
    using NUnit.Framework;
    using Testing;

    [TestFixture]
    public class UnicastPublishRouterConnectorTests
    {
        [Test]
        public async Task Should_set_messageintent_to_publish()
        {
            var router = new UnicastPublishRouterConnector(new Router(), new DistributionPolicy());
            var context = new TestableOutgoingPublishContext();

            await router.Invoke(context, ctx => TaskEx.CompletedTask);

            Assert.AreEqual(1, context.Headers.Count);
            Assert.AreEqual(MessageIntentEnum.Publish.ToString(), context.Headers[Headers.MessageIntent]);
        }

        class Router : IUnicastRouter
        {
            public Task<IEnumerable<UnicastRoutingStrategy>> Route(Type messageType, DistributionStrategy distributionStrategy, ContextBag contextBag)
            {
                IEnumerable<UnicastRoutingStrategy> unicastRoutingStrategies = new List<UnicastRoutingStrategy>
                {
                    new UnicastRoutingStrategy("Fake")
                };
                return Task.FromResult(unicastRoutingStrategies);
            }
        }
    }
}