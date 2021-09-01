using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share
{
    public class FileJson
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string NamePlural { get; set; }
        public string DatabaseTableName { get; set; }
        public string Namespace { get; set; }
        public string BaseClass { get; set; }
        public string PrimaryKeyType { get; set; }
        public bool IsMultiTenant { get; set; }
        public bool ShouldCreateUserInterface { get; set; }
        public bool ShouldCreateBackend { get; set; }
        public bool ShouldAddMigration { get; set; }
        public bool ShouldUpdateDatabase { get; set; }
        public bool CreateTests { get; set; }
        public string NavigationProperties { get; set; }
        public string PhysicalFileName { get; set; }
        public List<Properties> Properties { get; set; }
        public List<NavigationProperties> navigationProperties { get; set; }

        public FileJson(string name, string databaseTableName)
        {
            this.Name = name;
            this.DatabaseTableName = databaseTableName;
            this.Id = System.Guid.NewGuid().ToString();
            this.IsMultiTenant = false;
            this.ShouldCreateUserInterface = true;
            this.ShouldCreateBackend = true;
            this.ShouldAddMigration = true;
            this.ShouldUpdateDatabase = true;
            this.CreateTests = true;
            this.NavigationProperties = "[]";
            this.PhysicalFileName = Name + ".json";
            this.NamePlural = this.Name + "s";
            this.Namespace = this.Name + "s";
            this.OriginalName = this.Name;
            this.BaseClass = "Entity";
            this.PrimaryKeyType = "long";
            this.Properties = new List<Properties>();
            this.navigationProperties = new List<NavigationProperties>();
        }
    }
}
