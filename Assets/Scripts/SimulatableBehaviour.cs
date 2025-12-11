using UnityEngine;

/// <summary>
/// Base abstract class that will handle registering simulatable objects to the simulation manager.
/// </summary>
public abstract class SimulatableBehaviour : MonoBehaviour, ISimulatable
{
    protected virtual void OnEnable()
    {
        SimulationManager.Instance?.Register(this);
    }

    protected virtual void OnDisable()
    {
        SimulationManager.Instance?.Unregister(this);
    }

    public abstract void Simulate(float dt);
}
