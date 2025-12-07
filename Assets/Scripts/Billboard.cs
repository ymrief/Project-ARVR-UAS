using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform lookAt; // assign Main Camera or player head
    public bool onlyY = true;

    void LateUpdate()
    {
        if (lookAt == null) return;

        if (onlyY)
        {
            Vector3 dir = lookAt.position - transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude <= 0.001f) return;
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else
        {
            transform.LookAt(lookAt);
            transform.Rotate(0, 0, 0); 
        }
    }
}
