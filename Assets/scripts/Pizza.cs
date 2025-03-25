using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pizza
{
    [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>(); // List of ingredient names

    // Add ingredient to the pizza
    public void AddIngredient(Ingredient ingredient)
    {
        if (!ingredients.Contains(ingredient))
        {
            ingredients.Add(ingredient);
            Debug.Log($"{ingredient} added to the pizza.");
        }
        else
        {
            Debug.Log($"{ingredient} is already on the pizza.");
        }
    }

    // Compare two pizzas based on their ingredients
    public bool CompareTo(Pizza otherPizza)
    {
        // Get the ingredient names for both pizzas
        List<string> currentPizzaNames = ingredients.Select(i => i.getName()).ToList();
        List<string> otherPizzaNames = otherPizza.ingredients.Select(i => i.getName()).ToList();

        // Print ingredients of both pizzas for debugging
        Debug.Log("Current Pizza Ingredients: " + string.Join(", ", currentPizzaNames));
        Debug.Log("Other Pizza Ingredients: " + string.Join(", ", otherPizzaNames));

        // Check if both pizzas have the same ingredient count
        if (currentPizzaNames.Count != otherPizzaNames.Count)
        {
            Debug.Log("Mismatch in ingredient count.");
            return false;
        }

        // Sort both lists to ensure order doesn't matter
        currentPizzaNames.Sort();
        otherPizzaNames.Sort();

        // Compare the sorted lists
        for (int i = 0; i < currentPizzaNames.Count; i++)
        {
            if (currentPizzaNames[i] != otherPizzaNames[i])
            {
                Debug.Log($"Mismatch found: {currentPizzaNames[i]} is not in the same place as {otherPizzaNames[i]}.");
                return false;
            }
        }

        Debug.Log("Pizzas are the same.");
        return true;
    }




    // To visualize the ingredients in the Unity Inspector
    public List<Ingredient> GetIngredients()
    {
        return ingredients;
    }

    public void ClearIngredients()
    {
        ingredients.Clear(); // Assuming 'ingredients' is the List storing the ingredient names
    }



}
