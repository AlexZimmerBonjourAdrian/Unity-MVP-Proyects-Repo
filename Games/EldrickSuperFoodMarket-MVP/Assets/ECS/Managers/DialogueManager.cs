using UnityEngine;
using System.Collections.Generic;
using ECS.Core;
using ECS.Components.Dialogue;
using ECS.Components.State;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager instance;
        public static DialogueManager Instance { get { return instance; } }

        private World world;
        private Entity currentDialogueEntity;
        private bool isInDialogue;
        private DialogueComponent currentDialogue;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
        }

        private void Initialize()
        {
            isInDialogue = false;
        }

        private void Start()
        {
        }

        public void StartDialogue(Entity entity)
        {
        }

        public void EndDialogue()
        {
        }

        public void ShowDialogue(string text)
        {
        }

        public void ShowDialogueOptions(List<string> options)
        {
        }

        public void SelectDialogueOption(int optionIndex)
        {
        }

        public bool IsInDialogue()
        {
            return isInDialogue;
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        public void UpdateFlags(string flagName, bool value)
        {
        }
    }
}

