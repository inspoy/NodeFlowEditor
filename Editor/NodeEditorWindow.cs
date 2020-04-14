using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Instech.NodeEditor
{
    public class NodeEditorWindow : EditorWindow
    {
        private readonly List<Node> _nodes = new List<Node>();
        private readonly List<ConnectionLine> _connectionLines = new List<ConnectionLine>();

        [MenuItem(Constants.MenuItemOpenWindow)]
        private static void OpenWindow()
        {
            var window = GetWindow<NodeEditorWindow>(Constants.EditorWindowTitle);
            window.titleContent = new GUIContent(Constants.EditorWindowTitle);
        }

        private void OnGUI()
        {
            DrawNodes();
            DrawConnectionLines();
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
            }
        }

        private void ShowContextMenu(Vector2 pos)
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent(Constants.EditorWindowContextAddNode), false, () => OnClickAddNode(pos));
            genericMenu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 pos)
        {
            var node = new Node(pos, new Vector2(200, 50)) { Title = "New Node" };
            _nodes.Add(node);
        }
    }
}
