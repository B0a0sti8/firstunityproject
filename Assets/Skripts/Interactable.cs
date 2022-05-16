// Dieses Skript kann f�r alle Objekte verwendet werden, mit denen Interaktionen �ber Rechtsklick stattfinden
// z.B. Items, Truhen, H�ndler etc.
// Public virtual void Interact() wird dann �berschrieben, je nach Objekttyp
// Verantwortlicher f�r den Quatsch: Basti

using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Transform interactionTransform; // Punkt von dem aus mit dem Interactable agiert werden kann (nicht zwangsl�ufig Position des Interactable selbst)
    public float radius = 3f;              // Radius den man entfernt sein darf
    bool isFocus = false;                  // Stellt sp�ter fest ob Interactible bereits im Fokus ist
    Transform player;                      // Position des Spielers 
    bool hasInteracted = false;            // Schaut ob bereits Interaktion stattgefunden hat

    public virtual void Interact () // Diese Methode wird �berschrieben, je nachdem mit was interagiert wird (H�ndler, Item, Gegner, Schatzkiste etc.)
    {
        // Diese Methode wird �berschrieben, je nachdem mit was interagiert wird (H�ndler, Item, Gegner, Schatzkiste etc.)
        Debug.Log("Interacting with " + transform.name);
    }

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            hasInteracted = true;
            float distance = Vector2.Distance(player.position, interactionTransform.position); // Berechnet den Abstand zwischen Spieler und Objekt
            if (distance <= radius)
            {
                Interact(); // F�hrt Interaktion aus, je nachdem mit welchem Objekt 
            }
        }
    }

    public void OnFocused (Transform playerTransform) // Wird ausgef�hrt, sobald Objekt fokussiert wird
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;

        if (gameObject.layer == LayerMask.NameToLayer("Enemy") || gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            //Debug.Log("Focus enemy"); // 2x im Log
            gameObject.transform.Find("Charakter").transform.localScale = new Vector3(8f, 1.5f, 1f); // wide boii

            gameObject.transform.Find("Canvas UI").gameObject.SetActive(true);
            gameObject.GetComponent<EnemyStats>().enemyUIHealthActive = true;
        }
    }

    public void OnDefocused() // Wird ausgef�hrt, sobald Objekt nicht mehr fokussiert wird
    {
        isFocus = false;
        player = null;
        hasInteracted = false;

        if (gameObject.layer == LayerMask.NameToLayer("Enemy") || gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            //Debug.Log("Defocus enemy");
            gameObject.transform.Find("Charakter").transform.localScale = new Vector3(2.5f, 2.5f, 1f);

            gameObject.GetComponent<EnemyStats>().enemyUIHealthActive = false;
            gameObject.transform.Find("Canvas UI").gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected ()        // Malt gelben Kreis mit Interaktionsradius
    {
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
