using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class GridPlaceButton : MonoBehaviour
{
    public BuildingDefinition buildingDef;
    private Button button;
    private GridPlacer gridPlacer;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaceResource);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PlaceResource);
    }

    private void PlaceResource()
    {
        gridPlacer = FindFirstObjectByType<GridPlacer>();

        if (gridPlacer == null) return;

        gridPlacer.GetComponent<GridPlacer>().StartPlacing(buildingDef);
    }
}
