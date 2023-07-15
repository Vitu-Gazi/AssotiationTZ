using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : Singleton<InputController>, IPointerDownHandler
{
    private Camera mainCamera;

    public Action<Cell> ChooseCell;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1;

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Cell cell))
            {
                ChooseCell?.Invoke(cell);
            }
        }

    }
}
