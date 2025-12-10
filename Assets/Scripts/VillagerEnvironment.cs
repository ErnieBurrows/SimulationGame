using UnityEngine;

public class VillagerEnvironment : MonoBehaviour
{
    public bool IsInWater { get; private set; }
    public Resource CurrentWaterSource { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Resource res = other.GetComponent<Resource>();
        if (res != null && res.resourceType == ResourceType.Water)
        {
            IsInWater = true;
            CurrentWaterSource = res;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Resource res = other.GetComponent<Resource>();
        if (res != null && res == CurrentWaterSource)
        {
            IsInWater = false;
            CurrentWaterSource = null;
        }
    }
}
