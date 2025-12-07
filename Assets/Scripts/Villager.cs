using UnityEngine;
using UnityEngine.EventSystems;

    // Todo: Begin creating AI for the village that will move to whatever it needs most.
    // Todo: Create a class for the objects that hold needs. i.e well.
    // Todo: This object will need to be drained or filled depending on what is happening.
    // Todo: Create and actual AI controller for the character and move the stuff out of VillagerNeedsController.
    // Todo: Create SO data objects that we can use to init the villager Data.
    // Todo: Reverse the "thirst" float in VillagerNeeds. Because a higher number should indicate more thirsty not less.
    // BUG: If the villager runs out of water while inside the collider they will bug out and just kinda be stuck there.

    // AI
    // Todo: Find the closest needed resource that has resources in it.
    // Todo: Figure out how running low/ out of player needs will affect the player.
    // Todo: Add resource limits to resource building.
    // Todo: Add some sort of command list or something for villager jobs
    // Todo: Lets get the villagers brining water from a lake to the buildings.
    // Todo: Add button that allows players to choose AI choice making i.e go to closest building, go to building with most resource etc.

    //UI
    // Todo: Create a script that makes in world UI look at the camera.
    

public class Villager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private VillagerFloatingUI floatingUI;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float yOffset;

    public VillagerNeedsController needsController;
    public VillagerNeeds needs;

    private void Awake()
    {
        needs = new VillagerNeeds();

        needsController.Initialize(needs);
        
        floatingUI.Bind(needsController);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        VillagerSelectionManager.Instance.Select(needsController);
    }  
}
