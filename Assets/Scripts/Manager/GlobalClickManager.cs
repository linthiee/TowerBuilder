using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalClickManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}