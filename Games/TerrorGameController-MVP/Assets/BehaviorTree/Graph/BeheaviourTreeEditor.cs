using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


//public class BeheaviourTreeEditor : EditorWindow
//{
//    CBeheaviourTreeView treeView;
//    InspectorView inspectorView;
//    [MenuItem("BeheaviourTreeEditor/Editor ... ")]
//    public static void OpenWindow()
//    {
//        BeheaviourTreeEditor wnd = GetWindow<BeheaviourTreeEditor>();
//        wnd.titleContent = new GUIContent("BeheaviourTreeEditor");
//    }

//    public void CreateGUI()
//    {
//        // Each editor window contains a root VisualElement object
//        VisualElement root = rootVisualElement;

//        // Import UXML
//        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/BehaviorTree/Graph/BeheaviourTreeEditor.uxml");
//        visualTree.CloneTree(root);
//        // A stylesheet can be added to a VisualElement.
//        // The style will be applied to the VisualElement and all of its children.
//        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviorTree/Graph/BeheaviourTreeEditor.uss");
//        root.styleSheets.Add(styleSheet);

//        treeView = root.Q<CBeheaviourTreeView>();
//        inspectorView = root.Q<InspectorView>();
//        treeView.OnNodesSelected = OnNodeSelectionChanged;
//        OnSelectionChange();
//    }

//    private void OnSelectionChange()
//    {
//        CBehaviourTree tree = Selection.activeObject as CBehaviourTree;
//        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
//        {
//            treeView.PopulateView(tree);
//        }
//    }

//    void OnNodeSelectionChanged(CNodeView node)
//    {
//        inspectorView.UpdateSelection(node);
//    }
//}