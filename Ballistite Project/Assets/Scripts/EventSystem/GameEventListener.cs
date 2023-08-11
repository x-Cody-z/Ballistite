using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<GameEventData> { }
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public CustomGameEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(GameEventData eventData)
    {
        response.Invoke(eventData);
    }
}
