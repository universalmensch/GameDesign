using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 _offset;
    
    public float tiltStrength = 10f;
    public float maxTilt = 90f; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        Vector3 moveDir = rb.linearVelocity;

        Quaternion targetYRotation = Quaternion.identity;

        if (moveDir.sqrMagnitude > 0.1f)
        {
            targetYRotation = Quaternion.LookRotation(moveDir.normalized, Vector3.up);
        }

        float targetY = targetYRotation.eulerAngles.y;

        float verticalSpeed = rb.linearVelocity.y;

        float tiltX = Mathf.Clamp(-Mathf.Sign(verticalSpeed) * Mathf.Pow(Mathf.Abs(verticalSpeed), 3f) * tiltStrength, -maxTilt, maxTilt);

        Quaternion targetRotation = Quaternion.Euler(tiltX, targetY, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2.5f * Time.deltaTime);

        transform.position = player.transform.position + transform.rotation * _offset;
    }
}
