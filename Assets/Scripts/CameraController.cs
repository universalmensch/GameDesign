using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody playerA;
    public Rigidbody playerB;
    public Rigidbody playerC;
    public GameController gameController;
    
    private const float TiltStrength = 10f;
    private const float MaxTilt = 45f;
    private static readonly Vector3 Offset = new(0.0f, 1.0f, -5.0f);

    private void LateUpdate()
    {
        var currentPlayer = gameController.selectedPlayer switch
        {
            Player.PlayerA => playerA,
            Player.PlayerB => playerB,
            Player.PlayerC => playerC,
            _ => playerA
        };

        var movementDirection = currentPlayer.linearVelocity;
        var targetRotation = Quaternion.Euler(GetTargetXRotation(movementDirection), GetTargetYRotation(movementDirection), 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2.5f * Time.deltaTime);
        transform.position = currentPlayer.transform.position + transform.rotation * Offset;
    }

    private static float GetTargetYRotation(Vector3 movementDirection)
    {
        var targetYRotation = Quaternion.identity;

        if (movementDirection.sqrMagnitude > 0.1f)
        {
            targetYRotation = Quaternion.LookRotation(movementDirection.normalized, Vector3.up);
        }

        return targetYRotation.eulerAngles.y;
    }

    private static float GetTargetXRotation(Vector3 movementDirection)
    {
        var verticalSpeed = movementDirection.y;
        return Mathf.Clamp(-Mathf.Sign(verticalSpeed) * Mathf.Pow(Mathf.Abs(verticalSpeed), 3f) * TiltStrength, -MaxTilt, MaxTilt);
    }
}
