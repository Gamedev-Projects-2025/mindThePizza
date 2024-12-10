using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pizza
{
    [SerializeField] private List<string> ingredients = new List<string>(); // List of ingredient names

    // Add ingredient to the pizza
    public void AddIngredient(string ingredient)
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
        // Check if both pizzas have the same ingredients (unordered)
        if (ingredients.Count != otherPizza.ingredients.Count)
        {
            return false;
        }

        foreach (var ingredient in ingredients)
        {
            if (!otherPizza.ingredients.Contains(ingredient))
            {
                return false;
            }
        }

        return true;
    }

    // To visualize the ingredients in the Unity Inspector
    public List<string> GetIngredients()
    {
        return ingredients;
    }
}
