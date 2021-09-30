using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadableObject
{
    public delegate void CallBack();

    void OnLoad(CallBack _callback);
}
