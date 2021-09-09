

namespace Manager.Events.Type
{
    public class StatChangeEventArgs : System.EventArgs
    {
        public InteractableObjectStatEnum StatType { get; private set; }
        public int Amount { get; }

        public StatChangeEventArgs(InteractableObjectStatEnum _statType, int _amount)
        {
            StatType = _statType;
            Amount = _amount;
        }
    }
}