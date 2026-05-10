using UnityEngine;

public static class GlobalBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServices()
    {
        ServiceLoader.ClearServices();

        IEventBus globalEventBus = new EventBus();
        ServiceLoader.AddService<IEventBus>(globalEventBus);

        Debug.Log("initialized global services");
    }
}