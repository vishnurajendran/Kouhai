using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeDeveloperConsole {
    public class ConsoleWindowHandler : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField]
        [Header("Leave empty to use main camera")]
        private Camera eventCamera;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private TMPro.TMP_InputField inputfield;

        [Header("Settings")]
        [SerializeField]
        private KeyCode consoleKey = KeyCode.BackQuote;

        [Header("Events")]
        [SerializeField]
        private RectTransform panelParent;
        [SerializeField]
        private Button closeButton;
        [SerializeField] 
        EventTrigger topbarEventTrigger;
        
        public bool IsOpen => panelParent.gameObject.activeSelf;

        private void Start()
        {
            if (eventCamera == null)
                eventCamera = Camera.main;

            if(EventSystem.current == null)
                new GameObject("Event System").AddComponent<EventSystem>().gameObject
                                              .AddComponent<StandaloneInputModule>();

            panelParent.gameObject.SetActive(false);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseWindow);
            SetupDragEvents();
        }

        private void Update()
        {
            if (Input.GetKeyDown(consoleKey))
                ToggleWindow();
        }

        private void SetupDragEvents()
        {
            //dragging
            EventTrigger.Entry dragWindowEntry = new EventTrigger.Entry();
            dragWindowEntry.eventID = EventTriggerType.Drag;
            dragWindowEntry.callback.AddListener((data) => OnDrag((PointerEventData)data));
            topbarEventTrigger.triggers.Add(dragWindowEntry);
        }

        public void ToggleWindow()
        {
            panelParent.gameObject.SetActive(!IsOpen);
            if(IsOpen)
                inputfield.ActivateInputField();
            else
                inputfield.DeactivateInputField();
            inputfield.text = string.Empty;
        }

        public void OpenWindow()
        {
            panelParent.gameObject.SetActive(true);
            inputfield.ActivateInputField();
        }

        public void CloseWindow()
        {
            panelParent.gameObject.SetActive(false);
            inputfield.DeactivateInputField();
        }

        //Dragging
        private void OnDrag(PointerEventData data)
        {
            panelParent.anchoredPosition += data.delta / canvas.scaleFactor;
        }
    }
}
