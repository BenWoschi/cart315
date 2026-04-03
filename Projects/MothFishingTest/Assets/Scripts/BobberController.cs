using UnityEngine;

public class BobberController : MonoBehaviour
{
    public Rigidbody rb;

    public void Launch(Vector3 direction, float force)
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }

    public void Stop()
    {
        if (rb != null)
            rb.isKinematic = true;
    }
}