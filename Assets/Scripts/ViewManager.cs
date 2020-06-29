using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour {

    bool onHomeScreen = true;

    public Animator firstScreenAnimator;
    public Animator secondScreenAnimator;

    int openId;

    void Start () {
        openId = Animator.StringToHash ("Open");
    }

    bool changeScreens = false;

    void Update() {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (onHomeScreen) {
                Application.Quit ();
            } else {
                onHomeScreen = !onHomeScreen;
                firstScreenAnimator.SetBool (openId, true);
                secondScreenAnimator.SetBool (openId, false);
            }
        }

        if (Input.GetKeyDown (KeyCode.Alpha1)) {
            firstScreenAnimator.SetBool (openId, false);
            secondScreenAnimator.SetBool (openId, true);
            //Debug.Log ("First closed, second open");
        }

        if (Input.GetKeyDown (KeyCode.Alpha2)) {
            firstScreenAnimator.SetBool (openId, true);
            secondScreenAnimator.SetBool (openId, false);
            //Debug.Log ("First open, second closed");
        }
    }

    void FixedUpdate () {
        if (changeScreens) {
            firstScreenAnimator.SetBool (openId, onHomeScreen);
            secondScreenAnimator.SetBool (openId, !onHomeScreen);
            changeScreens = false;
        }
    }

    public void OnSuccessfulConnection () {
        onHomeScreen = false;
        changeScreens = true;
    }

}
