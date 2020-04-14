using UnityEngine;

namespace Instech.NodeEditor
{
    public enum NodePortType
    {
        In,
        Out
    }

    public class NodePort
    {
        public Rect Rect { get; private set; }
        public NodePortType Type { get; private set; }
        private Node _node;

        public NodePort(Node node, NodePortType type)
        {
            Rect = new Rect(0, 0, 10, 10);
            _node = node;
            Type = type;
        }

        public void Draw()
        {
            var tmp = Rect;
            tmp.position = _node.GetPortPosition(this);
            Rect = tmp;
            if (GUI.Button(Rect, ""))
            {
                _node.OnPortClick(this);
            }
        }
    }
}
