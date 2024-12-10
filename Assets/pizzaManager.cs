using UnityEngine;

public class pizzaManager : MonoBehaviour
{
    public Pizza myPizza;
    [SerializeField] private string target;

    void Start()
    {
        // Initialization if needed
    }

    void Update()
    {
        // Update logic if needed
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            // Add ingredient to pizza
            myPizza.AddIngredient(collision.gameObject.GetComponent<Ingredient>().nameIngredient);

            // Clone the collided object
            GameObject ingredientClone = Instantiate(collision.gameObject);

            // Remove the collider from the cloned object
            Collider2D cloneCollider = ingredientClone.GetComponent<Collider2D>();
            if (cloneCollider != null)
            {
                Destroy(cloneCollider); // or cloneCollider.enabled = false;
            }

            // Copy the sprite renderer and set it to the center of the pizza
            SpriteRenderer originalSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer cloneSpriteRenderer = ingredientClone.GetComponent<SpriteRenderer>();

            if (originalSpriteRenderer != null && cloneSpriteRenderer != null)
            {
                cloneSpriteRenderer.sprite = originalSpriteRenderer.sprite; // Copy the sprite

                // Position the clone at the center of the pizza (you can adjust the position as needed)
                ingredientClone.transform.position = gameObject.transform.position;

                // Scale up the clone (adjust the scale factor as needed)
                ingredientClone.transform.localScale = ingredientClone.transform.localScale * 3f;
            }

            // Destroy the original ingredient (collided object)
            Destroy(collision.gameObject);
        }
    }
}
