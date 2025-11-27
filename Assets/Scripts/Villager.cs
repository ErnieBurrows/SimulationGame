using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class Villager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float yOffset;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 10f, layerMask))
        {
            yOffset = transform.position.y - hit.point.y;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hit");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
