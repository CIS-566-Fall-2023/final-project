using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField]
    GameObject root;
    [SerializeField]
    Material mat;
    [SerializeField]
    List<Renderer> rends;

    public float dissolveTime = 2.0f;

    bool dissolving = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(!dissolving && collision.gameObject.CompareTag("Player"))
        {
            dissolving = true;
            StartCoroutine(Dissolve());
        }
    }
    IEnumerator Dissolve()
    {
        Debug.Log("Dissolve!");
        Material tempMat = new Material(mat);
        foreach(Renderer r in rends)
        {
            r.material = tempMat;
        }

        float ratio = 0.0f;
        float del = 0.02f;
        while(ratio <= 1.0f)
        {
            ratio += del / dissolveTime;
            tempMat.SetFloat("_DissolveRatio", ratio);
            yield return new WaitForSeconds(del);
            Debug.Log("ratio = " + ratio.ToString());
        }
        Destroy(root);
    }
}
