using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using System.Collections.Generic;

namespace Manager.Streaming.Type
{
    public sealed partial class Action
    {
        public sealed partial class ActionInterface
        {
            public abstract class Default : MethodTypeAbstract, IActed<IActiveObject>, ISubController<ActedDefaultDel>, ICheckAnswer<bool, InteractableObject>
            {
                protected event ActedDefaultDel ActedDefaultEvent;

                public virtual bool Act(IActiveObject _object, StackDataDel action = null)
                {
                    throw new System.NotImplementedException();
                }

                public virtual bool Check(InteractableObject _object)
                {
                    throw new System.NotImplementedException();
                }

                public void SubscribeController(ActedDefaultDel sender)
                {               
                    ActedDefaultEvent += sender;
                }
                public void UnSubscribeController(ActedDefaultDel sender)
                {
                    ActedDefaultEvent -= sender;
                }
                protected void InvokeController(IActiveObject enact, ComputerConditionSetEventArgs.setUIDelegate chooseUISequence)
                {
                    ActedDefaultEvent.Invoke(new ComputerConditionSetEventArgs(enact, chooseUISequence));
                }

            }

            public abstract class Assingment : MethodTypeAbstract, IActed<IActiveObject>, ISubController<ActedDefaultDel>, ICheckAnswer<int, ComputerConditionSetEventArgs>
            {
                protected event ActedDefaultDel ActedDefaultEvent;
                public abstract List<int> GenerateSolution(Dictionary<string, int> dictionary, OperationEnum operation);
                protected abstract Dictionary<string, int> GenerateAssignment();
                protected abstract void ChooseUISequence(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent);
                protected abstract void ChooseUIButton(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent);
                protected abstract SinglyList<EventPhase> EndEventPhaseData();
                public void SubscribeController(ActedDefaultDel sender)
                {
                    ActedDefaultEvent += sender;
                }
                public void UnSubscribeController(ActedDefaultDel sender)
                {
                    ActedDefaultEvent -= sender;
                }
                protected void InvokeController(IActiveObject enact, Dictionary<string, int> assingment, List<int> answers, OperationEnum operation, SinglyList<EventPhase> endActionData = null, bool equal = true, int numberOfDigits = 3)
                {
                    ActedDefaultEvent.Invoke(new ComputerConditionSetEventArgs(ChooseUIMethod, enact, assingment, answers, operation, equal, numberOfDigits, endActionData));
                }
                protected void ChooseUIMethod(UnityEngine.Transform screenUI, ComputerConditionSetEventArgs computerConditionSetEvent, int method_Id)
                {
                    switch (method_Id)
                    {
                        case 0:
                            ChooseUISequence(screenUI, computerConditionSetEvent);
                            break;
                        case 1:
                            ChooseUIButton(screenUI, computerConditionSetEvent);
                            break;
                        default:
                            break;
                    }
                }
                protected int Calculate(int x, int y, OperationEnum operation)
                {
                    //UnityEngine.Debug.Log(x.ToString() + OperationToString(operation) + y.ToString() );
                    int rez = 0;
                    switch (operation)
                    {
                        case OperationEnum.Plus:
                            rez = x + y;
                            break;
                        case OperationEnum.Minus:
                            rez = x - y;
                            break;
                        case OperationEnum.Multiply:
                            rez = x * y;
                            break;
                            /*case OperationEnum.Divide:
                                rez = x / y;
                                break;*/
                    }
                    return rez;
                }
                protected string OperationToString(OperationEnum operation)
                {
                    switch (operation)
                    {
                        case OperationEnum.Plus:
                            return "+";
                        case OperationEnum.Minus:
                            return "-";
                        case OperationEnum.Multiply:
                            return "*";
                        /*case OperationEnum.Divide:
                            return ":";*/
                        default:
                            return "Error";
                    }
                }

                public virtual int Check(ComputerConditionSetEventArgs _object)
                {
                    throw new System.NotImplementedException();
                }

                public virtual bool Act(IActiveObject _object, StackDataDel action = null)
                {
                    throw new System.NotImplementedException();
                }
            }

            public abstract class Interactable : MethodTypeAbstract, IActed<InteractableObject>, ISubController<ActedInteractableDel>, ICheckAnswer<bool, InteractableObject>
            {
                protected event ActedInteractableDel ActedInteractableDel;
                public void SubscribeController(ActedInteractableDel sender)
                {
                    ActedInteractableDel += sender;
                }
                public void UnSubscribeController(ActedInteractableDel sender)
                {
                    ActedInteractableDel -= sender;
                }
                public sealed override void InvokeController(IActiveObject enact)
                {
                    try
                    {
                        ActedInteractableDel.Invoke(new ActedUponEventArgs((InteractableObject)enact));
                    }
                    catch (System.NullReferenceException e)
                    {
                        UnityEngine.Debug.LogError(e);
                        UnityEngine.Debug.LogError("Action is probably not subscribed in Tracker, info about the object: "+enact.Region +" -> "+ enact.ObjectType +" -> "+ enact.SubObjectType +" -> "+ enact.ActionType);
                        
                    }
                    
                }
                public virtual bool Act(InteractableObject _object, StackDataDel action = null)
                {
                    _object.InvokeEventHolder.Pop().Invoke(new StackData(_object));
                    _object.InvokeEventHolder.Push(Enact);
                    return true;
                }
                public void EndAct(Stack<StackDataDel> stack)
                {
                    stack.Pop().Invoke(new StackData());
                }

                public virtual bool Check(InteractableObject _object)
                {
                    throw new System.NotImplementedException();
                }

                
            }
        }
    }

}
