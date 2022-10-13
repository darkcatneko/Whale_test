using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawInputGismos : MonoBehaviour
{
    private void OnDrawGizmos()
    {       
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance = 100f;
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
        
    }
}
