using GitHub_Intermediary_Api.Interfaces;
using System.Reflection;
using System.Xml.Linq;

namespace GitHub_Intermediary_Api.Services {
    public class Converter : IConverter {
        public string ConvertToXml(object obj, string rootName) {
            XElement root = new(rootName);
            ParseObjectToXml(obj, root);
            return root.ToString();
        }

        private static void ParseObjectToXml(object obj, XElement root) {
            if (obj == null) return;

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties) {
                object? value = property.GetValue(obj);
                if (value is IEnumerable<object> list && value is not string) {
                    XElement listElement = new(property.Name);
                    foreach (object item in list) {
                        XElement listItemElement = new(property.Name.TrimEnd('s'));
                        ParseObjectToXml(item, listItemElement);
                        listElement.Add(listItemElement);
                    }
                    root.Add(listElement);
                } else {
                    root.Add(new XElement(property.Name, value ?? ""));
                }
            }
        }
    }
}
