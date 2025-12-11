using UnityEngine;

public class VillagerEnvironment : MonoBehaviour
{
    public bool IsInWater { get; private set; }
    public bool IsInFood {get; private set;}
    public Resource currentResource { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Resource resource = other.GetComponent<Resource>();
        if (resource != null)
        {
            switch (resource.resourceType)
            {
                case ResourceType.Water:
                    IsInWater = true;
                break;

                case ResourceType.Food:
                    IsInFood = true;
                break;

                default:
                break;
            }

            currentResource = resource;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Resource resource = other.GetComponent<Resource>();
        if (resource != null)
        {
            IsInWater = false;
            IsInFood = false;
            currentResource = null;
        }
    }
}
