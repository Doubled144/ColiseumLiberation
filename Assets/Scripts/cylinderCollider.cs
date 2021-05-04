using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cylinderCollider: MonoBehaviour
{
    public int detail = 4;
    public bool over = false;
    private List<GameObject> colliders = new List<GameObject>();
    void Start()
    {
        float angle = 0f;
        float degIncrement = 90f/detail;

        for (int i = 0; i < detail; i++)
        {
            GameObject obj = new GameObject ("col-"+i);
            obj.transform.parent = gameObject.transform;
            obj.transform.position = gameObject.transform.position;
            obj.transform.localScale  = new Vector3(1f, 1f, 1f);

            BoxCollider sc = obj.AddComponent(typeof(BoxCollider)) as BoxCollider;
            isMouseOver overScript = obj.AddComponent(typeof(isMouseOver)) as isMouseOver;
            sc.isTrigger = true;
            colliders.Add(obj);
            angle += degIncrement;
            obj.transform.rotation  = Quaternion.AngleAxis(angle, transform.up);
        }
    }

    void Update ()
    {
        over = mouseOverCircle ();
        if (over)
        {
            print ("Over all");
        }
    }

    bool mouseOverCircle ()
    {
        for (int i=0; i < colliders.Count; i++)
        {
            GameObject col = colliders[i];
            isMouseOver overScript = col.GetComponent<isMouseOver>();
            if (overScript.over == false)
            {
                return false;
            }
        }
        return true;
    }
}
