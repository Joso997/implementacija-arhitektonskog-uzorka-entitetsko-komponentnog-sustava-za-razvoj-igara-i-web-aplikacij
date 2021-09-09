using System.Collections;
using System.Collections.Generic;

namespace Manager.Events.Type
{
    public enum SubObjectTypeEnum { ParentObject, DummyParentObject, LeftButton, RightButton, MiddleButton, Indicator };
    public abstract class SubObjectTypeAbstract
    {
        public virtual bool ChooseAction(InteractableObject @object, StackDataDel invokeLogic)
        {
            return ((IActed<InteractableObject>)Condition.Conditions[@object.ActionType]).Act(@object);
        }

        public bool ChooseAction(IActiveObject @object)
        {
            return ((IActed<IActiveObject>)Condition.Conditions[@object.ActionType]).Act(@object);
        }
        public virtual StatChangeDel Subscribe(ConditionDel conditionDel, System.Action<System.Delegate> SubToConditions, StatChangeDel damageDel = null)
        {
            return null;
            //throw new System.Exception();
        }
    }
    public sealed partial class ObjectTypes
    {
        public static Dictionary<SubObjectTypeEnum, SubObjectTypeAbstract> SubObjectTypes { get; private set; } = new Dictionary<SubObjectTypeEnum, SubObjectTypeAbstract>()
            {
                {SubObjectTypeEnum.ParentObject, new ParentObject()},
                {SubObjectTypeEnum.DummyParentObject, new DummyParentObject()},
                {SubObjectTypeEnum.LeftButton, new LeftButton()},
                {SubObjectTypeEnum.MiddleButton, new MiddleButton()},
                {SubObjectTypeEnum.RightButton, new RightButton()},
                {SubObjectTypeEnum.Indicator, new Indicator()}
            };

        public static void InitializeSubObjectTypes()
        {
            SubObjectTypes = new Dictionary<SubObjectTypeEnum, SubObjectTypeAbstract>()
            {
                {SubObjectTypeEnum.ParentObject, new ParentObject()},
                {SubObjectTypeEnum.DummyParentObject, new DummyParentObject()},
                {SubObjectTypeEnum.LeftButton, new LeftButton()},
                {SubObjectTypeEnum.MiddleButton, new MiddleButton()},
                {SubObjectTypeEnum.RightButton, new RightButton()},
                {SubObjectTypeEnum.Indicator, new Indicator()}
            };
        }

        public sealed class ParentObject : SubObjectTypeAbstract
        {
            public override StatChangeDel Subscribe(ConditionDel conditionDel, System.Action<System.Delegate> SubToConditions, StatChangeDel damageDel = null)
            {
                SubToConditions(conditionDel);
                return damageDel;
            }
        }

        public sealed class DummyParentObject : SubObjectTypeAbstract
        {
            public override StatChangeDel Subscribe(ConditionDel conditionDel, System.Action<System.Delegate> SubToConditions, StatChangeDel damageDel = null)
            {
                return damageDel;
            }
            public override bool ChooseAction(InteractableObject @object, StackDataDel invokeLogic)
            {
                return ((IActed<InteractableObject>)Condition.Conditions[@object.ActionType]).Act(@object, invokeLogic);
            }
        }

        public sealed class LeftButton : SubObjectTypeAbstract
        {
            public override bool ChooseAction(InteractableObject @object, StackDataDel invokeLogic)
            {
                return ((IActed<InteractableObject>)Condition.Conditions[@object.ActionType]).Act(@object, invokeLogic);
            }
        }

        public sealed class MiddleButton : SubObjectTypeAbstract
        {
            public override bool ChooseAction(InteractableObject @object, StackDataDel invokeLogic)
            {
                return ((IActed<InteractableObject>)Condition.Conditions[@object.ActionType]).Act(@object, invokeLogic);
            }
        }

        public sealed class RightButton : SubObjectTypeAbstract
        {
            public override bool ChooseAction(InteractableObject @object, StackDataDel invokeLogic)
            {
                return ((IActed<InteractableObject>)Condition.Conditions[@object.ActionType]).Act(@object, invokeLogic);
            }
        }

        public sealed class Indicator : SubObjectTypeAbstract
        {
            public override bool ChooseAction(InteractableObject @object, StackDataDel invokeLogic)
            {
                return base.ChooseAction(@object, invokeLogic);//
            }
        }
    }
}

