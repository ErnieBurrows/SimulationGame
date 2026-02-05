using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TEST_VillagerNeeds : MonoBehaviour
{
    private List<VillagerNeedsController> villagers = new List<VillagerNeedsController>();
    public void DrainVillagersWater()
    {
        villagers = FindObjectsByType<VillagerNeedsController>(FindObjectsSortMode.None).ToList();
        foreach (VillagerNeedsController villager in villagers)
        {
            villager.DrainWater();
        }
    }

    public void DrainVillagersFood()
    {
        villagers = FindObjectsByType<VillagerNeedsController>(FindObjectsSortMode.None).ToList();
        foreach (VillagerNeedsController villager in villagers)
        {
            villager.DrainFood();
        }
    }
}
