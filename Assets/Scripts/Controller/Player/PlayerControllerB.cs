
namespace Controller.Player
{
    public class PlayerControllerB : PlayerController
    {
        protected override void Start()
        {
            base.Start();
            controlledPlayer = Entity.Player.PlayerB;
        }
    }
}
