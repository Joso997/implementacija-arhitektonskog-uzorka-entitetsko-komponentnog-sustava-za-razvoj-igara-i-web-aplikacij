using Manager.Events.Type;
using Manager.Mechanics.Interface;
using Manager.UI;

namespace Manager.Mechanics.MemoryGame
{
    public sealed class MemoryGameMechanicController : MechanicInterface.HasUIController<AssingmentUI>
    {
        readonly Alarms Alarms;
        public MemoryGameMechanicController(AssingmentUI _uIContoller, Alarms _alarms) : base(_uIContoller)
        {
            Alarms = _alarms;
        }

        protected override void SubscribeConditions()
        {
            (Condition.Conditions[ConditionEnum.Avoid] as ISubController<ActedDefaultDel>).SubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Course] as ISubController<ActedDefaultDel>).SubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Dock] as ISubController<ActedDefaultDel>).SubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Hack] as ISubController<ActedDefaultDel>).SubscribeController(ActivateUI);
        }
        public override void Stop()
        {
            (Condition.Conditions[ConditionEnum.Avoid] as ISubController<ActedDefaultDel>).UnSubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Course] as ISubController<ActedDefaultDel>).UnSubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Dock] as ISubController<ActedDefaultDel>).UnSubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Hack] as ISubController<ActedDefaultDel>).UnSubscribeController(ActivateUI);
            base.Stop();
        }

        public void StartAssingment()
        {
            if(ConditionSet != null)
            {
                ConditionSet.Alarm = Alarms;
                UIContoller.StartUI(UIScreens, new ExtraData(ConditionSet.GetConditionDefinition, ConditionSet.CheckAnswers, RefreshChildCount), ConditionSet.Child_Id, 5);
                Alarms.ActivateAlarm(1);
            }
                
            NotFirstTask = true;
        }

        protected override void ActivateUI(ComputerConditionSetEventArgs e)
        {
            if(ConditionSet != null)
                UIContoller.StopUI(UIScreens);//TODO check if tranform is needed
            ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Computer].NullifyLogic();
            ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Computer].SubscribeLogic(Button);
            ConditionSet = e;
            ConditionSet.UIContoller = UIContoller;
            UIScreens = ConditionSet.GetUIScreen();
            ConditionSet.Assingment = Shuffle(ConditionSet.Assingment);
            if (NotFirstTask)
                StartAssingment();
        }

        protected override void Button(SubObjectTypeEnum button_id)
        {
            if (ConditionSet == null)
                return;
            if (ConditionSet.Child_Id > 1)
            {
                switch (button_id)
                {
                    case SubObjectTypeEnum.LeftButton:
                        ConditionSet.LeftDigit++;
                        if (ConditionSet.LeftDigit > ConditionSet.NumberOfDigits)
                            ConditionSet.LeftDigit = 0;
                        ConditionSet.GetButtonDefinition(UIScreens, button_id);
                        break;
                    case SubObjectTypeEnum.MiddleButton:
                        //temp variables just to be safe
                        var child_Id = ConditionSet.Child_Id;
                        var tempUI = UIScreens;
                        ConditionSet.GetButtonDefinition(UIScreens, button_id);
                        if (UIScreens.childCount == child_Id + 1)
                        {
                            var tempConditionSet = ConditionSet;
                            ConditionSet = null;
                            UIContoller.StopUI(tempUI.transform, child_Id);
                            tempConditionSet.CheckAnswers();
                        }
                        break;
                    case SubObjectTypeEnum.RightButton:
                        ConditionSet.RightDigit++;
                        if (ConditionSet.RightDigit > ConditionSet.NumberOfDigits)
                            ConditionSet.RightDigit = 0;
                        ConditionSet.GetButtonDefinition(UIScreens, button_id);
                        break;
                }
            }                         
        }
    }
}

