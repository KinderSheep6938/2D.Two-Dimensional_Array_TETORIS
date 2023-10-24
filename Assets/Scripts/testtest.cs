using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class testtest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private delegate void a();

    // Update is called once per frame
    void Update()
    {
        a b;
        b = imo;
        b();

        b += satumaimo;
    }

    void imo()
    {
        Debug.Log("aaaaaaaa");
    }

    void satumaimo()
    {
        Debug.Log("bbbbbb");
    }
}
