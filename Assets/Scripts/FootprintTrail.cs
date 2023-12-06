using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintTrail : MonoBehaviour
{
    public int ClonesPerSecond = 4;
    public Sprite sprite;
    public Vector2 pos;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Transform tf;
    private List<SpriteRenderer> clones;
    public Vector3 scalePerSecond = new Vector3(1f, 1f, 1f);
    public Color colorPerSecond = new Color(255, 255, 255, 1f);
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        clones = new List<SpriteRenderer>();
        StartCoroutine(trail());
    }

    void Update()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            clones[i].color -= colorPerSecond * Time.deltaTime;
            clones[i].transform.localScale -= scalePerSecond * Time.deltaTime;
            if (clones[i].color.a <= 0f || clones[i].transform.localScale == Vector3.zero)
            {
                Destroy(clones[i].gameObject);
                clones.RemoveAt(i);
                i--;
            }
        }
    }

    IEnumerator trail()
    {
        for (; ; ) //while(true)
        {
                var clone = new GameObject("trailClone");
                clone.transform.position = pos;
                clone.transform.localScale = tf.localScale;
                var cloneRend = clone.AddComponent<SpriteRenderer>();
                cloneRend.sprite = sr.sprite;
                cloneRend.sortingOrder = sr.sortingOrder - 1;
                clones.Add(cloneRend);

            yield return new WaitForSeconds(1f / ClonesPerSecond);
        }
    }
}
