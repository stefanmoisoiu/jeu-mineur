using Sirenix.OdinInspector;
using UnityEngine;

public class ResetData : MonoBehaviour
{
    [Button]
    public void ResetDataToDefault()
    {
        ES3.DeleteFile();
    }
}
