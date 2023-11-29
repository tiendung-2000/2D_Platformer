public class Shardmite : EnemyProperties
{
    private void Start()
    {
        OnInit();
    }

    void Update()
    {
        PatrolMovement();
    }
}
