using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDefinition", menuName = "Scriptable Objects/BuildingDefinition")]
public class BuildingDefinition : ScriptableObject
{
    public GameObject prefab;
    public Vector2Int size = Vector2Int.one;
    public Vector3 offset = Vector3.zero; 
    public bool allowRotation = true;

    [Header("Preview")]
    public Material previewMaterial;
}
