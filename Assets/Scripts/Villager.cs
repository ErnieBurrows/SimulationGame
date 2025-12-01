using UnityEngine;
using UnityEngine.EventSystems;


    // Todo: Create a class that holds the villages needs.
    // Give each villager and instance of the above class.
    // When click on a specific villager they will show UI that shows their needs.

    //Todo: Fix the system that drains and adds needs. The courtines are overlapping.
    // Todo: I honestly think I need to have a function called simulate needs or something. And it simulates all villager needs.

    
    // Begin creating AI for the village that will move to whatever it needs most.
    // Create a class for the objects that hold needs. i.e well.
    // This object will need to be drained or filled depending on what is happening.
public class Villager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler, IEndDragHandler
{
    [SerializeField] private VillagerNeedsController needsController;
    [SerializeField] private VillagerFloatingUI floatingUI;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float yOffset;

    private VillagerNeeds needs;

    private void Awake()
    {
        needs = new VillagerNeeds();

        needsController.Initialize(needs);
        
        floatingUI.Bind(needsController);
    }

    private bool isDragging = false;
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 200f, layerMask))
        {
            Vector3 newPos = hit.point;
            newPos.y += yOffset;

            transform.position = newPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 10f, layerMask))
        {
            yOffset = transform.position.y - hit.point.y;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Water"))
        {
            needs.isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Water"))
        {
            needs.isInWater = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        VillagerSelectionManager.Instance.Select(needsController);
    }  
}
