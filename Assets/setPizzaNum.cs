using UnityEngine;
using UnityEngine.UI;

public class setPizzaNum : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider slider;
    void Start()
    {
        
    }

    public void setPizzaNumSlider()
    {
        gameManager.numberOfPizzas = (int)slider.value;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
