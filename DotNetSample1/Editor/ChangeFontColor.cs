
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    public class ChangeFontColor : IEditor
    {
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string PluginName
        {
            get
            {
                return "改变颜色";
            }
        }

        /// <summary>
        /// 执行按钮事件
        /// </summary>
        /// <param name="txtbox"></param>
        public void Execute(TextBox txtbox)
        {
            if (!string.IsNullOrEmpty(txtbox.Text))
            {
                txtbox.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                MessageBox.Show("请先输入文字！");
            }
        }
    }
}