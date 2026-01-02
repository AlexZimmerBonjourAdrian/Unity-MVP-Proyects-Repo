//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor.Experimental.GraphView;
//using System.Linq;
//public class CNodeView : UnityEditor.Experimental.GraphView.Node
//{
//    public Action<CNodeView> OnNodesSelected;
//    public CNode node;
//    public Port input;
//    public Port output;
//    public CNodeView(CNode node)
//    {
//        this.node = node;
//        this.title = node.name;
//        this.viewDataKey = node.guid;

//        style.left = node.position.x;
//        style.top = node.position.y;

//        CreateInputPorts();
//        CreateOutputPorts();

//    }

//    private void CreateOutputPorts()
//    {
//       if(node is CActionNode)
//        {
            
//        }
//        else if(node is CCompositeNode)
//        {
//            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
//        }
//       else if (node is CDecoratorNode)
//        {
//            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
//        }

//        else if (node is CRootNode)
//        {
//            output = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
//        }

//        if (output != null)
//        {
//            output.portName = "";
//            outputContainer.Add(output);
//        }
//    }

//    private void CreateInputPorts()
//    {
//        if(node is CActionNode)
//        {
//            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
//        }
//        else if(node is CCompositeNode)
//        {
//            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
//        }
//        else if(node is CDecoratorNode)
//        {
//            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
//        }
//        else if(node is CRootNode)
//        {
//            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
//        }
//        if(input != null)
//        {
//            input.portName = "";
//            inputContainer.Add(input);
//        }
//    }

//    public override void SetPosition(Rect newPos)
//    {
//        base.SetPosition(newPos);
//        node.position.x = newPos.xMin;
//        node.position.y = newPos.yMin;
//    }

//    public override void OnSelected()
//    {
//        base.OnSelected();
//        if(OnNodesSelected != null)
//        {
//            OnNodesSelected.Invoke(this);
//        }
//    }
//}
