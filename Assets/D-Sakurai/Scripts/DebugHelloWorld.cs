using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelloWorld : MonoBehaviour
{
    public void HelloWorld(string text = "Hello World!"){
        Debug.Log(text);
    }
}
