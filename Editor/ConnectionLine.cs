using UnityEditor;
using UnityEngine;

namespace Instech.NodeEditor
{
    public class ConnectionLine
    {
        public NodePort FromPort { get; private set; }
        public NodePort ToPort { get; private set; }

        public ConnectionLine(NodePort from, NodePort to)
        {
            FromPort = from;
            ToPort = to;
        }

        public void Draw()
        {
            Handles.DrawBezier(
                FromPort.Rect.center,
                ToPort.Rect.center,
                FromPort.Rect.center + Vector2.left * 50f,
                FromPort.Rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            if (Handles.Button((FromPort.Rect.center + ToPort.Rect.center) * 0.5f,
                Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                Debug.Log("xxx");
            }
        }
    }
}
