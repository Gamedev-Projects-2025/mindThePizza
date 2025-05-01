using UnityEngine;
using System.Linq;
using System.Collections;

public class PizzaChecker : MonoBehaviour
{
    [SerializeField] private GameObject pizzaObjectToCheck;
    [SerializeField] private GameObject perfectPizzaObject;
    [SerializeField] private string gameoverScene;

    private Pizza pizzaToCheck;
    private Pizza perfectPizza;
    private PerfectPizzaManager perfectPizzaManagerScript;
    bool hadIngredient = false;
    private float startTime;

    public SlideItem slideObject;
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

        startTime = Time.time; // Start timer
    }

    public IEnumerator Check()
    {
        foreach (Ingredient ingredient in perfectPizza.GetIngredients())
        {
            if (pizzaToCheck.GetIngredients().Last<Ingredient>().nameIngredient == ingredient.nameIngredient)
            {
                Debug.Log("found it!");
                hadIngredient = true;
            }
            Debug.Log(pizzaToCheck.GetIngredients().Last<Ingredient>().nameIngredient + "=/=" + ingredient.nameIngredient);
        }
        if (!hadIngredient)
        {
            gameManager.timesPutWrongIngredient++;
            Debug.Log("oh no");

        }
        hadIngredient = false;

        if (pizzaToCheck.GetIngredients().Count == perfectPizza.GetIngredients().Count)
        {
            yield return StartCoroutine(slideObject.SlideRoutine());
            if (perfectPizza.CompareTo(pizzaToCheck))
            {
                gameManager.piesMade++;

                // Calculate time taken for this successful pie
                float timeTaken = Time.time - startTime;
                gameManager.timeTakenToAssemble = (int)(gameManager.timeTakenToAssemble + timeTaken) / gameManager.piesMade;

                // Restart timer
                startTime = Time.time;

                pizzaObjectToCheck.GetComponent<pizzaManager>().ClearPizza();
                perfectPizzaManagerScript.CheckPizzaCorrect();
            }
            else
            {
                gameManager.piesFailed++;
                pizzaObjectToCheck.GetComponent<pizzaManager>().ClearPizza();
                //SceneManager.LoadSceneAsync(gameoverScene);
            }
        }

    }
}
