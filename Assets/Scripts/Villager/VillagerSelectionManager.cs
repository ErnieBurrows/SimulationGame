using UnityEngine;

public class VillagerSelectionManager : MonoBehaviour
{
    public static VillagerSelectionManager Instance;

    [SerializeField] private VillagerInspectorUI inspectorUI;

    private VillagerNeedsController current;

    private void Awake()
    {
        Instance = this;
    }

    public void Select(Villager villager)
    {
        // Case 1: Clicking the same villager = close inspector
        if (current == villager.needsController)
        {
            inspectorUI.Close();
            current = null;
            return;
        }

        // Case 2: Switching to a new villager
        current = villager.needsController;
        inspectorUI.Show(villager);
    }

    public void ClearSelection()
    {
        current = null;
        inspectorUI.Close();
    }
}
