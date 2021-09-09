
namespace Manager.Events.Type
{
    public sealed partial class ObjectTypes
    {
        public class InteractableObjectInterface
        {
            public abstract class Default : InteractableObjectTypeAbstract
            {
                
                public sealed override bool ChooseSubType(IActiveObject @object)
                {
                    return SubObjectTypes[@object.SubObjectType].ChooseAction(@object);
                }
                public sealed override void Subscribe(SubObjectTypeEnum subObjectType, ConditionDel conditionDel, StatChangeDel statChangeDel = null)
                {
                    SubObjectTypes[subObjectType].Subscribe(conditionDel, SubscribeConditions);
                }
            }

            public abstract class IChangeStat : InteractableObjectTypeAbstract
            {
                private event StatChangeDel StatChangeEvent;
                public sealed override void InvokeStatChange(InteractableObjectStatEnum statType, int amount)
                {
                    StatChangeEvent.Invoke(new StatChangeEventArgs(statType, amount));
                }
                public sealed override void Subscribe(SubObjectTypeEnum subObjectType, ConditionDel conditionDel, StatChangeDel statChangeDel = null)
                {
                    if(statChangeDel == null)
                        SubObjectTypes[subObjectType].Subscribe(conditionDel, SubscribeConditions, statChangeDel);
                    else
                        StatChangeEvent += SubObjectTypes[subObjectType].Subscribe(conditionDel, SubscribeConditions, statChangeDel);
                }
            }
        }
    }
}


