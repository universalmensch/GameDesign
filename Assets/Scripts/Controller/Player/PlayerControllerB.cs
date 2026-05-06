
namespace Controller.Player
{
    public class PlayerControllerB : PlayerController
    {
        protected override void Start()
        {
            base.Start();
            ControlledPlayer = Entity.Player.PlayerB;
        }
    }
}
