using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VillagerInspectorUI))]
public class VillagerInspectorUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VillagerInspectorUI inspectorUI = (VillagerInspectorUI)target;

        GUILayout.BeginHorizontal("box");

        if (GUILayout.Button("Show Needs Panel", GUILayout.Height(125)))
        {
            inspectorUI.ChangePanel(PanelType.Needs);
        }

        if (GUILayout.Button("Show Behaviour Panel", GUILayout.Height(125)))
        {
            inspectorUI.ChangePanel(PanelType.Behaviour);
        }

        GUILayout.EndHorizontal();

        DrawDefaultInspector();

        
    }

   

}
