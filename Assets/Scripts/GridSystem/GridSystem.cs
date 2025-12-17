using System.Collections.Generic;
using UnityEngine;

// Todo: Lock object's positions to the ground. 
// Todo: Decide how we know if objects can be placed on top of each other.
// Todo: Make some simple UI that allows us to choose placeable objects.
// Todo: Create a shader that prints the grid.
public class GridSystem : MonoBehaviour
{
    public GameObject objectToPlace;
    public float gridSize = 1.0f;
    private GameObject ghostObject;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();


    private void Start()
    {
        CreateGhostObject();
    }

    private void Update()
    {
        UpdateGhostPosition();

        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }
    }
    private void CreateGhostObject()
    {
        ghostObject = Instantiate(objectToPlace);
        ghostObject.GetComponent<Collider>().enabled = false;

        Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            Color color = material.color;
            color.a = 0.5f;
            material.color = color;

            material.SetFloat("_Mode", 2);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

        }
    }

    private void UpdateGhostPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;

            Vector3 snappedPosition = new Vector3 (
                Mathf.Round(point.x / gridSize) * gridSize,
                Mathf.Round(point.y / gridSize) * gridSize,
                Mathf.Round(point.z / gridSize) * gridSize
            );

            ghostObject.transform.position = snappedPosition;

            if (occupiedPositions.Contains(snappedPosition))
                SetGhostColor(Color.red);
            else
                SetGhostColor(new Color(1.0f, 1.0f, 1.0f, 0.5f));
        }
    }

    private void SetGhostColor(Color color)
    {
        Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            material.color = color;
        }
        
    }

    private void PlaceObject()
    {
        Vector3 placementPosition = ghostObject.transform.position;

        if (!occupiedPositions.Contains(placementPosition))
        {
            Instantiate(objectToPlace, placementPosition, Quaternion.identity);

            occupiedPositions.Add(placementPosition);
        }
    }
}
