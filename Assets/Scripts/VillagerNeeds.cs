using UnityEngine;

[System.Serializable]
public class VillagerNeeds
{
    public float thirst = 1;
    public float hunger = 1;
    public bool isInWater = false;
    public bool isUIOpen = false;

    [Tooltip("The wait time between village resource draining")]
    public float tickRate = 0.1f;

    [Tooltip("The percentage (Measured in decimal) that the water drains per tick")]
    public float thirstDrainRate = 0.001f, hungerDrainRate = 0.001f;

}