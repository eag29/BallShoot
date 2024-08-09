using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] GameManager gm;

    Rigidbody rgb;
    Renderer renk;

    private void Start()
    {
        rgb = GetComponent<Rigidbody>();
        renk = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("kova"))
        {
            gm.BallEffect(transform.position, renk.material.color);
            TeknikIslemler();
            gm.TopGirdi();
        }
        if (other.CompareTag("plane"))
        {
            TeknikIslemler();
            gm.TopCikti();
        }
    }
    void TeknikIslemler()
    {
        gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        rgb.velocity = Vector2.zero;
        rgb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
