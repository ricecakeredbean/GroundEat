using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MonoSingleTon<T> : MonoBehaviour  where T  : MonoBehaviour
{
    public List<GameObject> objectList;

    public static T instance;
    protected virtual void Awake()
        => instance = this as T;

    //Func<int> a;
    //private void Update()
    //{
    //}

    //public void CloseObject(Vector3 pos)
    //{
    //    objectList.Where((obj) => obj.transform.localScale.x > 10).ToList();
    //}
}
