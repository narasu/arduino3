using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voorbeeld : MonoBehaviour
{
    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        t = 5*Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (t > 0) t--;
        if (t <= 0)
        {
            //do the thing
            t = 5*Time.deltaTime;
        }
    }

    private IEnumerator DoTheThing()
    {
        //do the thing

        yield return new WaitForSeconds(5);
        
    }
}
