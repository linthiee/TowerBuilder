using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    [SerializeField] private BoxCollider slope;

    [Header("Movement")]
    [SerializeField] private float smoothSpeed = 5f;
    public float shakeTime = 0.3f;
    [SerializeField] private float shakeAmount = 0.1f;

    private IEventBus _eventBus;

    [Header("Movement")]
    private float shakeDuration = 0f;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 originalPos = Vector3.zero; 

    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<BlockLandedEvent>(OnBlockLanded);
        _eventBus.Subscribe<PerfectLandEvent>(OnPerfectLand);

        targetPos = cameraPos.position;
        originalPos = targetPos;
    }

    private void Update()
    {
        originalPos = Vector3.Lerp(originalPos, targetPos, Time.deltaTime * smoothSpeed);

        Vector3 shakeOffset = Vector3.zero;
        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }

        cameraPos.position = originalPos + shakeOffset;
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
    }

    private void OnBlockLanded(BlockLandedEvent eventData)
    {
        if (!eventData.groundLand)
            MoveCamera();
    }

    private void MoveCamera()
    {
        targetPos += new Vector3(0, slope.size.y, 0);
    }
}