using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourTreshold = 45; //Angle limite avant d'activer l'effet//
    public Transform origin = null; //creation d'une variable origin//
    public GameObject streamPrefab = null; 

    private bool isPouring = false;
    private Stream currentStream=null;

    private void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourTreshold;  //Verifie si l'inclinaison est inferieur a l'angle limite//

        if(isPouring != pourCheck) //Check pour savoir s'il faut lancer l'animation
        {
            isPouring = pourCheck;

            if (isPouring)    // angle < angle limite//
            {
                StartPour();
            }
            else                // angle >= angles limite//
            {
                EndPour();
            }
        }
    }

    private void StartPour()
    {
        print("Start");
        currentStream = CreateStream();
        currentStream.Begin(); //Appel a la fonction Begin du script stream//
    }

    private void EndPour()
    {
        print("End");
        currentStream.End(); //Appel a la fonction End du script stream//
        currentStream = null;
    }

    private float CalculatePourAngle() //Permet de calculer l'angle du GameObject//
    {
        return transform.forward.y * Mathf.Rad2Deg; //Make sure to use the correct direction compatible with your mesh.
                                                    //Here the calculation is relevant the y-axis//
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }
}