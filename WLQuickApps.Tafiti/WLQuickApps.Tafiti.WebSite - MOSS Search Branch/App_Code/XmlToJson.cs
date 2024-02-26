using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace WLQuickApps.Tafiti.WebSite
{
    /// <summary>
    /// XmlToJson convers an XmlDocument to JSON format.
    /// </summary>
    public static class XmlToJson
    {
        const string ATTRIBUTE_PREFIX = "_";

        public static string Convert(XmlDocument xmlDoc)
        {
            return Convert(xmlDoc.ChildNodes);
        }

        public static string Convert(XmlNodeList nodeList)
        {
            ValueType root = ValueType.NewCompoundValue();
            foreach (XmlNode childNode in nodeList)
            {
                string name = childNode.Name;
                ValueType childValue = GetValue(childNode);
                if (childValue != null)
                    root.Add(name, childValue);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            root.ToJson(sb);
            return sb.ToString();
        }

        private static ValueType GetValue(XmlNode xmlNode)
        {
            ValueType value;
            switch (xmlNode.NodeType)
            {
                case XmlNodeType.XmlDeclaration:
                    XmlDeclaration declaration = (XmlDeclaration)xmlNode;
                    value = ValueType.NewCompoundValue();
                    if (!string.IsNullOrEmpty(declaration.Version))
                    {
                        value.Add(Attribute("version"), ValueType.NewStringValue(declaration.Version));
                    }
                    if (!string.IsNullOrEmpty(declaration.Encoding))
                    {
                        value.Add(Attribute("encoding"), ValueType.NewStringValue(declaration.Encoding));
                    }
                    if (!string.IsNullOrEmpty(declaration.Standalone))
                    {
                        value.Add(Attribute("standalone"), ValueType.NewStringValue(declaration.Standalone));
                    }
                    break;

                case XmlNodeType.Element:
                    value = GetXmlElementValue(xmlNode);
                    break;

                case XmlNodeType.Text:
                    value = ValueType.NewStringValue(xmlNode.Value);
                    break;

                default:
                    value = null;
                    break;
            }
            return value;
        }

        private static ValueType GetXmlElementValue(XmlNode xmlNode)
        {
            ValueType value;
            if (xmlNode.HasChildNodes || (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0))
            {
                if (xmlNode.Attributes.Count == 0 && xmlNode.ChildNodes.Count == 1 && xmlNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                {
                    // Special case: value of "<name>value</name>" => "value"
                    value = ValueType.NewStringValue(xmlNode.ChildNodes[0].Value);
                }
                else
                {
                    // Parse a compound value
                    value = ValueType.NewCompoundValue();
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        string name = Attribute(attribute.Name);
                        ValueType attrValue = ValueType.NewStringValue(attribute.Value);
                        value.Add(name, attrValue);
                    }
                    foreach (XmlNode childNode in xmlNode.ChildNodes)
                    {
                        string name = childNode.Name;
                        ValueType childValue = GetValue(childNode);
                        if (childValue != null)
                            value.Add(name, childValue);
                    }
                }
            }
            else
            {
                // Special case: value of "<name/>" => ""
                value = ValueType.NewStringValue("");
            }
            return value;
        }

        private static string Attribute(string s)
        {
            return ATTRIBUTE_PREFIX + s;
        }

        private enum ValueKind { StringValue, CompoundValue, CollectionValue };

        private class ValueType
        {
            public ValueKind Kind;
            public string String; // valid iff Kind == NodeKind.StringNode
            public Dictionary<string, ValueType> Compound; // valid iff Kind == NodeKind.CompoundValue
            public List<ValueType> Collection; // valid iff Kind == NodeKind.CollectionValue

            public static ValueType NewStringValue(string s)
            {
                ValueType v = new ValueType();
                v.Kind = ValueKind.StringValue;
                v.String = s;
                return v;
            }

            public static ValueType NewCompoundValue()
            {
                ValueType v = new ValueType();
                v.Kind = ValueKind.CompoundValue;
                v.Compound = new Dictionary<string, ValueType>();
                return v;
            }

            public static ValueType NewCollectionValue()
            {
                ValueType v = new ValueType();
                v.Kind = ValueKind.CollectionValue;
                v.Collection = new List<ValueType>();
                return v;
            }

            public void Add(string name, ValueType value)
            {
                switch (this.Kind)
                {
                    case ValueKind.StringValue:
                        throw new InvalidOperationException();
                    case ValueKind.CollectionValue:
                        this.Collection.Add(value);
                        break;
                    case ValueKind.CompoundValue:
                        if (this.Compound.ContainsKey(name))
                        {
                            this.Compound[name].EnsureIsCollection();
                            this.Compound[name].Collection.Add(value);
                        }
                        else
                        {
                            this.Compound.Add(name, value);
                        }
                        break;
                }
            }

            // Convert this instance to a CollectionValue if it isn't already one
            private void EnsureIsCollection()
            {
                switch (this.Kind)
                {
                    case ValueKind.StringValue:
                        ValueType old = ValueType.NewStringValue(this.String);
                        this.String = null;
                        this.Kind = ValueKind.CollectionValue;
                        this.Collection = new List<ValueType>();
                        this.Collection.Add(old);
                        break;
                    case ValueKind.CompoundValue:
                        ValueType old2 = (ValueType)this.MemberwiseClone();
                        this.Compound = null;
                        this.Kind = ValueKind.CollectionValue;
                        this.Collection = new List<ValueType>();
                        this.Collection.Add(old2);
                        break;
                    case ValueKind.CollectionValue:
                        break;
                }
            }

            public void ToJson(System.Text.StringBuilder sb)
            {
                switch (this.Kind)
                {
                    case ValueKind.StringValue:
                        sb.AppendFormat(" \"{0}\"", Json.JScriptEscape(this.String));
                        break;
                    case ValueKind.CollectionValue:
                        sb.Append(" [");
                        foreach (ValueType v in this.Collection)
                        {
                            v.ToJson(sb);
                            sb.Append(", ");
                        }
                        sb.Remove(sb.Length - 2, 1); // remove the last ','
                        sb.Append("]");
                        break;
                    case ValueKind.CompoundValue:
                        sb.Append(" {");
                        foreach (string name in this.Compound.Keys)
                        {
                            sb.AppendFormat(" \"{0}\" :", Json.JScriptEscape(name));
                            ValueType v = this.Compound[name];
                            if (v != null)
                                v.ToJson(sb);
                            sb.Append(", ");
                        }
                        sb.Remove(sb.Length - 2, 1); // remove the last ','
                        sb.Append("}");
                        break;
                }
            }
       }
    }
}