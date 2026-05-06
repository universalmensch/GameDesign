using UnityEngine;

namespace Controller.Pickup
{
    public class PickupController : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }
    }
}
