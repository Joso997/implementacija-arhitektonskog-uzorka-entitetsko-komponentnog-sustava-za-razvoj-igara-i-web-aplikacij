

namespace Manager.Events.Type
{
    public interface IActiveObject
    {
        RegionEnum Region { get; set; }
        InteractableObjectTypeEnum ObjectType { get; set; }
        SubObjectTypeEnum SubObjectType { get; set; }
        ConditionEnum ActionType { get; set; }
        string Dialogue { get; set; }
        void ConfirmAction(ConditionEnum _action, EndActionData endActionData);
        EndPhaseDel RecordProgress { get; set; }
    }
}


