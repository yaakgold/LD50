using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    /// <summary>
    /// 0 means that it is an instant
    /// </summary>
    public float timer;
    public float amount;
    public Sprite spr;
    public string pupName;
    public Func<bool> action;
    public Func<bool> deactivate;
}
