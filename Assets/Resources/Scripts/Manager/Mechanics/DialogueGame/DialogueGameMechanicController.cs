using Manager.Events.Type;
using Manager.Mechanics.Interface;
using Manager.UI;

namespace Manager.Mechanics.DialogueGame
{
    public class DialogueGameMechanicController : MechanicInterface.HasUIController<DialogueUI>
    {
        public DialogueGameMechanicController(DialogueUI _uIContoller) : base(_uIContoller) { }

        protected override void SubscribeConditions()
        {
            (Condition.Conditions[ConditionEnum.Dialogue] as ISubController<ActedDefaultDel>).SubscribeController(ActivateUI);
        }
        public override void Stop()
        {
            (Condition.Conditions[ConditionEnum.Dialogue] as ISubController<ActedDefaultDel>).UnSubscribeController(ActivateUI);
            ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Computer].UnSubscribeLogic(Button);
            base.Stop();
        }
        public void MakeUIStartable()
        {
            if (ConditionSet != null)
                UIContoller.StartUI(UIScreens, new ExtraData(ConditionSet.GetConditionDefinition, ConditionSet.CheckAnswers, RefreshChildCount));
            NotFirstTask = true;
                     
        }

        protected override void ActivateUI(ComputerConditionSetEventArgs e)
        {
            ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Computer].SubscribeLogic(Button);
            ConditionSet = e;
            ConditionSet.UIContoller = UIContoller;
            UIScreens = ConditionSet.GetUIScreen();
            UnityEngine.Debug.Log(NotFirstTask);
            UnityEngine.Debug.Log(e.DefaultObject.ObjectType);
            if (NotFirstTask)
                UIContoller.StartUI(UIScreens, new ExtraData(ConditionSet.GetConditionDefinition, ConditionSet.CheckAnswers, RefreshChildCount));
        }
        protected override void Button(SubObjectTypeEnum button_id)
        {
            switch (button_id)
            {
                case SubObjectTypeEnum.MiddleButton:
                    //temp variables just to be safe
                    var tempUI = UIScreens;   
                    if (string.IsNullOrEmpty(ConditionSet.GetDialogue(ConditionSet.Child_Id + 1)))
                    {
                        ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Computer].NullifyLogic();
                        UIContoller.StopUI(tempUI, 0);
                        ConditionSet.CheckAnswers(true);
                    }
                    else
                        UIContoller.Restart(tempUI, 0, 5);
                    break;
                default:
                    break;
            }
        }
        protected override int RefreshChildCount(int id)
        {
            if (!string.IsNullOrEmpty(ConditionSet.GetDialogue(ConditionSet.Child_Id + 1)))
            {
                ConditionSet.Child_Id++;
                return 0;
            }
            else
            {
                return 100;
            }
                
        }
    }
}
