using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share
{
    public class Properties
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Type TypeInput { get; set; }
        public string Type
        {
            get { return ShareCode.NormalizeType(this.TypeInput); }
        }
        public string EnumType { get; set; }
        public string EnumNamespace { get; set; }
        public string EnumAngularImport { get; set; }
        public bool IsNullable { get; set; }
        public bool IsRequired { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public int SortOrder { get; set; }
        public int SortType { get; set; }
        public string Regex { get; set; }
        public bool EmailValidation { get; set; }
        public string EnumValues { get; set; }

        public Properties(string name, Type type)
        {
            this.Name = name;
            this.TypeInput = type;

            this.Id = System.Guid.NewGuid().ToString();
            this.EnumAngularImport = "shared/enums";
            this.EmailValidation = false;
            this.EnumType = "";
            this.EnumNamespace = "";
            this.IsNullable = false;
            this.IsRequired = false;
            this.MinLength = null;
            this.MaxLength = null;
            this.SortOrder = 0;
            this.SortType = 0;
            this.EnumValues = "";
            this.Regex = "";

            //Internal Normalize
            this.IsNullable = ShareCode.IsNullable(this.TypeInput);
            this.IsRequired = !this.IsNullable;
        }
    }
}
