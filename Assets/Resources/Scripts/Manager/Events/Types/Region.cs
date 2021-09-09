

using System.Collections.Generic;

namespace Manager.Events.Type
{
    public enum RegionEnum { InsideRocket };
    public abstract class RegionAbstract
    {
        protected abstract Dictionary<object, object> SetObjects();
        public abstract Dictionary<InteractableObjectTypeEnum, InteractableObjectTypeAbstract> Objects { get; protected set; }

        internal abstract void Subscribe(InteractableObjectTypeEnum objectType, SubObjectTypeEnum subObjectType, ConditionDel conditionDel, StatChangeDel damageDel);
    }

    public sealed partial class ObjectTypes
    {
        public static Dictionary<RegionEnum, RegionAbstract> RegionTypes { get; private set; } = new Dictionary<RegionEnum, RegionAbstract>()
            {
                {RegionEnum.InsideRocket, new InsideRocket()}
            };

        public static void InitializeRegionTypes()
        {
            RegionTypes = new Dictionary<RegionEnum, RegionAbstract>()
            {
                {RegionEnum.InsideRocket, new InsideRocket()}
            };
        }

        

        public sealed class InsideRocket : RegionAbstract
        {
            protected override Dictionary<object, object> SetObjects()
            {
                return new Dictionary<object, object>(){
                    { System.Enum.GetName(typeof(InteractableObjectTypeEnum), InteractableObjectTypeEnum.Computer),InteractableObjectTypeEnum.Computer },
                    { System.Enum.GetName(typeof(InteractableObjectTypeEnum), InteractableObjectTypeEnum.Stasis),InteractableObjectTypeEnum.Stasis },
                    { System.Enum.GetName(typeof(InteractableObjectTypeEnum), InteractableObjectTypeEnum.Gas),InteractableObjectTypeEnum.Gas },
                    { System.Enum.GetName(typeof(InteractableObjectTypeEnum), InteractableObjectTypeEnum.Charger),InteractableObjectTypeEnum.Charger },
                    { System.Enum.GetName(typeof(InteractableObjectTypeEnum), InteractableObjectTypeEnum.Clock),InteractableObjectTypeEnum.Clock },
                    { System.Enum.GetName(typeof(InteractableObjectTypeEnum), InteractableObjectTypeEnum.Reactor),InteractableObjectTypeEnum.Reactor }
                };
            }

            internal override void Subscribe(InteractableObjectTypeEnum objectType, SubObjectTypeEnum subObjectType, ConditionDel conditionDel, StatChangeDel damageDel)
            {
                Objects[objectType].Subscribe(subObjectType, conditionDel, damageDel);
            }

            //Not meant to be used this way, but since Regions were added later and this game uses only one scene it is fine.
            public override Dictionary<InteractableObjectTypeEnum, InteractableObjectTypeAbstract> Objects { get; protected set; } = new Dictionary<InteractableObjectTypeEnum, InteractableObjectTypeAbstract>()
            {
                {InteractableObjectTypeEnum.Computer, InteractableObjectTypes[InteractableObjectTypeEnum.Computer]},
                {InteractableObjectTypeEnum.Stasis, InteractableObjectTypes[InteractableObjectTypeEnum.Stasis]},
                {InteractableObjectTypeEnum.Gas, InteractableObjectTypes[InteractableObjectTypeEnum.Gas]},
                {InteractableObjectTypeEnum.Charger, InteractableObjectTypes[InteractableObjectTypeEnum.Charger]},
                {InteractableObjectTypeEnum.Clock, InteractableObjectTypes[InteractableObjectTypeEnum.Clock]},
                {InteractableObjectTypeEnum.Reactor, InteractableObjectTypes[InteractableObjectTypeEnum.Reactor]}
            };
        }

        /*public sealed class InsideRocketExtra : RegionAbstract
        {
            protected override Dictionary<object, object> SetObjects()
            {
                return new Dictionary<object, object>(){
                    { System.Enum.GetName(typeof(InteractableObjectTypeEnum), InteractableObjectTypeEnum.Reactor),InteractableObjectTypeEnum.Reactor }
                };
            }

            internal override void Subscribe(InteractableObjectTypeEnum objectType, SubObjectTypeEnum subObjectType, ConditionDel conditionDel, StatChangeDel damageDel)
            {
                Objects[objectType].Subscribe(subObjectType, conditionDel, damageDel);
            }

            //Not meant to be used this way, but since Regions were added later and this game uses only one scene it is fine.
            public override Dictionary<InteractableObjectTypeEnum, InteractableObjectTypeAbstract> Objects { get; protected set; } = new Dictionary<InteractableObjectTypeEnum, InteractableObjectTypeAbstract>()
            {
                {InteractableObjectTypeEnum.Reactor, InteractableObjectTypes[InteractableObjectTypeEnum.Reactor]}
            };
        }*/

    }
}