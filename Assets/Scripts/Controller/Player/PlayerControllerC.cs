
namespace Controller.Player
{
    public class PlayerControllerC : PlayerController
    {
        protected override void Start()
        {
            base.Start();
            ControlledPlayer = Entity.Player.PlayerC;
        }
    }
}
