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
    private float startTime = 0;

    public GameObject successImage;
    public GameObject failureImage;
    public float flashDuration = 2f;
    public float waitTime = 2f;
    public AudioClip right, wrong;

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

        if ((pizzaToCheck.GetIngredients().Count == perfectPizza.GetIngredients().Count) && gameManager.autoDeliver)
        {
            yield return StartCoroutine(CheckFunction());
        }


    }
    public IEnumerator CheckFunction()
    {
        yield return StartCoroutine(slideObject.SlideRoutineUP());
        if (perfectPizza.CompareTo(pizzaToCheck))
        {
            gameObject.GetComponent<AudioSource>().resource = right;

            gameManager.piesMade++;

            // Calculate time taken for this successful pie
            float timeTaken = Time.time - startTime;
            gameManager.timeTakenToAssemble = (int)(gameManager.timeTakenToAssemble + timeTaken) / gameManager.piesMade;

            // Restart timer
            startTime = Time.time;

            pizzaObjectToCheck.GetComponent<pizzaManager>().ClearPizza();
            perfectPizzaManagerScript.CheckPizzaCorrect();
            StartCoroutine(FlashImage(successImage));
            gameObject.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(slideObject.SlideRoutineDOWN());
        }
        else
        {
            // Restart timer
            startTime = Time.time;
            gameObject.GetComponent<AudioSource>().resource = wrong;
            gameManager.piesFailed++;
            pizzaObjectToCheck.GetComponent<pizzaManager>().ClearPizza();
            //SceneManager.LoadSceneAsync(gameoverScene);
            StartCoroutine(FlashImage(failureImage));
            gameObject.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(slideObject.SlideRoutineDOWN());
        }
    }
    public void checkButton()
    {
        StartCoroutine(CheckFunction());
    }
    private IEnumerator FlashImage(GameObject image)
    {
        image.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        image.SetActive(false);
    }
}
