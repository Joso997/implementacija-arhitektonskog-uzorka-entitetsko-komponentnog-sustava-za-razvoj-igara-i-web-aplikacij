using Manager.Events.Type;
using Manager.Mechanics.Interface;
using Manager.UI;

namespace Manager.Mechanics.PowerGame
{
    public class PowerGameMechanicController : MechanicInterface.HasUIController<PowerUI>
    {
        private InteractableObject InteractableObject;
        public PowerGameMechanicController(PowerUI _uIContoller) : base(_uIContoller) 
        {
            UIScreens = UIContoller.Screens[ConditionEnum.Vent];
            UIContoller.StartUI(UIScreens, null, 0, 5);
        }

        protected override void SubscribeConditions()
        {
            (Condition.Conditions[ConditionEnum.Vent] as ISubController<ActedInteractableDel>).SubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Dialogue] as ISubController<ActedDefaultDel>).SubscribeController(TerminateUI);
            (Condition.Conditions[ConditionEnum.Hack] as ISubController<ActedDefaultDel>).SubscribeController(TerminateUI);
            (Condition.Conditions[ConditionEnum.Avoid] as ISubController<ActedDefaultDel>).SubscribeController(TerminateUI);
            (Condition.Conditions[ConditionEnum.Course] as ISubController<ActedDefaultDel>).SubscribeController(TerminateUI);
        }

        public override void Stop()
        {
            (Condition.Conditions[ConditionEnum.Vent] as ISubController<ActedInteractableDel>).UnSubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Dialogue] as ISubController<ActedDefaultDel>).UnSubscribeController(TerminateUI);
            (Condition.Conditions[ConditionEnum.Hack] as ISubController<ActedDefaultDel>).UnSubscribeController(TerminateUI);
            (Condition.Conditions[ConditionEnum.Avoid] as ISubController<ActedDefaultDel>).UnSubscribeController(TerminateUI);
            (Condition.Conditions[ConditionEnum.Course] as ISubController<ActedDefaultDel>).UnSubscribeController(TerminateUI);
            base.Stop();
        }

        void TerminateUI(ComputerConditionSetEventArgs e)
        {
            ConditionSet = null;
            UIContoller.StopUI(UIScreens, 0);
            if(InteractableObject != null)
                ObjectTypes.InteractableObjectTypes[InteractableObject.ObjectType].NullifyLogic();
        }

        void ActivateUI(ActedUponEventArgs e)
        {
            MonitorHeaderScroll.textToScroll = "!OPREZ! Pripazite na pritisak reaktora.";
            InteractableObject = e.InteractableObject;
            UIContoller.RestartAll(UIScreens, 1, 2);
            ObjectTypes.InteractableObjectTypes[e.InteractableObject.ObjectType].SubscribeLogic(Button);
        }
        protected override void Button(SubObjectTypeEnum button_id)
        {
            switch (button_id)
            {
                case SubObjectTypeEnum.MiddleButton:
                    ConditionSet = null;
                    UIContoller.StopUI(UIScreens, 0);
                    ObjectTypes.InteractableObjectTypes[InteractableObject.ObjectType].NullifyLogic();
                    break;
                default:
                    break;
            }
        }

        protected override void ActivateUI(ComputerConditionSetEventArgs e)
        {
            ConditionSet = e;
        }
    }
}
