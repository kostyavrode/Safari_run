using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeDetector : MonoBehaviour
{
	private Vector2 fingerDownPos;
	private Vector2 fingerUpPos;

	public bool detectSwipeAfterRelease = false;

	public float SWIPE_THRESHOLD = 20f;

	// Update is called once per frame
	void Update()
	{

		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				fingerUpPos = touch.position;
				fingerDownPos = touch.position;
			}

			//Detects Swipe while finger is still moving on screen
			if (touch.phase == TouchPhase.Moved)
			{
				if (!detectSwipeAfterRelease)
				{
					fingerDownPos = touch.position;
					DetectSwipe();
				}
			}

			//Detects swipe after finger is released from screen
			if (touch.phase == TouchPhase.Ended)
			{
				fingerDownPos = touch.position;
				DetectSwipe();
			}
		}
	}

	void DetectSwipe()
	{

		if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
		{
			Debug.Log("Vertical Swipe Detected!");
			if (fingerDownPos.y - fingerUpPos.y > 0)
			{
				OnSwipeUp();
			}
			else if (fingerDownPos.y - fingerUpPos.y < 0)
			{
				OnSwipeDown();
			}
			fingerUpPos = fingerDownPos;

		}
		else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
		{
			Debug.Log("Horizontal Swipe Detected!");
			if (fingerDownPos.x - fingerUpPos.x > 0)
			{
				OnSwipeRight();
			}
			else if (fingerDownPos.x - fingerUpPos.x < 0)
			{
				OnSwipeLeft();
			}
			fingerUpPos = fingerDownPos;

		}
		else
		{
			Debug.Log("No Swipe Detected!");
		}
	}

	float VerticalMoveValue()
	{
		return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
	}

	float HorizontalMoveValue()
	{
		return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
	}

	void OnSwipeUp()
	{
		Debug.Log("OnSwipeUp");
		PlayerController.instance.CanIdle(false, false, true, false);
		PlayerController.instance.CanMove(false, false, true, false) ;
		//PlayerController.instance.Moving(new Vector3(transform.position.x, transform.position.y, transform.position.z + PlayerController.instance.moveDistance)); PlayerController.instance.SetMoveForwardState();
	}

	void OnSwipeDown()
	{
		PlayerController.instance.CanIdle(false, false, false, true);
		PlayerController.instance.CanMove(false, false, false, true);
		PlayerController.instance.Moving(new Vector3(transform.position.x, transform.position.y, transform.position.z - PlayerController.instance.moveDistance));
	}

	void OnSwipeLeft()
	{
		PlayerController.instance.CanIdle(true, false, false, false);
		PlayerController.instance.CanMove(true, false, false, false);
		PlayerController.instance.Moving(new Vector3(transform.position.x - PlayerController.instance.moveDistance, transform.position.y, transform.position.z ));
	}

	void OnSwipeRight()
	{
		PlayerController.instance.CanIdle(false, true, false, false);
		PlayerController.instance.CanMove(false, true, false, false);
		PlayerController.instance.Moving(new Vector3(transform.position.x - PlayerController.instance.moveDistance, transform.position.y, transform.position.z ));
	}
}