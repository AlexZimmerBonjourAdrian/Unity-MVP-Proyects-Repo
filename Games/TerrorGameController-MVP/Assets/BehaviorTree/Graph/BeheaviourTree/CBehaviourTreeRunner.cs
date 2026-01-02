//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CBehaviourTreeRunner : MonoBehaviour
//{
//    CBehaviourTree tree;

//    // Start is called before the first frame update
//    void Start()
//    {
//        var pause1 = ScriptableObject.CreateInstance<CWaitNode>();
//        tree = ScriptableObject.CreateInstance<CBehaviourTree>();

//        var log1 = ScriptableObject.CreateInstance<CDebugLogNode>();
//        log1.message = "Hello World 111";

//        var pause2 = ScriptableObject.CreateInstance<CWaitNode>();

//        var log2 = ScriptableObject.CreateInstance<CDebugLogNode>();
//        log2.message = "Hello World 222";

//        var pause3 = ScriptableObject.CreateInstance<CWaitNode>();

//        var log3 = ScriptableObject.CreateInstance<CDebugLogNode>();
//        log3.message = "Hello World 333";

//        var sequence = ScriptableObject.CreateInstance<CSequencerNode>();
        
//        sequence.children.Add(log1);
//        sequence.children.Add(pause1);
//        sequence.children.Add(log2);
//        sequence.children.Add(pause2);
//        sequence.children.Add(log3);
//        sequence.children.Add(pause3);

//        var loop = ScriptableObject.CreateInstance<CRepeatNode>();
//        loop.child = sequence;

//        tree.rootNode = loop;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        tree.Update();
//    }
//}
