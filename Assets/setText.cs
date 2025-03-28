using TMPro;
using UnityEngine;

public class setText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = gameManager.GetStatsString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
