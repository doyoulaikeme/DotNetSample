// Copyright © 2010 Xamasoft

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
//using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using JsonToObject.CodeWriters;
//using JShibo.Net;



namespace JsonToObject
{
    /// <summary>
    /// Json转对象代码生成器
    /// </summary>
    public class JsonClassGenerator : IJsonClassGeneratorConfig
    {
        #region 字段

        /// <summary>
        /// 
        /// </summary>
        public string Example { get; set; }
        public string TargetFolder { get; set; }
        public string Namespace { get; set; }
        /// <summary>
        /// 命名空间的名称
        /// </summary>
        public string SecondaryNamespace { get; set; }
        public bool UseProperties { get; set; }
        public bool InternalVisibility { get; set; }
        /// <summary>
        /// 只有get属性，并且是通过对象获得属性中的值
        /// </summary>
        public bool ExplicitDeserialization { get; set; }
        public bool NoHelperClass { get; set; }
        /// <summary>
        /// 主类名称
        /// </summary>
        public string MainClass { get; set; }
        /// <summary>
        /// 是否添加JsonProperty头标记
        /// </summary>
        public bool UsePascalCase { get; set; }
        public bool UseNestedClasses { get; set; }
        public bool ApplyObfuscationAttributes { get; set; }
        /// <summary>
        /// 是否写入到独立文件
        /// </summary>
        public bool SingleFile { get; set; }
        public ICodeWriter CodeWriter { get; set; }
        public TextWriter OutputStream { get; set; }
        public bool AlwaysUseNullableValues { get; set; }
        /// <summary>
        /// 是否生成帮助文档
        /// </summary>
        public bool ExamplesInDocumentation { get; set; }


        public IList<JsonType> Types { get; private set; }
        private HashSet<string> Names = new HashSet<string>();
        //private PluralizationService pluralizationService = PluralizationService.CreateService(new CultureInfo("en-us"));
        private bool used = false;
        public bool UseNamespaces { get { return Namespace != null; } }

        #endregion

        #region 内部方法

        //public void GenerateClasses()
        //{
        //    if (CodeWriter == null) CodeWriter = new CSharpCodeWriter();
        //    if (ExplicitDeserialization && !(CodeWriter is CSharpCodeWriter)) throw new ArgumentException("Explicit deserialization is obsolete and is only supported by the C# provider.");

        //    if (used) throw new InvalidOperationException("This instance of JsonClassGenerator has already been used. Please create a new instance.");
        //    used = true;


        //    var writeToDisk = TargetFolder != null;
        //    if (writeToDisk && !Directory.Exists(TargetFolder)) Directory.CreateDirectory(TargetFolder);


        //    JObject[] examples;
        //    var example = Example.StartsWith("HTTP/") ? Example.Substring(Example.IndexOf("\r\n\r\n")) : Example;
        //    using (var sr = new StringReader(example))
        //    using (var reader = new JsonTextReader(sr))
        //    {
        //        var json = JToken.ReadFrom(reader);
        //        if (json is JArray)
        //        {
        //            examples = ((JArray)json).Cast<JObject>().ToArray();
        //        }
        //        else if (json is JObject)
        //        {
        //            examples = new[] { (JObject)json };
        //        }
        //        else
        //        {
        //            throw new Exception("Sample JSON must be either a JSON array, or a JSON object.");
        //        }
        //    }


        //    Types = new List<JsonType>();
        //    Names.Add(MainClass);
        //    var rootType = new JsonType(this, examples[0]);
        //    rootType.IsRoot = true;
        //    rootType.AssignName(MainClass);
        //    GenerateClass(examples, rootType);

        //    if (writeToDisk)
        //    {

        //        var parentFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //        if (writeToDisk && !NoHelperClass && ExplicitDeserialization) File.WriteAllBytes(Path.Combine(TargetFolder, "JsonClassHelper.cs"), Properties.Resources.JsonClassHelper);
        //        if (SingleFile)
        //        {
        //            WriteClassesToFile(Path.Combine(TargetFolder, MainClass + CodeWriter.FileExtension), Types);
        //        }
        //        else
        //        {

        //            foreach (var type in Types)
        //            {
        //                var folder = TargetFolder;
        //                if (!UseNestedClasses && !type.IsRoot && SecondaryNamespace != null)
        //                {
        //                    var s = SecondaryNamespace;
        //                    if (s.StartsWith(Namespace + ".")) s = s.Substring(Namespace.Length + 1);
        //                    folder = Path.Combine(folder, s);
        //                    Directory.CreateDirectory(folder);
        //                }
        //                WriteClassesToFile(Path.Combine(folder, (UseNestedClasses && !type.IsRoot ? MainClass + "." : string.Empty) + type.AssignedName + CodeWriter.FileExtension), new[] { type });
        //            }
        //        }
        //    }
        //    else if (OutputStream != null)
        //    {
        //        WriteClassesToFile(OutputStream, Types);
        //    }
        //}

        public string GenerateClassesAsString()
        {
            string str = string.Empty;
            if (CodeWriter == null) CodeWriter = new CSharpCodeWriter();
            if (ExplicitDeserialization && !(CodeWriter is CSharpCodeWriter)) throw new ArgumentException("Explicit deserialization is obsolete and is only supported by the C# provider.");

            if (used) throw new InvalidOperationException("This instance of JsonClassGenerator has already been used. Please create a new instance.");
            used = true;

            JObject[] examples;
            var example = Example.StartsWith("HTTP/") ? Example.Substring(Example.IndexOf("\r\n\r\n")) : Example;
            using (var sr = new StringReader(example))
            using (var reader = new JsonTextReader(sr))
            {
                var json = JToken.ReadFrom(reader);
                if (json is JArray)
                {
                    examples = ((JArray)json).Cast<JObject>().ToArray();
                }
                else if (json is JObject)
                {
                    examples = new[] { (JObject)json };
                }
                else
                {
                    throw new Exception("Sample JSON must be either a JSON array, or a JSON object.");
                }
            }

            Types = new List<JsonType>();
            Names.Add(MainClass);
            var rootType = new JsonType(this, examples[0]);
            rootType.IsRoot = true;
            rootType.AssignName(MainClass);
            GenerateClass(examples, rootType);

            StringBuilder buffer = new StringBuilder();
            WriteClassesToFile(buffer, Types);
            str = buffer.ToString();
            return str;
        }

        private string WriteClassesToFile(string path, IEnumerable<JsonType> types)
        {
            string str = string.Empty;
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteClassesToFile(sw, types);
            }
            return str;
        }

        private void WriteClassesToFile(TextWriter sw, IEnumerable<JsonType> types)
        {
            var inNamespace = false;
            var rootNamespace = false;

            CodeWriter.WriteFileStart(this, sw);
            foreach (var type in types)
            {
                if (UseNamespaces && inNamespace && rootNamespace != type.IsRoot && SecondaryNamespace != null) { CodeWriter.WriteNamespaceEnd(this, sw, rootNamespace); inNamespace = false; }
                if (UseNamespaces && !inNamespace) { CodeWriter.WriteNamespaceStart(this, sw, type.IsRoot); inNamespace = true; rootNamespace = type.IsRoot; }
                CodeWriter.WriteClass(this, sw, type);
            }
            if (UseNamespaces && inNamespace) CodeWriter.WriteNamespaceEnd(this, sw, rootNamespace);
            CodeWriter.WriteFileEnd(this, sw);
        }

        private void WriteClassesToFile(StringBuilder buffer, IEnumerable<JsonType> types)
        {
            var inNamespace = false;
            var rootNamespace = false;

            CodeWriter.WriteFileStart(this, buffer);
            foreach (var type in types)
            {
                if (UseNamespaces && inNamespace && rootNamespace != type.IsRoot && SecondaryNamespace != null) { CodeWriter.WriteNamespaceEnd(this, buffer, rootNamespace); inNamespace = false; }
                if (UseNamespaces && !inNamespace) { CodeWriter.WriteNamespaceStart(this, buffer, type.IsRoot); inNamespace = true; rootNamespace = type.IsRoot; }
                CodeWriter.WriteClass(this, buffer, type);
            }
            if (UseNamespaces && inNamespace) CodeWriter.WriteNamespaceEnd(this, buffer, rootNamespace);
            CodeWriter.WriteFileEnd(this, buffer);
        }

        private void GenerateClass(JObject[] examples, JsonType type)
        {
            var jsonFields = new Dictionary<string, JsonType>();
            var fieldExamples = new Dictionary<string, IList<object>>();

            var first = true;

            foreach (var obj in examples)
            {
                foreach (var prop in obj.Properties())
                {
                    JsonType fieldType;
                    var currentType = new JsonType(this, prop.Value);
                    var propName = prop.Name;
                    if (jsonFields.TryGetValue(propName, out fieldType))
                    {

                        var commonType = fieldType.GetCommonType(currentType);

                        jsonFields[propName] = commonType;
                    }
                    else
                    {
                        var commonType = currentType;
                        if (first) commonType = commonType.MaybeMakeNullable(this);
                        else commonType = commonType.GetCommonType(JsonType.GetNull(this));
                        jsonFields.Add(propName, commonType);
                        fieldExamples[propName] = new List<object>();
                    }
                    var fe = fieldExamples[propName];
                    var val = prop.Value;
                    if (val.Type == JTokenType.Null || val.Type == JTokenType.Undefined)
                    {
                        if (!fe.Contains(null))
                        {
                            fe.Insert(0, null);
                        }
                    }
                    else
                    {
                        var v = val.Type == JTokenType.Array || val.Type == JTokenType.Object ? val : val.Value<object>();
                        if (!fe.Any(x => v.Equals(x)))
                            fe.Add(v);
                    }
                }
                first = false;
            }

            if (UseNestedClasses)
            {
                foreach (var field in jsonFields)
                {
                    Names.Add(field.Key.ToLower());
                }
            }

            foreach (var field in jsonFields)
            {
                var fieldType = field.Value;
                if (fieldType.Type == JsonTypeEnum.Object)
                {
                    var subexamples = new List<JObject>(examples.Length);
                    foreach (var obj in examples)
                    {
                        JToken value;
                        if (obj.TryGetValue(field.Key, out value))
                        {
                            if (value.Type == JTokenType.Object)
                            {
                                subexamples.Add((JObject)value);
                            }
                        }
                    }

                    fieldType.AssignName(CreateUniqueClassName(field.Key));
                    GenerateClass(subexamples.ToArray(), fieldType);
                }

                if (fieldType.InternalType != null && fieldType.InternalType.Type == JsonTypeEnum.Object)
                {
                    var subexamples = new List<JObject>(examples.Length);
                    foreach (var obj in examples)
                    {
                        JToken value;
                        if (obj.TryGetValue(field.Key, out value))
                        {
                            if (value.Type == JTokenType.Array)
                            {
                                foreach (var item in (JArray)value)
                                {
                                    if (!(item is JObject)) throw new NotSupportedException("Arrays of non-objects are not supported yet.");
                                    subexamples.Add((JObject)item);
                                }

                            }
                            else if (value.Type == JTokenType.Object)
                            {
                                foreach (var item in (JObject)value)
                                {
                                    if (!(item.Value is JObject)) throw new NotSupportedException("Arrays of non-objects are not supported yet.");

                                    subexamples.Add((JObject)item.Value);
                                }
                            }
                        }
                    }

                    field.Value.InternalType.AssignName(CreateUniqueClassNameFromPlural(field.Key));
                    GenerateClass(subexamples.ToArray(), field.Value.InternalType);
                }
            }

            type.Fields = jsonFields.Select(x => new FieldInfo(this, x.Key, x.Value, UsePascalCase, fieldExamples[x.Key])).ToArray();

            Types.Add(type);

        }

        private string CreateUniqueClassName(string name)
        {
            name = ToTitleCase(name);

            var finalName = name;
            var i = 2;
            while (Names.Any(x => x.Equals(finalName, StringComparison.OrdinalIgnoreCase)))
            {
                finalName = name + i.ToString();
                i++;
            }

            Names.Add(finalName);
            return finalName;
        }

        private string CreateUniqueClassNameFromPlural(string plural)
        {
            //plural = ToTitleCase(plural);
            //return CreateUniqueClassName(pluralizationService.Singularize(plural));

            return "";
        }

        internal static string ToTitleCase(string str)
        {
            var sb = new StringBuilder(str.Length);
            var flag = true;

            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(flag ? char.ToUpper(c) : c);
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }

            return sb.ToString();
        }

        public bool HasSecondaryClasses
        {
            get { return Types.Count > 1; }
        }

        public static readonly string[] FileHeader = new string[] { 
            //"Generated by Xamasoft JSON Class Generator", 
            //"http://www.xamasoft.com/json-class-generator"
        };

        #endregion

        #region 公共方法

        /// <summary>
        /// 通过Json生成指定类型的对象的代码
        /// </summary>
        /// <param name="json">需要分析的Json</param>
        /// <returns>对象的字符串表示形式的代码</returns>
        public static string GenerateString(string json)
        {
            return GenerateString(json, CodeLanguage.CSharp);
        }

        /// <summary>
        /// 通过Json生成指定类型的对象的代码
        /// </summary>
        /// <param name="json">需要分析的Json</param>
        /// <param name="language">需要转换的对象的类型</param>
        /// <returns>对象的字符串表示形式的代码</returns>
        public static string GenerateString(string json, CodeLanguage language)
        {
            if (string.IsNullOrEmpty(json) == true)
                return string.Empty;
            //if (json.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) == true)
            //    json = NetUtils.GetString(json);
            //else if (json.StartsWith("file:///", StringComparison.CurrentCultureIgnoreCase) == true)
            //    json = File.ReadAllText(json);
            var gen = new JsonClassGenerator();
            gen.Example = json;//edtJson.Text;
            gen.InternalVisibility = false; //radInternal.Checked;

            if (language == CodeLanguage.CSharp)
                gen.CodeWriter = new CSharpCodeWriter();// (ICodeWriter)cmbLanguage.SelectedItem;
            else if (language == CodeLanguage.Java)
                gen.CodeWriter = new JavaCodeWriter();
            else if (language == CodeLanguage.TypeScript)
                gen.CodeWriter = new TypeScriptCodeWriter();
            else if (language == CodeLanguage.VisualBasic)
                gen.CodeWriter = new VisualBasicCodeWriter();

            gen.ExplicitDeserialization = false;//chkExplicitDeserialization.Checked && gen.CodeWriter is CSharpCodeWriter;
            gen.Namespace = "Example";//string.IsNullOrEmpty(edtNamespace.Text) ? null : edtNamespace.Text;
            gen.NoHelperClass = true;// chkNoHelper.Checked;
            gen.SecondaryNamespace = null;// radDifferentNamespace.Checked && !string.IsNullOrEmpty(edtSecondaryNamespace.Text) ? edtSecondaryNamespace.Text : null;
            gen.TargetFolder = null;//edtTargetFolder.Text;
            gen.UseProperties = true;// radProperties.Checked;
            gen.MainClass = "RootClass";// edtMainClass.Text;
            gen.UsePascalCase = true;// chkPascalCase.Checked;
            gen.UseNestedClasses = false;// radNestedClasses.Checked;
            gen.ApplyObfuscationAttributes = false;// chkApplyObfuscationAttributes.Checked;
            gen.SingleFile = true;// chkSingleFile.Checked;
            gen.ExamplesInDocumentation = false;// chkDocumentationExamples.Checked;

            return gen.GenerateClassesAsString();
        }

        /// <summary>
        /// 获得初始化数据的代码
        /// </summary>
        /// <param name="json">需要分析的Json</param>
        /// <returns>初始化数据的代码</returns>
        public static string GenerateInitializeString(string json)
        {
            return GenerateInitializeString(json, CodeLanguage.CSharp);
        }

        /// <summary>
        /// 获得初始化数据的代码
        /// </summary>
        /// <param name="json">需要分析的Json</param>
        /// <param name="language">需要转换的对象的类型</param>
        /// <returns>初始化数据的代码</returns>
        public static string GenerateInitializeString(string json, CodeLanguage language)
        {
            return null;
        }

        #endregion

    }
}
