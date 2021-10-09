using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public static class Events
{
    public static UnityEvent OnGameStart = new UnityEvent();
    public static UnityEvent OnGameEnd = new UnityEvent();
    public static UnityEvent OnGameRestart = new UnityEvent();
    public static UnityEvent OnCorrectChoice = new UnityEvent();
    public static UnityEvent OnIncorrectChoice = new UnityEvent();
}