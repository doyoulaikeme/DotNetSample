using System;
using System.Windows.Forms;

namespace Editor
{
    public interface IEditor
    {
        /// <summary>
        /// 按钮名称
        /// </summary>
        string PluginName
        {
            get;
        }

        /// <summary>
        /// 执行按钮事件
        /// </summary>
        /// <param name="txtbox"></param>
        void Execute(TextBox txtbox);
    }
}
