using System;
using UnityEngine;

public enum ResourceType
{
    Water,
    Food,
    Count
}
public class Resource : MonoBehaviour
{
    public float maxAmount;
    public float currentAmount;
    public ResourceType resourceType;

    private ResourceFloatingUI floatingUI;

    private void Awake()
    {
        floatingUI = GetComponentInChildren<ResourceFloatingUI>();
    }

    public void AddResource()
    {
        //floatingUI.UpdateCurrentResourceAmount(currentAmount/maxAmount);
    }

    public void RemoveResource(Villager villager, float amountToRemove)
    {
        if (currentAmount - amountToRemove >= 0)
        {
            villager.needsController.AddWater(amountToRemove);
            currentAmount -= amountToRemove;
            floatingUI.UpdateCurrentResourceAmount(currentAmount/maxAmount);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Villager villager =  other.gameObject.GetComponent<Villager>();
            villager.needs.isInWater = true;
            RemoveResource(villager, 1.0f);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Villager>().needs.isInWater = false;
        }
    }
}
