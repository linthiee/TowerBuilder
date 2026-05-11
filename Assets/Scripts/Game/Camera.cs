using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    [SerializeField] private BoxCollider slope;

    private IEventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<BlockLandedEvent>(OnBlockLanded);
    }

    private void OnBlockLanded(BlockLandedEvent eventData)
    {
        if (!eventData.groundLand)
            MoveCamera();
    }

    private void MoveCamera()
    {
        cameraPos.Translate(new Vector3(0.0f, slope.size.y, 0.0f));
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<BlockLandedEvent>(OnBlockLanded);
        }
    }
}
