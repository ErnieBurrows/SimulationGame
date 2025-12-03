using System.Collections.Generic;
using UnityEngine;

public class TEST_VillagerNeeds : MonoBehaviour
{
    public List<GameObject> villagers;
    public void DrainVillagersWater()
    {
        foreach (GameObject villager in villagers)
        {
            villager.GetComponent<VillagerNeedsController>().DrainWater();
        }
    }
}
