using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceImpact : MonoBehaviour
{
    private Transform parent;
    private Vector3 player;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        var trueScale = new Vector3(
                     1f / parent.lossyScale.x,
                     1f / parent.lossyScale.y,
                     1f / parent.lossyScale.z);

        transform.localScale = trueScale;
        Destroy(gameObject, 0.4f);
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
        transform.position = parent.transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
        player.z = 0f;
        player.x = player.x - transform.position.x;
        player.y = player.y - transform.position.y;

        float angle = Mathf.Atan2(player.y, player.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        StartCoroutine(Opacity(1f, 0, 1f));
    }
}
