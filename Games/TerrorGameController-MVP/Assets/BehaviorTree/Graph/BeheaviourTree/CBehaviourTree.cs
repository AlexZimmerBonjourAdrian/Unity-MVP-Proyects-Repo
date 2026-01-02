//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//[CreateAssetMenu()]
//public class CBehaviourTree : ScriptableObject
//{
//    public CNode rootNode;
//    public CNode.State treeState = CNode.State.Running;
//    public List<CNode> nodes = new List<CNode>();
//    public CNode.State Update() {
//        if(rootNode.state == CNode.State.Running)
//        {
//            treeState = rootNode.Update();
//        }
//        return treeState;
//    }

//    public CNode CreateNode(System.Type type)
//    {
//        CNode node = ScriptableObject.CreateInstance(type) as CNode;
//        node.name = type.Name;
//        node.guid = GUID.Generate().ToString();
//        nodes.Add(node);

//        AssetDatabase.AddObjectToAsset(node, this);
//        AssetDatabase.SaveAssets();
//        return node;

//    }

//    public void DeleteNode(CNode node)
//    {
//        nodes.Remove(node);
//        AssetDatabase.RemoveObjectFromAsset(node);
//        AssetDatabase.SaveAssets();
//    }
//    public void AddChild(CNode parent, CNode child)
//    {
//        CDecoratorNode decorator = parent as CDecoratorNode;
//        if (decorator)
//        {
//            decorator.child = child;
//        }
//        CRootNode rootNode = parent as CRootNode;
//        if (rootNode)
//        {
//            rootNode.child = child;
//        }
//        CCompositeNode Composite = parent as CCompositeNode;
//        if (Composite)
//        {
//            Composite.children.Add(child);
//        }
//    }
//    public void RemoveChild(CNode parent, CNode child)
//    {
//        CDecoratorNode decorator = parent as CDecoratorNode;
//        if (decorator)
//        {
//            decorator.child = null;
//        }
//        CRootNode rootNode = parent as CRootNode;
//        if (rootNode)
//        {
//            rootNode.child = null;
//        }
//        CCompositeNode Composite = parent as CCompositeNode;
//        if (Composite)
//        {
//            Composite.children.Remove(child);
//        }
//    }
//    public List<CNode> GetChildren(CNode parent)
//    {
//        List<CNode> children = new List<CNode>();

//        CDecoratorNode decorator = parent as CDecoratorNode;
//        if (decorator && decorator.child != null)
//        {
//            children.Add(decorator.child);
//        }
        
//        CRootNode rootNode = parent as CRootNode;
//        if(rootNode && rootNode.child != null)
//        {
//            children.Add(rootNode.child);
//        }


//        CCompositeNode Composite = parent as CCompositeNode;
//        if (Composite)
//        {
//            return Composite.children;
//        }
//        return children;
//    }
//}
