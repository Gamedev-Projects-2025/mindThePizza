using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pizzaManager : MonoBehaviour
{
    public Pizza myPizza;
    [SerializeField] private string target;
    private float ingredOffsetX;
    private float ingredOffsetY;
    [SerializeField] public int numOfIngred = 3;
    private int ingredCountSaver;

    [SerializeField] private Sprite tomatoSprite;
    [SerializeField] private Sprite tomatoSauceSprite;
    private Sprite originalPizzaSprite;

    private List<GameObject> ingredientClones = new List<GameObject>();

    void Start()
    {
        originalPizzaSprite = GetComponent<SpriteRenderer>().sprite;
        ingredCountSaver = numOfIngred;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            string ingredientName = collision.gameObject.GetComponent<Ingredient>().nameIngredient;
            myPizza.AddIngredient(ingredientName);

            GameObject ingredientClone = Instantiate(collision.gameObject);
            ingredientClones.Add(ingredientClone);

            Collider2D cloneCollider = ingredientClone.GetComponent<Collider2D>();
            if (cloneCollider != null)
            {
                Destroy(cloneCollider);
            }

            switch (numOfIngred % 4)
            {
                case 0:
                    ingredOffsetX = 2f;
                    ingredOffsetY = 0;
                    break;
                case 1:
                    ingredOffsetX = -2f;
                    ingredOffsetY = 0;
                    break;
                case 2:
                    ingredOffsetX = 0;
                    ingredOffsetY = 2f;
                    break;
                case 3:
                    ingredOffsetX = 0;
                    ingredOffsetY = -2f;
                    break;
            }
            numOfIngred--;

            SpriteRenderer originalSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer cloneSpriteRenderer = ingredientClone.GetComponent<SpriteRenderer>();

            if (originalSpriteRenderer != null && cloneSpriteRenderer != null)
            {
                cloneSpriteRenderer.sprite = originalSpriteRenderer.sprite;

                if (cloneSpriteRenderer.sprite == tomatoSprite)
                {
                    cloneSpriteRenderer.sprite = tomatoSauceSprite;
                    GetComponent<SpriteRenderer>().sprite = tomatoSauceSprite;
                    Destroy(ingredientClone);
                }
                else
                {
                    ingredientClone.transform.position = new Vector3(gameObject.transform.position.x + ingredOffsetX, gameObject.transform.position.y + ingredOffsetY, -1f);
                }

                ingredientClone.transform.localScale *= 2f;
            }

            Destroy(collision.gameObject);
        }
    }

    public void ClearPizza()
    {
        StartCoroutine(ClearPizzaWithDelay(0.15f));
    }

    private IEnumerator ClearPizzaWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        myPizza.ClearIngredients();

        foreach (GameObject clone in ingredientClones)
        {
            Destroy(clone);
        }
        ingredientClones.Clear();

        GetComponent<SpriteRenderer>().sprite = originalPizzaSprite;
        numOfIngred = ingredCountSaver;
    }
}
