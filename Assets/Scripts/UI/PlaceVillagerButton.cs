using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlaceVillagerButton : MonoBehaviour
{
    [SerializeField] private GameObject villagerPrefab;
    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SpawnVillager);
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(SpawnVillager);
    }

    private void SpawnVillager()
    {
        GameObject villager = Instantiate(villagerPrefab);
        villager.transform.position = Vector3.zero;
    }
}
