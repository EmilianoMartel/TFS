using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private CameraSource _source;
    [SerializeField] private LayerMask _layerMask;

    private void Update()
    {
        if (_source.Reference == null)
            return;

        Ray ray = _source.Reference.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _layerMask))
            transform.position = raycastHit.point;
    }
}