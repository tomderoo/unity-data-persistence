using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    public int PointValue;

    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        // Set some nice colors from blue to reddish purple.
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", new Color(0, 0, 1, 1));
                break;
            case 2:
                block.SetColor("_BaseColor", new Color(0.3f, 0, 0.9f, 1));
                break;
            case 5:
                block.SetColor("_BaseColor", new Color(0.7f, 0, 0.3f, 1));
                break;
            default:
                block.SetColor("_BaseColor", new Color(1, 0, 0.1f, 1));
                break;
        }
        renderer.SetPropertyBlock(block);
    }

    private void OnCollisionEnter(Collision other)
    {
        onDestroyed.Invoke(PointValue);
        
        //slight delay to be sure the ball have time to bounce
        Destroy(gameObject, 0.2f);
    }
}
