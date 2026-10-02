using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camaraJohn : MonoBehaviour
{
    public GameObject John;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.x = John.transform.position.x;
        transform.position = position;
    }
}
