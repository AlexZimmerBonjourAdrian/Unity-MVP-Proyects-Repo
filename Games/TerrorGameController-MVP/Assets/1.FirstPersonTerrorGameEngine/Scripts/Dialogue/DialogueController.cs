using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Legacy;
using System.Collections;
using System.Collections.Generic;

namespace HorrorEngine.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }

        [SerializeField]private DialogueRunner dialogueRunner;
        [SerializeField]private LineView lineView;

        [SerializeField]private bool autoAdvance = false;
        private Coroutine autoAdvanceCoroutine;

        [SerializeField]private List<YarnProject> dialogueProjects;

        public bool AutoAdvance
        {
            get => autoAdvance;
            set => SetAutoAdvance(value);
        }

        public List<YarnProject> DialogueProjects
        {
            get => dialogueProjects;
            set => dialogueProjects = value;
        }

        public DialogueRunner DialogueRunner
        {
            get => dialogueRunner;
            set => dialogueRunner = value;
        }

        public LineView LineView
        {
            get => lineView;
            set => lineView = value;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Multiple instances of DialogueController detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize the list of dialogue projects
        }

        void Start()
        {
            // Ensure the DialogueRunner and LineView are set up
            if (dialogueRunner == null || lineView == null)
            {
                Debug.LogError("DialogueRunner or LineView is not assigned in the inspector.");
                return;
            }

            dialogueRunner = GameObject.Find("Dialogue System Horror").GetComponent<DialogueRunner>();
            if (dialogueRunner == null)
            {
                Debug.LogError("DialogueRunner not found in the scene.");
                return;
            }
            
            // Attach the LineView to the DialogueRunner
            lineView = GameObject.Find("Line View").GetComponent<LineView>();
            if (lineView == null)
            {
                Debug.LogError("LineView not found in the scene.");
                return;
            }

            // Register a Yarn command to toggle auto-advance
            dialogueRunner.AddCommandHandler<bool>("setAutoAdvance", SetAutoAdvance);

            // Register a Yarn command to find and start a dialogue
            // dialogueRunner.AddCommandHandler<string>("findAndStartDialogue", FindAndStartDialogue);

            Debug.Log("DialogueController initialized successfully.");
        }

        void Update()
        {
            // Check for player input to advance dialogue
            if (Input.GetKeyDown(KeyCode.Q) && !autoAdvance && lineView != null)
            {
                lineView.OnContinueClicked();
            }
        }


        public void SetAutoAdvance(bool isActive)
        {
            autoAdvance = isActive;

            if (!autoAdvance && autoAdvanceCoroutine != null)
            {
                StopCoroutine(autoAdvanceCoroutine);
                autoAdvanceCoroutine = null;
            }
            else if (autoAdvance && autoAdvanceCoroutine == null)
            {
                autoAdvanceCoroutine = StartCoroutine(AutoAdvanceDialogue());
            }
        }

        private IEnumerator AutoAdvanceDialogue()
        {
            while (autoAdvance)
            {
                yield return new WaitForSeconds(2f); // Adjust the delay as needed

                if (autoAdvance && lineView != null) // Check again to ensure autoAdvance is still true
                {
                    lineView.OnContinueClicked();
                }
            }
        }

        // public void FindAndStartDialogue(string dialogueName)
        // {
        //     if (dialogueRunner == null)
        //     {
        //         Debug.LogError("DialogueRunner is not assigned or found in the scene.");
        //         return;
        //     }

        //     foreach (var project in dialogueProjects)
        //     {
        //         if (project.NodeNames.Contains<String>(dialogueName)) // Correctly check if the node exists in the YarnProject
        //         {
        //             dialogueRunner.SetProject(project); // Set the project in the DialogueRunner
        //             dialogueRunner.StartDialogue(dialogueName); // Start the dialogue
        //             return;
        //         }
        //     }

        //     Debug.LogError($"Dialogue node '{dialogueName}' not found in any of the dialogue projects.");
        // }
    }
}
