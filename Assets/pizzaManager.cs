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

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is an ingredient
        if (other.CompareTag("Ingredient"))
        {
            myPizza.AddIngredient(other.gameObject.GetComponent<Ingredient>().nameIngredient);
            Destroy(other.gameObject); 
        }
    }
}
