using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSlop : MonoBehaviour
{
    [SerializeField] private Rigidbody slop;

    private void Update()
    {
        if (slop.transform.parent != null)
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            slop.useGravity = true;
            slop.transform.parent = null;

            slop.isKinematic = false;

            slop.linearVelocity = Vector3.zero;
            slop.angularVelocity = Vector3.zero; 
        }
    }
}
