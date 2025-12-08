using TMPro;
using System.Collections;
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
    [Header("LeanTween Settings")]
    public float UIAnimationDuration = 0.25f;
    public LeanTweenType type;
    private Vector3 startPosition;

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

        startPosition = panel.transform.position;

        LeanTween.moveY(panel, Screen.height / 2.0f, UIAnimationDuration)
            .setEaseOutBack()
            .setOvershoot(1);

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
        

        LeanTween.moveY(panel, startPosition.y, UIAnimationDuration)
            .setEaseOutBack()
            .setOvershoot(1)
            .setOnComplete(() => 
            {
                panel.SetActive(false);
            });
            
        //panel.transform.position = startPosition;
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
                // Make the correct panel object active
                needsPanel.SetActive(true);
                behaviourPanel.SetActive(false);

                // Toggle interactivity on the panel decider buttons
                needsPanelButton.interactable = false;
                behaviourPanelButton.interactable = true;

            break;

            case PanelType.Behaviour:

                needsPanel.SetActive(false);
                behaviourPanel.SetActive(true);

                needsPanelButton.interactable = true;
                behaviourPanelButton.interactable = false;
            break;

            case PanelType.Count:
            break;
        }
    }
        
        
}
