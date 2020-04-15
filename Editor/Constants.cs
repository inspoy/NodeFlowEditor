using UnityEngine;

namespace Instech.NodeEditor
{
    public static class Strings
    {
        public const string MenuItemOpenWindow = "Instech/NodeEditor/Open Window";
        public const string EditorWindowTitle = "节点编辑器";
        public const string EditorWindowContextAddNode = "添加节点";
        public const string EditorWindowNodeRemove = "删除节点";
    }

    public static class Styles
    {
        public static readonly GUIStyle NodeNormal;
        public static readonly GUIStyle NodeSelected;

        static Styles()
        {
            NodeNormal = new GUIStyle();
            NodeSelected = new GUIStyle(NodeNormal)
            {
                border = new RectOffset(5, 5, 5, 5)
            };
        }
    }
}
