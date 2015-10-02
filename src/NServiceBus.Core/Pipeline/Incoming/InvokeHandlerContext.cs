namespace NServiceBus.Pipeline.Contexts
{
    using System.Collections.Generic;
    using Unicast.Behaviors;
    using Unicast.Messages;

    /// <summary>
    /// A context of handling a logical message by a handler.
    /// </summary>
    public class InvokeHandlerContext : IncomingContext
    {
        /// <summary>
        /// Initializes the handling stage context.
        /// </summary>
        public InvokeHandlerContext(MessageHandler handler, LogicalMessageProcessingContext parentContext)
            : base(parentContext)
        {
            MessageHandler = handler;
            Headers = parentContext.Headers;
            MessageBeingHandled = parentContext.Message.Instance;
            MessageMetadata = parentContext.Message.Metadata;
            MessageId = Headers[NServiceBus.Headers.MessageId];
        }

        /// <summary>
        /// The current <see cref="IHandleMessages{T}"/> being executed.
        /// </summary>
        public MessageHandler MessageHandler { get; private set; }

        /// <summary>
        /// Message headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// The message instance being handled.
        /// </summary>
        public object MessageBeingHandled { get; private set; }

        /// <summary>
        /// Call this to stop the invocation of handlers.
        /// </summary>
        public void DoNotInvokeAnyMoreHandlers()
        {
            HandlerInvocationAborted = true;
        }

        /// <summary>
        /// <code>true</code> if DoNotInvokeAnyMoreHandlers has been called.
        /// </summary>
        public bool HandlerInvocationAborted { get; private set; }

        /// <summary>
        /// Metadata for the incoming message.
        /// </summary>
        public MessageMetadata MessageMetadata { get; private set; }

        /// <summary>
        /// Id of the incoming message.
        /// </summary>
        public string MessageId { get; private set; }
    }
}