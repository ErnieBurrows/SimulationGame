using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public enum SimulationSpeed
{
    Paused,
    Normal,
    Fast,
    Fastest
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
            if (_applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(SimulationManager) +
                                 "' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SimulationManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<SimulationManager>();
                    singletonObject.name = "(Singleton) " + typeof(SimulationManager).ToString();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

     private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _applicationIsQuitting = true;
        }
    }

    #endregion
    
    // Todo: Gather all items that have ISimulatable and add them to this list at Start.
    [SerializeField] private List<GameObject> simulatable;

    [Tooltip("This will be divided to get a faster tickrate. To make the entire system faster decrease this.")]
    [SerializeField] private float tickRateMultiplier = 1;
    private float tickRate;

    [HideInInspector]
    public SimulationSpeed simulationSpeed = SimulationSpeed.Normal;

    private void Start()
    {
        GetTickRate();

        StartCoroutine(SimulateALL());
    }

    private void GetTickRate()
    {
        // Avoid dividing by 0 if the simulation is paused.
        if (simulationSpeed != SimulationSpeed.Paused)
        {
            tickRate = tickRateMultiplier / (int)simulationSpeed;
        }
    }

    private IEnumerator SimulateALL()
    {
        while(true)
        {
            if (simulationSpeed == SimulationSpeed.Paused)
            {
                yield return null;
                continue;     
            }

            foreach(GameObject go in simulatable)
            {
                go.GetComponent<ISimulatable>()?.Simulate();
            }

            yield return new WaitForSeconds(tickRate);
        }
    }

    public void ChangeSimulationSpeed(SimulationSpeed newSpeed)
    {
        simulationSpeed = newSpeed;

        GetTickRate();
    }


}
