using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private int gridWidth, gridHeight;

    private BuildingGridCell[,] grid;

    private void Start()
    {
        grid = new BuildingGridCell[gridWidth, gridHeight];


        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = new();
            }
        }
    }

    public void SetBuilding(GridBuilding building, List<Vector3> allBuildingPositions)
    {
        foreach (Vector3 position in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(position);
            grid[x, y].SetBuilding(building);
        }
    }

    public bool CanBuild(List<Vector3> allBuildingPositions)
    {
        foreach (Vector3 position in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(position);

            if (x < 0 || x >= gridWidth || y < 0 || y >+ gridHeight) return false;

            if (!grid[x, y].IsEmpty()) return false;
        }

        return true;
    }

    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / BuildingSystem.CellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).z / BuildingSystem.CellSize);

        return (x,y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (BuildingSystem.CellSize <= 0 || gridWidth <= 0 || gridHeight <= 0) return;

        Vector3 origin = transform.position;

        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = origin + new Vector3(0, 0.01f, y * BuildingSystem.CellSize);
            Vector3 end = origin + new Vector3(gridWidth * BuildingSystem.CellSize, 0.01f, y * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }       

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, 0);
            Vector3 end = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, gridWidth * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }
    }
}

public class BuildingGridCell
{
    private GridBuilding building;

    public void SetBuilding(GridBuilding building)
    {
        this.building = building;
    }

    public bool IsEmpty()
    {
        return building == null;
    }
}
