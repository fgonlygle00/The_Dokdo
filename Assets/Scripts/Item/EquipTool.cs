using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    public float attackDistance;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    public override void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resouce))
            {
                resouce.Gather(hit.point, hit.normal);
            }

            Debug.Log(hit.collider.gameObject.name);

            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable damageable))
            {
                Debug.Log("����");
                damageable.TakePhysicalDamage(damage);
            }
        }
    }
}