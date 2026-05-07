using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSlop : MonoBehaviour
{
    [SerializeField] private Rigidbody slop;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            slop.useGravity = true;
            slop.transform.parent = null;
        }
    }
}
