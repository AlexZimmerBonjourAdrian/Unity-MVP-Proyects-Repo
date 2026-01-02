//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEngine.UIElements;
//using UnityEditor.Experimental.GraphView;
//using System.Linq;
//using System;

//public class CBeheaviourTreeView : GraphView
//{
//    public Action<CNodeView> OnNodesSelected;
//    public new class UxmlFactory : UxmlFactory<CBeheaviourTreeView, GraphView.UxmlTraits> { }
//    CBehaviourTree tree;
//    public CBeheaviourTreeView()
//    {
//        Insert(0, new GridBackground());

//        this.AddManipulator(new ContentZoomer());
//        this.AddManipulator(new ContentDragger());
//        this.AddManipulator(new SelectionDragger());
//        this.AddManipulator(new RectangleSelector());

//        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviorTree/Graph/BeheaviourTreeEditor.uss");
//        styleSheets.Add(styleSheet);
//    } 

//    CNodeView FindNodeView(CNode node)
//    {
//        return GetNodeByGuid(node.guid) as CNodeView;
//    }

//    internal void PopulateView(CBehaviourTree tree)
//    {
//        this.tree = tree;

//        graphViewChanged -= OnGraphViewChanged;
//        DeleteElements(graphElements);
//        graphViewChanged += OnGraphViewChanged;

//        if(tree.rootNode == null)
//        {
//            tree.rootNode = tree.CreateNode(typeof(CRootNode)) as CRootNode;
//            EditorUtility.SetDirty(tree);
//            AssetDatabase.SaveAssets();
//        }

//        //Create node View
//        tree.nodes.ForEach(n => CreateNodeView(n));

//        //Create Edge View
//        tree.nodes.ForEach(n => {
//         var children = tree.GetChildren(n);
//            children.ForEach(c => {
//              CNodeView parentView = FindNodeView(n);
//                CNodeView ChildView = FindNodeView(c);

//               Edge edge = parentView.output.ConnectTo(ChildView.input);
//                AddElement(edge);
//            });
//        });

//    }

//    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
//    {
//        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
//    }

//    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
//    {
//        if(graphViewChange.elementsToRemove != null)
//        {
//            graphViewChange.elementsToRemove.ForEach(elem =>
//            {
//                CNodeView nodeView = elem as CNodeView;
//                if(nodeView != null)
//                {
//                    tree.DeleteNode(nodeView.node);
//                }

//                Edge edge = elem as Edge;
//                if (edge != null)
//                {
//                    CNodeView parentView = edge.output.node as CNodeView;
//                    CNodeView childView = edge.input.node as CNodeView;
//                    tree.RemoveChild(parentView.node, childView.node);
//                }
//            });
//        }
//        if (graphViewChange.edgesToCreate != null)
//        {
//            graphViewChange.edgesToCreate.ForEach(edge => {
//                CNodeView parentView = edge.output.node as CNodeView;
//                CNodeView childView = edge.input.node as CNodeView;
//                tree.AddChild(parentView.node, childView.node);
//            });

//        }
//            return graphViewChange;
//    }

//    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
//    {
//        //base.BuildContextualMenu(evt);
//        {
//            var types = TypeCache.GetTypesDerivedFrom<CActionNode>();
//            foreach(var type in types)
//            {
//                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
//            }
//        }
       
//        {
//            var types = TypeCache.GetTypesDerivedFrom<CCompositeNode>();
//            foreach (var type in types)
//            {
//                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
//            }
//        }
     
//        {
//            var types = TypeCache.GetTypesDerivedFrom<CDecoratorNode>();
//            foreach (var type in types)
//            {
//                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
//            }
//        }

//    }

//    void CreateNode(System.Type type)
//    {
//        CNode node = tree.CreateNode(type);
//        CreateNodeView(node);
//    }

//    void CreateNodeView(CNode node)
//    {
//        CNodeView nodeView = new CNodeView(node);
//        nodeView.OnNodesSelected = OnNodesSelected;
//        AddElement(nodeView);
//    }

   
   
//}
