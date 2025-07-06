using UnityEngine;

public class IngredientSelector : MonoBehaviour
{
    public GameObject ingredientPrefab;           // Assign the ingredient prefab
    public static GameObject currentIngredient;   // Currently selected ingredient
    public Transform spawnLocation;               // Assign this in the Inspector (where ingredient spawns)

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
    }

    private void OnMouseDown()
    {
        if (ingredientPrefab != null && spawnLocation != null)
        {
            UIAudioManager.Instance.PlayClick();

            if (currentIngredient != null)
            {
                Debug.Log("Holding something");
                Destroy(currentIngredient);
            }

            currentIngredient = Instantiate(ingredientPrefab, spawnLocation.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Missing ingredientPrefab or spawnLocation!");
        }
    }
}
