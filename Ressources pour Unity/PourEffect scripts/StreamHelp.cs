using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
   private LineRenderer lineRenderer = null;
   private ParticleSystem splashParticle = null; //Variable pour les particules//

   private Coroutine pourRoutine = null; //Setting the position of the start and the end of a line renderer
   private Vector3 targetPosition = Vector3.zero;

   private void Awake() //Set up the lineRenderer
   {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
   }

   private void Start() //Instanciation du script//
   {
        MoveToPosition(0, transform.position); //Initialisation de la position du line renderer au niveau de la postion du Stream. L'index sert a identifier le line renderer//
        MoveToPosition(1, transform.position); //Et au niveau du EndPoint//
   }

   public void Begin() //Appelee dans la fonction StartPour du script PourDetector. Elle permet de demarrer la coroutine//
   {
          StartCoroutine(UpdateParticle());
          pourRoutine = StartCoroutine(BeginPour());
   }

   private IEnumerator BeginPour() //Permet de declarer ou prend fin le Stream//
   {
        while(gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();

            MoveToPosition(0, transform.position);
            AnimateToPosition(1, targetPosition); //Anime jusqu'a atteindre la position targetPosition//

            yield return null; //Fin de la Coroutine BeginPour//
        }  
}

     public void End()
     {
          StopCoroutine(pourRoutine);
          pourRoutine = StartCoroutine(EndPour());
     }

     private IEnumerator EndPour()
     {
          while(!HasReachedPosition(0, targetPosition))
          {
               AnimateToPosition(0, targetPosition);
               AnimateToPosition(1, targetPosition);
               
          yield return null; //Fin de la Coroutine EndPour//
          }

          Destroy(gameObject);
     }

   private Vector3 FindEndPoint() //Appelee par BeginPour afin de detecter un obstacle (objet, sol,...)//
   {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Physics.Raycast(ray, out hit, 2.0f); //2.0f refere a la longueur du rayon//
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f); //Test collision : on a soit la postion de la collision, soit celle du Raycast//

        return endPoint;
   }

   private void MoveToPosition(int index, Vector3 targetPosition) //Permet d'avoir l'animation au niveau de la bouteille lors d'un changement de position//
   {
        lineRenderer.SetPosition(index, targetPosition);
   }

   private void AnimateToPosition(int index, Vector3 targetPosition)
   {
     Vector3 currentPoint = lineRenderer.GetPosition(index);
     Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 1.75f);
     lineRenderer.SetPosition(index, newPosition);
   }

   private bool HasReachedPosition(int index, Vector3 targetPosition)
   {
          Vector3 currentPosition = lineRenderer.GetPosition(index);
          return currentPosition == targetPosition;
   }

   private IEnumerator UpdateParticle()
   {
          while(gameObject.activeSelf)
          {
               splashParticle.gameObject.transform.position = targetPosition;

               bool isHitting = HasReachedPosition(1, targetPosition);
               splashParticle.gameObject.SetActive(isHitting);

               yield return null; //Fin de la Coroutine UpdateParticle//
          }
   }
}
