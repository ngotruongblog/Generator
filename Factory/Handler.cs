using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using Share;

namespace FactoryGenEntity
{
    enum EType
    {
        Create,
        Replace
    }
    public class Handler
    {
        private string namespaceModel = "";
        private string pathOutput = "";
        private List<string> listException = new List<string>();
        private string NameModule = "";
        private string NameProject = "";
        private string PathEntityBuilder = "";
        private EType eType;

        /// <summary>
        /// GetHandlerCreate
        /// </summary>
        /// <param name="_nameSpaceModels">Nơi chưa source entity được gen từ power tool tại source này</param>
        /// <param name="_pathOutput">Output path các file json</param>
        /// <param name="_nameProject">Tên project module abp - lúc new tại tool abp</param>
        /// <returns></returns>
        public static Handler GetHandlerCreate(
            string _nameSpaceModels, string _pathOutput, string _nameProject)
        {
            return new Handler(_nameSpaceModels, _pathOutput, _nameProject, "", "", EType.Create);
        }

        /// <summary>
        /// GetHandlerReplace
        /// </summary>
        /// <param name="_pathEntityBuilder">Đường dẫn đến file *DbContextModelCreatingExtensions.cs</param>
        /// <param name="_nameModule">Tên Microservice module</param>
        /// <param name="_nameSpaceModels">Nơi chưa source entity được gen từ power tool tại source này</param>
        /// <returns></returns>
        public static Handler GetHandlerReplace(
            string _pathEntityBuilder, string _nameModule, string _nameSpaceModels)
        {
            return new Handler(_nameSpaceModels, "", "", _pathEntityBuilder, _nameModule, EType.Replace);
        }

        public void Process()
        {
            if(this.eType == EType.Create)
            {
                Run();
            }
            if(this.eType == EType.Replace)
            {
                Replaces();
            }
        }

        #region Private

        private void Run()
        {
            CreateSetJSON();
        }
        private void Replaces()
        {
            var lines = ReplaceNameCol();
            lines = AddColID(lines);
            lines = AddColForeignKey(lines);

            System.IO.File.WriteAllLines(PathEntityBuilder, lines);
        }
        private Handler() { }
        private Handler(
            string _nameSpaceModels, string _pathOutput, string _nameProject,
            string _pathEntityBuilder, string _nameModule, EType _eType)
        {
            namespaceModel = _nameSpaceModels;
            pathOutput = _pathOutput;
            NameProject = _nameProject;
            listException.Add("ExtensionClass");
            PathEntityBuilder = _pathEntityBuilder;
            NameModule = _nameModule;
            this.eType = _eType;

        }
        private List<FileJson> CreateListFileJson()
        {
            List<FileJson> res = new List<FileJson>();
            List<Type> types = getAssemblys();
            foreach (var item in types) //item = class
            {
                if(item.BaseType.Name != "DbContext")
                {
                    res.Add(createFileJson(item));
                }
            }

            return res;
        }
        private bool check(string name)
        {
            return listException.Contains(name);
        }
        private List<Type> getAssemblys()
        {
            List<Type> res = new List<Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = asm.GetTypes();
                foreach (var item in types)
                {
                    if (item.FullName.StartsWith(namespaceModel) == true && !check(item.Name))
                    {
                        res.Add(item);
                    }
                }

            }

            return res;
        }
        private FileJson createFileJson(Type item)
        {
            TableAttribute nametabledb = Attribute.GetCustomAttribute(item, typeof(TableAttribute)) as TableAttribute;
            FileJson fileJson = new FileJson(item.Name, nametabledb.Name);
            var pros = CreateListProperties(GetInstance(item.FullName));
            fileJson.Properties = pros.Item1;
            fileJson.navigationProperties = pros.Item2;

            return fileJson;
        }
        private bool ExistColumnAtt(object[] attrs)
        {
            var flag = false;
            foreach (object attr in attrs)
            {
                ColumnAttribute colAttribute = attr as ColumnAttribute;
                if (colAttribute != null)
                {
                    flag = true;
                }
            }

            return flag;
        }
        private (List<Properties>, List<NavigationProperties>) CreateListProperties(object obj)
        {
            List<Properties> properties = new List<Properties>();
            List<NavigationProperties> navigationProperties = new List<NavigationProperties>();
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> nameForgenKeys = new List<string>();
            foreach (PropertyInfo item in props)
            {
                var flag = false;
                var pro = new Properties(item.Name, item.PropertyType);
                object[] attrs = item.GetCustomAttributes(true);

                foreach (object attr in attrs)
                {
                    RequiredAttribute requiredAttribute = attr as RequiredAttribute;
                    KeyAttribute keyAttribute = attr as KeyAttribute;
                    ForeignKeyAttribute forKeyAttribute = attr as ForeignKeyAttribute;
                    StringLengthAttribute stringLenAttribute = attr as StringLengthAttribute;

                    if (keyAttribute != null)
                    {
                        flag = true;
                        break;
                    }
                    if (requiredAttribute != null)
                    {
                        pro.IsRequired = true;
                        pro.IsNullable = false;
                    }
                    if (stringLenAttribute != null)
                    {
                        pro.MaxLength = stringLenAttribute.MaximumLength;
                        pro.MinLength = stringLenAttribute.MinimumLength;
                    }
                    if (forKeyAttribute != null)
                    {
                        flag = true;
                        var entityName = item.PropertyType.Name;
                        var nameNav = forKeyAttribute.Name;
                        var typeForgKey = GetTypeForgKey(nameNav, props);
                        var nameDisplayNameProperty = GetNameDisplayNameProperty(item);
                        var nav = new NavigationProperties(entityName, typeForgKey, nameNav, nameDisplayNameProperty, NameProject);
                        navigationProperties.Add(nav);
                        nameForgenKeys.Add(nameNav);
                    }
                }

                if (flag == false && ExistColumnAtt(attrs))
                {
                    properties.Add(pro);
                }
            }
            // Remove duplicate foreign key
            for (int i = properties.Count - 1; i >= 0; i--)
            {
                var value = properties[i].Name;
                if (nameForgenKeys.Contains(value))
                {
                    properties.RemoveAt(i);
                }
            }

            return (properties, navigationProperties);
        }
        private Type GetTypeForgKey(string nameNav, PropertyInfo[] props)
        {
            var typeNav = typeof(long);
            foreach (PropertyInfo subitem in props)
            {
                if (subitem.Name == nameNav)
                {
                    typeNav = subitem.PropertyType;
                    break;
                }
            }

            return typeNav;
        }
        private string GetNameDisplayNameProperty(PropertyInfo item)
        {
            var objTmp = GetInstance(item.PropertyType.FullName);
            Type typeSub = objTmp.GetType();
            PropertyInfo[] subPros = typeSub.GetProperties();
            foreach (var subitem in subPros)
            {
                var flagKey = false;
                object[] attSub = subitem.GetCustomAttributes(true);
                foreach (object itemAtt in attSub)
                {
                    RequiredAttribute requiredAttributeSub = itemAtt as RequiredAttribute;
                    KeyAttribute keyAttributeSub = itemAtt as KeyAttribute;
                    if (requiredAttributeSub != null)
                    {
                        return subitem.Name;
                    }
                    if (keyAttributeSub != null)
                    {
                        flagKey = true;
                        break;
                    }
                }
                if (flagKey == false)
                {
                    return subitem.Name;
                }

            }

            return "ID";
        }
        private object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }
        private void CopyFiles()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string projectDirectory = Directory.GetParent(startupPath).Parent.FullName;

            string pathSourceData = Path.Combine(projectDirectory, "DataOutput");
            string pathSourceModel = Path.Combine(projectDirectory, "ModelOutput");

            string pathTarData = Path.Combine(pathOutput, "Data");
            string pathTarModel = Path.Combine(pathOutput, "Models");

            if (Directory.Exists(pathTarData))
            {
                Directory.Delete(pathTarData, true);
            }
            if (Directory.Exists(pathTarModel))
            {
                Directory.Delete(pathTarModel, true);
            }
            Directory.CreateDirectory(pathTarData);
            Directory.CreateDirectory(pathTarModel);

            foreach (var file in Directory.GetFiles(pathSourceData))
            {
                File.Copy(file, Path.Combine(pathTarData, Path.GetFileName(file)));
            }
            foreach (var file in Directory.GetFiles(pathSourceModel))
            {
                File.Copy(file, Path.Combine(pathTarModel, Path.GetFileName(file)));
            }
        }
       
        private void CreateSetJSON()
        {
            string path = Path.Combine(pathOutput, "FileJson");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);

            foreach (var item in CreateListFileJson())
            {
                var pathTmp = Path.Combine(path, item.PhysicalFileName);
                TemplateFileJson tpl = new TemplateFileJson(item);
                String valueStr = tpl.TransformText();
                System.IO.File.WriteAllText(pathTmp, valueStr);
            }
        }
        private string PatternFind(string nameClass, string namePro)
        {
            return $"nameof({nameClass}.{namePro})";
        }
        private string PatternTableClass(string nameTable)
        {
            return $"b.ToTable({NameModule}DbProperties.DbTablePrefix + \"{nameTable}\", {NameModule}DbProperties.DbSchema);";
        }
        private string PatternAddCol(string name)
        {
            return $"b.Property(x => x.Id).HasColumnName(\"{name}\").ValueGeneratedOnAdd();";
        }
        private string GetNameFill(string namePro)
        {
            return "\"" + namePro + "\"";
        }
        private string[] ReadFile()
        {
            return File.ReadAllLines(PathEntityBuilder);
        }
        private List<string> ReplaceNameCol()
        {
            string[] lines = ReadFile();
            List<Type> types = getAssemblys();
            foreach (var item in types)
            {
                var nameClass = item.Name;
                var fullNameClass = item.FullName;
                var objClass = GetInstance(item.FullName);

                Type t = objClass.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo x in props)
                {
                    var pattern = PatternFind(nameClass, x.Name);
                    var nameCol = GetNameCol(x);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(pattern))
                        {
                            lines[i] = lines[i].Replace(pattern, GetNameFill(nameCol));
                        }
                    }
                }
            }

            return lines.ToList();
        }
        private string FindKey(Type item)
        {
            var objClass = GetInstance(item.FullName);
            Type t = objClass.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo x in props)
            {
                var isKey = false;
                object[] attrs = x.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    KeyAttribute keyAttribute = attr as KeyAttribute;
                    if (keyAttribute != null)
                    {
                        isKey = true;
                    }
                }
                foreach (object attr in attrs)
                {
                    ColumnAttribute colAttribute = attr as ColumnAttribute;
                    if (colAttribute != null && isKey)
                    {
                        return colAttribute.Name;
                    }
                }
            }

            return "";
        }
        private List<string> AddColID(List<string> lines)
        {
            var list = ListColumnKey();
            foreach (var item in list)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains(item.Key))
                    {
                        lines.Insert(i + 2, item.Value);
                    }
                }
            }

            return lines;
        }
        private void AddColItem(Type item, ref Dictionary<string, string> res)
        {
            TableAttribute nametabledb = Attribute.GetCustomAttribute(item, typeof(TableAttribute)) as TableAttribute;
            var nameColKey = FindKey(item);
            var pattern = PatternAddCol(nameColKey);
            var patternTableClass = PatternTableClass(nametabledb.Name);
            res.Add(patternTableClass, pattern);
        }
        private Dictionary<string, string> ListColumnKey()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            List<Type> types = getAssemblys();
            foreach (var item in types) //item = class
            {
                AddColItem(item, ref res);
            }

            return res;
        }
        private List<string> AddColForeignKey(List<string> lines)
        {
            var list = ListColumnForeignKey();
            foreach (var item in list)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains(item.Key))
                    {
                        lines.Insert(i + 1, item.Value);
                    }
                }
            }

            return lines;
        }
        private string GetNameCol(PropertyInfo x)
        {
            object[] attrs = x.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                ColumnAttribute colAttribute = attr as ColumnAttribute;
                if (colAttribute != null)
                {
                    return colAttribute.Name;
                }
            }

            return "";
        }
        private void CreateColForeignKey(object obj, ref Dictionary<string, string> res)
        {
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo item in props)
            {
                object[] attrs = item.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    ForeignKeyAttribute forKeyAttribute = attr as ForeignKeyAttribute;
                    if (forKeyAttribute != null)
                    {
                        var namePros = GetNameForeignKey(attrs);
                        var typeName = GetNameTypeForeignKey(attrs, item);
                        var typeForgKey = GetTypeForgKey(namePros, props);
                        var isRequired = !ShareCode.IsNullable(typeForgKey);
                        var nameColumnDB = FindNameColumnDBForeignKey(props, namePros);

                        var patternFind = PatternForeignKey(typeName, namePros, isRequired);
                        var patternFill = PatternFillForeignKey(namePros, nameColumnDB, isRequired);
                        if (!res.ContainsKey(patternFind))
                            res.Add(patternFind, patternFill);
                    }
                }
            }
        }
        private Dictionary<string, string> ListColumnForeignKey()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            List<Type> types = getAssemblys();
            foreach (var item in types) //item = class
            {
                CreateColForeignKey(GetInstance(item.FullName), ref res);
            }

            return res;
        }
        private string FindNameColumnDBForeignKey(PropertyInfo[] props, string namePro)
        {
            foreach (PropertyInfo item in props)
            {
                if (item.Name == namePro)
                {
                    object[] attrs = item.GetCustomAttributes(true);
                    foreach (object attr in attrs)
                    {
                        ColumnAttribute colAttribute = attr as ColumnAttribute;
                        if (colAttribute != null)
                        {
                            return colAttribute.Name;
                        }
                    }
                }
            }

            return "";
        }
        private string PatternFillForeignKey(string name, string nameDb, bool isRequired)
        {
            if (isRequired)
            {
                return $"b.Property(x => x.{name}).HasColumnName(\"{nameDb}\").IsRequired();";
            }
            else
            {
                return $"b.Property(x => x.{name}).HasColumnName(\"{nameDb}\");";
            }

        }
        private string GetNameTypeForeignKey(object[] attrs, PropertyInfo item)
        {
            foreach (object attr in attrs)
            {
                ForeignKeyAttribute forKeyAttribute = attr as ForeignKeyAttribute;
                if (forKeyAttribute != null)
                {
                    return item.PropertyType.Name;
                }
            }

            return "";
        }
        private string GetNameForeignKey(object[] attrs)
        {
            foreach (object attr in attrs)
            {
                ForeignKeyAttribute forKeyAttribute = attr as ForeignKeyAttribute;
                if (forKeyAttribute != null)
                {
                    return forKeyAttribute.Name;
                }
            }

            return "";
        }
        private string PatternForeignKey(string nameClass, string proName, bool isRequired)
        {
            if (isRequired)
            {
                return $"b.HasOne<{nameClass}>().WithMany().IsRequired().HasForeignKey(x => x.{proName});";
            }
            else
            {
                return $"b.HasOne<{nameClass}>().WithMany().HasForeignKey(x => x.{proName});";
            }
        }

        #endregion
    }
}
