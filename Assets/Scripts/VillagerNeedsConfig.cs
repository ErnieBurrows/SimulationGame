using UnityEngine;

[CreateAssetMenu(fileName = "VillagerNeedsConfig", menuName = "Scriptable Objects/VillagerNeedsConfig")]
public class VillagerNeedsConfig : ScriptableObject
{
    public float thirstDrainRate = 0.005f;
    public float hungerDrainRate = 0.001f;
}
