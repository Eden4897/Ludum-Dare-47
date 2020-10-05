using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utility : MonoBehaviour
{
    public static Utility instance;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static Action Invoke(Action target, float timeout)
    {
        instance.StartCoroutine(instance.InvokeRoutine(target, timeout));
        return target;
    }

    private IEnumerator InvokeRoutine(Action target, float timeout)
    {
        yield return new WaitForSecondsRealtime(timeout);
        try
        {
            target();
        }
        catch (MissingReferenceException)
        {
            Debug.LogWarning("Lost reference to target action!");
        }
    }

    public static bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static T WeightedChoice<T>(Dictionary<T, float> choices)
    {
        float totalWeight = 0;
        foreach(var choice in choices)
        {
            totalWeight += choice.Value;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        foreach (var choice in choices)
        {
            if(randomValue < choice.Value)
            {
                return choice.Key;
            }
            randomValue -= choice.Value;
        }
        return default;
    }
}