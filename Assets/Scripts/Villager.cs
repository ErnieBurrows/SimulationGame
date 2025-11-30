using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

    // Todo: Create a class that holds the villages needs.
    // Give each villager and instance of the above class.
    // When click on a specific villager they will show UI that shows their needs.

    //Todo: Fix the system that drains and adds needs. The courtines are overlapping.

    
    // Begin creating AI for the village that will move to whatever it needs most.
    // Create a class for the objects that hold needs. i.e well.
    // This object will need to be drained or filled depending on what is happening.

[System.Serializable]
public class VillagerNeeds
{
    public bool isInWater = false;

    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private Image waterImage;

    public float thirst = 1;
    private float hunger = 1;

    [Tooltip("The wait time between village resource draining")]
    public float tickRate;

    [Tooltip("The percentage (Measured in decimal) that the water drains per tick")]
    public float thirstDrainRate, hungerDrainRate;

    public void ShowUI()
    {
        if (uiCanvas == null) return;

        uiCanvas.SetActive(true);

        
    }

    public IEnumerator UpdateVillagerUI()
    {
        while(true)
        {
            waterImage.fillAmount = thirst;
            yield return new WaitForSeconds(tickRate);
        }
    }

    public void HideUI()
    {
        if (uiCanvas == null) return;

        uiCanvas.SetActive(false);     
    }

    public IEnumerator RemoveWaterFill( )
    {
        while(thirst > 0 && !isInWater)
        {
            thirst -= thirstDrainRate;
            
            yield return new WaitForSeconds(tickRate);
        }

    }

    public IEnumerator AddWaterFill( )
    {
        while(thirst < 1 && isInWater)
        {
            thirst += thirstDrainRate;
            yield return new WaitForSeconds(tickRate);
        }

    }
}

public class Villager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler, IEndDragHandler
{
    #region Character Dragging
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float yOffset;

    private bool isDragging = false;
    private bool isUIOpen = false;
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

    #endregion

    #region Villager needs
    // Needs Section

    public VillagerNeeds needs;
    


    private void Start()
    {
        StartCoroutine(needs.RemoveWaterFill());
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Water"))
        {
            needs.isInWater = true;
            StartCoroutine(needs.AddWaterFill());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Water"))
        {
            needs.isInWater = false;
            StartCoroutine(needs.RemoveWaterFill());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isUIOpen)
        {
            isUIOpen = true;
            needs.ShowUI();
            StartCoroutine(needs.UpdateVillagerUI());
        }
        else
        {
            isUIOpen = false;
            needs.HideUI();
            StopCoroutine(needs.UpdateVillagerUI());
        }
    }

  


    #endregion
}
