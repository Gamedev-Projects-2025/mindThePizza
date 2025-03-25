using UnityEngine;

public class IngredientSelector : MonoBehaviour
{
    public GameObject ingredientPrefab; // Assign the ingredient prefab in the Inspector
    public GameObject ingredientSprite;
    private static GameObject currentIngredient; // Tracks the currently selected ingredient
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (currentIngredient != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Set the distance to the camera
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            // Adjust Z position to avoid blocking clicks
            worldPosition.y += 0.75f;

            currentIngredient.transform.position = worldPosition;
        }
    }

    private void OnMouseDown()
    {
        if (ingredientPrefab != null)
        {
            if (currentIngredient != null)
            {
                Debug.Log("Holding something");
                Destroy(currentIngredient);
            }
            currentIngredient = Instantiate(ingredientPrefab);
        }
    }

}
