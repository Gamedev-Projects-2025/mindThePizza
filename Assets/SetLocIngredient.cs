using System.Collections.Generic;
using UnityEngine;

public class SetLocIngredient : MonoBehaviour
{
    public List<Transform> locations;
    public List<GameObject> ingredients;

    void Start()
    {
        setLocation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setLocation()
    {
        if (gameManager.scramble)
        {
            Shuffle(locations);
        }
        for (int i = 0; i < locations.Count; i++)
        {
            ingredients[i].transform.position = locations[i].transform.position;
        }
    }
    public static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
