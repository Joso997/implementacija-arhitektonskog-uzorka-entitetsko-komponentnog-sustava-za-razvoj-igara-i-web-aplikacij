
using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using Manager.Mechanics.Interface;
using Manager.UI;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Manager.Mechanics.TimeGame
{
    public class TimeGameMechanicController : MechanicInterface.HasUIController<TimeUI>
    {
        private InteractableObject InteractableObject;
        private System.Func<bool, EndActionData, bool> DefaultObjectFunc;
        public TimeGameMechanicController(TimeUI _uIContoller) : base(_uIContoller) { }
        protected override void SubscribeConditions()
        {
            (Condition.Conditions[ConditionEnum.Time] as ISubController<ActedDefaultDel>).SubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Vent] as ISubController<ActedInteractableDel>).SubscribeController(TerminateUI);
            (Condition.Conditions[ConditionEnum.Avoid] as ISubController<ActedDefaultDel>).SubscribeController(SetEndingCondtion);
            (Condition.Conditions[ConditionEnum.Course] as ISubController<ActedDefaultDel>).SubscribeController(SetEndingCondtion);
            (Condition.Conditions[ConditionEnum.Dock] as ISubController<ActedDefaultDel>).SubscribeController(SetEndingCondtion);
            (Condition.Conditions[ConditionEnum.Hack] as ISubController<ActedDefaultDel>).SubscribeController(SetEndingCondtion);
        }
        public override void Stop()
        {
            (Condition.Conditions[ConditionEnum.Time] as ISubController<ActedDefaultDel>).UnSubscribeController(ActivateUI);
            (Condition.Conditions[ConditionEnum.Vent] as ISubController<ActedInteractableDel>).UnSubscribeController(TerminateUI);
            (Condition.Conditions[ConditionEnum.Avoid] as ISubController<ActedDefaultDel>).UnSubscribeController(SetEndingCondtion);
            (Condition.Conditions[ConditionEnum.Course] as ISubController<ActedDefaultDel>).UnSubscribeController(SetEndingCondtion);
            (Condition.Conditions[ConditionEnum.Dock] as ISubController<ActedDefaultDel>).UnSubscribeController(SetEndingCondtion);
            (Condition.Conditions[ConditionEnum.Hack] as ISubController<ActedDefaultDel>).UnSubscribeController(SetEndingCondtion);
            base.Stop();
        }
        protected override void ActivateUI(ComputerConditionSetEventArgs e)
        {  
            if(ConditionSet == null)
            {
                ConditionSet = e;
                ConditionSet.UIContoller = UIContoller;
                UIScreens = ConditionSet.GetUIScreen();
                UIContoller.StartUI(UIScreens, new ExtraData(ConditionSet.GetConditionDefinition, CheckAnswers, RefreshChildCount), ConditionSet.Child_Id, 1);
            }        
        }

        void SetEndingCondtion(ComputerConditionSetEventArgs e)
        {
            DefaultObjectFunc = e.CheckAnswers;
        }

        void TerminateUI(ActedUponEventArgs e)
        {
            InteractableObject = e.InteractableObject;
            ObjectTypes.InteractableObjectTypes[e.InteractableObject.ObjectType].SubscribeLogic(Button);
        }

        protected override void Button(SubObjectTypeEnum button_id)
        {
            switch (button_id)
            {
                case SubObjectTypeEnum.MiddleButton:  
                    if (InteractableObject.Stats[InteractableObjectStatEnum.EnergyConsumption].Points > 0)
                    {
                        ConditionSet = null;
                        UIContoller.StopUI(UIScreens, 0);
                        ObjectTypes.InteractableObjectTypes[InteractableObject.ObjectType].NullifyLogic();
                    }   
                    break;
                default:
                    break;
            }
        }

        public bool CheckAnswers(bool skip = false, EndActionData endActionData = new EndActionData())
        {
            ConditionSet = null;           
            if (InteractableObject != null)
            {
                ObjectTypes.InteractableObjectTypes[InteractableObject.ObjectType].NullifyLogic();
                UnityEngine.Debug.Log("Yes");
                var eventPhase = new SinglyList<EventPhase>();
                var consequences = new List<Consequence>() { new Consequence() {
                    Code = 0, CodeEventPhase = -1,
                    Probability = Tracking.ProbabilityEnum.Unavoidable,
                    DamageType = DamageTypeEnum.ReactorSystems,
                    ObjectType = InteractableObjectTypeEnum.Reactor,
                    Region = RegionEnum.InsideRocket,
                    Severity = Tracking.Type.SeverityEnum.Minor}
                };
                eventPhase.EnList(new EventPhase() { Code = -1, Consequences = consequences, EndingCondition = System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.None), RepeatNumber = 1 });
                endActionData.EndEventPhaseData = eventPhase;
                InteractableObject.ConfirmAction(InteractableObject.EndingCondition, endActionData);
            }              
            else
            {               
                DefaultObjectFunc.Invoke(true, endActionData);
            }    
            return true;
        }

        protected override int RefreshChildCount(int id)
        {
            id--;
            ConditionSet.Child_Id = id;
            return id;
        }
    }
}