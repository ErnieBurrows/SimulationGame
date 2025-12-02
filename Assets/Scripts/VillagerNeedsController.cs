using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VillagerNeedsController : MonoBehaviour
{
    public VillagerNeeds Needs { get; private set; }
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


    public void Initialize(VillagerNeeds needs)
    {
        Needs = needs;

        StartCoroutine(SimulateNeeds());
    }

    private IEnumerator SimulateNeeds()
    {
        while(true)
        {
            if (Needs.isInWater)
            {
                if (!hydratedTriggered)
                {
                    hydratedTriggered = true;
                    Needs.thirst = 1.0f; // Have this water deducted from the recourse building.
                    OnHydrated?.Invoke();  
                }
            }
            else
            {
                Needs.thirst -= Needs.thirstDrainRate;
                hydratedTriggered = false;
            }

                OnThirstChanged?.Invoke(Needs.thirst);

            

            if (Needs.thirst < Needs.thirstThreshold)
            {
                if (!thirstyTriggered)
                {
                    thirstyTriggered = true;
                    HandleThirsty();
                }
            }
            else
            {
                thirstyTriggered = false;
            }
            
            


            Needs.hunger -= Needs.hungerDrainRate;
            OnHungerChanged?.Invoke(Needs.hunger);

            if (Needs.hunger < Needs.hungerThreshold)
            {
                if (!hungryTriggered)
                {
                    hungryTriggered = true;
                    HandleHunger();
                }
            }
            else
            {
                hungryTriggered = false;
            }
                

            yield return new WaitForSeconds(Needs.tickRate);
        }
    }

    private void HandleThirsty()
    {
        OnThirsty?.Invoke();
    }

    private void HandleHunger()
    {
        OnHungry?.Invoke();
    }
}
  