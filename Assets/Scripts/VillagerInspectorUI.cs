using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PanelType
{
    Needs,
    Behaviour,
    Count
};

public class VillagerInspectorUI : MonoBehaviour
{
    // Panel Selection
    [Header("Panel Selection References")]
    [SerializeField] private GameObject needsPanel;
    [SerializeField] private GameObject behaviourPanel;
    [SerializeField] private Button needsPanelButton;
    [SerializeField] private Button behaviourPanelButton;

    [Header("Needs References")]
    [SerializeField] private Image thirstBar;
    [SerializeField] private Image hungerBar;

    [Header("Villager Data References")]
    [SerializeField] private TextMeshProUGUI villagerName;

    [Header("Main Inspector References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button closeButton;

    private VillagerNeedsController current;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);

        needsPanelButton.onClick.AddListener(() => 
        {
            ChangePanel(PanelType.Needs);
        });

        behaviourPanelButton.onClick.AddListener(() => 
        {
            ChangePanel(PanelType.Behaviour);
        });
    }    

     private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);

        needsPanelButton.onClick.RemoveAllListeners();
        behaviourPanelButton.onClick.RemoveAllListeners();
    }  

    public void Show(VillagerNeedsController controller)
    {
        // Unsubscribe from previous controller
        if (current != null)
        {
            current.OnThirstChanged -= UpdateThirst;
            current.OnHungerChanged -= UpdateHunger;
        }

        current = controller;

        // Set the name
        villagerName.text = current.Data.villagerName;

        current.OnThirstChanged += UpdateThirst;
        current.OnHungerChanged += UpdateHunger;

        thirstBar.fillAmount = current.Needs.thirst;
        hungerBar.fillAmount = current.Needs.hunger;

        panel.SetActive(true);

        ChangePanel(PanelType.Needs);
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

    private void UpdateThirst(float value) 
    {
        thirstBar.fillAmount = value;
    }
    private void UpdateHunger(float value)
    {
        hungerBar.fillAmount = value;
    } 

    // Note: This is called by the custom inspector class, and is callable within edit mode.
    public void ChangePanel(PanelType type)
    {
        switch (type)
        {
            case PanelType.Needs:
                needsPanel.SetActive(true);
                behaviourPanel.SetActive(false);
            break;

            case PanelType.Behaviour:
                needsPanel.SetActive(false);
                behaviourPanel.SetActive(true);
            break;

            case PanelType.Count:
            break;
        }
    }
        
        
}
