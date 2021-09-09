

using Manager.Events.Type;

public class DependentObject : IActiveObject
{
    public RegionEnum Region { get; set; }
    public InteractableObjectTypeEnum ObjectType { get; set; }
    public SubObjectTypeEnum SubObjectType { get; set; }
    public ConditionEnum ActionType { get; set; }
    public string Dialogue { get; set; }
    public EndPhaseDel RecordProgress { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void ConfirmAction(ConditionEnum _action, EndActionData endActionData)
    {
        throw new System.NotImplementedException();
    }
}
