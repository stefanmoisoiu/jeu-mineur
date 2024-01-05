using UnityEngine;

public class PVisible : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;
    
    public void SetVisible(bool visible)
    {
        foreach (GameObject target in targets) target.SetActive(visible);
    }
}