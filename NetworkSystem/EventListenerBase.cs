using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

using UnityEditor;
public abstract class EventListenerBase : MonoBehaviour
{
    [SerializeField]
    protected GameEvent _gameEvent;

    protected GameEvent.EventType _eventType;
    public string message = "";
    protected ScoreTracker st;
    private void Awake()
    {
//        st = GameObject.FindWithTag("ScoreTracker").GetComponent<ScoreTracker>();
        _eventType = _gameEvent.GetType();
    }
    private void OnEnable()
    {
        _gameEvent.RegisterAbstract(this);
    }

    private void OnDisable()
    {
        _gameEvent.DeRegisterAbstract(this);
    }

    private void OnDestroy()
    {
        _gameEvent.DeRegisterAbstract(this);
    }
    public abstract void EventMethod();

}
