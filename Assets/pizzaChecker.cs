using UnityEngine;
using UnityEngine.SceneManagement;

public class pizzaChecker : MonoBehaviour
{

    [SerializeField] private GameObject pizzaObjectToCheck;
    [SerializeField] private Pizza perfectPizza;
    private Pizza pizzaToCheck;

    [SerializeField] private string victory, gameover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pizzaToCheck = pizzaObjectToCheck.GetComponent<pizzaManager>().myPizza;
    }

    // Update is called once per frame
    void Update()
    {
        if (perfectPizza.GetIngredients().Count == pizzaToCheck.GetIngredients().Count)
        {
            if (perfectPizza.CompareTo(pizzaToCheck))
            {
                SceneManager.LoadScene(victory);
            }
            else
            {
                SceneManager.LoadSceneAsync(gameover);
            }

        }
    }
}
