using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPage : MonoBehaviour
{
    public bool isTransitionedIn;
    public float freq;
    public float damp;
    public float resp;
    SmoothVector menuPos;
    bool isTransitioning;
    float maxTransitionTime = 5f;
    float elapsedTransitionTime;
    public Vector3 transitionOutPosition = Vector3.one * 1500;
    public Vector3 transitionInPosition;

    void Start() {
        isTransitioning = false;
        transitionInPosition = gameObject.GetComponent<RectTransform>().localPosition;
        elapsedTransitionTime = 0;
        menuPos = new SmoothVector(isTransitionedIn ? Vector3.zero : transitionOutPosition, freq, damp, resp);
        if (!isTransitionedIn) {
            gameObject.GetComponent<RectTransform>().localPosition = transitionOutPosition;
        }
    }
    void Update() {
        if (isTransitioning) {
            if (elapsedTransitionTime < maxTransitionTime) {
                elapsedTransitionTime += Time.deltaTime;
                menuPos.TimeStep(Time.deltaTime);
                gameObject.GetComponent<RectTransform>().localPosition = menuPos.Value;
            }
            else {
                isTransitioning = false;
            }
        }
    }
    public void TransitionIn() {
        if (isTransitionedIn) throw new System.Exception("Transition in called when menu already transitioned in");
        menuPos.TargetValue = Vector3.zero;
        isTransitioning = true;
        isTransitionedIn = true;
        elapsedTransitionTime = 0;
    }
    public void TransitionOut() {
        if (!isTransitionedIn) throw new System.Exception("Transition out called when menu already transitioned out");
        menuPos.TargetValue = transitionOutPosition;
        isTransitioning = true;
        elapsedTransitionTime = 0;
        isTransitionedIn = false;
    }
}
