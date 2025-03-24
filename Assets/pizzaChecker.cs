using UnityEngine;
using UnityEngine.SceneManagement;

public class pizzaChecker : MonoBehaviour
{
    [SerializeField] private GameObject pizzaObjectToCheck;
    [SerializeField] private GameObject perfectPizzaObject;
    private Pizza perfectPizza;
    private Pizza pizzaToCheck;
    private perfectPizzaManager perfectPizzaManagerScript;
    [SerializeField] private string gameover;

    void Start()
    {
        pizzaToCheck = pizzaObjectToCheck.GetComponent<pizzaManager>().myPizza;
        perfectPizza = perfectPizzaObject.GetComponent<perfectPizzaManager>().myPizza;
        perfectPizzaManagerScript = perfectPizzaObject.GetComponent<perfectPizzaManager>(); // Reference to perfectPizzaManager
    }

    void Update()
    {
        Debug.Log("Current pizza ingredients: " + string.Join(", ", pizzaToCheck.GetIngredients()));
        Debug.Log("Perfect pizza ingredients: " + string.Join(", ", perfectPizza.GetIngredients()));

        if (perfectPizza.GetIngredients().Count == pizzaToCheck.GetIngredients().Count)
        {
            if (perfectPizza.CompareTo(pizzaToCheck))
            {
                pizzaObjectToCheck.GetComponent<pizzaManager>().ClearPizza(); // Clear pizza ingredients
                perfectPizzaManagerScript.CheckPizzaCorrect(); // Progress to next pizza
            }
            else
            {
                Debug.Log("Pizza is incorrect. Loading gameover...");
                SceneManager.LoadSceneAsync(gameover);
            }
        }
    }
}
