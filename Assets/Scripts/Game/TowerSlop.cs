using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerSlop : MonoBehaviour
{
    [SerializeField] private Rigidbody slop;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material[] possibleMaterials;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip blockDropped;
    [SerializeField] private AudioClip groundCollision;
    [SerializeField] private AudioClip onSlopeCollision;

    [SerializeField] private float snapTolerance = 0.5f;

    private static float sharedPitch = 1.0f;

    private bool hasLanded = false;
    private IEventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        slop.useGravity = false;
        slop.isKinematic = false;

        if (possibleMaterials.Length > 0 && meshRenderer != null)
        {
            int randomIdx = Random.Range(0, possibleMaterials.Length);

            Debug.Log($"{randomIdx}");

            meshRenderer.material = possibleMaterials[randomIdx];
        }
    }
    private void Update()
    {
        if (slop.transform.parent != null)
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                audioSource.PlayOneShot(blockDropped);

                slop.useGravity = true;
                slop.transform.parent = null;

                slop.isKinematic = false;

                slop.linearVelocity = Vector3.zero;
                slop.angularVelocity = Vector3.zero;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded)
            return;

        bool hitBlock = collision.gameObject.TryGetComponent(out TowerSlop collidedBlock);
        bool hitGround = collision.gameObject.TryGetComponent(out Ground _);

        if ((hitBlock || hitGround) && slop.transform.parent == null)
        {
            if (hitBlock)
            {
                float distanceX = Mathf.Abs(transform.position.x - collidedBlock.transform.position.x);

                if (distanceX <= snapTolerance)
                {
                    Vector3 perfectPos = transform.position;
                    perfectPos.x = collidedBlock.transform.position.x;
                    transform.position = perfectPos;

                    slop.linearVelocity = Vector3.zero;
                    slop.angularVelocity = Vector3.zero;

                    transform.rotation = collidedBlock.transform.rotation;

                    Debug.Log("perfect snap!");

                    sharedPitch += 0.05f;
                    audioSource.pitch = sharedPitch;
                    audioSource.PlayOneShot(groundCollision);

                    PerfectLandEvent perfectLandEvent = new PerfectLandEvent();
                    _eventBus.Publish(perfectLandEvent);
                }
                else
                {
                    audioSource.pitch = 1.0f;
                    audioSource.PlayOneShot(groundCollision);
                }
            }
            else
            {
                audioSource.PlayOneShot(onSlopeCollision);
            }

            hasLanded = true;

            BlockLandedEvent blockEvent = new BlockLandedEvent();
            blockEvent.groundLand = hitGround;
            _eventBus.Publish(blockEvent);
        }
    }
}
