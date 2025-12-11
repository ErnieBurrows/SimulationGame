using UnityEngine;

public class VillagerEnvironment : MonoBehaviour
{
    public bool IsInWater { get; private set; }
    public Resource CurrentWaterSource { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Resource resource = other.GetComponent<Resource>();
        if (resource != null && resource.resourceType == ResourceType.Water)
        {
            IsInWater = true;
            CurrentWaterSource = resource;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Resource resource = other.GetComponent<Resource>();
        if (resource != null && resource == CurrentWaterSource)
        {
            IsInWater = false;
            CurrentWaterSource = null;
        }
    }
}
