using System.Collections.Generic;

namespace Manager.Events.Type
{
    public enum InteractableObjectTypeEnum { Reactor, Computer, Stasis, Gas, Charger, Clock };
    public delegate void LogicDelegate(SubObjectTypeEnum subObjectType);
    public abstract class InteractableObjectTypeAbstract
    {
        private event LogicDelegate LogicInvoked;
        protected abstract void SubscribeConditions(System.Delegate sender);
        protected abstract Dictionary<object, object> SetSubComponents();
        protected abstract Dictionary<object, object> SetPossibleActions();
        protected abstract Dictionary<object, object> SetStats();
        public abstract void Subscribe(SubObjectTypeEnum subObjectType, ConditionDel conditionDel, StatChangeDel damageDel = null);
        public virtual void InvokeStatChange(InteractableObjectStatEnum statType, int amount)
        {
            throw new System.NotImplementedException();
        }
        public List<Dictionary<object, object>> GetComponentParts()
        {
            return new List<Dictionary<object, object>>(){SetSubComponents(), SetPossibleActions(), SetStats()};
        }
    
        public virtual bool ChooseSubType(InteractableObject @object)
        {
            return ObjectTypes.SubObjectTypes[@object.SubObjectType].ChooseAction(@object, InvokeLogic);
        }

        public virtual bool ChooseSubType(IActiveObject @object)
        {
            return ObjectTypes.SubObjectTypes[@object.SubObjectType].ChooseAction(@object);
        }

        public void SubscribeLogic(LogicDelegate logicDel)
        {
            LogicInvoked += logicDel;
        }
        public void UnSubscribeLogic(LogicDelegate logicDel)
        {
            LogicInvoked -= logicDel;
        }
        public void NullifyLogic()
        {
            LogicInvoked = null;
        }
        protected void InvokeLogic(StackData stackData)
        {
            LogicInvoked?.Invoke(stackData.InteractableObject.SubObjectType);
            stackData.InteractableObject.InvokeEventHolder.Pop().Invoke(stackData);
        }

    }
    
    public sealed partial class ObjectTypes
    {

        public static Dictionary<InteractableObjectTypeEnum, InteractableObjectTypeAbstract> InteractableObjectTypes { get; private set; } = new Dictionary<InteractableObjectTypeEnum, InteractableObjectTypeAbstract>()
            {
                {InteractableObjectTypeEnum.Reactor, new Reactor()},
                {InteractableObjectTypeEnum.Computer, new Computer()},
                {InteractableObjectTypeEnum.Stasis, new Stasis()},
                {InteractableObjectTypeEnum.Gas, new Gas()},
                {InteractableObjectTypeEnum.Charger, new Charger()},
                {InteractableObjectTypeEnum.Clock, new Clock()}
            };

        public static void InitializeObjectTypes()
        {
            MakeNull();
            InteractableObjectTypes = new Dictionary<InteractableObjectTypeEnum, InteractableObjectTypeAbstract>()
            {
                {InteractableObjectTypeEnum.Reactor, new Reactor()},
                {InteractableObjectTypeEnum.Computer, new Computer()},
                {InteractableObjectTypeEnum.Stasis, new Stasis()},
                {InteractableObjectTypeEnum.Gas, new Gas()},
                {InteractableObjectTypeEnum.Charger, new Charger()},
                {InteractableObjectTypeEnum.Clock, new Clock()}
            };
        }

        public static void MakeNull()
        {
            RegionTypes = null;
            InteractableObjectTypes = null;
            SubObjectTypes = null;
        }

        //REACTOR CLASS DEFINITION
        public sealed class Reactor : InteractableObjectInterface.IChangeStat
        {
            protected override void SubscribeConditions(System.Delegate sender)
            {
                Condition.Conditions[ConditionEnum.Refuel].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.Maintain].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.Repair].Subscribe(sender as ConditionDel);

            }

            protected override Dictionary<object, object> SetSubComponents()
            {
                return new Dictionary<object, object>(){
                    { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.ParentObject),SubObjectTypeEnum.ParentObject },
                    { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.LeftButton),SubObjectTypeEnum.LeftButton },
                    { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.RightButton),SubObjectTypeEnum.RightButton },
                    { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.MiddleButton),SubObjectTypeEnum.MiddleButton }

                };
            }

            protected override Dictionary<object, object> SetPossibleActions()
            {
                return new Dictionary<object, object>()
                    {
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.Repair),ConditionEnum.Repair },
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.Refuel),ConditionEnum.Refuel },
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.Maintain),ConditionEnum.Maintain }
                    };
            }

            protected override Dictionary<object, object> SetStats()
            {
                return new Dictionary<object, object>() {
                    {InteractableObjectStatEnum.Health, new ObjectHealth()},
                    {InteractableObjectStatEnum.EnergyGeneration, new ObjectEnergyGeneration()},
                    {InteractableObjectStatEnum.Fuel, new ObjectFuel()},
                    {InteractableObjectStatEnum.Pressure, new ObjectPressure()},
                    {InteractableObjectStatEnum.EnergyDemand, new ObjectEnergyDemand()}
                };
            }

        }

        //COMPUTER CLASS DEFINITION
        public sealed class Computer : InteractableObjectInterface.Default
        {
            protected override void SubscribeConditions(System.Delegate sender)
            {
                Condition.Conditions[ConditionEnum.Hack].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.Dock].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.Avoid].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.Course].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.Dialogue].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.Animation].Subscribe(sender as ConditionDel);
            }

            protected override Dictionary<object, object> SetSubComponents()
            {
                return new Dictionary<object, object>()
                    {
                        { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.ParentObject),SubObjectTypeEnum.ParentObject},
                        { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.LeftButton),SubObjectTypeEnum.LeftButton},
                        { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.RightButton),SubObjectTypeEnum.RightButton },
                        { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.MiddleButton),SubObjectTypeEnum.MiddleButton }
                    };
            }

            protected override Dictionary<object, object> SetPossibleActions()
            {
                return new Dictionary<object, object>()
                    {
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.None),ConditionEnum.None }
                    };
            }

            protected override Dictionary<object, object> SetStats()
            {
                return new Dictionary<object, object>() {
                        {InteractableObjectStatEnum.EnergyConsumption, new ObjectEnergyGeneration()}
                    };
            }
        }

        //STASIS CLASS DEFINITION
        public sealed class Stasis : InteractableObjectInterface.IChangeStat
        {
            protected override void SubscribeConditions(System.Delegate sender)
            {
                Condition.Conditions[ConditionEnum.RepairChamber].Subscribe(sender as ConditionDel);//TODO Probably will have to make it through mechanic
                Condition.Conditions[ConditionEnum.Eject].Subscribe(sender as ConditionDel);
            }

            protected override Dictionary<object, object> SetSubComponents()
            {
                return new Dictionary<object, object>()
                    {
                        {  System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.MiddleButton),SubObjectTypeEnum.MiddleButton },
                        {  System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.DummyParentObject),SubObjectTypeEnum.DummyParentObject },
                        {  System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.ParentObject),SubObjectTypeEnum.ParentObject },
                        {  System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.Indicator),SubObjectTypeEnum.Indicator }
                    };
            }

            protected override Dictionary<object, object> SetPossibleActions()
            {
                return new Dictionary<object, object>()
                    {
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.RepairChamber),ConditionEnum.RepairChamber },
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.Eject),ConditionEnum.Eject },
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.None),ConditionEnum.None }
                    };
            }

            protected override Dictionary<object, object> SetStats()
            {
                return new Dictionary<object, object>() {
                        {InteractableObjectStatEnum.Health, new ObjectHealth()},
                        {InteractableObjectStatEnum.EnergyConsumption, new ObjectEnergyConsumption()},
                        {InteractableObjectStatEnum.Boost, new ObjectBoost()}
                    };
            }
        }

        public sealed class Gas : InteractableObjectInterface.IChangeStat
        {
            protected override void SubscribeConditions(System.Delegate sender)
            {
                Condition.Conditions[ConditionEnum.Maintain].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.RepairGas].Subscribe(sender as ConditionDel);
                Condition.Conditions[ConditionEnum.Vent].Subscribe(sender as ConditionDel);
            }

            protected override Dictionary<object, object> SetSubComponents()
            {
                return new Dictionary<object, object>(){
                    { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.ParentObject),SubObjectTypeEnum.ParentObject },
                    { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.MiddleButton),SubObjectTypeEnum.MiddleButton }
                };
            }

            protected override Dictionary<object, object> SetPossibleActions()
            {
                return new Dictionary<object, object>()
                    {
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.RepairGas),ConditionEnum.RepairGas },
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.Maintain),ConditionEnum.Maintain },
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.Vent),ConditionEnum.Vent }
                    };
            }

            protected override Dictionary<object, object> SetStats()
            {
                return new Dictionary<object, object>() {
                    {InteractableObjectStatEnum.Health, new ObjectHealth()},
                    {InteractableObjectStatEnum.EnergyConsumption, new ObjectEnergyConsumption()}
                };
            }

        }

        public sealed class Charger : InteractableObjectInterface.IChangeStat
        {
            protected override void SubscribeConditions(System.Delegate sender)
            {
                Condition.Conditions[ConditionEnum.Charge].Subscribe(sender as ConditionDel);
            }

            protected override Dictionary<object, object> SetSubComponents()
            {
                return new Dictionary<object, object>(){
                    { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.ParentObject),SubObjectTypeEnum.ParentObject }
                };
            }

            protected override Dictionary<object, object> SetPossibleActions()
            {
                return new Dictionary<object, object>()
                    {
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.Charge),ConditionEnum.Charge }
                    };
            }

            protected override Dictionary<object, object> SetStats()
            {
                return new Dictionary<object, object>() {
                    {InteractableObjectStatEnum.EnergyConsumption, new ObjectEnergyConsumption()}
                };
            }

        }

        public sealed class Clock : InteractableObjectInterface.Default
        {
            protected override void SubscribeConditions(System.Delegate sender)
            {
                Condition.Conditions[ConditionEnum.Time].Subscribe(sender as ConditionDel);
            }

            protected override Dictionary<object, object> SetSubComponents()
            {
                return new Dictionary<object, object>(){
                    { System.Enum.GetName(typeof(SubObjectTypeEnum), SubObjectTypeEnum.ParentObject),SubObjectTypeEnum.ParentObject }
                };
            }

            protected override Dictionary<object, object> SetPossibleActions()
            {
                return new Dictionary<object, object>()
                    {
                        {  System.Enum.GetName(typeof(ConditionEnum), ConditionEnum.None),ConditionEnum.None }
                    };
            }

            protected override Dictionary<object, object> SetStats()
            {
                return new Dictionary<object, object>();
            }

        }


    }
}

