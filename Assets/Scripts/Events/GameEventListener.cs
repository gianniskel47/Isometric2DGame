using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    //event to listen to
    public SO_VoidEventChannel Event;

    // response when hame event is raised
    public UnityEvent Response;

  
    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
