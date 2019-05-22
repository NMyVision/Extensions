using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NMyVision.Extensions
{
    public static class XmlExtensions
    {
        /// <summary>
        ///  Returns the System.Xml.Linq.XAttribute of this System.Xml.Linq.XElement that has the specified System.Xml.Linq.XName.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xe"></param>
        /// <param name="name">The System.Xml.Linq.XName of the System.Xml.Linq.XAttribute to get.</param>
        /// <param name="defaultValue">The value returned if attribute doesn't exist or throws error.</param>
        /// <returns>Strongly typed value of the specified attribute.</returns>
        public static T GetAttribute<T>(this XElement xe, string name, T defaultValue)
        {
            var xa = xe.Attribute(name);
            if (xa == null)
                return defaultValue;

            return xa.Value.To<T>();
        }

        /// <summary>
        /// Gets the value of the attribute with the specified local name and namespace URI.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xe"></param>
        /// <param name="name">The local name of the attribute. localName is case-sensitive.</param>
        /// <param name="defaultValue">The value returned if attribute doesn't exist or throws error.</param>
        /// <returns>A System.String that contains the value of the specified attribute; System.String.Empty if a matching attribute is not found, or if the System.Xml.XPath.XPathNavigator is not positioned on an element node.</returns>
        public static T GetAttribute<T>(this XPathNavigator xe, string name, T defaultValue) 
            => GetAttribute<T>(xe, name, string.Empty, defaultValue);

        /// <summary>
        /// Gets the value of the attribute with the specified local name and namespace URI.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xe"></param>
        /// <param name="name">The local name of the attribute. localName is case-sensitive.</param>
        /// <param name="namespaceURI">The namespace URI of the attribute.</param>
        /// <param name="defaultValue">The value returned if attribute doesn't exist or throws error.</param>
        /// <returns>A System.String that contains the value of the specified attribute; System.String.Empty if a matching attribute is not found, or if the System.Xml.XPath.XPathNavigator is not positioned on an element node.</returns>
        public static T GetAttribute<T>(this XPathNavigator xe, string name, string namespaceURI, T defaultValue)
        {
            
            var text = xe.GetAttribute(name, namespaceURI);
            if (string.IsNullOrEmpty(text))
                return defaultValue;

            return text.To<T>();
        }
    }
}
