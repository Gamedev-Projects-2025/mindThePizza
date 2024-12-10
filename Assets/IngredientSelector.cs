using UnityEngine;

public class IngredientSelector : MonoBehaviour
{
    public GameObject ingredientPrefab; // Assign the ingredient prefab in the Inspector
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
            currentIngredient.transform.position = worldPosition;
        }
    }

    private void OnMouseDown()
    {
        if (ingredientPrefab != null)
        {
            if (currentIngredient != null)
            {
                // Disable collider temporarily to avoid interference
                Collider2D currentCollider = currentIngredient.GetComponent<Collider2D>();
                if (currentCollider != null)
                {
                    currentCollider.enabled = false;
                }

                Destroy(currentIngredient);
            }
            currentIngredient = Instantiate(ingredientPrefab);

            // Re-enable collider after instantiation
            Collider2D newCollider = currentIngredient.GetComponent<Collider2D>();
            if (newCollider != null)
            {
                newCollider.enabled = true;
            }
        }
    }
}
