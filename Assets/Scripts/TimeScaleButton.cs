using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TimeScaleButton : MonoBehaviour
{
    [SerializeField] private SimulationSpeed simulationSpeed;

    private Button button;
    private List<TimeScaleButton> siblingButtons;

    private void Awake()
    {
        button = GetComponent<Button>();

        // Get sibling TimeScaleButtons only
        siblingButtons = transform.parent
            .GetComponentsInChildren<TimeScaleButton>(includeInactive: true)
            .Where(tsb => tsb != this)
            .ToList();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClicked);

        // Sync UI when enabled
        RefreshState(SimulationManager.Instance.simulationSpeed);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClicked);
    }

    private void OnClicked()
    {
        // Set simulation speed
        SimulationManager.Instance.SetSimulationSpeed(simulationSpeed);

        // Update UI for ALL buttons
        RefreshGroup();
    }

    // Refreshes this button based on sim speed
    private void RefreshState(SimulationSpeed current)
    {
        bool isSelected = (current == simulationSpeed);
        button.interactable = !isSelected;
    }

    // Updates this and siblings
    private void RefreshGroup()
    {
        RefreshState(SimulationManager.Instance.simulationSpeed);

        foreach (var sibling in siblingButtons)
        {
            sibling.RefreshState(SimulationManager.Instance.simulationSpeed);
        }
    }
}
