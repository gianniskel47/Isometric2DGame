using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Scriptable Objects/Events/SO_VoidEventChannel")]
public class SO_VoidEventChannel : ScriptableObject
{
    [TextArea]
    [SerializeField] string description;
    [SerializeField] string listeners;
    [SerializeField] string broadcasters;

    public event Action<object> OnEventRaised;

    public void RaiseEvent(object arg = null)
    {
        if(OnEventRaised == null)
        {
            Debug.Log($"No listeners found for {name}");
        }

        OnEventRaised(arg);
    }
}
