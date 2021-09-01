using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share
{
    public class NavigationProperties
    {
        public string Namespace { get; set; }
        public string EntityName { get; set; }
        public string EntityNameWithDuplicationNumber { get; set; }
        public string EntitySetNameWithDuplicationNumber { get; set; }
        public string EntitySetName { get; set; }
        public string DtoNamespace { get; set; }
        public string DtoEntityName { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ReferencePropertyName { get; set; }
        public string DisplayProperty { get; set; }
        public string UiPickType { get; set; }
        public bool IsRequired { get; set; }

        public NavigationProperties(
            string entityName, Type type, string name , string displayProperty, string NameProject)
        {
            this.EntityName = entityName;
            this.Namespace = NameProject + "." + entityName + "s";
            this.EntityNameWithDuplicationNumber = entityName;
            this.EntitySetNameWithDuplicationNumber = entityName + "s";
            this.EntitySetName = entityName + "s";
            this.DtoNamespace = NameProject + "." + entityName + "s";
            this.DtoEntityName = entityName + "Dto";
            this.Type = ShareCode.NormalizeType(type);
            this.Name = name;
            this.ReferencePropertyName = entityName;
            this.DisplayProperty = displayProperty;
            this.UiPickType = "Modal";
            this.IsRequired = !ShareCode.IsNullable(type);
        }
    }
}
