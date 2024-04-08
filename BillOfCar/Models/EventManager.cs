using BillOfCar.Interfaces;

namespace BillOfCar.Models;


public class EventManager : IEventManager
{
    
    public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
    {
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, listenerBeingAdded);
        }

        Delegate d = eventTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            throw new Exception("Attempting to add listener Error");
        }
    }

    public void OnListenerRemoving(string eventType, Delegate listenerBeingRemove)
    {
        if (eventTable.ContainsKey(eventType))
        {
            Delegate d = eventTable[eventType];
            if (d == null)
            {
                throw new Exception($"Attempting to remove listener Error {eventType}");
            }
            else if (d.GetType() != listenerBeingRemove.GetType())
            {
                throw new Exception($"Attempting to remove listener Error {eventType}");
            }
        }
        else
        {
            throw new Exception($"Attempting to remove listener Type Error {eventType}");
        }
    }

    public void OnListenerRemoved(string eventType)
    {
        if (eventTable[eventType] != null)
        {
            eventTable.Remove(eventType);
        }
    }

    public void OnBroadcasting(string eventType)
    {
        
    }


    public void AddListener(string eventType, Callback handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback)eventTable[eventType] + handler;
    }

    public void AddListener<T>(string eventType, Callback<T> handler)
    {
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
    }

    public void RemoveListener(string eventType, Callback handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (Callback)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }
    
    public void RemoveListener<T>(string eventType, Callback<T> handler)
    {
        OnListenerRemoving(eventType, handler);
        eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }

    public void Broadcast(string eventType)
    {
        OnBroadcasting(eventType);
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback callback = d as Callback;
            if (callback != null)
            {
                callback();
            }
            else
            {
                throw new Exception($"{eventType} error Broadcast");
            }
        }
    }

    public void Broadcast<T>(string eventType, T arg1)
    {
        OnBroadcasting(eventType);
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback<T> callback = d as Callback<T>;
            if (callback != null)
            {
                callback(arg1);
            }
        }
        else
        {
            throw new Exception($"{eventType} error Broadcast");
        }
    }
}