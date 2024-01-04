using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Pipe : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SplineContainer spline;
    public Spline Spline => spline.Spline;
    [SerializeField] private PipeStretchMat pipeStretchMat;
    [SerializeField] private TexturedSpline texturedSpline;
    
    
    private Coroutine _moveStretchPositionCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) MoveStretchPosition(true, 10);
        if (Input.GetKeyDown(KeyCode.I)) MoveStretchPosition(false, 10);
    }

    public void MoveStretchPosition(bool forward, float speed)
    {
        if (_moveStretchPositionCoroutine != null) StopCoroutine(_moveStretchPositionCoroutine);
        _moveStretchPositionCoroutine = StartCoroutine(MoveStretchPositionCoroutine(forward,speed));
    }
    private IEnumerator MoveStretchPositionCoroutine(bool forward, float speed)
    {
        float advancement = 0;
        while (advancement < 1)
        {
            advancement += Time.deltaTime / texturedSpline.MaxLength * speed;
            
            float position = (forward ? advancement : 1 - advancement) * texturedSpline.MaxLength;
            pipeStretchMat.SetStretchPosition(position);
            yield return null;
        }
    }
}
