// Dieses Skript sorgt daf?r, dass im Projekt Verzeichnis neue Objekte generiert werden k?nnen:
// Rechtsklick -> Create -> Inventory -> Item
// Diese ?bernehmen die unten stehenden Parameter
// Verantwortlicher f?r den Quatsch: Basti

using UnityEngine;
using Unity.Netcode;

public enum ItemQuality { Common, Uncommon, Rare, Epic, Mythic }

public abstract class Item : ScriptableObject, IMoveable // Statt Monobehaviour (?bernimmt Sachen von Objekt dem das Skript zugewiesen ist): ScriptableObjekt
{
    [SerializeField] public ItemQuality itemQuality;

    [HideInInspector] public string tooltipItemName = "";
    [HideInInspector] public string tooltipItemDescription = "";
    public Sprite tooltipItemSprite = null;

    [SerializeField] private int price;
    
    new public string name = "New Item";     // Bisherige Definiton des Namens wird ?berschrieben
    public bool isDefaultItem = false;       // Zus?tzlicher m?glicher Unterscheidungsparameter. z.B. keine Default Items ins Inventar oder ?hnliches.

    [SerializeField] private Sprite icon = null;               // Item Sprite 

    [SerializeField] private int stackSize;

    private InventorySlotScript slot;


    public NetworkObject user; 

    public InventorySlotScript MySlot { get => slot; set => slot = value; }
    public Sprite MyIcon { get => icon;  }
    public int MyStackSize { get => stackSize; }


    private CharPanelButtonScript charButton;

    public CharPanelButtonScript MyCharButton 
    {
        get
        {
            return charButton;
        }
        set
        {
            MySlot = null;
            charButton = value;
        }
    }

    public int MyPrice { get => price; }

    public virtual void Use()               // Wird ?berschrieben, je nach Itemsorte.
    {
        // Item verwenden.
        // Es k?nnte etwas passieren (je nach Item)
        NetworkObjectReference userReference = GetUserServerRpc();
        userReference.TryGet(out NetworkObject user);
        Debug.Log("Using " + name);
    }

    [ServerRpc(RequireOwnership = false)]
    public NetworkObjectReference GetUserServerRpc()
    {
        user = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject;
        NetworkObjectReference userReference = user.GetComponent<NetworkObjectReference>();
        return userReference;
    }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
            MySlot.onRemoved = true;
        }
    }

    public virtual void Update()
    {

    }

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {

    }
}
