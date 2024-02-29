using NServiceBus;
using NServiceBus.Testing;
using System.Collections.Concurrent;

namespace LarsProjekt.IntegrationTests.TestSetup;

public class MyMessageTestSession : IMessageSession
{
    public PublishedMessage<object>[] PublishedMessages => publishedMessages.ToArray();

    readonly ConcurrentQueue<PublishedMessage<object>> publishedMessages = new ConcurrentQueue<PublishedMessage<object>>();

    public virtual Task Publish(object message, PublishOptions publishOptions, CancellationToken cancellationToken = default)
    {
        publishedMessages.Enqueue(new PublishedMessage<object>(message, publishOptions));
        return Task.CompletedTask;
    }

    public virtual Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Send(object message, SendOptions sendOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Send<T>(Action<T> messageConstructor, SendOptions sendOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Subscribe(Type eventType, SubscribeOptions subscribeOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Unsubscribe(Type eventType, UnsubscribeOptions unsubscribeOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
