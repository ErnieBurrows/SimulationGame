using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VillagerNeedsController : MonoBehaviour
{
    public VillagerNeeds Needs { get; private set; }

    public event Action<float> OnThirstChanged;
    public event Action<float> OnHungerChanged;

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
                Needs.thirst += Needs.thirstDrainRate; // or a fill rate
            else
                Needs.thirst -= Needs.thirstDrainRate;

            OnThirstChanged?.Invoke(Needs.thirst);

            Needs.hunger -= Needs.hungerDrainRate;
            OnHungerChanged?.Invoke(Needs.hunger);

            if (Needs.thirst < Needs.thirstThreshold)
               HandleThirsty();

            if (Needs.hunger < Needs.hungerThreshold)
                HandleHunger();

            yield return new WaitForSeconds(Needs.tickRate);
        }
    }

    private void HandleThirsty()
    {
         Debug.Log("Damn I am Thirsty");
    }

    private void HandleHunger()
    {
        Debug.Log("Shit I am Hungry");
    }
}
  