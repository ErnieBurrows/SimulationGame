using UnityEngine;
using UnityEngine.UI;
public class VillagerFloatingUI : MonoBehaviour
{
    [SerializeField] private Image thirstBar;
    [SerializeField] private Image hungerBar;

    private VillagerNeedsController controller;

    public void Bind(VillagerNeedsController c)
    {
        controller = c;
        controller.OnThirstChanged += UpdateThirst;
        controller.OnHungerChanged += UpdateHunger;
    }

    private void UpdateThirst(float v) => thirstBar.fillAmount = v;
    private void UpdateHunger(float v) => hungerBar.fillAmount = v;
}
