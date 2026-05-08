using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField] private GameObject rope;
    [SerializeField] private Transform pivot;
    [SerializeField] private float amplitude = 15.0f;
    [SerializeField] private GameObject slopePrefab;
    [SerializeField] private Transform spawnPosition;

    private IEventBus _eventBus;
    private float gravity = 9.81f;

    private void Start()
    {
        Debug.Log("2- eventBus initialized");

        _eventBus = ServiceLoader.GetService<IEventBus>();

        if (_eventBus == null)
            Debug.Log("event bus is null in rope");

        _eventBus.Subscribe<BlockLandedEvent>(OnBlockLanded);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<BlockLandedEvent>(OnBlockLanded);
        }
    }
    private void FixedUpdate()
    {
        float sin = Mathf.Sin(Time.time) * amplitude;

        float acceleration = -(gravity / rope.transform.localScale.y) * sin;

        Vector3 euler = pivot.localEulerAngles;
        euler.z = acceleration;
        pivot.eulerAngles = euler;
    }

    private void OnBlockLanded(BlockLandedEvent eventData)
    {
        SpawnNextBlock();
    }

    private void SpawnNextBlock()
    {
        Debug.Log("block landed");
        Instantiate(slopePrefab, spawnPosition.position, pivot.rotation, spawnPosition);
    }
}
