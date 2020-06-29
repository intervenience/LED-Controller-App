using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorMessage : MonoBehaviour {

    public TextMeshProUGUI text;

    public void DestroyOnClick () {
        Destroy (gameObject);
    }

}
