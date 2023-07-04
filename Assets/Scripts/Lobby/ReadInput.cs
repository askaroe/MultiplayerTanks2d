using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
    private string _input;
    
    public void ReadStringInput(string s)
    {
        _input = s;
        Debug.Log(_input);
    }
}
