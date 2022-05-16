// Dieses Skript kann für alle Objekte verwendet werden, mit denen Interaktionen über Rechtsklick stattfinden
// z.B. Items, Truhen, Händler etc.
// Public virtual void Interact() wird dann überschrieben, je nach Objekttyp
// Verantwortlicher für den Quatsch: Basti

using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Transform interactionTransform; // Punkt von dem aus mit dem Interactable agiert werden kann (nicht zwangsläufig Position des Interactable selbst)
    public float radius = 3f;              // Radius den man entfernt sein darf
    bool isFocus = false;                  // Stellt später fest ob Interactible bereits im Fokus ist
    Transform player;                      // Position des Spielers 
    bool hasInteracted = false;            // Schaut ob bereits Interaktion stattgefunden hat

    public virtual void Interact () // Diese Methode wird überschrieben, je nachdem mit was interagiert wird (Händler, Item, Gegner, Schatzkiste etc.)
    {
        // Diese Methode wird überschrieben, je nachdem mit was interagiert wird (Händler, Item, Gegner, Schatzkiste etc.)
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
                Interact(); // Führt Interaktion aus, je nachdem mit welchem Objekt 
            }
        }
    }

    public void OnFocused (Transform playerTransform) // Wird ausgeführt, sobald Objekt fokussiert wird
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

    public void OnDefocused() // Wird ausgeführt, sobald Objekt nicht mehr fokussiert wird
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
