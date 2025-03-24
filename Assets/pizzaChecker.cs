using UnityEngine;
using UnityEngine.SceneManagement;

public class PizzaChecker : MonoBehaviour
{
    [SerializeField] private GameObject pizzaObjectToCheck;
    [SerializeField] private GameObject perfectPizzaObject;
    [SerializeField] private string gameoverScene;

    private Pizza pizzaToCheck;
    private Pizza perfectPizza;
    private PerfectPizzaManager perfectPizzaManagerScript;

    void Start()
    {
        if (pizzaObjectToCheck == null || perfectPizzaObject == null)
        {
            Debug.LogError("Missing GameObject references!");
            return;
        }

        pizzaToCheck = pizzaObjectToCheck.GetComponent<pizzaManager>().myPizza;
        perfectPizza = perfectPizzaObject.GetComponent<PerfectPizzaManager>().displayPizza.myPizza;
        perfectPizzaManagerScript = perfectPizzaObject.GetComponent<PerfectPizzaManager>();
    }

    public void check()
    {
        if (pizzaToCheck.GetIngredients().Count == perfectPizza.GetIngredients().Count)
        {
            if (perfectPizza.CompareTo(pizzaToCheck))
            {
                pizzaObjectToCheck.GetComponent<pizzaManager>().ClearPizza();
                perfectPizzaManagerScript.CheckPizzaCorrect();
            }
            else
            {
                SceneManager.LoadSceneAsync(gameoverScene);
            }
        }
    }
}
