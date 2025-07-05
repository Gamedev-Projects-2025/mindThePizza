using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PizzaCheckerMulti : MonoBehaviour
{
    [SerializeField] private List<GameObject> pizzaObjectToCheck;
    [SerializeField] private List<GameObject> perfectPizzaObject;
    [SerializeField] private List<GameObject> diplayPizzaObject;

    [SerializeField] private string gameoverScene;

    private List<Pizza> pizzaToCheck;
    private List<Pizza> perfectPizza;
    private List<PerfectPizzaManager> perfectPizzaManagerScript;

    private float startTime = 0;
    private int hint_timer = 0;

    public GameObject successImage;
    public GameObject failureImage;
    public float flashDuration = 2f;
    public float waitTime = 2f;
    public AudioClip right, wrong;
    public SlideItem slideObject;

    [SerializeField] private GameObject ingredientParentObject;
    [SerializeField] private float hintFlashTime = 0.3f;
    [SerializeField] private int hintFlashCount = 6;
    [SerializeField] private int flashFailsCount = 3;
    [SerializeField] private GameObject flashingPizzaObject;
    [SerializeField] private List<SpriteRenderer> allBarIngredients;

    [SerializeField] private int audioFailsCount = 1;
    [SerializeField] private AudioSource hintAudioSource;

    [System.Serializable]
    public class IngredientAudioEntry
    {
        public string ingredientSpriteName;
        public AudioClip clip;
    }
    [SerializeField] private List<IngredientAudioEntry> ingredientAudioList;

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

    private Dictionary<string, AudioClip> ingredientAudioMap = new Dictionary<string, AudioClip>()
    {
        { "shroom_topping", null },
        { "grated_cheese", null },
        { "olives_topping", null },
        { "tomato_sauce_0", null },
        { "sausage_topping", null },
        { "cornSprite_0", null },
        { "pineappleSlices", null }
    };

    void Start()
    {
        pizzaToCheck = new List<Pizza>();
        perfectPizza = new List<Pizza>();
        perfectPizzaManagerScript = new List<PerfectPizzaManager>();

        if (pizzaObjectToCheck == null || perfectPizzaObject == null)
        {
            Debug.LogError("Missing references!");
            return;
        }

        foreach (GameObject pizzaObject in pizzaObjectToCheck)
            pizzaToCheck.Add(pizzaObject.GetComponent<pizzaManager>().myPizza);

        foreach (GameObject pizzaObjectPerfect in perfectPizzaObject)
        {
            perfectPizza.Add(pizzaObjectPerfect.GetComponent<PerfectPizzaManager>().displayPizza.myPizza);
            perfectPizzaManagerScript.Add(pizzaObjectPerfect.GetComponent<PerfectPizzaManager>());
        }

        startTime = Time.time;

        foreach (var entry in ingredientAudioList)
        {
            if (!string.IsNullOrEmpty(entry.ingredientSpriteName) && entry.clip != null)
                ingredientAudioMap[entry.ingredientSpriteName] = entry.clip;
        }

        // ✅ Enforce numberOfPizzas limit
        int limit = Mathf.Min(gameManager.numberOfPizzas, pizzaToCheck.Count, perfectPizza.Count);

        // Destroy excess player pizzas
        for (int i = pizzaObjectToCheck.Count - 1; i >= limit; i--)
        {
            Destroy(pizzaObjectToCheck[i]);
            pizzaObjectToCheck.RemoveAt(i);
            pizzaToCheck.RemoveAt(i);
        }

        // Destroy excess perfect pizzas
        for (int i = perfectPizzaObject.Count - 1; i >= limit; i--)
        {
            Destroy(perfectPizzaObject[i]);
            perfectPizzaObject.RemoveAt(i);
            perfectPizza.RemoveAt(i);
            perfectPizzaManagerScript.RemoveAt(i);
        }
    }


    public void checkButton()
    {
        StartCoroutine(Check());
    }

    public IEnumerator Check()
    {
        while (!AllPizzasAssembled())
            yield return null;

        yield return StartCoroutine(CheckFunction());
    }

    private bool AllPizzasAssembled()
    {
        for (int i = 0; i < pizzaToCheck.Count; i++)
        {
            if (pizzaToCheck[i].GetIngredients().Count != perfectPizza[i].GetIngredients().Count)
                return false;
        }
        return true;
    }

    public IEnumerator CheckFunction()
    {
        yield return StartCoroutine(slideObject.SlideRoutineUP());

        bool allCorrect = true;

        for (int i = 0; i < pizzaToCheck.Count; i++)
        {
            if (!pizzaToCheck[i].Equals(perfectPizza[i]))
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            gameObject.GetComponent<AudioSource>().clip = right;
            gameManager.piesMade++;

            float timeTaken = Time.time - startTime;
            gameManager.totalTime += timeTaken;
            gameManager.timeTakenToAssemble = (int)(gameManager.totalTime / gameManager.piesMade);
            startTime = Time.time;

            foreach (var pizza in pizzaObjectToCheck)
                pizza.GetComponent<pizzaManager>().ClearPizza();

            foreach (var ppm in perfectPizzaManagerScript)
                ppm.CheckPizzaCorrect();

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

            if (hint_timer % audioFailsCount == 0)
                yield return StartCoroutine(PlayAudioHint());

            if (hint_timer % flashFailsCount == 0)
                yield return StartCoroutine(FlashHint());

            foreach (var pizza in pizzaObjectToCheck)
                pizza.GetComponent<pizzaManager>().ClearPizza();

            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(slideObject.SlideRoutineDOWN());
        }
    }

    private IEnumerator FlashImage(GameObject image)
    {
        image.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        image.SetActive(false);
    }

    private IEnumerator FlashHint()
    {
        if (flashingPizzaObject == null || allBarIngredients == null)
            yield break;

        var pizzaManager = flashingPizzaObject.GetComponent<pizzaManager>();
        if (pizzaManager == null)
            yield break;

        var flashIngredientObjects = pizzaManager.GetIngredientClones();
        var flashNames = new List<string>();

        foreach (var obj in flashIngredientObjects)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
                flashNames.Add(sr.sprite.name);
        }

        var mappedBarNames = new HashSet<string>();
        foreach (var name in flashNames)
        {
            if (flashingSpriteMap.TryGetValue(name, out var mapped))
                mappedBarNames.Add(mapped);
        }

        var flashTargets = new List<SpriteRenderer>();
        foreach (var sr in allBarIngredients)
        {
            if (sr != null && sr.sprite != null && mappedBarNames.Contains(sr.sprite.name))
                flashTargets.Add(sr);
        }

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

    private IEnumerator PlayAudioHint()
    {
        if (flashingPizzaObject == null || hintAudioSource == null || ingredientAudioMap == null)
            yield break;

        var pizzaManager = flashingPizzaObject.GetComponent<pizzaManager>();
        if (pizzaManager == null)
            yield break;

        var clones = pizzaManager.GetIngredientClones();
        if (clones.Count == 0)
            yield break;

        foreach (var clone in clones)
        {
            var sr = clone.GetComponent<SpriteRenderer>();
            if (sr == null || sr.sprite == null)
                continue;

            string spriteName = sr.sprite.name;

            if (ingredientAudioMap.TryGetValue(spriteName, out AudioClip clip) && clip != null)
            {
                hintAudioSource.Stop();
                hintAudioSource.clip = clip;
                hintAudioSource.Play();

                yield return new WaitWhile(() => hintAudioSource.isPlaying);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
