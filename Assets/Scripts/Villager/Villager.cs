using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(VillagerNeedsController))]
public class Villager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private VillagerFloatingUI floatingUI;
    [SerializeField] private string villagerName = "Bob";

    public VillagerNeedsController needsController;
    public VillagerNeeds needs;
    public VillagerData data;

    private void Awake()
    {
        needsController = GetComponent<VillagerNeedsController>();

        if (needs == null) 
            needs = new VillagerNeeds();

        if (data == null) 
            data = new VillagerData(villagerName);

        needsController.Initialize(needs, data);

        if (floatingUI != null) 
            floatingUI.Bind(needsController);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        VillagerSelectionManager.Instance.Select(needsController);
    }
}
