using System;
using UnityEngine;

public class VillagerNeedsController : SimulatableBehaviour
{
    public VillagerNeeds Needs { get; private set; }
    public VillagerEnvironment Environment {get; private set;}
    public VillagerData Data { get; private set; }
    [SerializeField] private VillagerNeedsConfig Config;


    public event Action<float> OnThirstChanged;
    public event Action<float> OnHungerChanged;
    public event Action OnHungry;
    public event Action OnFull;
    public event Action OnThirsty;
    public event Action OnHydrated;

    private bool thirstyTriggered = false;
    private bool hydratedTriggered = false;
    private bool hungryTriggered = false;
    private bool fullTriggered = false;


    private void Awake()
    {
        // Create the needs immediately so simulation never starts without them.
        Needs = new VillagerNeeds();

        // Use placeholder villager name if Data isn't set yet
        Data = new VillagerData("Unnamed");

        // Grab environment 
        Environment = GetComponent<VillagerEnvironment>();

        if (Environment == null)
            Debug.LogError($"{name} has no VillagerEnvironment component!");

    }
    public void Initialize(VillagerNeeds needs, VillagerData data)
    {
        Needs = needs;
        Data = data;
    }

 

    public void SimulateNeeds(float dt)
    {
        if (Needs == null) return;

        // --- DRINKING LOGIC ---
        if (Environment.IsInWater && Environment.CurrentWaterSource != null)
        {
            float need = Needs.maxThirst - Needs.thirst;
            if (need > 0)
            {
                // Withdraw from resource
                float drank = Environment.CurrentWaterSource.Withdraw(need);

                // Apply water to needs
                AddWater(drank);

                if (!hydratedTriggered)
                {
                    hydratedTriggered = true;
                    OnHydrated?.Invoke();
                }
            }
        }
        else
        {
            hydratedTriggered = false;
            Needs.thirst -= Config.thirstDrainRate * dt;
            Needs.thirst = Mathf.Clamp01(Needs.thirst);
        }

        OnThirstChanged?.Invoke(Needs.thirst);

        if (Needs.thirst < Needs.thirstThreshold && !thirstyTriggered)
        {
            thirstyTriggered = true;
            OnThirsty?.Invoke();
        }
        else if (Needs.thirst >= Needs.thirstThreshold)
        {
            thirstyTriggered = false;
        }

        // --- HUNGER logic stays unchanged ---
        Needs.hunger -= Config.hungerDrainRate * dt;
        Needs.hunger = Mathf.Clamp01(Needs.hunger);
        OnHungerChanged?.Invoke(Needs.hunger);
    }


    private void HandleThirsty() => OnThirsty?.Invoke();
    private void HandleHunger() => OnHungry?.Invoke();

    public void DrainWater()
    {
        // 0.0f = not thirsty / full
        Needs.thirst = 0.0f;
        OnThirstChanged?.Invoke(Needs.thirst);
    }

    public void AddWater(float thirstFilled)
    {
        // capture clamped value
        Needs.thirst = Mathf.Clamp01(Needs.thirst + thirstFilled);
        OnThirstChanged?.Invoke(Needs.thirst);
    }

   public override void Simulate(float deltaTime)
    {
        SimulateNeeds(deltaTime);
    }
}
