using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPrefabScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameHandlerScript gameHandler;
    public Vector2 InternalPos;
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        gameHandler.HandleMove(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
