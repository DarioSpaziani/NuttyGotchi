using System;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed;
    public bool startFromDown;
    public Vector3 offset;

    private bool moveDown = false;
    private Vector3 pos;
    private Vector3 targetDown;
    private Vector3 targetUp;
    private float elapsedTime = 0;

    private void Start() {
        pos = transform.position;
        targetDown = pos - offset;
        targetUp = pos + offset;

        moveDown = startFromDown;
    }

    private void Update() {
        elapsedTime += Time.deltaTime;
        float t = speed * elapsedTime;

        if (moveDown) {
            pos = Vector3.Slerp(pos, targetDown, t);
            transform.position = pos;

            if (t >= 1) {
                elapsedTime = 0;
                moveDown = false;
            }
        }

        else {
            pos = Vector3.Slerp(pos, targetUp, t);
            transform.position = pos;

            if (t >= 1) {
                elapsedTime = 0;
                moveDown = true;
            }
        }
    }

}
