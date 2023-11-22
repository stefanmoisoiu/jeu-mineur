using Sirenix.OdinInspector;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GenerateWaterPlane waterPlane;
    
    
    [SerializeField] private float springDamping,springStrength;
    [SerializeField] private int precision = 10;
    [SerializeField][Range(0,2)] private float influence = 1f;
    
    [SerializeField] private int testSplashInfluenceSize = 2;
    [SerializeField] private float testSplashForce = 5;
    
    
    
    
    private FloatSpringComponent[] _springs;
    
    private void Start()
    {
        _springs = new FloatSpringComponent[precision + 1];
        for (int i = 0; i <= precision; i++)
        {
            _springs[i] = new FloatSpringComponent(0,0,0,springStrength,springDamping);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _springs.Length; i++)
        {
            float force = _springs[i].UpdateSpring(Time.deltaTime);
            int leftIndex = i - 1;
            int rightIndex = i + 1;

            if (leftIndex >= 0)
            {
                _springs[leftIndex].velocity -= force * influence / 2 / precision * 10;
            }
            if (rightIndex < _springs.Length)
            {
                _springs[rightIndex].velocity -= force * influence / 2 / precision * 10;
            }
        }
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        // move vertices
        Vector3[] vertices = meshFilter.sharedMesh.vertices;
        for (int i = 0; i <= precision; i++)
        {
            vertices[i * 2 + 1].y = _springs[i].currentPosition + waterPlane.Size.y / 2;
        }
        meshFilter.sharedMesh.vertices = vertices;
    }

    [Button]
    private void TestSplash()
    {
        Splash(transform.position,testSplashForce,testSplashInfluenceSize);
    }

    [Button]
    private void UpdateDampingStrengthRuntime()
    {
        for (int i = 0; i < _springs.Length; i++)
        {
            _springs[i].damping = springDamping;
            _springs[i].strength = springStrength;
        }
    }

    public void Splash(Vector3 position,float force,int splashInfluenceSize)
    {
        // get closest spring
        int closestSpringIndex = GetClosestSpringIndex(position.x);
        
        // apply force to closest spring
        _springs[closestSpringIndex].velocity += force;
        
        // apply force to surrounding springs
        for (int i = 1; i <= splashInfluenceSize; i++)
        {
            int leftIndex = closestSpringIndex - i;
            int rightIndex = closestSpringIndex + i;
            float splashInfluence = 1 - Mathf.Pow((float)i / splashInfluenceSize,2);
            if (leftIndex >= 0)
            {
                _springs[leftIndex].velocity += force * splashInfluence;
            }
            if (rightIndex < _springs.Length)
            {
                _springs[rightIndex].velocity += force * splashInfluence;
            }
        }
    }
    
    private FloatSpringComponent GetClosestSpring(float positionX)
    {
        float minDistance = float.MaxValue;
        FloatSpringComponent closestSpring = null;
        for (int i = 0; i < _springs.Length; i++)
        {
            float distance = Mathf.Abs(GetSpringWorldPosition(i).x - positionX);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSpring = _springs[i];
            }
        }

        return closestSpring;
    }

    private int GetClosestSpringIndex(float positionX)
    {
        float minDistance = float.MaxValue;
        int closestSpring = 0;
        for (int i = 0; i < _springs.Length; i++)
        {
            float distance = Mathf.Abs(GetSpringWorldPosition(i).x - positionX);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSpring = i;
            }
        }

        return closestSpring;
    }
    private Vector3 GetSpringWorldPosition(int index)
    {
        float posY = transform.position.y + waterPlane.Size.y / 2;
        float posX = transform.position.x - waterPlane.Size.x / 2 + waterPlane.Size.x * index / precision;
        return new Vector3(posX,posY,0);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.blue;
        for (int i = 0; i <= precision; i++)
        {
            float size = Application.isPlaying ? _springs[i].currentPosition: .5f;
            UnityEditor.Handles.ArrowHandleCap(i,GetSpringWorldPosition(i),Quaternion.Euler(-90,0,0),size,EventType.Repaint);
        }
    }
    #endif
}
