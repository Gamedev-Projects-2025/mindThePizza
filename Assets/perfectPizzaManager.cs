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
    public SetLocIngredient locManager;
    [SerializeField] private int numOfIngredientsMax = 3;
    [SerializeField] private int numOfIngredientsMin = 1;

    [SerializeField] private int difficultyThreshold = 3;
    [SerializeField] private int minNumOfIngredientsUpper = 3;
    [SerializeField] private int minNumOfIngredientsLower = 1;
    [SerializeField] private int maxNumOfIngredientsUpper = 6;
    [SerializeField] private int maxNumOfIngredientsLower = 5;
    [SerializeField] private string victoryScene;


    [SerializeField] private List<GameObject> ingredients;

    private List<string> lastPizzaIngredients = new List<string>();

    void Start()
    {
        GenerateRandomPerfectPizza();
    }

    public void CheckPizzaCorrect()
    {
        GenerateRandomPerfectPizza();
        if (gameManager.scramble)
        {
            locManager.setLocation();
        }

        numOfIngredientsMin = Mathf.Min(minNumOfIngredientsLower + gameManager.piesMade / difficultyThreshold, maxNumOfIngredientsLower);
        numOfIngredientsMax = Mathf.Min(minNumOfIngredientsUpper + gameManager.piesMade / difficultyThreshold, maxNumOfIngredientsUpper);
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
            int extra = Random.Range(numOfIngredientsMin, numOfIngredientsMax);

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