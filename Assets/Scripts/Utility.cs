﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utility : MonoBehaviour
{
    public static Utility instance;

    private void Awake()
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

    public static void Invoke(Action target, float timeout)
    {
        instance.StartCoroutine(instance.InvokeRoutine(target, timeout));
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
        EventSystem current = EventSystem.current;
        return current.IsPointerOverGameObject()
               && current.currentSelectedGameObject != null
               && current.currentSelectedGameObject.GetComponent<CanvasRenderer>() != null;
    }
}