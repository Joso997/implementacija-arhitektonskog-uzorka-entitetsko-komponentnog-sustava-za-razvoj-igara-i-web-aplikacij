using Manager.Events.Type;
using System.Collections.Generic;

namespace Manager.Streaming
{
    public class StreamHandler
    {
        readonly DebugLog.WriteInLogDelegate WriteToFile;
        public Streamer<InteractableObject> Streamer { get; private set; }

        public event ActedInteractableDel ActedEvent;

        public StreamHandler(DebugLog.WriteInLogDelegate _writeToFile)
        {
            WriteToFile = _writeToFile;
            Streamer = new Streamer<InteractableObject>(Handle);
        }

        void Handle(InteractableObject receiver)
        {
            FillStack(receiver.InvokeEventHolder);
#if UNITY_EDITOR
            WriteToFile.Invoke(new List<string>()
                {
                    "Player Action",
                    System.Enum.GetName(typeof(InteractableObjectTypeEnum), receiver.ObjectType),
                    System.Enum.GetName(typeof(SubObjectTypeEnum), receiver.SubObjectType),
                    System.Enum.GetName(typeof(ConditionEnum), receiver.ActionType),
                    Events.Type.ObjectTypes.InteractableObjectTypes[receiver.ObjectType].ChooseSubType(receiver).ToString()
                }
            );
#else
            Events.Type.ObjectTypes.InteractableObjectTypes[receiver.ObjectType].ChooseSubType(receiver);
#endif
        }

        void FillStack(Stack<StackDataDel> stack)
        {
            stack.Clear();
            stack.Push(InvokeStreamNext);
            stack.Push(InvokeActedEvent);
        }

        private void InvokeStreamNext(StackData stackData)
        {
            Streamer.StreamNext();
        }

        public void InvokeActedEvent(StackData stackData)
        {
            ActedEvent.Invoke(new ActedUponEventArgs(stackData.InteractableObject));
        }

    }
}

