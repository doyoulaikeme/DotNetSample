using Editor;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DotNetSample1
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            // 加载插件
            LoadPlugins();

        }

        private void LoadPlugins()
        {
            // 1.加载plugins目录下的所有的dll文件（需要将类库生成的DLL放到plugins文件夹下）
            string plugins = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            //   1.1 搜索plugins目录下的所有的dll文件 
            string[] dlls = Directory.GetFiles(plugins, "*.dll");
            // 2.循环将每个dll文件都加载起来
            foreach (string dllPath in dlls)
            {
                //  2.1 动态加载当前循环的dll文件
                Assembly assembly = Assembly.LoadFile(dllPath);
                //  2.2 获取当前dll中的所有的public类型
                Type[] types = assembly.GetExportedTypes();
                //  2.3 获取IEditor接口的Type
                Type typeIEditor = typeof(IEditor);

                for (int i = 0; i < types.Length; i++)
                {
                    // 2.4 验证当前的类型即实现了IEditor接口并且该类型还可以被实例化
                    if (typeIEditor.IsAssignableFrom(types[i]) && !types[i].IsAbstract)
                    {
                        IEditor editor = (IEditor)Activator.CreateInstance(types[i]);
                        // 2.5 向菜单栏中动态添加一个菜单项
                        ToolStripItem toolItem = toolStripProgressBar2.DropDownItems.Add(editor.PluginName);
                        // 2.6 为刚刚增加的菜单项注册一个单击事件
                        toolItem.Click += new EventHandler(toolItem_Click);
                        toolItem.Tag = editor;
                    }
                }
            }
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item != null)
            {
                if (item.Tag != null)
                {
                    IEditor editor = item.Tag as IEditor;
                    if (editor != null)
                    {
                        // 运行该插件
                        editor.Execute(this.textBox1);
                    }
                }
            }
        }


    }
}
