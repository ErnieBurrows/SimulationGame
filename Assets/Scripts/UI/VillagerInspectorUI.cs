using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum PanelType
{
    Needs,
    Behaviour,
    Count
};
// Todo: I need to adjust this logic so that the villager reference can come from somewhere other than serialized field.
// Remeber we call this as a single inspector, so we pass in a new controller each time. Maybe on the show or something.
// I also should change all references to just reference the Villager itself. Lets use Villager.cs as the main logic gate.
// Villager.cs should hold public reference to everything
public class VillagerInspectorUI : MonoBehaviour
{
    private Villager villager;
    private VillagerNeedsController currentController;

    [Header("LeanTween Settings")]
    public float UIAnimationDuration = 0.25f;
    public LeanTweenType type;
    private Vector3 startPosition;

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

    [Header("Current Task References")]
    [SerializeField] private TextMeshProUGUI currentTaskText;

   
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

    private void ChangeJobText(string currentTask)
    {
        currentTaskText.text = currentTask;
    }

    public void Show(Villager villager)
    {   
        if (villager == null)
        {
            Close();
            return;
        }

        RebindJobEvents(villager);
        RebindNeedsEvents(villager);

        villagerName.text = currentController.Data.villagerName;

        thirstBar.fillAmount = currentController.Needs.thirst;
        hungerBar.fillAmount = currentController.Needs.hunger;

        panel.SetActive(true);

        startPosition = panel.transform.position;

        LeanTween.moveY(panel, Screen.height / 2.0f, UIAnimationDuration)
            .setEaseOutBack()
            .setOvershoot(1);

        ChangePanel(PanelType.Needs);
    }

    public void Close()
    {
        villager.controller.OnJobChanged -= ChangeJobText;

        if (currentController != null)
        {
            currentController.OnThirstChanged -= UpdateThirst;
            currentController.OnHungerChanged -= UpdateHunger;
        }

        currentController = null;
        

        LeanTween.moveY(panel, startPosition.y, UIAnimationDuration)
            .setEaseOutBack()
            .setOvershoot(1)
            .setOnComplete(() => 
            {
                panel.SetActive(false);
            });
    }

    private void RebindJobEvents(Villager newVillager)
    {
        // Unsubscribe old
        if (villager?.controller != null)
            villager.controller.OnJobChanged -= ChangeJobText;

        villager = newVillager;

        // Subscribe new
        if (villager?.controller != null)
            villager.controller.OnJobChanged += ChangeJobText;

        // Initial sync (don’t rely on timing)
        if (villager?.controller != null)
            ChangeJobText(villager.controller.DebugCurrentStateText());
    }

    
    private void RebindNeedsEvents(Villager newVillager)
    {
        // Unsubscribe old
        if (currentController != null)
        {
            currentController.OnThirstChanged -= UpdateThirst;
            currentController.OnHungerChanged -= UpdateHunger;
        }

        currentController = newVillager.needsController;

        // Subscribe new (guard)
        if (currentController != null)
        {
            currentController.OnThirstChanged += UpdateThirst;
            currentController.OnHungerChanged += UpdateHunger;
        }
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
