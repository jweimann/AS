namespace AS.Interfaces
{
    public interface IMapToClientCommand
    {
        object GetClientCommand(int entityId);
    }
}
