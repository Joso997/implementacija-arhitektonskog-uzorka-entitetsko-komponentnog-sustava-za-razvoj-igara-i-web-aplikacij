
public interface ISubController<ActedDel>
{
    void SubscribeController(ActedDel sender);
    void UnSubscribeController(ActedDel sender);
}
