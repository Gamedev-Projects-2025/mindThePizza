using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class PerfectPizzaManager : MonoBehaviour
{
    public pizzaManager displayPizza;
    [SerializeField] private int numOfIngredients = 3;
    [SerializeField] private int pizzasToMake = 3;
    [SerializeField] private string victoryScene;

    [SerializeField] private Sprite shroomSprite;
    [SerializeField] private Sprite sauceSprite;
    [SerializeField] private Sprite cheeseSprite;
    [SerializeField] private Sprite sausageSprite;

    private Dictionary<string, Sprite> ingredientSprites = new Dictionary<string, Sprite>();
    private List<string> allIngredients = new List<string> { "shroom", "cheese", "sausage" };
    private List<string> lastPizzaIngredients = new List<string>();

    void Start()
    {
        ingredientSprites["shroom"] = shroomSprite;
        ingredientSprites["sauce"] = sauceSprite;
        ingredientSprites["cheese"] = cheeseSprite;
        ingredientSprites["sausage"] = sausageSprite;
        GenerateRandomPerfectPizza();
    }

    public void CheckPizzaCorrect()
    {
        pizzasToMake--;
        Debug.Log("Correct pizza! Pizzas left: " + pizzasToMake);

        if (pizzasToMake <= 0)
        {
            Debug.Log("Victory! Loading scene: " + victoryScene);
            SceneManager.LoadScene(victoryScene);
        }
        else
        {
            GenerateRandomPerfectPizza();
        }
    }

    private void GenerateRandomPerfectPizza()
    {
        Debug.Log("NEWPIZA");
        ClearPreviousIngredients();

        List<string> selectedIngredients;
        int attempts = 0, maxAttempts = 5;

        do
        {
            selectedIngredients = new List<string> { "sauce" };
            int extra = Random.Range(1, numOfIngredients);
            selectedIngredients.AddRange(allIngredients.OrderBy(x => Random.value).Take(extra));

            selectedIngredients.Sort();
            attempts++;

        } while (selectedIngredients.SequenceEqual(lastPizzaIngredients) && attempts < maxAttempts);

        lastPizzaIngredients = selectedIngredients;

        for (int i = 0; i < selectedIngredients.Count; i++)
        {
            AddIngredientManually(selectedIngredients[i], i);
        }
    }

    private void AddIngredientManually(string ingredient, int index)
    {
        if (ingredientSprites.TryGetValue(ingredient, out Sprite sprite))
        {
            displayPizza.AddIngredientManually(sprite);
        }
        else
        {
            Debug.LogError($"Sprite not found for ingredient: {ingredient}");
        }
    }

    private void ClearPreviousIngredients()
    {
        displayPizza.ClearPizza();
    }
}
