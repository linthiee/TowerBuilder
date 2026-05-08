using UnityEngine;

[DefaultExecutionOrder(-100)] 
public class GameBootStrapper : MonoBehaviour 
{
    [SerializeField] private GameObject gameManagerPrefab;

    private void Awake()
    {
        Debug.Log("1- eventBus init");

        IEventBus eventBus = new EventBus();

        ServiceLoader.AddService<IEventBus>(eventBus);
    }
}
