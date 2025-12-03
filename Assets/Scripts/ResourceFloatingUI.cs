using UnityEngine;
using UnityEngine.UI;

public class ResourceFloatingUI : MonoBehaviour
{
    [SerializeField] Image resourceBar;

    /// <summary>
    /// Updates the resource bar with the new percentageFill. 
    /// Float must be passed in as a percentage value between 0 - 1.
    /// </summary>
    /// <param name="percentageFill"></param>
    public void UpdateCurrentResourceAmount(float percentageFill)
    {
        resourceBar.fillAmount = percentageFill;
    }



}
