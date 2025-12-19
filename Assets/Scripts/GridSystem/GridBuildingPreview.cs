using System.Collections.Generic;
using UnityEngine;

public class GridBuildingPreview : MonoBehaviour
{
    public enum BuildingPreviewState
    {
        Positive,
        Negative
    }
    [SerializeField] private Material positiveMaterial, negativeMaterial;

    public BuildingPreviewState State {get; private set;} = BuildingPreviewState.Negative;
    public GridBuildingData Data {get; private set;}
    public BuildingModel BuildingModel {get; private set;}
    private List<Renderer> renderers = new();
    private List<Collider> colliders = new();

    public void Setup(GridBuildingData data)
    {
        Data = data;
        BuildingModel = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        renderers.AddRange(BuildingModel.GetComponentsInChildren<Renderer>());
        colliders.AddRange(BuildingModel.GetComponentsInChildren<Collider>());

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        SetPreviewMaterial(State);
    }

    public void ChangeState(BuildingPreviewState newState)
    {
        if (newState == State) return;
        State = newState;
        SetPreviewMaterial(State);
    }

    public void Rotate(int rotationStep)
    {
        BuildingModel.Rotate(rotationStep);
    }

    private void SetPreviewMaterial(BuildingPreviewState newState)
    {
        Material previewMaterial = newState == BuildingPreviewState.Positive ? positiveMaterial : negativeMaterial;

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = new Material[renderer.sharedMaterials.Length];

            for ( int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterial;
            }
            renderer.materials = materials;
        }
    }
}
