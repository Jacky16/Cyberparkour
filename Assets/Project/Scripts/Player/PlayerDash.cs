using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashVelocity;
    [SerializeField] float timeBtwDashes;
    [SerializeField] int dashesCount = 3;
    [SerializeField] float dragDashing = 10;
    float currentDrag;
    int currentDashes = 0;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Dash()
    {
        //if(currentDashes < dashesCount)
        StartCoroutine(DashCoroutine());
    }
    IEnumerator DashCoroutine()
    {
        currentDrag = rb.drag;
        rb.drag = dragDashing;

        rb.velocity = Camera.main.transform.forward * dashVelocity;
        currentDashes++;

        if (currentDashes >= dashesCount)
            currentDashes = dashesCount;

        yield return new WaitForSeconds(timeBtwDashes);
        rb.drag = currentDrag;
    }

    void ResetDash()
    {
        currentDashes = 0;
    }
}
