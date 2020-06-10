using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonToModelCode
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            initializeComBox();

        }

        private void initializeComBox()
        {
            //绑定编程语言枚举数据源
            cbo_Select.DataSource = System.Enum.GetNames(typeof(JsonToObject.CodeLanguage));
        }

        private void btn_ConvertToModel_Click(object sender, EventArgs e)
        {
            //获取要转化的编程语言类型
            JsonToObject.CodeLanguage selectENum = (JsonToObject.CodeLanguage)Enum.Parse(typeof(JsonToObject.CodeLanguage), cbo_Select.SelectedItem.ToString(), false);

            try
            {
                var model = JsonToObject.JsonClassGenerator.GenerateString(txtBox_Json.Text.Trim(), selectENum);
                txtBox_Model.Text = model;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
