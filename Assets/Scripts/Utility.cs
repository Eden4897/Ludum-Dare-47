using System;
using System.Collections;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Utility instance;

    private void Awake()
    {
        if(instance == null)
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
        target();
    }
}
