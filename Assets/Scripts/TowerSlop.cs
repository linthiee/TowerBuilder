using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSlop : MonoBehaviour
{
    [SerializeField] Rigidbody slop;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            slop.useGravity = true;
            slop.transform.parent = null;
        }
    }
}
