using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerSlop : MonoBehaviour
{
    [SerializeField] private Rigidbody slop;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material[] possibleMaterials;

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

        bool hitBlock = collision.gameObject.TryGetComponent(out TowerSlop _);
        bool hitGround = collision.gameObject.TryGetComponent(out Ground _);

        if ((hitBlock || hitGround) && slop.transform.parent == null)
        {
            hasLanded = true;

            BlockLandedEvent blockEvent = new BlockLandedEvent();
            blockEvent.groundLand = hitGround;

            _eventBus.Publish(blockEvent);
        }
    }
}
