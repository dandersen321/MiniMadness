using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchToZoom : MonoBehaviour {

    public float perspectiveZoomSpeed = .5f;
    public float orthoZoomSpeed = .5f;

    private Camera myCamera;

	// Use this for initialization
	void Start () {

        myCamera = GetComponent<Camera>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitueDiff = prevTouchDeltaMag - touchDeltaMag;

            if(myCamera.orthographic)
            {
                myCamera.orthographicSize += deltaMagnitueDiff * orthoZoomSpeed;
                myCamera.orthographicSize = Mathf.Max(myCamera.orthographicSize, .1f);
            }
            else
            {
                myCamera.fieldOfView += deltaMagnitueDiff * perspectiveZoomSpeed;
                myCamera.fieldOfView = Mathf.Clamp(myCamera.fieldOfView, .1f, 179.9f);
            }
        }
	}
}
