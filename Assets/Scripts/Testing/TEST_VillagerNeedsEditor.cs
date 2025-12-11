using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TEST_VillagerNeeds))]
public class TEST_VillagerNeedsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TEST_VillagerNeeds needsTesting = (TEST_VillagerNeeds)target;

        if (GUILayout.Button("Drain Villagers Water"))
        {
            needsTesting.DrainVillagersWater();
        }

        if (GUILayout.Button("Drain Villagers Food"))
        {
            needsTesting.DrainVillagersFood();
        }


    }
}
