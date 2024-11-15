using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tank : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
         if (mainCamera == null)
            mainCamera = Camera.main;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y; // Keep the tank's Y position unchanged
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f); // Smooth rotation
        }
    }
}
