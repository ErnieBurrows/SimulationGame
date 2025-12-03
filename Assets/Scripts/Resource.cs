using UnityEngine;

public class Resource : MonoBehaviour
{
    public float maxAmount;
    public float currentAmount;

    public void AddResource()
    {
        
    }

    public void RemoveResource()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Villager>().needs.isInWater = true;
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
