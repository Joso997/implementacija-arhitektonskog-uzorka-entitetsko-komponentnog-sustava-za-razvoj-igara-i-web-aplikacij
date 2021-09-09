using Manager.Events.Type;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DefaultObject : ObjectTemplate, IActiveObject
{
   
    void Awake()
    {
        Region = (RegionEnum)Region_index;
        ObjectType = (InteractableObjectTypeEnum)InteractableObjectType_index;
        SubObjectType = (SubObjectTypeEnum)SubObjectType_index;
        ActionType = (ConditionEnum)Action_index;
        Condition.Subscribe(this.SetEndingCondition, Region, ObjectType, SubObjectType);
    }


    void SetEndingCondition(TaskLoadedEventArgs e)
    {
        EndingCondition = e.ConditionEnum;
        EndPhase = e.EndPhase;
        ActionType = e.ConditionEnum;
        Dialogue = e.Dialogue;
        ObjectTypes.InteractableObjectTypes[this.ObjectType].ChooseSubType(this);
    }
}

