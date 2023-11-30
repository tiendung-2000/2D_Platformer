public interface IStatePlayer
{
    void OnEnter(PlayerController player);
    void OnUpdate(PlayerController player);
    void OnExit(PlayerController player);
}
