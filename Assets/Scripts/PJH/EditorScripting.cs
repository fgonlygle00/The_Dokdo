using UnityEngine;

public class EditorScripting : MonoBehaviour
{
    void OnDrawGizmos()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + collider.center, collider.radius);
        }
    }
}
