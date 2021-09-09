using Manager.Events;
using Manager.Events.Type;
using Manager.Tracking;
using Manager.Tracking.Type;

namespace Manager.Streaming.Type
{
    public abstract class MethodTypeAbstract
    {
        protected event ConditionDel CondEvent;
        protected virtual void Enact(StackData stackData)
        {
            throw new System.NotImplementedException();
        }
        public void Subscribe(ConditionDel sender)
        {
            CondEvent += sender;
        }
        public virtual void InvokeController(IActiveObject enact)
        {
            throw new System.NotImplementedException();
        }
        public void InvokeCondition(ConditionEnum conditionDel, EndPhaseDel endPhase, string dialogue)
        {
            CondEvent.Invoke(new TaskLoadedEventArgs(conditionDel, endPhase, dialogue));
        }
    }
    public sealed partial class Action
    {
        public sealed class None : ActionInterface.Interactable
        {
            public override bool Act(InteractableObject _object, StackDataDel action)
            {
                _object.InvokeEventHolder.Pop().Invoke(new StackData(_object));
                _object.InvokeEventHolder.Push(Enact);
                _object.InvokeEventHolder.Push(action);
                return true;
            }

            protected override void Enact(StackData stackData)
            {
                EndAct(stackData.InteractableObject.InvokeEventHolder);
            }
        }

        public sealed class Refuel : ActionInterface.Interactable
        {
            protected override void Enact(StackData stackData)
            {
                if (stackData.PlayerStats.CanRefuel)
                {
                    stackData.InteractableObject.Stats[InteractableObjectStatEnum.Fuel].Points = 100;
                    stackData.InteractableObject.ConfirmAction(ConditionEnum.Refuel, new EndActionData());
                    stackData.PlayerStats.CanRefuel = false;
                }
                
                EndAct(stackData.InteractableObject.InvokeEventHolder);
            }

            public override bool Check(InteractableObject _object)
            {
                if (_object.Stats[InteractableObjectStatEnum.Fuel].Points >= 75)
                    return true;
                else
                    return false;
            }
        }

        public sealed class Maintain : ActionInterface.Interactable
        {
            protected override void Enact(StackData stackData)
            {
                int severityEnum;
                var reactorStats = stackData.InteractableObject.ParentObject.Stats;
                switch (stackData.InteractableObject.SubObjectType)
                {
                    case SubObjectTypeEnum.LeftButton:
                        if (reactorStats[InteractableObjectStatEnum.EnergyGeneration].Points == 0)
                            severityEnum = reactorStats[InteractableObjectStatEnum.EnergyDemand].Points + 50 * (stackData.PlayerStats.CanVent ? 1 : 0);
                        else
                            severityEnum = (int)SeverityEnum.PowerUp;
                        break;
                    case SubObjectTypeEnum.RightButton:
                        severityEnum = (int)SeverityEnum.PowerDown;
                        if (reactorStats[InteractableObjectStatEnum.Pressure].Points - severityEnum < 50 && stackData.PlayerStats.CanVent)
                            severityEnum = 0;
                        break;
                    default:
                        severityEnum = (int)SeverityEnum.Critical;
                        break;
                }
                UnityEngine.Debug.Log("Health: " + reactorStats[InteractableObjectStatEnum.Health].Points);
                UnityEngine.Debug.Log("EnergyGeneration: " + reactorStats[InteractableObjectStatEnum.EnergyGeneration].Points);
                UnityEngine.Debug.Log("Pressure: " + reactorStats[InteractableObjectStatEnum.Pressure].Points);
                ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.EnergyGeneration, severityEnum); 
                UnityEngine.Debug.Log("Health: "+reactorStats[InteractableObjectStatEnum.Health].Points);
                UnityEngine.Debug.Log("EnergyGeneration: " + reactorStats[InteractableObjectStatEnum.EnergyGeneration].Points);
                UnityEngine.Debug.Log("Pressure: " + reactorStats[InteractableObjectStatEnum.Pressure].Points);
                EndAct(stackData.InteractableObject.InvokeEventHolder);
            }

            public override bool Check(InteractableObject _object)
            {
                return false;
            }
        }

        public sealed class Repair : ActionInterface.Interactable
        {
            protected override void Enact(StackData stackData)
            {
                if (stackData.PlayerStats.CanRepair)
                {
                    ObjectTypes.InteractableObjectTypes[stackData.InteractableObject.ObjectType].InvokeStatChange(InteractableObjectStatEnum.Health, 100);
                    stackData.InteractableObject.ConfirmAction(ConditionEnum.Repair, new EndActionData());
                    stackData.PlayerStats.CanRepair = false;
                }
                EndAct(stackData.InteractableObject.InvokeEventHolder);
            }

            public override bool Check(InteractableObject _object)
            {
                if (_object.Stats[InteractableObjectStatEnum.Health].Points >= 75)
                    return true;
                else
                    return false;
            }
        }

        public sealed class Eject : ActionInterface.Default
        {
            public override bool Act(IActiveObject _object, StackDataDel action)
            {
                InvokeController(_object, ChooseUISequence);
                return true;
            }
            public void ChooseUISequence(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent, int method_Id = 0)
            {
                screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.Child_Id.ToString();
            }
        }

        public sealed class Charge : ActionInterface.Interactable
        {
            protected override void Enact(StackData stackData)
            {
                stackData.PlayerStats.CanRepair = true;
                EndAct(stackData.InteractableObject.InvokeEventHolder);
            }
        }

        public sealed class Dialogue : ActionInterface.Default
        {
            public override bool Act(IActiveObject _object, StackDataDel action)
            {
                InvokeController(_object, ChooseUISequence);
                return true;
            }
            public void ChooseUISequence(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent, int method_Id = 0)
            {
                screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = computerConditionSetEvent.GetDialogue(computerConditionSetEvent.Child_Id);
            }
        }

        public sealed class Vent : ActionInterface.Interactable
        {
            public override bool Act(InteractableObject _object, StackDataDel action)
            {
                _object.InvokeEventHolder.Pop().Invoke(new StackData(_object));
                _object.InvokeEventHolder.Push(Enact);
                _object.InvokeEventHolder.Push(action);
                return true;
            }
            protected override void Enact(StackData stackData)
            {
                if (stackData.PlayerStats.CanVent)
                {
                    ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.Pressure, -(int)SeverityEnum.Minor);
                    stackData.PlayerStats.CanVent = false;
                }
                else if (!stackData.PlayerStats.CanVent)
                {
                    ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.Pressure, (int)SeverityEnum.Minor);
                    stackData.PlayerStats.CanVent = true;
                }
                if (stackData.InteractableObject.ParentObject.Stats[InteractableObjectStatEnum.EnergyConsumption].Points > 0)//TODO probably needs to be updated
                    stackData.InteractableObject.ParentObject.ConfirmAction(ConditionEnum.Vent, new EndActionData());
                    
                EndAct(stackData.InteractableObject.InvokeEventHolder);
            }

            public override bool Check(InteractableObject _object)
            {
                return false;
            }
        }
        public sealed class Time : ActionInterface.Default
        {
            public override bool Act(IActiveObject _object, StackDataDel action)
            {
                InvokeController(_object, ChooseUISequence);
                return true;
            }

            public void ChooseUISequence(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent, int method_Id = 0)
            {
                screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = (computerConditionSetEvent.Child_Id).ToString();
            }
        }

        public sealed class Choose : ActionInterface.Interactable
        {
            public override bool Act(InteractableObject _object, StackDataDel action)
            {
                _object.InvokeEventHolder.Pop().Invoke(new StackData(_object));
                _object.SubObjectType = (SubObjectTypeEnum)_object.name.GetHashCode();
                _object.InvokeEventHolder.Push(Enact);
                _object.InvokeEventHolder.Push(action);
                return true;
            }
            protected override void Enact(StackData stackData)
            {
                stackData.InteractableObject.SubObjectType = SubObjectTypeEnum.DummyParentObject;
                EndAct(stackData.InteractableObject.InvokeEventHolder);
            }
        }

        public sealed class Animation : ActionInterface.Default
        {
            public override bool Act(IActiveObject _object, StackDataDel action)
            {
                InvokeController(_object, ChooseUISequence);
                return true;
            }
            public void ChooseUISequence(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent, int method_Id = 0)
            {
                screenUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = (computerConditionSetEvent.Child_Id).ToString();
            }
        }

        public sealed class RepairChamber : ActionInterface.Interactable
        {
            public override bool Act(InteractableObject _object, StackDataDel action)
            {
                _object.InvokeEventHolder.Pop().Invoke(new StackData(_object));
                _object.SubObjectType = (SubObjectTypeEnum)_object.name.GetHashCode();
                _object.InvokeEventHolder.Push(Enact);
                _object.InvokeEventHolder.Push(action);
                return true;
            }
            protected override void Enact(StackData stackData)
            {
                stackData.InteractableObject.SubObjectType = SubObjectTypeEnum.DummyParentObject;
                EndAct(stackData.InteractableObject.InvokeEventHolder);
            }
        }

        public sealed class RepairGas : ActionInterface.Default
        {

        }

    }

}
