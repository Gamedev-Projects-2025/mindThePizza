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
    [SerializeField] private GameObject flashingPizzaObject;
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

    // FlashingPizzaObject sprite name → Bar sprite name
    private readonly Dictionary<string, string> flashingSpriteMap = new Dictionary<string, string>()
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

    //private IEnumerator FlashHint()
    //{
    //    Debug.Log("entered FlashHint");

    //    if (pizzaObjectToCheck == null || perfectPizzaObject == null)
    //    {
    //        Debug.LogWarning("Missing required GameObjects");
    //        yield break;
    //    }

    //    var placedIngredients = new HashSet<string>(
    //        pizzaObjectToCheck.GetComponent<pizzaManager>().myPizza.GetIngredients().Select(i => i.nameIngredient)
    //    );

    //    var neededIngredients = perfectPizzaObject.GetComponent<PerfectPizzaManager>()
    //                                              .displayPizza.myPizza.GetIngredients();

    //    var missingIngredients = neededIngredients
    //        .Where(ingredient => !placedIngredients.Contains(ingredient.nameIngredient))
    //        .Select(ingredient => ingredient.ingredientSprite.GetComponent<SpriteRenderer>().sprite.name)
    //        .ToList();

    //    Debug.Log("Missing Ingredients: " + string.Join(", ", missingIngredients));

    //    // Map missing ingredient sprite names to bar button names
    //    var mappedBarNames = new HashSet<string>();
    //    foreach (var missing in missingIngredients)
    //    {
    //        if (spriteNameMap.TryGetValue(missing, out var barName))
    //        {
    //            mappedBarNames.Add(barName);
    //            Debug.Log($"Mapped missing: {missing} → {barName}");
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"No spriteNameMap match for missing: {missing}");
    //        }
    //    }

    //    // Find bar sprites to flash
    //    var flashTargets = allBarIngredients
    //        .Where(sr => sr != null && sr.sprite != null && mappedBarNames.Contains(sr.sprite.name))
    //        .ToList();

    //    Debug.Log($"Total flash targets: {flashTargets.Count}");

    //    // Flash
    //    for (int i = 0; i < hintFlashCount; i++)
    //    {
    //        foreach (var sr in flashTargets)
    //            sr.enabled = false;

    //        yield return new WaitForSeconds(hintFlashTime);

    //        foreach (var sr in flashTargets)
    //            sr.enabled = true;

    //        yield return new WaitForSeconds(hintFlashTime);
    //    }
    //}

    private IEnumerator FlashHint()
    {
        Debug.Log("entered FlashHint");

        if (flashingPizzaObject == null || allBarIngredients == null)
        {
            Debug.LogWarning("Missing flashingPizzaObject or allBarIngredients");
            yield break;
        }

        var pizzaManager = flashingPizzaObject.GetComponent<pizzaManager>();
        if (pizzaManager == null)
        {
            Debug.LogWarning("pizzaManager missing on flashingPizzaObject");
            yield break;
        }

        // Extract ingredients (the ones that should flash)
        var flashIngredientObjects = pizzaManager.GetIngredientClones();
        var flashNames = new List<string>();

        foreach (var obj in flashIngredientObjects)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                string spriteName = sr.sprite.name;
                flashNames.Add(spriteName);
                Debug.Log($"Need to flash ingredient sprite: {spriteName}");
            }
        }

        // Map ingredient sprite names to bar sprite names
        var mappedBarNames = new HashSet<string>();
        foreach (var name in flashNames)
        {
            if (flashingSpriteMap.TryGetValue(name, out var mapped))
            {
                mappedBarNames.Add(mapped);
                Debug.Log($"Mapped {name} → {mapped}");
            }
            else
            {
                Debug.LogWarning($"No mapping found for: {name}");
            }
        }

        // Find matching bar ingredient sprites
        var flashTargets = new List<SpriteRenderer>();
        foreach (var sr in allBarIngredients)
        {
            if (sr != null && sr.sprite != null && mappedBarNames.Contains(sr.sprite.name))
            {
                Debug.Log($"Matched bar sprite: {sr.sprite.name}");
                flashTargets.Add(sr);
            }
        }

        Debug.Log($"Total flash targets: {flashTargets.Count}");

        // Flash them
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
