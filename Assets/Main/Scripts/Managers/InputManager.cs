using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputManager : MonoBehaviour
{
    private Action _mouseLeftClickAction;

    private void OnEnable()
    {
        _mouseLeftClickAction += CardViewer.ViewCard;
    }

    private void OnDisable()
    {
        _mouseLeftClickAction -= CardViewer.ViewCard;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _mouseLeftClickAction?.Invoke();
        }
    }
}