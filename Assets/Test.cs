using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform t;
    
    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.Find(R.DirectionalLight_1_Children.ComemeLaPOsha).GetComponent<Transform>();
    }
    
}
