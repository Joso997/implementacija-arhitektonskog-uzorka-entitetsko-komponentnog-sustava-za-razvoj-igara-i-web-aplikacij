using Manager.Events.Type;
using Manager.Mechanics.Interface;
using Manager.UI;
using System.Collections.Generic;

namespace Manager.Mechanics.PowerGame
{
    public class EndingGameController : MechanicInterface.HasUIController<EndingUI>
    {
        private InteractableObject InteractableObject;
        protected Dictionary<int, StasisData.ChamberData> StasisChambers { get; set; }
        public EndingGameController(EndingUI _uIContoller, Dictionary<int, StasisData.ChamberData> _stasisChambers) : base(_uIContoller)
        {
            StasisChambers = _stasisChambers;
        }

        protected override void SubscribeConditions()
        {
            (Condition.Conditions[ConditionEnum.Animation] as ISubController<ActedDefaultDel>).SubscribeController(ActivateUI);
        }

        public override void Stop()
        {
            (Condition.Conditions[ConditionEnum.Animation] as ISubController<ActedDefaultDel>).UnSubscribeController(ActivateUI);
            base.Stop();
        }

        void TerminateUI(ComputerConditionSetEventArgs e)
        {
            ConditionSet = null;
            UIContoller.StopUI(UIScreens, 0);
            if (InteractableObject != null)
                ObjectTypes.InteractableObjectTypes[InteractableObject.ObjectType].NullifyLogic();
        }

        void ActivateUI(ActedUponEventArgs e)
        {
            MonitorHeaderScroll.textToScroll = "!OPREZ! Pripazite na pritisak reaktora.";
            InteractableObject = e.InteractableObject;
            UIContoller.RestartAll(UIScreens, 1, 50);
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
            MonitorHeaderScroll.textToScroll = "Kraj Igre.";
            UIScreens = UIContoller.Screens[ConditionEnum.Animation];
            UIContoller.StartUI(UIScreens, new ExtraData(StasisChambers, null, null), 0, 5);
        }
    }
}
