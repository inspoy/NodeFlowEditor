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
        public enum PortState
        {
            Idle,
            Selected,
            Connected
        }

        public Rect Rect { get; private set; }
        public NodePortType Type { get; private set; }
        public Node Owner { get; private set; }
        public PortState State { get; set; }

        public NodePort(Node owner, NodePortType type)
        {
            Rect = new Rect(0, 0, 10, 10);
            Owner = owner;
            Type = type;
            State = PortState.Idle;
        }

        public void Draw()
        {
            var tmp = Rect;
            tmp.position = Owner.GetPortPosition(this);
            Rect = tmp;
            var label = "";
            switch (State)
            {
                case PortState.Selected:
                    label = ".";
                    break;
                case PortState.Connected:
                    label = "x";
                    break;
            }

            if (GUI.Button(Rect, label))
            {
                Owner.OnPortClick(this);
            }
        }
    }
}
