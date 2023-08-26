using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour {
    //public Camera c;
    public int width;
    public int height;
    public float sizeMultiplier;
    // Use this for initialization
    void Start () {
        width = Screen.width;
        height = Screen.height;
        //c = Camera.main;
        Resize(sizeMultiplier);
    }

    public void Reset(bool center, GameObject start)
    {
        if (center)
        {
            transform.position = Vector3.zero;
        }
        else
        {
            transform.position = start.transform.position;
        }
        
    }

    public void RandomStart(float margin)
    {
        var w = Screen.width;
        var h = Screen.height;
        var wp = w / margin;
        var hp = h / margin;
        var rw = Random.Range(1 + wp, w - wp);
        var rh = Random.Range(1 + hp, h - hp);
        Debug.Log(rw + "and" + rh);

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(rw, rh, -10));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void Vanish()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void Appear()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void Resize(float m)
    {
        transform.localScale = new Vector3(m, 2, m );
    }
}
