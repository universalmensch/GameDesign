
public class PlayerControllerA : PlayerController
{
    protected override void Start()
    {
        base.Start();
        ControlledPlayer = Player.PlayerA;
    }
}
