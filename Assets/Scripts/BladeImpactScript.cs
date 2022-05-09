using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeImpactScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Opacity(1f, 0f, 0.3f)); // Smoothly lower the opacity until fully transparent
        Destroy(gameObject, 0.3f);
    }

    IEnumerator Opacity(float start, float end, float duration)
    {
        Color tmp = transform.GetComponent<SpriteRenderer>().color;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            tmp.a = Mathf.Lerp(start, end, normalizedTime);
            transform.GetComponent<SpriteRenderer>().color = tmp;
            yield return null;
        }
        tmp.a = end; //without this, the value will end at something like 0.9992367
        transform.GetComponent<SpriteRenderer>().color = tmp;
    }

    // Update is called once per frame
    void Update()
    {
        // Smoothly increase scale of sprite over time
        transform.localScale += new Vector3(2f, 2f, 2f) * Time.deltaTime;
    }
}
