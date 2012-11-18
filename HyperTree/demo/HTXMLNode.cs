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
using System.Collections.Generic;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.Diagnostics;

namespace HyperTree.demo
{
    public class HTXMLNode : AbstractHTNode
    {
        private XElement element = null;
        private Dictionary<string, HTXMLNode> children = null;
        private int exCount = 0;

        public HTXMLNode(XElement element)
        {
            this.element = element;
            this.children = new Dictionary<string, HTXMLNode>();

            if (this.GetColor() != Colors.Black)
            {
                this.exCount += 1;
            }
            if (!IsLeaf())
            {
                
                foreach (var e in element.Elements("entry"))
                {
                    HTXMLNode child = new HTXMLNode(e);
                    
                    this.exCount += child.GetExtensionCount();
                    AddChild(child);
                    //Debug.WriteLine("entry:" + e.Attribute("name").Value);
                }
            }
            Debug.WriteLine("entry:" + this.element.Attribute("name").Value + "exCount:" + this.GetExtensionCount());
        }

        private void AddChild(HTXMLNode child)
        {
            this.children.Add(child.GetName(), child);
        }

        public override IEnumerator Children()
        {
            return this.children.Values.GetEnumerator();
        }

        public override bool IsLeaf()
        {
            //Debug.WriteLine("is this element:" + element.Attribute("name").Value + "a leaf?" + !(this.element.HasElements));
            return (!(this.element.HasElements));
        }

        public override String GetName()
        {
            if (this.element.Name == "entry")
            {
                return this.element.Attribute("name").Value;
            }
            return null;
        }

        public override Color GetColor()
        {
            if (this.element.Attribute("color") != null)
            {
                switch (this.element.Attribute("color").Value)
                {
                    case "red":
                        return Colors.Red;
                        break;
                    case "blue":
                        return Colors.Blue;
                        break;
                    case "green":
                        return Colors.Green;
                        break;
                    case "orange":
                        return Colors.Orange;
                        break;
                    case "purple":
                        return Colors.Purple;
                        break;
                    default:
                        return Colors.Black;
                        break;
                }
            }
                
            return Colors.Black;
        }

        //TODO
        public override String GetShortName()
        {
            return null;
        }

        public override int GetExtensionCount()
        {
            return this.exCount;
        }

            
    }
}
