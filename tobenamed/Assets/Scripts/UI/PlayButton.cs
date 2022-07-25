using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button button;

    private void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate {
            SceneManager.LoadScene("SampleScene");
        });
    }
}
