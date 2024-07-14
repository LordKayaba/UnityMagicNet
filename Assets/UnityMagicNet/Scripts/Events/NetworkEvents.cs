using System;
using System.Collections.Generic;
using UnityMagicNet;

public class NetworkEvents
{
    Dictionary<string, Action<Role>> ServerEvents = new Dictionary<string, Action<Role>>();
    Dictionary<string, Action<Role>> ClientEvents = new Dictionary<string, Action<Role>>();

    public void OnServer(string type, Action<Role> listener)
    {
        if (ServerEvents.TryGetValue(type, out Action<Role> thisEvent))
        {
            thisEvent += listener;
            ServerEvents[type] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            ServerEvents.Add(type, thisEvent);
        }
    }

    public void OnClient(string type, Action<Role> listener)
    {
        if (ClientEvents.TryGetValue(type, out Action<Role> thisEvent))
        {
            thisEvent += listener;
            ClientEvents[type] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            ClientEvents.Add(type, thisEvent);
        }
    }

    public void Invoke(Role role, bool isServer)
    {
        if (isServer)
        {
            if (ServerEvents.TryGetValue(role.header.Type, out Action<Role> thisEvent))
            {
                thisEvent?.Invoke(role);
            }
        }
        else
        {
            if (ClientEvents.TryGetValue(role.header.Type, out Action<Role> thisEvent))
            {
                thisEvent?.Invoke(role);
            }
        }
    }
}
