using UnityEngine;

public class Ingredient : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string nameIngredient;
    public GameObject ingredientSprite;

    public Ingredient(Ingredient ingredient)
    {
        nameIngredient = ingredient.nameIngredient;
        ingredientSprite = ingredient.ingredientSprite;
    }
    public string getName()
    {
        return nameIngredient;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
