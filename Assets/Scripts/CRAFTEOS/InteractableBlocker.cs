using UnityEngine;

public class InteractableBlocker : MonoBehaviour
{
    [SerializeField]
    private bool startBlocked; // Control desde el Inspector

    private bool isBlocked;
    private CraftingAnvil craftingAnvil;
    private AnchorPointManager anchorPointManager;

    private void Awake()
    {
        craftingAnvil = GetComponent<CraftingAnvil>();
        anchorPointManager = GetComponent<AnchorPointManager>();
        
        // Establece el estado inicial desde el Inspector
        if (startBlocked)
        {
            Block();
        }
    }

    public void Block()
    {
        isBlocked = true;
        if (craftingAnvil != null)
            craftingAnvil.enabled = false;

        if (anchorPointManager != null)
            anchorPointManager.enabled = false;
    }

    public void Unlock()
    {
        isBlocked = false;
        if (craftingAnvil != null)
            craftingAnvil.enabled = true;

        if (anchorPointManager != null)
            anchorPointManager.enabled = true;
    }

    public bool IsBlocked() => isBlocked; // Verificar si está bloqueado
}