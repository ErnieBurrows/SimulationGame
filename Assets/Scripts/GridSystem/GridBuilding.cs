using System.Data.Common;
using UnityEngine;

public class GridBuilding : MonoBehaviour
{
//     public string Description => data.Description;
//     public int Cost => data.Cost;
    private BuildingModel model;
    private GridBuildingData data;
    public void Setup(GridBuildingData data, float rotation)
    {
        this.data = data;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.Rotate(rotation);
    }
}
