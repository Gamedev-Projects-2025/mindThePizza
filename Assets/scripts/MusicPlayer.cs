using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Kill duplicate
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }
}
