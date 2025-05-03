using System.Collections;
using UnityEngine;

public class SlideItem : MonoBehaviour
{
    public float slideDistance = 2f;       // How far to slide up
    public float slideDuration = 0.5f;     // How long the slide takes
    public float waitTime = 2f;
    private Vector3 originalPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void slide()
    {
        StartCoroutine(SlideRoutine());
    }

    public IEnumerator SlideRoutine()
    {
        Vector3 targetPosition = originalPosition + Vector3.up * slideDistance;
        yield return StartCoroutine(SlideToPosition(targetPosition));
        yield return new WaitForSeconds(waitTime);
        yield return StartCoroutine(SlideToPosition(originalPosition));
    }

    public IEnumerator SlideRoutineUP()
    {
        Vector3 targetPosition = originalPosition + Vector3.up * slideDistance;
        yield return StartCoroutine(SlideToPosition(targetPosition));
    }

    public IEnumerator SlideRoutineDOWN()
    {
        yield return StartCoroutine(SlideToPosition(originalPosition));
    }



    public IEnumerator SlideToPosition(Vector3 target)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target; // Ensure exact target position
    }
}
