using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonToObject.CodeWriters
{
    public class JavaCodeWriter : ICodeWriter
    {
        public string FileExtension
        {
            get { return ".java"; }
        }

        public string DisplayName
        {
            get { return "Java"; }
        }

        private const string NoRenameAttribute = "[Obfuscation(Feature = \"renaming\", Exclude = true)]";
        private const string NoPruneAttribute = "[Obfuscation(Feature = \"trigger\", Exclude = false)]";

        private bool ShouldApplyNoRenamingAttribute(IJsonClassGeneratorConfig config)
        {
            return config.ApplyObfuscationAttributes && !config.ExplicitDeserialization && !config.UsePascalCase;
        }
        private bool ShouldApplyNoPruneAttribute(IJsonClassGeneratorConfig config)
        {
            return config.ApplyObfuscationAttributes && !config.ExplicitDeserialization && config.UseProperties;
        }





        public string GetTypeName(JsonType type, IJsonClassGeneratorConfig config)
        {
            var arraysAsLists = !config.ExplicitDeserialization;

            switch (type.Type)
            {
                case JsonTypeEnum.Anything: return "Object";
                case JsonTypeEnum.Array: return arraysAsLists ? "ArrayList<" + GetTypeName(type.InternalType, config) + ">" : GetTypeName(type.InternalType, config) + "[]";
                case JsonTypeEnum.Dictionary: return "Map<String, " + GetTypeName(type.InternalType, config) + ">";
                case JsonTypeEnum.Boolean: return "boolean";
                case JsonTypeEnum.Float: return "double";
                case JsonTypeEnum.Integer: return "int";
                case JsonTypeEnum.Long: return "long";
                case JsonTypeEnum.Date: return "Date";
                case JsonTypeEnum.NonConstrained: return "Object";
                //case JsonTypeEnum.NullableBoolean: return "bool?";
                //case JsonTypeEnum.NullableFloat: return "double?";
                //case JsonTypeEnum.NullableInteger: return "int?";
                //case JsonTypeEnum.NullableLong: return "long?";
                //case JsonTypeEnum.NullableDate: return "DateTime?";
                case JsonTypeEnum.NullableSomething: return "Object";
                case JsonTypeEnum.Object: return type.AssignedName;
                case JsonTypeEnum.String: return "String";
                default: throw new System.NotSupportedException("Unsupported json type");
            }
        }

        public void WriteClass(IJsonClassGeneratorConfig config, TextWriter sw, JsonType type)
        {
            throw new NotImplementedException();
        }

        public void WriteFileStart(IJsonClassGeneratorConfig config, TextWriter sw)
        {
            foreach (var line in JsonClassGenerator.FileHeader)
            {
                sw.WriteLine("// " + line);
            }
        }

        public void WriteFileEnd(IJsonClassGeneratorConfig config, TextWriter sw)
        {
            throw new NotImplementedException();
        }

        public void WriteNamespaceStart(IJsonClassGeneratorConfig config, TextWriter sw, bool root)
        {
            throw new NotImplementedException();
        }

        public void WriteNamespaceEnd(IJsonClassGeneratorConfig config, TextWriter sw, bool root)
        {
            throw new NotImplementedException();
        }







        public void WriteFileStart(IJsonClassGeneratorConfig config, StringBuilder sw)
        {
            if (config.UseNamespaces)
            {
                foreach (var line in JsonClassGenerator.FileHeader)
                {
                    sw.AppendLine("// " + line);
                }
                sw.AppendLine();
                //sw.AppendLine("import System;");
                //sw.AppendLine("import System.Collections.Generic;");
                if (ShouldApplyNoPruneAttribute(config) || ShouldApplyNoRenamingAttribute(config))
                    sw.AppendLine("import System.Reflection;");
                if (!config.ExplicitDeserialization && config.UsePascalCase)
                    sw.AppendLine("import Newtonsoft.Json;");
                //sw.AppendLine("import Newtonsoft.Json.Linq;");
                if (config.ExplicitDeserialization)
                    sw.AppendLine("import JsonCSharpClassGenerator;");
                if (config.SecondaryNamespace != null && config.HasSecondaryClasses && !config.UseNestedClasses)
                {
                    sw.AppendFormat("import {0};", config.SecondaryNamespace);
                    //sw.AppendLine(string.Format("using {0};", config.SecondaryNamespace));
                }
            }

            if (config.UseNestedClasses)
            {
                sw.AppendFormat("    {0} class {1}", config.InternalVisibility ? "private" : "public", config.MainClass);
                sw.AppendLine();
                //sw.AppendLine(string.Format("    {0} class {1}", config.InternalVisibility ? "internal" : "public", config.MainClass));
                sw.AppendLine("    {");
            }
        }

        public void WriteFileEnd(IJsonClassGeneratorConfig config, StringBuilder sw)
        {
            if (config.UseNestedClasses)
            {
                sw.AppendLine("    }");
            }
        }


        public void WriteNamespaceStart(IJsonClassGeneratorConfig config, StringBuilder sw, bool root)
        {
            sw.AppendLine();
            sw.AppendFormat("package {0}", root && !config.UseNestedClasses ? config.Namespace : (config.SecondaryNamespace ?? config.Namespace));
            sw.AppendLine();
            sw.AppendLine("{");
            sw.AppendLine();
        }

        public void WriteNamespaceEnd(IJsonClassGeneratorConfig config, StringBuilder sw, bool root)
        {
            sw.AppendLine("}");
        }

        public void WriteClass(IJsonClassGeneratorConfig config, StringBuilder sw, JsonType type)
        {
            var visibility = config.InternalVisibility ? "private" : "public";



            if (config.UseNestedClasses)
            {
                if (!type.IsRoot)
                {
                    if (ShouldApplyNoRenamingAttribute(config)) sw.AppendLine("        " + NoRenameAttribute);
                    if (ShouldApplyNoPruneAttribute(config)) sw.AppendLine("        " + NoPruneAttribute);
                    sw.AppendFormat("        {0} class {1}", visibility, type.AssignedName);
                    sw.AppendLine();
                    sw.AppendLine("        {");
                }
            }
            else
            {
                if (ShouldApplyNoRenamingAttribute(config)) sw.AppendLine("    " + NoRenameAttribute);
                if (ShouldApplyNoPruneAttribute(config)) sw.AppendLine("    " + NoPruneAttribute);
                sw.AppendFormat("    {0} class {1}", visibility, type.AssignedName);
                sw.AppendLine();
                sw.AppendLine("    {");
            }

            var prefix = config.UseNestedClasses && !type.IsRoot ? "            " : "        ";


            var shouldSuppressWarning = config.InternalVisibility && !config.UseProperties && !config.ExplicitDeserialization;
            if (shouldSuppressWarning)
            {
                sw.AppendLine("#pragma warning disable 0649");
                if (!config.UsePascalCase) sw.AppendLine();
            }

            if (type.IsRoot && config.ExplicitDeserialization) WriteStringConstructorExplicitDeserialization(config, sw, type, prefix);

            if (config.ExplicitDeserialization)
            {
                if (config.UseProperties) WriteClassWithPropertiesExplicitDeserialization(sw, type, prefix);
                else WriteClassWithFieldsExplicitDeserialization(sw, type, prefix);
            }
            else
            {
                WriteClassMembers(config, sw, type, prefix);
            }

            if (shouldSuppressWarning)
            {
                sw.AppendLine();
                sw.AppendLine("#pragma warning restore 0649");
                sw.AppendLine();
            }


            if (config.UseNestedClasses && !type.IsRoot)
                sw.AppendLine("        }");

            if (!config.UseNestedClasses)
                sw.AppendLine("    }");

            sw.AppendLine();


        }


        #region Code for (obsolete) explicit deserialization

        private void WriteClassMembers(IJsonClassGeneratorConfig config, StringBuilder sw, JsonType type, string prefix)
        {
            foreach (var field in type.Fields)
            {
                if (config.UsePascalCase || config.ExamplesInDocumentation) sw.AppendLine();

                if (config.ExamplesInDocumentation)
                {
                    sw.AppendLine(prefix + "/// <summary>");
                    sw.AppendLine(prefix + "/// Examples: " + field.GetExamplesText());
                    sw.AppendLine(prefix + "/// </summary>");
                }

                if (config.UsePascalCase)
                {

                    sw.AppendFormat(prefix + "[JsonProperty(\"{0}\")]", field.JsonMemberName);
                    sw.AppendLine();
                }

                if (config.UseProperties)
                {
                    //sw.AppendFormat(prefix + "public {0} {1} {{ get; set; }}", field.Type.GetTypeName(), field.MemberName);

                    //public String geta()
                    //{
                    //    return a;
                    //}

                    sw.AppendFormat(prefix + "private {0} {1};", field.Type.GetTypeName(), field.MemberName);
                    sw.AppendLine();

                    sw.AppendFormat(prefix + "public {0} get{1}()", field.Type.GetTypeName(), field.MemberName);
                    sw.AppendLine();
                    sw.AppendLine("        {");
                    sw.AppendFormat(prefix + "  return this.{0};", field.MemberName);
                    sw.AppendLine();
                    sw.AppendLine("        }");
                    sw.AppendLine();

                    sw.AppendFormat(prefix + "public void set{1}({0} {1})", field.Type.GetTypeName(), field.MemberName);
                    sw.AppendLine();
                    sw.AppendLine("        {");
                    sw.AppendFormat(prefix + "  this.{0} ={0};", field.MemberName);
                    sw.AppendLine();
                    sw.AppendLine("        }");
                    sw.AppendLine();
                }
                else
                {
                    sw.AppendFormat("return {0};", field.MemberName);
                    sw.AppendLine();
                }
            }

        }

        private void WriteClassWithPropertiesExplicitDeserialization(StringBuilder sw, JsonType type, string prefix)
        {

            sw.AppendLine(prefix + "private JObject __jobject;");
            sw.AppendFormat(prefix + "public {0}(JObject obj)", type.AssignedName);
            sw.AppendLine();
            sw.AppendLine(prefix + "{");
            sw.AppendLine(prefix + "    this.__jobject = obj;");
            sw.AppendLine(prefix + "}");
            sw.AppendLine();

            foreach (var field in type.Fields)
            {

                string variable = null;
                if (field.Type.MustCache)
                {
                    variable = "_" + char.ToLower(field.MemberName[0]) + field.MemberName.Substring(1);
                    sw.AppendLine(prefix + "[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]");
                    sw.AppendFormat(prefix + "private {0} {1};", field.Type.GetTypeName(), variable);
                    sw.AppendLine();
                }


                sw.AppendFormat(prefix + "public {0} {1}", field.Type.GetTypeName(), field.MemberName);
                sw.AppendLine();
                sw.AppendLine(prefix + "{");
                sw.AppendLine(prefix + "    get");
                sw.AppendLine(prefix + "    {");
                if (field.Type.MustCache)
                {
                    sw.AppendFormat(prefix + "        if ({0} == null)", variable);
                    sw.AppendLine();
                    sw.AppendFormat(prefix + "            {0} = {1};", variable, field.GetGenerationCode("__jobject"));
                    sw.AppendLine();
                    sw.AppendFormat(prefix + "        return {0};", variable);
                    sw.AppendLine();
                }
                else
                {
                    sw.AppendFormat(prefix + "        return {0};", field.GetGenerationCode("__jobject"));
                    sw.AppendLine();
                }
                sw.AppendLine(prefix + "    }");
                sw.AppendLine(prefix + "}");
                sw.AppendLine();

            }

        }


        private void WriteStringConstructorExplicitDeserialization(IJsonClassGeneratorConfig config, StringBuilder sw, JsonType type, string prefix)
        {
            sw.AppendLine();
            sw.AppendFormat(prefix + "public {1}(string json)", config.InternalVisibility ? "internal" : "public", type.AssignedName);
            sw.AppendLine();
            sw.AppendLine(prefix + "    : this(JObject.Parse(json))");
            sw.AppendLine(prefix + "{");
            sw.AppendLine(prefix + "}");
            sw.AppendLine();
        }

        private void WriteClassWithFieldsExplicitDeserialization(StringBuilder sw, JsonType type, string prefix)
        {


            sw.AppendFormat(prefix + "public {0}(JObject obj)", type.AssignedName);
            sw.AppendLine();
            sw.AppendLine(prefix + "{");

            foreach (var field in type.Fields)
            {
                sw.AppendFormat(prefix + "    this.{0} = {1};", field.MemberName, field.GetGenerationCode("obj"));
                sw.AppendLine();

            }

            sw.AppendLine(prefix + "}");
            sw.AppendLine();

            foreach (var field in type.Fields)
            {
                sw.AppendFormat(prefix + "public readonly {0} {1};", field.Type.GetTypeName(), field.MemberName);
                sw.AppendLine();
            }
        }
        #endregion
    }
}
