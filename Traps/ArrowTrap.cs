using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;

    private void Attack()
    {
        int arrowIndex = FindArrow();

        // Check if a valid arrow was found
        if (arrowIndex != -1)
        {
            cooldownTimer = 0;
            arrows[arrowIndex].GetComponent<EnemyProjectile>().ActivateProjectile();
            arrows[arrowIndex].transform.position = firePoint.position;
        }
        else
        {
            Debug.LogWarning("No inactive arrows found.");
        }
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        // If no inactive arrow is found, return -1 to indicate an issue
        return -1;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
            Attack();
    }
}
