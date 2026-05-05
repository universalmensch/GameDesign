
public class PlayerControllerC : PlayerController
{
    protected override void Start()
    {
        base.Start();
        ControlledPlayer = Player.PlayerC;
    }
}
