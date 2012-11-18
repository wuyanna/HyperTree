using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace HyperTree.demo
{
    public class HTFileNode : AbstractHTNode
    {
        private string path = null;
        private Dictionary<string,HTFileNode> children = null;

        public HTFileNode(string path)
        {
            this.path = path;
            children = new Dictionary<string, HTFileNode>();

            if (!IsLeaf())
            {
                for (IEnumerator<string> i = Directory.EnumerateFileSystemEntries(path).GetEnumerator(); i.MoveNext(); )
                {
                    HTFileNode child = new HTFileNode(i.Current);
                    AddChild(child);
                }
            }
        }


        override public bool IsLeaf()
        {
            FileAttributes attr = File.GetAttributes(path);
            return (!((attr & FileAttributes.Directory) == FileAttributes.Directory));
        }

        protected void AddChild(HTFileNode child)
        {
            children.Add(child.GetName(), child);
        }

        override public string GetName()
        {
            return path;
        }

        override public IEnumerator Children()
        {
            return this.children.Values.GetEnumerator();
        }

        public override string GetShortName()
        {
            return null;
        }

        //Implement color
        /*public Color GetColor()
        {
            return new Color(path.GetHashCode());
        }*/

        public override int GetExtensionCount()
        {
            return 0;
        }
    }
}
