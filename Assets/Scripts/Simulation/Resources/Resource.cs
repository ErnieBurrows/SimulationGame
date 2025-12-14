using UnityEngine;

public enum ResourceType
{
    WaterDrinkable,
    Water,
    Food
}

public class Resource : MonoBehaviour
{
    public float maxAmount = 10f;
    public float currentAmount = 10f;
    public ResourceType resourceType;

    private ResourceFloatingUI floatingUI;

    private void Awake()
    {
        floatingUI = GetComponentInChildren<ResourceFloatingUI>();
        UpdateUI();
    }

    public float Withdraw(float amount)
    {
        float taken = Mathf.Min(currentAmount, amount);
        currentAmount -= taken;

        UpdateUI();
        return taken;
    }

    public void Add(float amount)
    {
        currentAmount = Mathf.Clamp(currentAmount + amount, 0, maxAmount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (floatingUI != null)
            floatingUI.UpdateCurrentResourceAmount(currentAmount / maxAmount);
    }

    private void OnEnable()
    {
        ResourceRegistry.Register(this);
    }

    private void OnDisable()
    {
        ResourceRegistry.Unregister(this);
    }
}
