using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pizzaManager : MonoBehaviour
{
    public Pizza myPizza;
    [SerializeField] private string target;
    [SerializeField] public int numOfIngred = 3;
    private int ingredCountSaver;

    [Header("Sprites")]
    [SerializeField] private Sprite tomatoSprite;
    [SerializeField] private Sprite tomatoSauceSprite;
    [SerializeField] private Sprite cheeseSprite;
    [SerializeField] private Sprite shroomSprite;
    [SerializeField] private Sprite sausageSprite;
    [SerializeField] private Sprite oliveSprite;

    private Sprite originalPizzaSprite;
    private List<GameObject> ingredientClones = new List<GameObject>();

    void Start()
    {
        originalPizzaSprite = GetComponent<SpriteRenderer>().sprite;
        ingredCountSaver = numOfIngred;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            // Get the base ingredient name from the Ingredient script
            string ingredientName = collision.gameObject.GetComponent<Ingredient>().nameIngredient;

            // Ensure we don't add duplicates to the pizza
            if (!myPizza.GetIngredients().Contains(ingredientName))
            {
                myPizza.AddIngredient(ingredientName);
                Debug.Log("Added ingredient to pizza: " + ingredientName);
            }

            // Handle tomato (change the pizza base sprite to tomato sauce)
            if (ingredientName == "tomato")
            {
                // Check if the sauce is already added to avoid duplicates
                if (transform.Find("TomatoSauceTopping") == null)
                {
                    // Create a GameObject for tomato sauce on top of the pizza
                    GameObject tomatoSauceClone = new GameObject("TomatoSauceTopping");
                    tomatoSauceClone.transform.parent = this.transform;

                    // Add SpriteRenderer to the sauce GameObject and set its sprite
                    SpriteRenderer tomatoCloneSR = tomatoSauceClone.AddComponent<SpriteRenderer>();
                    tomatoCloneSR.sprite = tomatoSauceSprite;

                    // Set the sorting order to ensure it's on top of other ingredients
                    tomatoCloneSR.sortingOrder = 10;

                    // Position the sauce on top of the pizza, slightly scaled
                    tomatoSauceClone.transform.position = transform.position;
                    tomatoSauceClone.transform.localScale = Vector3.one * 1.0f; // Adjust scale to fit

                    // Add it to the clones list for future cleanup
                    ingredientClones.Add(tomatoSauceClone);
                }

                Destroy(collision.gameObject); // Destroy the original tomato object
                return; // Skip further processing for tomato
            }

            // For other ingredients, spawn them on top of the pizza
            GameObject ingredientClone = new GameObject(ingredientName + "_topping");
            ingredientClones.Add(ingredientClone);
            ingredientClone.transform.parent = this.transform;

            // Add SpriteRenderer to the clone
            SpriteRenderer sr = ingredientClone.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 20; // Ensure toppings show on top

            // Choose the appropriate sprite based on the ingredient name
            if (ingredientName == "cheese") sr.sprite = cheeseSprite;
            else if (ingredientName == "shroom") sr.sprite = shroomSprite;
            else if (ingredientName == "sausage") sr.sprite = sausageSprite;
            else if (ingredientName == "olive") sr.sprite = oliveSprite;
            else
            {
                Debug.LogWarning("No matching sprite for ingredient: " + ingredientName);
                Destroy(ingredientClone);
                Destroy(collision.gameObject);
                return;
            }

            // Random slight offset for ingredient placement on the pizza
            Vector2 randomOffset = new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f));
            ingredientClone.transform.position = transform.position + new Vector3(randomOffset.x, randomOffset.y, -0.1f);

            // Scale the ingredient to look good on the pizza
            ingredientClone.transform.localScale = Vector3.one * 0.4f;

            // Remove the ingredient object from the scene (we don't need it after cloning)
            Destroy(collision.gameObject);
        }
    }

    public void ClearPizza()
    {
        StartCoroutine(ClearPizzaWithDelay(0.15f));
    }

    private IEnumerator ClearPizzaWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        myPizza.ClearIngredients();  // Clear ingredients list

        // Destroy topping clones
        foreach (GameObject clone in ingredientClones)
        {
            Destroy(clone);
        }
        ingredientClones.Clear();

        // Reset pizza sprite to original
        GetComponent<SpriteRenderer>().sprite = originalPizzaSprite;
        numOfIngred = ingredCountSaver;
    }
}
