using UnityEngine;

public class pizzaManager : MonoBehaviour
{
    public Pizza myPizza;
    [SerializeField] private string target;
    private float ingredOffsetX;
    private float ingredOffsetY;
    [SerializeField] public int numOfIngred = 3;


    void Start()
    {
    }

    void Update()
    {
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

            switch (numOfIngred % 4)
            {
                case 0:
                    ingredOffsetX = 2f;
                    ingredOffsetY = 0;
                    break;

                case 1:
                    ingredOffsetX = -2f;
                    ingredOffsetY = 0;
                    break;

                case 2:
                    ingredOffsetX = 0;
                    ingredOffsetY = 2f;
                    break;

                case 3:
                    ingredOffsetX = 0;
                    ingredOffsetY = -2f;
                    break;
            }
            numOfIngred--;

            // Copy the sprite renderer and set it to the center of the pizza
            SpriteRenderer originalSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer cloneSpriteRenderer = ingredientClone.GetComponent<SpriteRenderer>();

            if (originalSpriteRenderer != null && cloneSpriteRenderer != null)
            {
                cloneSpriteRenderer.sprite = originalSpriteRenderer.sprite; // Copy the sprite

                if (cloneSpriteRenderer.sprite.name == "tomato")
                {
                    cloneSpriteRenderer = Resources.Load<Sprite>("tomato_sauce");
                }

                // Position the clone at the center of the pizza (you can adjust the position as needed)
                ingredientClone.transform.position = new Vector3(gameObject.transform.position.x + ingredOffsetX, gameObject.transform.position.y + ingredOffsetY, -1f);

                // Scale up the clone (adjust the scale factor as needed)
                ingredientClone.transform.localScale = ingredientClone.transform.localScale * 3f;
            }

            // Destroy the original ingredient (collided object)
            Destroy(collision.gameObject);
        }
    }
}
