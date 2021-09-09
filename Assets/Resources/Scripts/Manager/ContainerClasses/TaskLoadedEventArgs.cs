

namespace Manager.Events.Type
{
    public class TaskLoadedEventArgs : System.EventArgs
    {
        public ConditionEnum ConditionEnum { get; }
        public EndPhaseDel EndPhase { get; }
        public string Dialogue { get; }

        public TaskLoadedEventArgs(ConditionEnum _conditionEnum, EndPhaseDel _endPhase, string _dialogue)
        {
            ConditionEnum = _conditionEnum;
            EndPhase = _endPhase;
            Dialogue = _dialogue;
        }
    }
}