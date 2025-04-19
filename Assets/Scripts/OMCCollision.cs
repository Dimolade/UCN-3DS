using UnityEngine;

public class OMCCollision : MonoBehaviour
{
    public OMC omc;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Debug.Log("Start OMC Dialog.");
            omc.StartDialog();
        }
    }
}