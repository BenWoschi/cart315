using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public GameObject effectPrefab;

    public float cooldown = 6f; // seconds between casts
    private float lastCastTime = -Mathf.Infinity;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= lastCastTime + cooldown)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

            effect.transform.SetParent(transform);
            effect.transform.localPosition = Vector3.zero;

            lastCastTime = Time.time;
        }
    }

}