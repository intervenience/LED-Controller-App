using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorManager : MonoBehaviour {

    [SerializeField] GameObject errorMessagePrefab;
    [SerializeField] Transform canvas;

    public void RaiseError (System.Exception e) {
        var g = Instantiate (errorMessagePrefab, canvas);
        g.GetComponent<ErrorMessage> ().text.text = e.Message;
    }

}
