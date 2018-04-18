using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    public bool controller;

    bool outOfReach = false;

    private Vector3 inputDirection;

    public Transform character;

    private Vector3 moveToPosition;

    public float lookSensitivity = 1;

    private Vector3 centerPosition;
    private Vector3 newLocation;
    private float radius = 10;
    private float distance;

    public ManageGameOver gameOver;

    void Start()
    {
        transform.position = character.transform.position;
    }

    void Update ()
    {
        if (!gameOver.playerIsDead)
        {
            if (controller)
            {
                centerPosition = character.position;
                distance = Vector3.Distance(transform.position, centerPosition);

                if (distance > radius)
                {
                    Vector3 fromOriginToObject = transform.position - centerPosition;
                    fromOriginToObject *= radius / distance;
                    moveToPosition = centerPosition + fromOriginToObject;
                }

                inputDirection.x = Input.GetAxis("RightJoystickX");
                inputDirection.y = Input.GetAxis("RightJoystickY");

                if (inputDirection.x < 0.1 && inputDirection.x > -0.1)
                {
                    inputDirection.x = 0;
                }

                if (inputDirection.y < 0.1 && inputDirection.y > -0.1)
                {
                    inputDirection.y = 0;
                }

                moveToPosition += inputDirection * lookSensitivity;
                transform.position = moveToPosition;
            }
            else
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

                transform.position = new Vector3(mousePosition.x, mousePosition.y, 1);
            }
        }     
    }
}
