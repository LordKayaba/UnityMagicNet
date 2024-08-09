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
        Debug.Log(Time.realtimeSinceStartup - time);
        Debug.Log(role.header.Token2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sd();
        }
    }

    float time;
    void sd()
    {
        time = Time.realtimeSinceStartup;
        NetworkManager.Server.Send("Test", DataType.NoCompress,"1234000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

    }
}
