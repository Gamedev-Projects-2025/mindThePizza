using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class perfectPizzaManager : MonoBehaviour
{
    public Pizza myPizza;
    [SerializeField] public int numOfIngred = 3;
    [SerializeField] public int pizzasToMake = 3;
    [SerializeField] private string victoryScene;

    [SerializeField] private Sprite shroomSprite;
    [SerializeField] private Sprite sauceSprite;
    [SerializeField] private Sprite cheeseSprite;
    [SerializeField] private Sprite sausageSprite;

    private Dictionary<string, Sprite> ingredientSprites = new Dictionary<string, Sprite>();
    private List<string> allIngredients = new List<string> { "shroom", "cheese", "sausage" };

    private float ingredientYOffset = -1.3f;
    private List<string> lastPizzaIngredients = new List<string>();

    private bool canCheckPizza = true;

    void Start()
    {
        ingredientSprites["shroom"] = shroomSprite;
        ingredientSprites["sauce"] = sauceSprite;
        ingredientSprites["cheese"] = cheeseSprite;
        ingredientSprites["sausage"] = sausageSprite;

        GenerateRandomPerfectPizza();
    }

    public void CheckPizzaCorrect()
    {
        if (!canCheckPizza) return;

        List<string> currentIngredients = myPizza.GetIngredients();
        List<string> correctIngredients = new List<string>(lastPizzaIngredients);

        currentIngredients.Sort();
        correctIngredients.Sort();

        Debug.Log($"Current Pizza: {string.Join(", ", currentIngredients)}");
        Debug.Log($"Expected Pizza: {string.Join(", ", correctIngredients)}");

        if (currentIngredients.SequenceEqual(correctIngredients))
        {
            pizzasToMake--;
            Debug.Log("Correct pizza! Pizzas left: " + pizzasToMake);

            if (pizzasToMake <= 0)
            {
                Debug.Log("Victory! Loading scene: " + victoryScene);
                SceneManager.LoadScene(victoryScene);
            }
            else
            {
                StartCoroutine(DelayedGenerateNewPizza(0.5f));
            }
        }
        else
        {
            Debug.Log("Wrong pizza! Try again.");
        }
    }

    private IEnumerator DelayedGenerateNewPizza(float delay)
    {
        canCheckPizza = false;
        yield return new WaitForSeconds(delay);
        GenerateRandomPerfectPizza();
        canCheckPizza = true;
    }

    public void GenerateRandomPerfectPizza()
    {
        myPizza.ClearIngredients();
        ClearPreviousIngredientObjects();

        List<string> selectedIngredients;
        int maxRetries = 5;
        int attempts = 0;

        do
        {
            selectedIngredients = new List<string> { "sauce" };
            int extraIngredientsToAdd = GetBiasedRandomIngredientCount();
            List<string> shuffledExtras = allIngredients.OrderBy(x => Random.value).Take(extraIngredientsToAdd).ToList();
            selectedIngredients.AddRange(shuffledExtras);

            selectedIngredients.Sort(); // Sort before checking for duplicates

            attempts++;
            if (attempts > maxRetries)
            {
                Debug.LogWarning("Too many repeated pizza attempts, forcing new one.");
                break;
            }

        } while (selectedIngredients.SequenceEqual(lastPizzaIngredients));

        lastPizzaIngredients = new List<string>(selectedIngredients);
        Debug.Log($"Perfect pizza will have {selectedIngredients.Count} ingredients: {string.Join(", ", selectedIngredients)}");

        for (int i = 0; i < selectedIngredients.Count; i++)
        {
            AddIngredientManually(selectedIngredients[i], i);
        }
    }

    int GetBiasedRandomIngredientCount()
    {
        int maxPossible = Mathf.Min(numOfIngred - 1, allIngredients.Count);
        List<int> possibleCounts = Enumerable.Range(0, maxPossible + 1).ToList();
        List<int> weightedList = new List<int>();

        for (int i = 0; i < possibleCounts.Count; i++)
        {
            int weight = (i + 1);
            weightedList.AddRange(Enumerable.Repeat(possibleCounts[i], weight));
        }

        return weightedList[Random.Range(0, weightedList.Count)];
    }

    void AddIngredientManually(string ingredientName, int index)
    {
        if (!ingredientSprites.ContainsKey(ingredientName))
        {
            Debug.LogError("No sprite found for ingredient: " + ingredientName);
            return;
        }

        myPizza.AddIngredient(ingredientName);
        Debug.Log("Added ingredient: " + ingredientName);

        GameObject ingredientObj = new GameObject(ingredientName);
        SpriteRenderer sr = ingredientObj.AddComponent<SpriteRenderer>();
        sr.sprite = ingredientSprites[ingredientName];

        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 20;

        ingredientObj.transform.position = transform.position + new Vector3(0, ingredientYOffset * index, 0f);

        if (ingredientName == "sauce")
            ingredientObj.transform.localScale = Vector3.one * 0.2f;
        else if (ingredientName == "sausage")
            ingredientObj.transform.localScale = Vector3.one * 0.35f;
        else
            ingredientObj.transform.localScale = Vector3.one * 0.5f;

        ingredientObj.transform.parent = transform;
    }

    void ClearPreviousIngredientObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
