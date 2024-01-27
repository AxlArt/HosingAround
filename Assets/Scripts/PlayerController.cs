using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    ConfigurableJoint joint;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float waterDistance;
    [SerializeField] float waterForce;
    [SerializeField] float waterForceOnRigidbodies;
    [SerializeField] GameObject waterCylinder;
    [SerializeField] GameObject shoulderCam;
    [SerializeField] GameObject regularCam;
    float targetRotationX;
    float targetRotationY;
    bool inShoulderCamMode;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        joint = GetComponent<ConfigurableJoint>();  

        Cursor.visible = false;
        CursorLockMode lockMode = CursorLockMode.Locked;
        Cursor.lockState = lockMode;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            inShoulderCamMode = true;
            regularCam.SetActive(false);
            shoulderCam.SetActive(true);
            targetRotationX = 0 ;
            targetRotationY = 0;
        }

        if(Input.GetMouseButtonUp(1))
        {
            inShoulderCamMode = false;
            regularCam.SetActive(true);
            shoulderCam.SetActive(false);
            joint.targetRotation = Quaternion.Euler(0, 0, 0);
        }

        waterCylinder.SetActive(false);

        if (Input.GetMouseButton(0))
        {
            waterCylinder.SetActive(true);

            if (Physics.Raycast(transform.position,transform.forward,out RaycastHit hit,waterDistance))
            {
                
                rb.AddForce(-transform.forward * waterForce * Time.deltaTime);

                if(hit.transform.TryGetComponent(out Rigidbody hitRigidBody))
                {
                    hitRigidBody.AddForce(waterForceOnRigidbodies * (hit.transform.position - hit.point).normalized);
                }
            }
        }

        if (!inShoulderCamMode)
        {
            float inputY = Input.GetAxisRaw("Vertical");
            rb.AddForce(transform.forward * moveSpeed * inputY * Time.deltaTime);
            
        }

        else
        {

           
            float inputY = Input.GetAxisRaw("Vertical");

            
            targetRotationX -= rotateSpeed * Time.deltaTime * inputY;
           

        }

        float inputX = Input.GetAxisRaw("Horizontal");
        targetRotationY -= rotateSpeed * Time.deltaTime * inputX;
        joint.targetRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0);
    }
}
