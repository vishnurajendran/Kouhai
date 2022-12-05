using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace RuntimeDeveloperConsole
{
    public class ConsoleWindow : MonoBehaviour, IConsoleWindow
    {
        [SerializeField]
        RectTransform layoutToRebuild;
        [SerializeField]
        ScrollRect scrollView;

        [SerializeField]
        private TMPro.TMP_InputField inputField;
        [SerializeField]
        private TMPro.TMP_Text consoleOutput;

        public string ConsoleOutput => consoleOutput.text;

        private int commandStackPointer = 0;
        private List<string> commandStack;

        private ConsoleWindowHandler windowHandler;

        public bool IsOpen => windowHandler.IsOpen;

        private void Start()
        {
            windowHandler = GetComponent<ConsoleWindowHandler>();
            commandStack = new List<string>();
            inputField.onSubmit.AddListener(SubmitCommand);
            ConsoleSystem.SetConsoleWindow(this);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                commandStackPointer = Mathf.Clamp(commandStackPointer--, 0, commandStack.Count - 1);
                SelectFromCommandStack(commandStackPointer);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                commandStackPointer = Mathf.Clamp(commandStackPointer++, 0, commandStack.Count);
                SelectFromCommandStack(commandStackPointer);
            }
        }

        private void SelectFromCommandStack(int id)
        {
            if (commandStack.Count <= 0)
                return;

            if (id > commandStack.Count)
                inputField.text = string.Empty;
            else
                inputField.text = commandStack[id];
        }

        private void SubmitCommand(string commandString)
        {
            if (string.IsNullOrEmpty(commandString))
            {
                inputField.text = string.Empty;
                return;
            }

            //add command to output
            consoleOutput.text += $"{ConsoleConstants.TERM_KEY}<color=yellow>{commandString}</color>\n";
            inputField.text = string.Empty;

            commandStack.Add(commandString);
            commandStackPointer = commandStack.Count - 1;

            HandleCommand(commandString);
            inputField.ActivateInputField();
        }

        private void HandleCommand(string commandString)
        {
            ConsoleSystem.HandleCommand(commandString);
        }

        public void PrintLineToConsole(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            consoleOutput.text += $"{ConsoleConstants.TERM_KEY}{message}\n";
            StartCoroutine(HandleTextUpdate());
        }

        IEnumerator HandleTextUpdate()
        {
            LayoutRebuilder.MarkLayoutForRebuild(layoutToRebuild);
            yield return new WaitForEndOfFrame();
            scrollView.normalizedPosition= Vector3.zero;
        }

        public void Clear()
        {
            consoleOutput.text = string.Empty;
            StartCoroutine(HandleTextUpdate());
        }
    }
}
