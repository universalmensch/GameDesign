using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 _offset;
    
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

        if (moveDir.sqrMagnitude > 0.1f)
        {
            Quaternion targetYRotation = Quaternion.LookRotation(moveDir.normalized, Vector3.up);
            
            Quaternion fixedTilt = Quaternion.Euler(50.0f, targetYRotation.eulerAngles.y, 0);

            // Weiches Nachdrehen
            transform.rotation = Quaternion.Lerp(transform.rotation, fixedTilt, 5.0f * Time.deltaTime);
        }

        Quaternion yRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // 3rd Person Position
        transform.position = player.transform.position + yRotation * _offset;
    }
}
