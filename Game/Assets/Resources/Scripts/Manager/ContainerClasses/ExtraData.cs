using Manager.Events.Type;
using System.Collections.Generic;

namespace Manager.Mechanics
{
    public sealed class ExtraData
    {
        public System.Func<int, int> RefreshChildCount;
        public System.Action<UnityEngine.Transform> GetConditionDefinition;
        public System.Func<bool, EndActionData, bool> CheckAnswers;
        public Dictionary<int, StasisData.ChamberData> StasisChambers;
        public System.Action GetAction;
        public System.Func<string> GetFunc;

        public ExtraData(Dictionary<int, StasisData.ChamberData> _stasisChambers, System.Action _getAction, System.Func<string> _getFunc)
        {
            StasisChambers = _stasisChambers;
            this.GetAction = _getAction;
            this.GetFunc = _getFunc;
        }
        public ExtraData(System.Action<UnityEngine.Transform> _getConditionDefinition, System.Func<bool, EndActionData, bool> _checkAnswers, System.Func<int, int> _refreshChildCount)
        {
            this.GetConditionDefinition = _getConditionDefinition;
            this.RefreshChildCount = _refreshChildCount;
            this.CheckAnswers = _checkAnswers;
        }
    }
}