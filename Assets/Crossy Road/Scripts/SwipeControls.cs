using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwipeControls : MonoBehaviour
{


    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public float treshold=50; 


    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            Vector2 inputVector = endTouchPosition - startTouchPosition;
            
            if ((Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y)))
            {
                if ((Mathf.Abs(inputVector.x) > treshold))
                    {
                    if (inputVector.x > 0)
                    {
                        RightSwipe();
                    }
                    else
                    {
                        LeftSwipe();
                    }
                }
            }
            else if ( ((Mathf.Abs(inputVector.y) > treshold)))
            {
                if (inputVector.y > 0)
                {
                    UpSwipe();
                }
                else
                {
                    DownSwipe();
                }
            }
        }

    }

    private void UpSwipe()
    {
        PlayerController.instance.CanIdle(false, false, true, false);
        PlayerController.instance.CanMove(false, false, true, false);
    }
    private void DownSwipe()
    {
        print("down");
        PlayerController.instance.CanIdle(false, false, false, true);
        PlayerController.instance.CanMove(false, false, false, true);
    }
    private void LeftSwipe()
    {
        print("left");
        PlayerController.instance.CanIdle(true, false, false, false);
        PlayerController.instance.CanMove(true, false, false, false);
    }
    private void RightSwipe()
    {
        PlayerController.instance.CanIdle(false, true, false, false);
        PlayerController.instance.CanMove(false, true, false, false);
        print("right");
    }

}