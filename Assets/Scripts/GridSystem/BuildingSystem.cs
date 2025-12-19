using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public const float CellSize = 5.0f;

    [SerializeField] private GridBuildingData buildingData1, buildingData2, buildingData3;
    [SerializeField] private GridBuildingPreview previewPrefab;
    [SerializeField] private GridBuilding buildingPrefab;
    [SerializeField] private BuildingGrid grid;
    private GridBuildingPreview preview;

    private void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        if (preview != null)
        {
            HandlePreview(mousePosition);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                preview = CreatePreview(buildingData1, mousePosition);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                preview = CreatePreview(buildingData2, mousePosition);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                preview = CreatePreview(buildingData3, mousePosition);
            }
        }
    }

    private void HandlePreview(Vector3 mouseWorldPosition)
    {
        preview.transform.position = mouseWorldPosition;
        List<Vector3> buildPositions = preview.BuildingModel.GetAllBuildingPositions();
        bool canBuild = grid.CanBuild(buildPositions);

        if(canBuild)
        {
            preview.transform.position = GetSnappedCenterPosition(buildPositions);
            preview.ChangeState(GridBuildingPreview.BuildingPreviewState.Positive);
            if (Input.GetMouseButton(0))
            {
                PlaceBuilding(buildPositions);
            }
        }
        else
        {
            preview.ChangeState(GridBuildingPreview.BuildingPreviewState.Negative);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            preview.Rotate(90);
        }
    }

    private void PlaceBuilding(List<Vector3> buildingPositions)
    {
        GridBuilding building = Instantiate(buildingPrefab, preview.transform.position, Quaternion.identity);
        building.Setup(preview.Data, preview.BuildingModel.Rotation);
        grid.SetBuilding(building, buildingPositions);
        Destroy(preview.gameObject);
        preview = null;
    }

    private Vector3 GetSnappedCenterPosition(List<Vector3> allBuildingPositions)
    {
        List<int> xs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.x)).ToList();
        List<int> zs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.z)).ToList();

        float centerX = (xs.Min() + xs.Max()) / 2.0f + CellSize / 2.0f;
        float centerZ = (zs.Min() + zs.Max()) / 2.0f + CellSize / 2.0f;

        return new(centerX, 0, centerZ);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    private GridBuildingPreview CreatePreview(GridBuildingData data, Vector3 position)
    {
        GridBuildingPreview buildingPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        buildingPreview.Setup(data);
        return buildingPreview;
    }
}
