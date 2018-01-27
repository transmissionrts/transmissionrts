using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtils : MonoBehaviour {

    public Transform GetChildByName(string name, Transform thisTransform)
    {
        for (int i = 0; i < thisTransform.childCount; i++)
        {
            if (thisTransform.GetChild(i).name == name)
                return thisTransform.GetChild(i);
        }
        return null;
    }
    
    public List<Transform> GetChildrenContaining(string substr, Transform thisTransform)
    {
        List<Transform> ret = new List<Transform>();
        for (int i = 0; i < thisTransform.childCount; i++)
        {
            if (thisTransform.GetChild(i).name.Contains(substr))
                ret.Add( thisTransform.GetChild(i) );
        }
        return ret;
    }
}
