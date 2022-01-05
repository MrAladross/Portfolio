#define DEBUG
#undef  DEBUG
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    private HashSet<EventListenerBase> _baseListeners = new HashSet<EventListenerBase>();
    public enum EventType
    {
        OnCalculate,
        OnRemovePlayers,
        OnTimerUp,
        NetworkUpdate,
        NetworkInstantiate,
        ToggleRockInMotion,
        RandomizeRockBehavior,
        EndRockGame,
        ChatMessage
        
    };
    [SerializeField] private EventType _eventType;
    public EventType GetType()
    {
        return _eventType;
    }

    public void InvokeWithMessage(string messageToSend)
    {
        //count downwards like this in case invoke is a destruction/deregister event
        for (int i=_baseListeners.Count-1;i>=0;--i)
        {
            _baseListeners.ElementAt(i).message = messageToSend;
            _baseListeners.ElementAt(i).EventMethod();
        }
    }
    public void Invoke() //using a scriptable object and a component listener, fire events for all subscribers at once
    {
        //count downwards like this in case invoke is a destruction/deregister event
        for (int i=_baseListeners.Count-1;i>=0;--i)
        {
            _baseListeners.ElementAt(i).EventMethod();
        }
        // the eventtype is chosen when creating the scriptable object
        //after making the scriptableobject, it is attached to the appropriate gameobject, like scoretracker
    }
    public void RegisterAbstract(EventListenerBase gameEventListener)
    {
        _baseListeners.Add(gameEventListener);
#if DEBUG
        Debug.Log("A listener was added: "+gameEventListener);
#endif
    }

    public void DeRegisterAbstract(EventListenerBase gameEventListener)
    {
        _baseListeners.Remove(gameEventListener);
#if DEBUG
        Debug.Log("A listener was removed: "+gameEventListener);
#endif
    }
}
