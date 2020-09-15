using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public interface INotifiable
{
    void OnNotify(object sender, params object[] args);
}
