using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

public class TeslaCoilSpline : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    
    [SerializeField] private Transform tesla1AttachPoint,tesla2AttachPoint;

    [Button("Update Spline Points")]
    private void OnValidate()
    {
        Vector3 localTesla1Pos = tesla1AttachPoint.position - transform.position;
        Vector3 localTesla2Pos = tesla2AttachPoint.position - transform.position;
        spline.Spline.SetKnot(0,new BezierKnot(localTesla1Pos,Vector3.zero,Vector3.zero));
        spline.Spline.SetKnot(1,new BezierKnot(localTesla2Pos,Vector3.zero,Vector3.zero));
    }
}
