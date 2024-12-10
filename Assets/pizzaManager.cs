using UnityEngine;

public class pizzaManager : MonoBehaviour
{
    public Pizza myPizza;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering is an ingredient
        Debug.Log("DETECTED");
        if (collision.CompareTag("Ingredient"))
        {
            myPizza.AddIngredient(collision.gameObject.GetComponent<Ingredient>().nameIngredient);
            Destroy(collision.gameObject);
        }
    }
}
