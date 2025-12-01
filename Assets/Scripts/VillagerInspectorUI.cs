using UnityEngine;
using UnityEngine.UI;

public class VillagerInspectorUI : MonoBehaviour
{
    [SerializeField] private Image thirstBar;
    [SerializeField] private Image hungerBar;
    [SerializeField] private GameObject panel;

    private VillagerNeedsController current;

    public void Show(VillagerNeedsController controller)
    {
        // Unsubscribe from previous controller
        if (current != null)
        {
            current.OnThirstChanged -= UpdateThirst;
            current.OnHungerChanged -= UpdateHunger;
        }

        current = controller;

        current.OnThirstChanged += UpdateThirst;
        current.OnHungerChanged += UpdateHunger;

        thirstBar.fillAmount = current.Needs.thirst;
        hungerBar.fillAmount = current.Needs.hunger;

        panel.SetActive(true);
    }

    public void Close()
    {
        if (current != null)
        {
            current.OnThirstChanged -= UpdateThirst;
            current.OnHungerChanged -= UpdateHunger;
        }

        current = null;
        panel.SetActive(false);
    }

    private void UpdateThirst(float v) => thirstBar.fillAmount = v;
    private void UpdateHunger(float v) => hungerBar.fillAmount = v;
}
