using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PizzaChecker : MonoBehaviour
{
    [SerializeField] private GameObject pizzaObjectToCheck;
    [SerializeField] private GameObject perfectPizzaObject;
    [SerializeField] private string gameoverScene;

    private Pizza pizzaToCheck;
    private Pizza perfectPizza;
    private PerfectPizzaManager perfectPizzaManagerScript;
    private float startTime = 0;
    private bool hadIngredient = false;
    private int hint_timer = 0;
    
    public GameObject successImage;
    public GameObject failureImage;
    public float flashDuration = 2f;
    public float waitTime = 2f;
    public AudioClip right, wrong;
    public SlideItem slideObject;

    [SerializeField] private GameObject ingredientParentObject; // Bar object
    [SerializeField] private float hintFlashTime = 0.3f;
    [SerializeField] private int hintFlashCount = 6;
    [SerializeField] private int hintFailsCount = 3; // Times of failed attempts before flashing
    [SerializeField] private List<SpriteRenderer> allBarIngredients;

    // Clone sprite name → Bar sprite name
    private readonly Dictionary<string, string> spriteNameMap = new Dictionary<string, string>()
    {
        { "shroom_topping", "shroomSlice_0" },
        { "grated_cheese", "Gratedcheese_0" },
        { "olives_topping", "oliceSliced_0" },
        { "tomato_sauce_0", "Paste_0" },
        { "sausage_topping", "pep_0" },
        { "cornSprite_0", "corn" },
        { "pineappleSlices", "pine_0" }
    };

    void Start()
    {
        if (pizzaObjectToCheck == null || perfectPizzaObject == null)
        {
            Debug.LogError("Missing references!");
            return;
        }

        pizzaToCheck = pizzaObjectToCheck.GetComponent<pizzaManager>().myPizza;
        perfectPizza = perfectPizzaObject.GetComponent<PerfectPizzaManager>().displayPizza.myPizza;
        perfectPizzaManagerScript = perfectPizzaObject.GetComponent<PerfectPizzaManager>();

        startTime = Time.time;
    }

    public IEnumerator Check()
    {
        foreach (Ingredient ingredient in perfectPizza.GetIngredients())
        {
            if (pizzaToCheck.GetIngredients().Last().nameIngredient == ingredient.nameIngredient)
            {
                Debug.Log("found it!");
                hadIngredient = true;
            }
            Debug.Log($"{pizzaToCheck.GetIngredients().Last().nameIngredient} =/= {ingredient.nameIngredient}");
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
            gameObject.GetComponent<AudioSource>().clip = right;
            gameManager.piesMade++;

            float timeTaken = Time.time - startTime;
            gameManager.totalTime += timeTaken;
            gameManager.timeTakenToAssemble = (int)(gameManager.totalTime / gameManager.piesMade);
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
            startTime = Time.time;
            gameObject.GetComponent<AudioSource>().clip = wrong;
            gameManager.piesFailed++;

            StartCoroutine(FlashImage(failureImage));
            gameObject.GetComponent<AudioSource>().Play();

            hint_timer++;

            if (hint_timer % hintFailsCount == 0)
                yield return StartCoroutine(FlashHint());

            pizzaObjectToCheck.GetComponent<pizzaManager>().ClearPizza();
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

    private IEnumerator FlashHint()
    {
        Debug.Log("entered FlashHint");

        if (pizzaObjectToCheck == null || ingredientParentObject == null)
        {
            Debug.LogWarning("Missing required GameObjects");
            yield break;
        }

        var pizzaManager = pizzaObjectToCheck.GetComponent<pizzaManager>();
        if (pizzaManager == null)
        {
            Debug.LogWarning("pizzaManager component missing");
            yield break;
        }

        var clones = pizzaManager.GetIngredientClones();
        var mappedBarNames = new HashSet<string>();

        // Step 1: Map clone sprite names to bar sprite names
        foreach (var clone in clones)
        {
            var sr = clone.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                string cloneSpriteName = sr.sprite.name;
                Debug.Log($"Clone sprite name: {cloneSpriteName}");

                if (spriteNameMap.TryGetValue(cloneSpriteName, out string barSpriteName))
                {
                    Debug.Log($"Mapped {cloneSpriteName} → {barSpriteName}");
                    mappedBarNames.Add(barSpriteName);
                }
                else
                {
                    Debug.LogWarning($"No mapping found for: {cloneSpriteName}");
                }
            }
        }

        // Step 2: Find matching bar objects by sprite name
        var flashTargets = new List<SpriteRenderer>();
        //foreach (Transform child in ingredientParentObject.transform)
        //{
        //    var sr = child.GetComponent<SpriteRenderer>();
        //    if (sr != null && sr.sprite != null && mappedBarNames.Contains(sr.sprite.name))
        //    {
        //        Debug.Log($"Matched bar sprite: {sr.sprite.name} → {child.name}");
        //        flashTargets.Add(sr);
        //    }
        //}

        foreach (var sr in allBarIngredients)
        {
            if (sr != null && sr.sprite != null && mappedBarNames.Contains(sr.sprite.name))
            {
                Debug.Log($"Matched manually assigned bar sprite: {sr.sprite.name}");
                flashTargets.Add(sr);
            }
        }


        Debug.Log($"Total flash targets: {flashTargets.Count}");

        // Step 3: Flash by enabling/disabling sprite renderers
        for (int i = 0; i < hintFlashCount; i++)
        {
            foreach (var sr in flashTargets)
                sr.enabled = false;

            yield return new WaitForSeconds(hintFlashTime);

            foreach (var sr in flashTargets)
                sr.enabled = true;

            yield return new WaitForSeconds(hintFlashTime);
        }
    }
}
