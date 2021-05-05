using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// This class is used to block ui elements from being clicked when you are hoverring other ui elements. For instance clicking on escape panel while trying to do combat in game will be blocked be cause of ui being in the way.
/// </summary>
[RequireComponent(typeof(EventTrigger))]
public class MouseInputUIBlocker : MonoBehaviour
{
    public bool BlockedByUI { get; set; } = false;

    private EventTrigger eventTrigger;

    /// <summary>
    /// Setup our event triggers on startup
    /// </summary>
    private void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            EventTrigger.Entry enterUIEntry = new EventTrigger.Entry();
            // Pointer Enter
            enterUIEntry.eventID = EventTriggerType.PointerEnter;
            enterUIEntry.callback.AddListener((eventData) => { EnterUI(); });
            eventTrigger.triggers.Add(enterUIEntry);

            //Pointer Exit
            EventTrigger.Entry exitUIEntry = new EventTrigger.Entry();
            exitUIEntry.eventID = EventTriggerType.PointerExit;
            exitUIEntry.callback.AddListener((eventData) => { ExitUI(); });
            eventTrigger.triggers.Add(exitUIEntry);
        }
    }

    public void EnterUI()
    {
        BlockedByUI = true;
    }
    public void ExitUI()
    {
        BlockedByUI = false;
    }

}