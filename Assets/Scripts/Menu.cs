using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject input, button1, button2;

    public void Start()
    {
        input.SetActive(false);
    }

    public void ShowInput()
    {
        input.SetActive(true);
        button1.SetActive(false);
        button2.SetActive(false);
    }
}
