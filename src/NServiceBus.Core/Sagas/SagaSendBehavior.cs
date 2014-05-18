﻿namespace NServiceBus.Sagas
{
    using System;
    using Pipeline;
    using Pipeline.Contexts;
    using Unicast.Transport;


    class SagaSendBehavior : IBehavior<OutgoingContext>
    {
        public void Invoke(OutgoingContext context, Action next)
        {
            if (context.OutgoingLogicalMessage.IsControlMessage())
            {
                next();
                return;
            }

            ActiveSagaInstance saga;

            if (context.TryGet(out saga) && !saga.NotFound)
            {
                context.OutgoingLogicalMessage.Headers[Headers.OriginatingSagaId] = saga.Instance.Entity.Id.ToString();
                context.OutgoingLogicalMessage.Headers[Headers.OriginatingSagaType] = saga.SagaType.AssemblyQualifiedName;
            }

            //auto correlate with the saga we are replying to if needed
            if (context.SendOptions.Intent == MessageIntentEnum.Reply && context.IncomingMessage != null)
            {
                string sagaId;
                string sagaType;

                if (context.IncomingMessage.Headers.TryGetValue(Headers.OriginatingSagaId, out sagaId))
                {
                    context.OutgoingLogicalMessage.Headers[Headers.SagaId] = sagaId;
                }

                if (context.IncomingMessage.Headers.TryGetValue(Headers.OriginatingSagaType, out sagaType))
                {
                    context.OutgoingLogicalMessage.Headers[Headers.SagaType] = sagaType;
                }
            }

            next();
        }
    }
}