/**
 * == CoolStuff_2019.1 ==
 * Assembly: Assembly-CSharp-Editor
 * FileName: Node.cs
 * Created on 2020/04/09 by chengyongtan
 * All rights reserved.
 */

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

        public Node(Vector2 position, Vector2 size)
        {
            _rect = new Rect(position, size);
        }

        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
        }

        public void Draw()
        {
            GUI.Box(_rect, Title);
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
                            GUI.changed = true;
                        }
                        else
                        {
                            GUI.changed = true;
                        }
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
    }
}
