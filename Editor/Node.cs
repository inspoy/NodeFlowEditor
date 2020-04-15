using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Instech.NodeEditor
{
    /// <summary>
    /// 单个节点类
    /// </summary>
    public class Node
    {
        public string Title { get; set; }

        private Rect _rect;
        private bool _isDragging;
        private bool _isSelected;
        private readonly List<NodePort> _inPorts;
        private readonly List<Action<NodePort>> _inPortCallbacks;
        private readonly List<NodePort> _outPorts;
        private readonly List<Action<NodePort>> _outPortCallbacks;
        private readonly Action<Node> _removeCallback;

        public Node(Vector2 position, Vector2 size, Action<Node> removeNodeCallback)
        {
            _rect = new Rect(position, size);
            _inPorts = new List<NodePort>();
            _inPortCallbacks = new List<Action<NodePort>>();
            _outPorts = new List<NodePort>();
            _outPortCallbacks = new List<Action<NodePort>>();
            _removeCallback = removeNodeCallback;
        }

        public void AddPort(NodePortType type, Action<NodePort> onClick)
        {
            if (type == NodePortType.In)
            {
                _inPorts.Add(new NodePort(this, type));
                _inPortCallbacks.Add(onClick);
            }
            else if (type == NodePortType.Out)
            {
                _outPorts.Add(new NodePort(this, type));
                _outPortCallbacks.Add(onClick);
            }
        }

        /// <summary>
        /// 节点是否拥有指定端口
        /// </summary>
        /// <returns>0-不拥有，正数-输出端口序号，负数-输入端口序号</returns>
        public int HasPort(NodePort port)
        {
            var idx = _inPorts.IndexOf(port);
            if (idx >= 0)
            {
                return -idx - 1;
            }

            idx = _outPorts.IndexOf(port);
            if (idx >= 0)
            {
                return idx + 1;
            }

            return 0;
        }

        public Vector2 GetPortPosition(NodePort port)
        {
            for (var i = 0; i < _inPorts.Count; i++)
            {
                var item = _inPorts[i];
                if (item == port)
                {
                    return new Vector2(_rect.x, _rect.y + _rect.height - 25 * (i + 1));
                }
            }

            for (var i = 0; i < _outPorts.Count; i++)
            {
                var item = _outPorts[i];
                if (item == port)
                {
                    return new Vector2(_rect.x + _rect.width - 10, _rect.y + _rect.height - 25 * (i + 1));
                }
            }

            throw new ArgumentOutOfRangeException(nameof(port), "Port is not bound to this node.");
        }

        public void OnPortClick(NodePort port)
        {
            for (var i = 0; i < _inPorts.Count; ++i)
            {
                if (_inPorts[i] == port)
                {
                    _inPortCallbacks[i]?.Invoke(port);
                }
            }

            for (var i = 0; i < _outPorts.Count; ++i)
            {
                if (_outPorts[i] == port)
                {
                    _outPortCallbacks[i]?.Invoke(port);
                }
            }
        }

        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
        }

        public void Draw()
        {
            foreach (var item in _inPorts)
            {
                item.Draw();
            }

            foreach (var item in _outPorts)
            {
                item.Draw();
            }

            GUI.Box(_rect, Title, _isSelected ? Styles.NodeSelected : Styles.NodeNormal);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (_rect.Contains(e.mousePosition))
                        {
                            _isDragging = true;
                            _isSelected = true;
                            e.Use();
                        }
                        else
                        {
                            _isSelected = false;
                        }

                        return true;
                    }
                    else if (e.button == 1 && _rect.Contains(e.mousePosition))
                    {
                        ShowContextMenu();
                        e.Use();
                    }

                    break;

                case EventType.MouseUp:
                    _isDragging = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && _isDragging)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }

                    break;
            }

            return false;
        }

        private void ShowContextMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent(Strings.EditorWindowNodeRemove), false, OnClickRemoveNode);
            menu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            _removeCallback?.Invoke(this);
        }
    }
}
