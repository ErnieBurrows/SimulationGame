using System;
using UnityEngine;

public class VillagerNeedsController : SimulatableBehaviour
{
    [SerializeField] private VillagerNeedsConfig Config;
    public VillagerNeeds Needs { get; private set; }
    public VillagerEnvironment Environment {get; private set;}
    public VillagerData Data { get; private set; }

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
        if (Environment.IsInWater && Environment.currentResource != null && Needs.thirst < Needs.thirstThreshold)
        {
            float need = Needs.maxThirst - Needs.thirst;
            if (need > 0)
            {
                // Withdraw from resource
                float consumed = Environment.currentResource.Withdraw(need);

                // Apply water to needs
                AddWater(consumed);

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
            Needs.thirst -= Config.thirstDrainRate;
            Needs.thirst = Mathf.Clamp01(Needs.thirst);
        }

        if (Environment.IsInFood && Environment.currentResource != null && Needs.hunger < Needs.hungerThreshold)
        {
            float need = Needs.maxHunger - Needs.hunger;
            if (need > 0)
            {
                float consumed = Environment.currentResource.Withdraw(need);

                AddFood(consumed);

                if (!fullTriggered)
                {
                    fullTriggered = true;
                    OnFull?.Invoke();
                }
            }
        }
        else
        {
            fullTriggered = false;
            Needs.hunger -= Config.hungerDrainRate;
            Needs.hunger = Mathf.Clamp01(Needs.hunger);
        }


        // Needs changed Actions
        OnThirstChanged?.Invoke(Needs.thirst);
        OnHungerChanged?.Invoke(Needs.hunger);


        // OnHungry and OnThirst Actions
        if (Needs.thirst < Needs.thirstThreshold && !thirstyTriggered)
            HandleThirsty();
        else if (Needs.thirst >= Needs.thirstThreshold)
            thirstyTriggered = false;

        if(Needs.hunger < Needs.hungerThreshold && !hungryTriggered)
            HandleHunger();
        else if (Needs.hunger >= Needs.hungerThreshold)
            hungryTriggered = false;
        
    }


    private void HandleThirsty()
    {
        thirstyTriggered = true;
        OnThirsty?.Invoke();
    }  
    private void HandleHunger()
    {
        hungryTriggered = true;
        OnHungry?.Invoke();
    } 
        

    public void DrainWater()
    {
        // 0.0f = not thirsty / full
        Needs.thirst = 0.0f;
        OnThirstChanged?.Invoke(Needs.thirst);
    }

    public void DrainFood()
    {
        Needs.hunger = 0.0f;
        OnHungerChanged?.Invoke(Needs.hunger);
    }

    public void AddWater(float thirstFilled)
    {
        Needs.thirst = Mathf.Clamp01(Needs.thirst + thirstFilled);
        OnThirstChanged?.Invoke(Needs.thirst);
    }

    public void AddFood(float hungerFilled)
    {
        Needs.hunger = Mathf.Clamp01(Needs.hunger + hungerFilled);
        OnHungerChanged?.Invoke(Needs.hunger);
    }

    public override void Simulate(float deltaTime)
    {
        SimulateNeeds(deltaTime);
    }

}
