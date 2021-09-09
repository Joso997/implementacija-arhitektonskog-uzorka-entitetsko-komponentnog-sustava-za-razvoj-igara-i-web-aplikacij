

namespace Manager.Events.Type
{
    public class ActedUponEventArgs : System.EventArgs
    {
        public InteractableObject InteractableObject { get; private set; }
        public ActedUponEventArgs(InteractableObject _object)
        {
            InteractableObject = _object;
        }
        public void SetWatcherOnEndPhase(EndPhaseDel recordProgress)
        {
            InteractableObject.RecordProgress = recordProgress;
        }
    }
}

