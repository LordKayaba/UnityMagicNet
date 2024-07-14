using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMagicNet;

public class Test : MonoBehaviour
{
    void Start()
    {
        NetworkManager.events.OnServer("Test", test);
    }

    void test(Role role)
    {
        Debug.Log(role.header.Token2);
    }
}
