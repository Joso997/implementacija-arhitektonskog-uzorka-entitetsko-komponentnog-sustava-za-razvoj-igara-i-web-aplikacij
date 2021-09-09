using Manager.Events.Type;

namespace Manager.Tracking
{
    public class Tracker
    {
        double passedPhases = 1;
        double failedPhases = 1;
        public Tracker()
        {
            SubscribeConditions();
        }
        //GlobalObjects can't be subbed
        void SubscribeConditions()
        {
            (Condition.Conditions[ConditionEnum.Vent] as ISubController<ActedInteractableDel>).SubscribeController(SetWatcher);
            (Condition.Conditions[ConditionEnum.Eject] as ISubController<ActedDefaultDel>).SubscribeController(SetWatcher);
            (Condition.Conditions[ConditionEnum.Avoid] as ISubController<ActedDefaultDel>).SubscribeController(SetWatcher);
            (Condition.Conditions[ConditionEnum.Course] as ISubController<ActedDefaultDel>).SubscribeController(SetWatcher);
            (Condition.Conditions[ConditionEnum.Dock] as ISubController<ActedDefaultDel>).SubscribeController(SetWatcher);
            (Condition.Conditions[ConditionEnum.Hack] as ISubController<ActedDefaultDel>).SubscribeController(SetWatcher);
            (Condition.Conditions[ConditionEnum.Repair] as ISubController<ActedInteractableDel>).SubscribeController(SetWatcher);
            (Condition.Conditions[ConditionEnum.Refuel] as ISubController<ActedInteractableDel>).SubscribeController(SetWatcher);
        }

        private void SetWatcher(ActedUponEventArgs e)
        {
            e.SetWatcherOnEndPhase(RecordProgress);
        }

        private void SetWatcher(ComputerConditionSetEventArgs e)
        {
            e.SetWatcherOnEndPhase(RecordProgress);
        }

        private void RecordProgress(EndActionData endActionData)
        {
            if (endActionData.ConditionFailed)
                failedPhases++;
            else
                passedPhases++;
        }
        public int CalculateExperiencedDifficulty()
        {
            int difference = (int)System.Math.Round(passedPhases / failedPhases, 0, System.MidpointRounding.AwayFromZero);
            if (difference > 1)
                return difference;
            else
                return difference * -1;
             
        }
    }
}

