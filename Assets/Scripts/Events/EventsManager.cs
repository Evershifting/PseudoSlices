using System;
using System.Collections.Generic;


public static class EventsManager
{
    private static Dictionary<EventsType, Delegate> events = new Dictionary<EventsType, Delegate>();

    public static void AddListener(EventsType eventName, Action callback)
    {
        SetupEvent(eventName);
        events[eventName] = (Action)events[eventName] + callback;
    }

    internal static void AddListenerWithParams(EventsType eventName, Action<object[]> callback)
    {
        SetupEvent(eventName);
        events[eventName] = (Action<object[]>)events[eventName] + callback;
    }

    public static void AddListener<T>(EventsType eventName, Action<T> callback)
    {
        SetupEvent(eventName);
        events[eventName] = (Action<T>)events[eventName] + callback;
    }

    public static void AddListener<T, U>(EventsType eventName, Action<T, U> callback)
    {
        SetupEvent(eventName);
        events[eventName] = (Action<T, U>)events[eventName] + callback;
    }

    public static void AddListener<T, U, W>(EventsType eventName, Action<T, U, W> callback)
    {
        SetupEvent(eventName);
        events[eventName] = (Action<T, U, W>)events[eventName] + callback;
    }

    public static void RemoveListener(EventsType eventName, Action callback)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] = (Action)events[eventName] - callback;
            ListenerRemoved(eventName);
        }
    }

    public static void RemoveListener<T>(EventsType eventName, Action<T> callback)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] = (Action<T>)events[eventName] - callback;
            ListenerRemoved(eventName);
        }
    }

    public static void RemoveListener<T, U>(EventsType eventName, Action<T, U> callback)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] = (Action<T, U>)events[eventName] - callback;
            ListenerRemoved(eventName);
        }
    }

    public static void RemoveListener<T, U, W>(EventsType eventName, Action<T, U, W> callback)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] = (Action<T, U, W>)events[eventName] - callback;
            ListenerRemoved(eventName);
        }
    }

    public static void RemoveListenerWithParams(EventsType eventName, Action<object[]> callback)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] = (Action<object[]>)events[eventName] - callback;
            ListenerRemoved(eventName);
        }
    }

    public static void Broadcast(EventsType eventName)
    {
        if (CallCondition(eventName))
            ((Action)events[eventName])();
    }

    public static void Broadcast<T>(EventsType eventName, T param)
    {
        if (CallCondition(eventName))
            ((Action<T>)events[eventName])(param);
    }

    public static void Broadcast<T, U>(EventsType eventName, T param, U param2)
    {
        if (CallCondition(eventName))
            ((Action<T, U>)events[eventName])(param, param2);
    }

    public static void Broadcast<T, U, W>(EventsType eventName, T param, U param2, W param3)
    {
        if (CallCondition(eventName))
            ((Action<T, U, W>)events[eventName])(param, param2, param3);
    }

    public static void BroadcastWithParams(EventsType eventName, params object[] param)
    {
        if (CallCondition(eventName))
            ((Action<object[]>)events[eventName])(param);
    }

    private static bool CallCondition(EventsType eventName)
    {
        return events.ContainsKey(eventName) && events[eventName] != null;
    }

    private static void ListenerRemoved(EventsType eventName)
    {
        if (events.ContainsKey(eventName) && events[eventName] == null)
            events.Remove(eventName);
    }

    private static void SetupEvent(EventsType eventName)
    {
        if (!events.ContainsKey(eventName))
            events.Add(eventName, null);
    }
}

