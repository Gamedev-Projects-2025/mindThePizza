using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.U2D;

public class PerfectPizzaManager : MonoBehaviour
{
    public pizzaManager displayPizza;
    [SerializeField] private int numOfIngredients = 3;
    [SerializeField] private int pizzasToMake = 3;
    [SerializeField] private string victoryScene;


    [SerializeField] private List<GameObject> ingredients;

    private List<string> lastPizzaIngredients = new List<string>();

    void Start()
    {
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
        Debug.Log("NEWPIZZA");
        ClearPreviousIngredients();

        List<string> selectedIngredients;
        int attempts = 0, maxAttempts = 5;

        do
        {
            selectedIngredients = new List<string> { "sauce" };
            int extra = Random.Range(1, numOfIngredients);

            List<string> ingredientNames = ingredients
                .OrderBy(x => Random.value)
                .Take(extra)
                .Select(x => x.GetComponent<Ingredient>().getName())
                .ToList();

            selectedIngredients.AddRange(ingredientNames);
            selectedIngredients.Sort();
            attempts++;

        } while (selectedIngredients.SequenceEqual(lastPizzaIngredients) && attempts < maxAttempts);

        lastPizzaIngredients = selectedIngredients;

        for (int i = 0; i < selectedIngredients.Count; i++)
        {
            int index = ingredients.FindIndex(x => x.GetComponent<Ingredient>().getName() == selectedIngredients[i]);
            if (index >= 0)
            {
                AddIngredientManually(index);
            }
            else
            {
                Debug.LogError($"Ingredient not found: {selectedIngredients[i]}");
            }
        }
    }


    private void AddIngredientManually(int index)
    {
        displayPizza.AddIngredientManually(ingredients[index].GetComponent<Ingredient>());
    }

    private void ClearPreviousIngredients()
    {
        displayPizza.ClearPizza();
    }
}
