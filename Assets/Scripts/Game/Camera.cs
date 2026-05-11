using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    [SerializeField] private BoxCollider slope;

    private IEventBus _eventBus;

    private float shakeDuration = 0f;
    public float shakeTime = 0.3f;

    private float shakeAmount = 0.1f;

    private Vector3 originalPos = Vector3.zero;

    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<BlockLandedEvent>(OnBlockLanded);
        _eventBus.Subscribe<PerfectLandEvent>(OnPerfectLand);
    }

    private void Update()
    {
        if (shakeDuration == 0)
        {
            return;
        }
        else if (shakeDuration > 0)
        {
            cameraPos.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            cameraPos.localPosition = originalPos;
        }
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<BlockLandedEvent>(OnBlockLanded);
            _eventBus.Unsubscribe<PerfectLandEvent>(OnPerfectLand);
        }
    }
    private void OnPerfectLand(PerfectLandEvent @event)
    {
        Debug.Log("camera shake event called");

        shakeDuration = shakeTime;
        originalPos = cameraPos.position;
    }

    private void OnBlockLanded(BlockLandedEvent eventData)
    {
        if (!eventData.groundLand)
            MoveCamera();
    }

    private void MoveCamera()
    {
        originalPos.y += slope.size.y;
        cameraPos.position = originalPos;
    }
}
