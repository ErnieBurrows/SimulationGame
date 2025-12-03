using UnityEngine;

public class Resource : MonoBehaviour
{
    public float maxAmount;
    public float currentAmount;

    public void AddResource()
    {
        
    }

    public void RemoveResource(Villager villager, float amountToRemove)
    {
        if (currentAmount - amountToRemove >= 0)
        {
            villager.needsController.AddWater(amountToRemove);
            currentAmount -= amountToRemove;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Villager villager =  other.gameObject.GetComponent<Villager>();
            villager.needs.isInWater = true;
            RemoveResource(villager, 1.0f);
            Debug.Log("Player has collided with " + gameObject.name);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Villager>().needs.isInWater = false;
            Debug.Log("Player has ended collision with " + gameObject.name);
        }
    }
}
