using UnityEngine;

[System.Serializable]
public class VillagerNeeds
{
    public float thirst = 1;
    public float maxThirst = 1;
    public float hunger = 1;
    public float maxHunger = 1;
    public float thirstThreshold = 0.2f;
    public float hungerThreshold = 0.1f;
    public bool isInWater = false;
    public bool isUIOpen = false;

    [Tooltip("The wait time between village resource draining")]
    public float tickRate = 0.1f;

    [Tooltip("The percentage (Measured in decimal) that the water drains per tick")]
    public float thirstDrainRate = 0.005f, hungerDrainRate = 0.001f;

}