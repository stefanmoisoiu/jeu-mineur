using System.Collections;
using System.Collections.Generic;
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
