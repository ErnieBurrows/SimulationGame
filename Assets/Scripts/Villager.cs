using UnityEngine;
using UnityEngine.EventSystems;

public class Villager : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hit");
    }

}
