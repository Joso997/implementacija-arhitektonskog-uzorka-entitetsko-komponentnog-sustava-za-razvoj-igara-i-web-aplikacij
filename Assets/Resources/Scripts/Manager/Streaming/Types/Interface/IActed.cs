
using Manager.Events.Type;

public interface IActed<ObjectType>
{
    bool Act(ObjectType _object, StackDataDel action = null);
}
