using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CyberTale.Collections;
using Manager.Streaming.Type;
using Manager.UI;

namespace Manager.Events.Type
{
    //IMPORTANT:: DON'T FORGET TO ADD/CHANGE EVENTS IN Action() CLASS
    public enum ConditionEnum { None, Refuel, Hack, Maintain, Dock, Avoid, Repair, Course, Eject, Charge, Dialogue, Vent, Time, Choose, Animation, RepairChamber, RepairGas};
    public enum OperationEnum { Plus, Minus, Multiply };
    public delegate void EndPhaseDel(EndActionData endActionData);
    public delegate void ActedDefaultDel(ComputerConditionSetEventArgs e);
    public delegate void ActedInteractableDel(ActedUponEventArgs e);
    public delegate void ConditionDel(TaskLoadedEventArgs e);
    public delegate void StatChangeDel(StatChangeEventArgs e);
    public delegate void StackDataDel(StackData stackData);
    public class Condition
    {
        public static Dictionary<ConditionEnum, MethodTypeAbstract> Conditions { get; private set; } = new Dictionary<ConditionEnum, MethodTypeAbstract>() {
            {ConditionEnum.None, new Action.None()},
            {ConditionEnum.Refuel, new Action.Refuel()},
            {ConditionEnum.Hack, new Action.Hack()},
            {ConditionEnum.Maintain, new Action.Maintain()},
            {ConditionEnum.Dock, new Action.Dock()},
            {ConditionEnum.Avoid, new Action.Avoid()},
            {ConditionEnum.Repair, new Action.Repair()},
            {ConditionEnum.Course, new Action.Course()},
            {ConditionEnum.Eject, new Action.Eject()},
            {ConditionEnum.Charge, new Action.Charge()},
            {ConditionEnum.Dialogue, new Action.Dialogue()},
            {ConditionEnum.Vent, new Action.Vent()},
            {ConditionEnum.Time, new Action.Time()},
            {ConditionEnum.Choose, new Action.Choose()},
            {ConditionEnum.Animation, new Action.Animation()},
            {ConditionEnum.RepairChamber, new Action.RepairChamber()},
            {ConditionEnum.RepairGas, new Action.RepairGas()}
            };

        /*public static void Subscribe(ConditionDel conditionDel, InteractableObjectTypeEnum objectType, SubObjectTypeEnum subObjectType, StatChangeDel damageDel = null)
        {
            ObjectTypes.InteractableObjectTypes[objectType].Subscribe(subObjectType, conditionDel, damageDel);
        }*/

        public static void Subscribe(ConditionDel conditionDel, RegionEnum regionType, InteractableObjectTypeEnum objectType, SubObjectTypeEnum subObjectType, StatChangeDel damageDel = null)
        {
            ObjectTypes.RegionTypes[regionType].Subscribe(objectType, subObjectType, conditionDel, damageDel);
        }

        public static void Initialize()
        {
            MakeNull();
            Conditions = new Dictionary<ConditionEnum, MethodTypeAbstract>() {
            {ConditionEnum.None, new Action.None()},
            {ConditionEnum.Refuel, new Action.Refuel()},
            {ConditionEnum.Hack, new Action.Hack()},
            {ConditionEnum.Maintain, new Action.Maintain()},
            {ConditionEnum.Dock, new Action.Dock()},
            {ConditionEnum.Avoid, new Action.Avoid()},
            {ConditionEnum.Repair, new Action.Repair()},
            {ConditionEnum.Course, new Action.Course()},
            {ConditionEnum.Eject, new Action.Eject()},
            {ConditionEnum.Charge, new Action.Charge()},
            {ConditionEnum.Dialogue, new Action.Dialogue()},
            {ConditionEnum.Vent, new Action.Vent()},
            {ConditionEnum.Time, new Action.Time()},
            {ConditionEnum.Choose, new Action.Choose()},
            {ConditionEnum.Animation, new Action.Animation()},
            {ConditionEnum.RepairChamber, new Action.RepairChamber()},
            {ConditionEnum.RepairGas, new Action.RepairGas()}
            };
        }

        public static void MakeNull()
        {
            Conditions = null;
        }

    }

    public struct EndActionData
    {
        public SinglyList<EventPhase> EndEventPhaseData;
        public bool ConditionFailed;
    }

    public struct StackData
    {
        public readonly StackDataDel stackDataDel;
        public readonly InteractableObject InteractableObject;
        public readonly PlayerStats PlayerStats;

        public StackData(InteractableObject _interactableObject) : this()
        {
            this.InteractableObject = _interactableObject;
        }

        public StackData(InteractableObject _interactableObject, PlayerStats _playerStats) : this(_interactableObject)
        {
            this.PlayerStats = _playerStats;
        }
    } 

    
}
