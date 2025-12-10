using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SimulationSpeed
{
    Paused,
    Normal,   // 1x
    Fast,     // 2x
    Fastest   // 4x
}

public class SimulationManager : MonoBehaviour
{
    #region Singleton
    private static SimulationManager _instance;
    private static bool _applicationIsQuitting = false;
    public static SimulationManager Instance
    {
        get
        {
            if (_applicationIsQuitting) return null;
            if (_instance == null) _instance = FindFirstObjectByType<SimulationManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); }
        else if (_instance != this) Destroy(gameObject);
    }

    private void OnDestroy() { if (_instance == this) _applicationIsQuitting = true; }
    #endregion

    private HashSet<ISimulatable> simulatables = new HashSet<ISimulatable>();

    [SerializeField] private float baseTickSeconds = 0.2f; // Base real seconds per simulation tick at Normal
    [HideInInspector] public SimulationSpeed simulationSpeed = SimulationSpeed.Normal;

    private IEnumerator Start()
    {
        yield return null; // wait one frame
        
        // Auto-register existing objects with ISimulatable
        foreach (MonoBehaviour monoBehaviour in FindObjectsByType<MonoBehaviour>(sortMode: FindObjectsSortMode.None))
        {
            if (monoBehaviour is ISimulatable simulatable) 
                Register(simulatable);
        }

        StartCoroutine(SimulateLoop());
    }

    // Register/unregister so runtime-instantiated objects can join
    public void Register(ISimulatable simulatable) 
    { 
        if (simulatable != null) 
        simulatables.Add(simulatable); 
    }
    public void Unregister(ISimulatable simulatable) 
    { 
        if (simulatable != null) 
        simulatables.Remove(simulatable); 
    }

    public float GetSimulationDelta()
    {
        if (simulationSpeed == SimulationSpeed.Paused) 
            return 0f;

        switch (simulationSpeed)
        {
            case SimulationSpeed.Fast: 
                return baseTickSeconds / 2f;

            case SimulationSpeed.Fastest: 
                return baseTickSeconds / 4f;

            case SimulationSpeed.Normal:
                default: return baseTickSeconds;
        }
    }

    private IEnumerator SimulateLoop()
    {
        while (true)
        {
            if (simulationSpeed == SimulationSpeed.Paused)
            {
                yield return null;
                continue;
            }

            float delta = GetSimulationDelta();

            // Snapshot to avoid modification during iteration
            List<ISimulatable> snapshot = new List<ISimulatable>(simulatables);
            for (int i = 0; i < snapshot.Count; i++)
            {
                try { snapshot[i].Simulate(delta); }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Simulate exception on {snapshot[i]}: {ex}");
                }
            }

            yield return new WaitForSeconds(delta);
        }
    }

    public void SetSimulationSpeed(SimulationSpeed newSpeed)
    {
        simulationSpeed = newSpeed;
    }
}
