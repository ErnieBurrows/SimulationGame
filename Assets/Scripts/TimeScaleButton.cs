using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TimeScaleButton : MonoBehaviour
{
    [SerializeField] private SimulationSpeed simulationSpeed;
    private List<Button> siblingButtons = new List<Button>();
    private Button button;


    private void Awake()
    {
        button = GetComponent<Button>();

        siblingButtons = transform.parent.GetComponentsInChildren<Button>().ToList<Button>();
    }

    private void ToggleSiblingInteractivity()
    {
        foreach (Button button in siblingButtons)
        {
            if (button != this.button)
                button.interactable = true;
            else
                button.interactable = false;
        }
    }

    private void OnEnable()
    {
        button.onClick.AddListener(() => 
        {
            SimulationManager.Instance.ChangeSimulationSpeed(simulationSpeed);
            ToggleSiblingInteractivity();
        });
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }


}
