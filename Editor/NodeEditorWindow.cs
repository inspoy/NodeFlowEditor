using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Instech.NodeEditor
{
    public class NodeEditorWindow : EditorWindow
    {
        private readonly List<Node> _nodes = new List<Node>();
        private readonly List<Node> _nodesToRemove = new List<Node>();
        private readonly List<ConnectionLine> _connectionLines = new List<ConnectionLine>();
        private readonly List<ConnectionLine> _linesToRemove = new List<ConnectionLine>();
        private NodePort _curSelectedPort;
        private bool _isDragging;

        [MenuItem(Strings.MenuItemOpenWindow)]
        private static void OpenWindow()
        {
            var window = GetWindow<NodeEditorWindow>(Strings.EditorWindowTitle);
            window.titleContent = new GUIContent(Strings.EditorWindowTitle);
        }

        private void OnGUI()
        {
            DrawNodes();
            DrawConnectionLines();
            ProcessRemovement();
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void DrawNodes()
        {
            foreach (var node in _nodes)
            {
                node.Draw();
            }
        }

        private void DrawConnectionLines()
        {
            foreach (var line in _connectionLines)
            {
                line.Draw();
            }
        }

        private void ProcessRemovement()
        {
            foreach (var line in _linesToRemove)
            {
                _connectionLines.Remove(line);
            }

            _linesToRemove.Clear();

            foreach (var node in _nodesToRemove)
            {
                _nodes.Remove(node);
            }

            _nodesToRemove.Clear();
        }

        private void ProcessNodeEvents(Event e)
        {
            foreach (var node in _nodes)
            {
                var guiChanged = node.ProcessEvents(e);
                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        // 鼠标右键
                        ShowContextMenu(e.mousePosition);
                    }

                    break;
                case EventType.MouseDrag:
                    if (e.button == 2)
                    {
                        // 鼠标中键
                        OnDrag(e.delta);
                    }

                    break;
            }
        }

        private void OnDrag(Vector2 delta)
        {
            foreach (var item in _nodes)
            {
                item.Drag(delta);
            }

            GUI.changed = true;
        }

        private void ShowContextMenu(Vector2 pos)
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent(Strings.EditorWindowContextAddNode), false, () => OnClickAddNode(pos));
            genericMenu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 pos)
        {
            var node = new Node(pos, new Vector2(200, 50), OnClickRemoveNode) { Title = "New Node" };
            node.AddPort(NodePortType.In, OnClickPort);
            node.AddPort(NodePortType.Out, OnClickPort);
            _nodes.Add(node);
        }

        private void OnClickPort(NodePort port)
        {
            if (_curSelectedPort == null)
            {
                _curSelectedPort = port;
                _curSelectedPort.State = NodePort.PortState.Selected;
                return;
            }

            if (port.Type != _curSelectedPort.Type && port.Owner != _curSelectedPort.Owner)
            {
                // Connect with two port
                _curSelectedPort.State = NodePort.PortState.Connected;
                port.State = NodePort.PortState.Connected;
                if (port.Type == NodePortType.In)
                {
                    _connectionLines.Add(new ConnectionLine(_curSelectedPort, port, OnClickRemoveLine));
                }
                else
                {
                    _connectionLines.Add(new ConnectionLine(port, _curSelectedPort, OnClickRemoveLine));
                }
            }

            _curSelectedPort.State = NodePort.PortState.Idle;
            _curSelectedPort = null;
        }

        private void OnClickRemoveLine(ConnectionLine line)
        {
            _linesToRemove.Add(line);
        }

        private void OnClickRemoveNode(Node node)
        {
            _nodesToRemove.Add(node);
            foreach (var line in _connectionLines)
            {
                if (node.HasPort(line.FromPort) > 0 || node.HasPort(line.ToPort) < 0)
                {
                    _linesToRemove.Add(line);
                }
            }
        }
    }
}
