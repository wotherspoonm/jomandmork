using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDEObject : MonoBehaviour
{
    public Vector3 targetRotation;
    public bool isSpinning = false;
    public void Bump() {
        gameObject.GetComponent<DEAnimator>().Bump(new(100,0,0));
    }
    public void Move() {
        var v = new Vector3(Random.Range(-5,5), Random.Range(-5, 5),transform.position.z);
        gameObject.GetComponent<DEAnimator>().MoveTo(v);
    }

    public void Spin() {
        if (isSpinning) {
            gameObject.GetComponent<DEAnimator>().SetSpin(Vector3.zero);
            isSpinning = !isSpinning;
        }
        else {
            var v = 90*(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            gameObject.GetComponent<DEAnimator>().SetSpin(v);
            isSpinning = !isSpinning;
        }
    }

    public void SetAngle() {
        gameObject.GetComponent<DEAnimator>().SetAngle(180 * (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))));
    }
}
