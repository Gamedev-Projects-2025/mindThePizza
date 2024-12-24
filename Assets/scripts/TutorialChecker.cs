using UnityEngine;
using TMPro;

public class TutorialChecker : MonoBehaviour
{
    [SerializeField] private GameObject pizzaObjectToCheck;
    [SerializeField] private Pizza perfectPizza;
    private Pizza pizzaToCheck;
    [SerializeField] private TMP_Text speechBubbleText; // Reference to the TextMeshPro text component
    [SerializeField] private string victoryMessage = "Victory! Perfect Pizza!";
    [SerializeField] private string gameoverMessage = "Game Over! Try Again!";

    void Start()
    {
        pizzaToCheck = pizzaObjectToCheck.GetComponent<pizzaManager>().myPizza;
    }

    void Update()
    {
        if (perfectPizza.GetIngredients().Count == pizzaToCheck.GetIngredients().Count)
        {
            if (perfectPizza.CompareTo(pizzaToCheck))
            {
                speechBubbleText.text = victoryMessage;
            }
            else
            {
                speechBubbleText.text = gameoverMessage;
            }
        }
        else if (perfectPizza.GetIngredients().Count < pizzaToCheck.GetIngredients().Count)
        {
            speechBubbleText.text = gameoverMessage;
        }
    }
}