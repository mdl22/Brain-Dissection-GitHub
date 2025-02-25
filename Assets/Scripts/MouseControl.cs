using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseControl : MonoBehaviour
{
    [SerializeField] GameObject controlsPanel;

    [SerializeField] Button exitButton;
    [SerializeField] Button resetButton;

    [SerializeField] float minOrthographicSize;
    [SerializeField] float scale;

    Quaternion startRotation;

    Vector3 startPosition;
    Vector3 startCameraPosition;
    Vector3 lastMousePosition;

    float startOrthographicSize;
    float lastSeparation;
    float separation;
    float deltaSeparation;

    void Start()
    {
        exitButton.GetComponent<Button>().onClick.AddListener(Reset);
        resetButton.GetComponent<Button>().onClick.AddListener(Reset);

        startPosition = transform.position;
        startRotation = transform.rotation;

        startCameraPosition = Camera.main.transform.position;
        startOrthographicSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        // mouse control only available when contol panel open and mouse not over a button
        if (controlsPanel.gameObject.activeSelf)
        {
            Vector3 dist = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            if (Input.GetMouseButton(0))                // left button for rotating
            {
                transform.Rotate(dist.y * 360f/Screen.height, -dist.x * 360f/Screen.width, 0,
                    Space.World);
            }
            else if (Input.GetMouseButton(2))           // middle button for zooming
            {
                Vector3 centreOfScreen = new Vector3(Screen.width/2, Screen.height/2, 0);

                if (Input.GetMouseButtonDown(2))
                {
                    lastSeparation = (centreOfScreen - Input.mousePosition).magnitude;
                }
                else
                {
                    float separation = (centreOfScreen - Input.mousePosition).magnitude;
                    float deltaSeparation = separation - lastSeparation;
                    lastSeparation = separation;

                    TestZoomBoundaries(deltaSeparation * scale/Screen.height);
                }
            }
            else if (Input.mouseScrollDelta.y != 0)     // scroll wheel for zooming
            {            
                    TestZoomBoundaries(-Mathf.Sign(Input.mouseScrollDelta.y) * scale/50);
            }
            else if (Input.GetMouseButton(1))           // right button for positioning
            {            
                // adjust scaling by the degree of zooming
                Camera.main.transform.position -= dist * scale/Screen.height *
                    Camera.main.orthographicSize/startOrthographicSize;
            }
        }
    }

    void TestZoomBoundaries(float deltaSize)
    {
        if ((Camera.main.orthographicSize -= deltaSize) < minOrthographicSize)
        {
            Camera.main.orthographicSize = minOrthographicSize;
        }
        else if (Camera.main.orthographicSize > startOrthographicSize)
        {
            Camera.main.orthographicSize = startOrthographicSize;
        }
   }

    public void Reset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        Camera.main.transform.position = startCameraPosition;
        Camera.main.orthographicSize = startOrthographicSize;
    }
}
