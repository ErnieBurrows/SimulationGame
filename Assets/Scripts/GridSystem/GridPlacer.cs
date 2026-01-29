using System.Collections.Generic;
using UnityEngine;

//Todo: This should maybe be a singleton
public class GridPlacer : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SimpleGrid grid;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundMask;

    [Header("Input")]
    [SerializeField] private KeyCode rotateKey = KeyCode.R;

    [Header("Preview Colors")]
    [SerializeField] private Color validTint = new(0, 1, 0, 0.35f);
    [SerializeField] private Color invalidTint = new(1, 0, 0, 0.35f);

    [SerializeField] BuildingDefinition defToPlace;

    private BuildingDefinition currentDef;
    private GameObject previewInstance;
    private int rotationSteps; // 0..3 (90-degree steps)

    // Cache preview renderers so we can tint easily
    private Renderer[] previewRenderers;

    public void StartPlacing(BuildingDefinition def)
    {
        CancelPlacing();

        currentDef = def;
        rotationSteps = 0;

        // spawn preview
        previewInstance = Instantiate(def.prefab);
        previewInstance.name = $"PREVIEW_{def.name}";
        previewInstance.layer = gameObject.layer; // optional

        // disable colliders on preview so it doesn't interfere with raycasts
        foreach (var col in previewInstance.GetComponentsInChildren<Collider>())
            col.enabled = false;

        previewRenderers = previewInstance.GetComponentsInChildren<Renderer>();

        // if you provided a special preview material, apply it
        if (def.previewMaterial != null)
        {
            foreach (var r in previewRenderers)
                r.sharedMaterial = def.previewMaterial;
        }
    }

    public void CancelPlacing()
    {
        currentDef = null;
        if (previewInstance != null) Destroy(previewInstance);
        previewInstance = null;
        previewRenderers = null;
    }

    private void Update()
    {
        if (currentDef == null) return;

        if (Input.GetKeyDown(rotateKey) && currentDef.allowRotation)
            rotationSteps = (rotationSteps + 1) % 4;

        if (!TryGetMouseWorld(out Vector3 hit))
            return;

        Vector2Int anchorCell = grid.WorldToCell(hit);
        Vector3 snappedWorld = grid.CellToWorldCenter(anchorCell);
        Vector3 offsetAdjustPosition = snappedWorld + currentDef.offset; 


        // move + rotate preview
        previewInstance.transform.position = offsetAdjustPosition;
        previewInstance.transform.rotation = Quaternion.Euler(0, rotationSteps * 90f, 0);

        // validate footprint
        var footprint = GetFootprint(anchorCell, currentDef.size, rotationSteps);
        bool canPlace = CanPlace(footprint);

        TintPreview(canPlace);

        if (canPlace && Input.GetMouseButtonDown(0))
        {
            PlaceBuilding(offsetAdjustPosition, rotationSteps, footprint);
        }

        // right click cancels (optional)
        if (Input.GetMouseButtonDown(1))
            CancelPlacing();
    }

    private bool TryGetMouseWorld(out Vector3 hitPoint)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 9999f, groundMask))
        {
            hitPoint = hit.point;
            return true;
        }

        hitPoint = default;
        return false;
    }

    private static IEnumerable<Vector2Int> GetFootprint(Vector2Int anchor, Vector2Int size, int rotSteps)
    {
        bool swap = rotSteps % 2 != 0;
        int w = swap ? size.y : size.x;
        int d = swap ? size.x : size.y;

        for (int x = 0; x < w; x++)
        for (int z = 0; z < d; z++)
            yield return new Vector2Int(anchor.x + x, anchor.y + z);
    }

    private bool CanPlace(IEnumerable<Vector2Int> cells)
    {
        foreach (var c in cells)
        {
            if (!grid.IsInside(c)) return false;
            if (grid.IsOccupied(c)) return false;
        }
        return true;
    }

    private void PlaceBuilding(Vector3 worldPos, int rotSteps, IEnumerable<Vector2Int> footprint)
    {
        // mark occupied
        grid.SetOccupied(footprint, true);

        // spawn real building
        Instantiate(currentDef.prefab, worldPos, Quaternion.Euler(0, rotSteps * 90f, 0));

        // keep placing same building (Timberborn style)
        // or call CancelPlacing(); if you want one-and-done.
    }

    private void TintPreview(bool canPlace)
    {
        if (previewRenderers == null) return;

        Color tint = canPlace ? validTint : invalidTint;

        foreach (var r in previewRenderers)
        {
            // uses material instance so we don't tint the shared prefab material
            if (r.material.HasProperty("_Color"))
            {
                var c = r.material.color;
                r.material.color = new Color(tint.r, tint.g, tint.b, tint.a);
            }
        }
    }
}
