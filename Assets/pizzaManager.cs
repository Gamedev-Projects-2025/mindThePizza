using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pizzaManager : MonoBehaviour
{
    public Pizza myPizza;
    [SerializeField] private string targetTag;
    public GameObject parent;
    private int ingredientCounter;
    private List<GameObject> ingredientClones = new List<GameObject>();
    private Sprite originalPizzaSprite;

    public PizzaChecker PizzaChecker;

    void Start()
    {
        originalPizzaSprite = GetComponent<SpriteRenderer>().sprite;
        ingredientCounter = 0;
    }

    private IEnumerator OnMouseDown()
    {
        if (IngredientSelector.currentIngredient != null)
        {
            AddIngredientManually(IngredientSelector.currentIngredient.GetComponent<Ingredient>());
            Destroy(IngredientSelector.currentIngredient);
            if (PizzaChecker != null)
            {
                yield return StartCoroutine(PizzaChecker.Check());
            }
        }
    }

    public void AddIngredientManually(Ingredient ingredient)
    {

        Debug.Log(ingredient.ToString());
        myPizza.AddIngredient(ingredient);

        GameObject clone = new GameObject("IngredientClone");
        clone.transform.position = new Vector3(transform.position.x, transform.position.y, ingredient.ingredientSprite.GetComponent<Transform>().position.z);
        clone.transform.localScale = ingredient.ingredientSprite.GetComponent<Transform>().localScale;
        clone.AddComponent<SpriteRenderer>().sprite = ingredient.ingredientSprite.GetComponent<SpriteRenderer>().sprite;
        clone.GetComponent<SpriteRenderer>().color = ingredient.ingredientSprite.GetComponent<SpriteRenderer>().color;
        ingredientClones.Add(clone);
        ingredientCounter++;
        if (parent != null)
        {
            clone.transform.parent = parent.transform;

        }
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

    }

    public List<GameObject> GetIngredientClones()
    {
        return ingredientClones;
    }


}
