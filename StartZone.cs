using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : Cylinder {
    public float count;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Cursor")
        {
            count += Time.deltaTime;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Cursor")
        {
            count = 0;
        }
    }
}
