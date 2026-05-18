using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerSlop : MonoBehaviour
{
    [SerializeField] private BlockSettingsSO settings;

    [Header("Materials")]
    public Material[] possibleMaterials;

    [Header("Components")]
    [SerializeField] private Rigidbody slop;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private AudioSource audioSource;

    [Header("State variables")]
    private static int towerHeight = 0;
    private static float sharedPitch = 1.0f;
    private static bool isGameOver = false;

    [Header("Collision Info")]
    private bool hasLanded = false;
    private bool isBaseBlock = false;

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

                audioSource.PlayOneShot(settings.blockDropped);

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
        if (isGameOver)
            return;

        bool hitBlock = collision.gameObject.TryGetComponent(out TowerSlop collidedBlock);
        bool hitGround = collision.gameObject.TryGetComponent(out Ground _);

        if (hitGround && towerHeight >= 1 && !isBaseBlock)
        {
            isGameOver = true;
            _eventBus.Publish(new TowerFallEvent());
            return;
        }

        if (hasLanded)
            return;
        
        bool perfectSnap = false;

        if ((hitBlock || hitGround) && slop.transform.parent == null)
        {
            if (hitGround && towerHeight == 0)
            {
                isBaseBlock = true;
            }

            if (hitBlock)
            {
                float distanceX = Mathf.Abs(transform.position.x - collidedBlock.transform.position.x);

                if (distanceX <= settings.snapTolerance)
                {
                    Vector3 perfectPos = transform.position;
                    perfectPos.x = collidedBlock.transform.position.x;
                    transform.position = perfectPos;

                    slop.linearVelocity = Vector3.zero;
                    slop.angularVelocity = Vector3.zero;

                    transform.rotation = collidedBlock.transform.rotation;

                    Debug.Log("perfect snap!");

                    sharedPitch += 0.15f;
                    audioSource.pitch = sharedPitch;
                    audioSource.PlayOneShot(settings.groundCollision);

                    PerfectLandEvent perfectLandEvent = new PerfectLandEvent();
                    perfectLandEvent.points = 250;

                    perfectSnap = true;

                    _eventBus.Publish(perfectLandEvent);
                }
                else
                {
                    audioSource.pitch = 1.0f;
                    audioSource.PlayOneShot(settings.groundCollision);
                }
            }
            else
            {
                audioSource.PlayOneShot(settings.onSlopeCollision);
            }

            hasLanded = true;

            BlockLandedEvent blockEvent = new BlockLandedEvent();

            blockEvent.groundLand = hitGround;
            blockEvent.blockLand = !perfectSnap;

            blockEvent.points = 100;

            towerHeight++;

            _eventBus.Publish(blockEvent);
        }
    }

    public static void ResetTowerState()
    {
        towerHeight = 0;
        sharedPitch = 1.0f;
        isGameOver = false;
    }
}
