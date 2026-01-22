using System.Collections.Generic;
using UnityEngine;

public class SimpleGrid : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private int width = 50;
    [SerializeField] private int height = 50; // z dimension

    // occupancy by cell
    private readonly HashSet<Vector2Int> occupied = new();

    public float CellSize => cellSize;
    public Vector3 Origin => transform.position;

    public Vector2Int WorldToCell(Vector3 world)
    {
        Vector3 local = world - Origin;
        int x = Mathf.FloorToInt(local.x / cellSize);
        int z = Mathf.FloorToInt(local.z / cellSize);
        return new Vector2Int(x, z);
    }

    public Vector3 CellToWorldCenter(Vector2Int cell)
    {
        return Origin + new Vector3(
            (cell.x + 0.5f) * cellSize,
            0f,
            (cell.y + 0.5f) * cellSize
        );
    }

    public bool IsInside(Vector2Int cell)
        => cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;

    public bool IsOccupied(Vector2Int cell) => occupied.Contains(cell);

    public void SetOccupied(IEnumerable<Vector2Int> cells, bool value)
    {
        foreach (var c in cells)
        {
            if (value) occupied.Add(c);
            else occupied.Remove(c);
        }
    }

    // Optional gizmo grid
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Vector3 o = Origin;

        for (int x = 0; x <= width; x++)
        {
            Vector3 a = o + new Vector3(x * cellSize, 0.01f, 0);
            Vector3 b = o + new Vector3(x * cellSize, 0.01f, height * cellSize);
            Gizmos.DrawLine(a, b);
        }

        for (int z = 0; z <= height; z++)
        {
            Vector3 a = o + new Vector3(0, 0.01f, z * cellSize);
            Vector3 b = o + new Vector3(width * cellSize, 0.01f, z * cellSize);
            Gizmos.DrawLine(a, b);
        }
    }
}
