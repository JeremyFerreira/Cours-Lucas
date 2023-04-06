using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[System.Flags]
public enum UpdateMode
{
    None = 0,
    Early = 10,
    Late = 20,
    Input= 30,
    Update = 40,
    Fixed= 50
}
public interface IBaseUpdate
{
    
}
public interface IAbstractUpdate : IBaseUpdate 
{
    void Updater();
}

public interface IUpdatable : IAbstractUpdate { }
public interface IEarlyUpdatable : IAbstractUpdate { }
public interface ILateUpdatable : IAbstractUpdate { }
public interface IInputUpdatable : IAbstractUpdate { }
public interface IFixedUpdatable : IAbstractUpdate { }


public class UpdateManager : MonoBehaviour
{
    private static List<IUpdatable> updtates = new List<IUpdatable>();
    private static List<IEarlyUpdatable> earlyUpdtates = new List<IEarlyUpdatable>();
    private static List<ILateUpdatable> lateUpdtates = new List<ILateUpdatable>();
    private static List<IInputUpdatable> inputUpdtates = new List<IInputUpdatable>();
    private static List<IFixedUpdatable> fixedUpdtates = new List<IFixedUpdatable>();

    public static void RegisterCallback(IBaseUpdate callback, UpdateMode mode)
    {
        if(mode.HasFlag(UpdateMode.Early) && callback is IEarlyUpdatable early)
        {
            earlyUpdtates.Add(early);
        }
        if (mode.HasFlag(UpdateMode.Late) && callback is ILateUpdatable late)
        {
            lateUpdtates.Add(late);
        }
        if (mode.HasFlag(UpdateMode.Input) && callback is IInputUpdatable input)
        {
            inputUpdtates.Add(input);
        }
        if (mode.HasFlag(UpdateMode.Fixed) && callback is IFixedUpdatable fixe)
        {
            fixedUpdtates.Add(fixe);
        }
        if (mode.HasFlag(UpdateMode.Update) && callback is IUpdatable updtate)
        {
            updtates.Add(updtate);
        }
    }

    public static void UnRegisterCallback(IBaseUpdate callback, UpdateMode mode)
    {
        if (mode.HasFlag(UpdateMode.Early) && callback is IEarlyUpdatable early)
        {
            earlyUpdtates.Remove(early);
        }
        if (mode.HasFlag(UpdateMode.Late) && callback is ILateUpdatable late)
        {
            lateUpdtates.Remove(late);
        }
        if (mode.HasFlag(UpdateMode.Input) && callback is IInputUpdatable input)
        {
            inputUpdtates.Remove(input);
        }
        if (mode.HasFlag(UpdateMode.Fixed) && callback is IFixedUpdatable fixe)
        {
            fixedUpdtates.Remove(fixe);
        }
        if (mode.HasFlag(UpdateMode.Update) && callback is IUpdatable updtate)
        {
            updtates.Remove(updtate);
        }
    }

    void Update()
    {
        CallUpdtate(earlyUpdtates);
        CallUpdtate(inputUpdtates);
        CallUpdtate(updtates);
        CallUpdtate(lateUpdtates);
    }

    void FixedUpdate()
    {
        CallUpdtate(fixedUpdtates);
    }

    void CallUpdtate<T>(List<T> update) where T : IAbstractUpdate
    {
        for (int i = update.Count; i-- > 0;)
        {
            try
            {
                update[i].Updater();
            }
            catch(Exception e)
            {
                if(update[i] as UnityEngine.Object)
                {
                    Debug.LogException(e, update[i] as UnityEngine.Object);
                }
                else
                {
                    Debug.LogException(e);
                }
                
            }
        }
    }
}


