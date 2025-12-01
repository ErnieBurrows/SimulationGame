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

            yield return new WaitForSeconds(Needs.tickRate);
        }
    }
}
  