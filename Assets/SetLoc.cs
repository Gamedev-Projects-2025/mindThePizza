using UnityEngine;

public class SetLoc : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!gameManager.left)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x*-1, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
