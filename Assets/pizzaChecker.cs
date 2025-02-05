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
        if (perfectPizza.GetIngredients().Count == pizzaToCheck.GetIngredients().Count)
        {
            if (perfectPizza.CompareTo(pizzaToCheck))
            {
                pizzaObjectToCheck.GetComponent<pizzaManager>().ClearPizza(); // Clear pizza ingredients
                perfectPizzaManagerScript.CheckPizzaCorrect(); // Progress to next pizza
            }
            else
            {
                SceneManager.LoadSceneAsync(gameover);
            }
        }
    }
}