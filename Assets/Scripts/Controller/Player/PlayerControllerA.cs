
namespace Controller.Player
{
    public class PlayerControllerA : PlayerController
    {
        protected override void Start()
        {
            base.Start();
            controlledPlayer = Entity.Player.PlayerA;
        }
    }
}
