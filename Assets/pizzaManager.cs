using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pizzaManager : MonoBehaviour
{
    public Pizza myPizza;
    [SerializeField] private string targetTag;
    [SerializeField] private int maxIngredients = 3;

    private int ingredientCounter;
    private List<GameObject> ingredientClones = new List<GameObject>();
    private Sprite originalPizzaSprite;

    public PizzaChecker PizzaChecker;
    private Vector2[] ingredientPositions = new Vector2[]
    {
        new Vector2(-1.5f, 1.5f),
        new Vector2(1.5f, 1.5f),
        new Vector2(0f, -1.5f),
        new Vector2(0f, 0f)
    };

    void Start()
    {
        originalPizzaSprite = GetComponent<SpriteRenderer>().sprite;
        ingredientCounter = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                AddIngredientManually(spriteRenderer.sprite);
                Destroy(collision.gameObject);
                PizzaChecker.check();
            }
        }
    }

    public void AddIngredientManually(Sprite ingredientSprite)
    {
        if (ingredientCounter >= maxIngredients) return;

        myPizza.AddIngredient(ingredientSprite.name);

        GameObject clone = new GameObject("IngredientClone");
        clone.transform.position = (Vector2)transform.position + ingredientPositions[ingredientCounter % ingredientPositions.Length];
        clone.transform.localScale = Vector3.one * 0.5f;
        clone.AddComponent<SpriteRenderer>().sprite = ingredientSprite;

        ingredientClones.Add(clone);
        ingredientCounter++;
    }

    public void ClearPizza()
    {
        myPizza.ClearIngredients();
        ingredientCounter = 0;

        foreach (var clone in ingredientClones)
        {
            Destroy(clone);
        }
        ingredientClones.Clear();
        GetComponent<SpriteRenderer>().sprite = originalPizzaSprite;
    }

}
