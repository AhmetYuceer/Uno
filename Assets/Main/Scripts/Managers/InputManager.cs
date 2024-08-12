using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    private Action _mouseLeftClickAction;

    private void OnEnable()
    {
        _mouseLeftClickAction += PlayerController.Instance.CastRay;
    }

    private void OnDisable()
    {
        _mouseLeftClickAction -= PlayerController.Instance.CastRay;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _mouseLeftClickAction?.Invoke();
        }
    }
}