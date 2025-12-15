using UnityEngine;

/// <summary>
/// Base abstract class that will handle registering simulatable objects to the simulation manager.
/// </summary>
public abstract class SimulatableBehaviour : MonoBehaviour, ISimulatable
{
    private SimulationManager simulationManager;
    protected virtual void OnEnable()
    {
        simulationManager = SimulationManager.Instance;
        
        if (simulationManager == null) return;
        
        simulationManager.Register(this);
        simulationManager.OnSimulationSpeedChanged += HandleSimulationSpeedChange;
    }

    protected virtual void OnDisable()
    {
        if (simulationManager == null) return;

        simulationManager.Unregister(this);
        simulationManager.OnSimulationSpeedChanged -= HandleSimulationSpeedChange;
        
        simulationManager = null;
    }

    public abstract void Simulate(float dt);

    public virtual void HandleSimulationSpeedChange()
    {
        
    }
}
