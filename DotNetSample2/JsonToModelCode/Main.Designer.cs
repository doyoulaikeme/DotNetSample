namespace JsonToModelCode
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBox_Json = new System.Windows.Forms.TextBox();
            this.btn_ConvertToModel = new System.Windows.Forms.Button();
            this.txtBox_Model = new System.Windows.Forms.TextBox();
            this.cbo_Select = new System.Windows.Forms.ComboBox();
            this.lb_txt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBox_Json
            // 
            this.txtBox_Json.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBox_Json.Location = new System.Drawing.Point(0, 0);
            this.txtBox_Json.Multiline = true;
            this.txtBox_Json.Name = "txtBox_Json";
            this.txtBox_Json.Size = new System.Drawing.Size(1022, 228);
            this.txtBox_Json.TabIndex = 0;
            // 
            // btn_ConvertToModel
            // 
            this.btn_ConvertToModel.Location = new System.Drawing.Point(395, 234);
            this.btn_ConvertToModel.Name = "btn_ConvertToModel";
            this.btn_ConvertToModel.Size = new System.Drawing.Size(102, 34);
            this.btn_ConvertToModel.TabIndex = 1;
            this.btn_ConvertToModel.Text = "转换成实体类";
            this.btn_ConvertToModel.UseVisualStyleBackColor = true;
            this.btn_ConvertToModel.Click += new System.EventHandler(this.btn_ConvertToModel_Click);
            // 
            // txtBox_Model
            // 
            this.txtBox_Model.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtBox_Model.Location = new System.Drawing.Point(0, 292);
            this.txtBox_Model.Multiline = true;
            this.txtBox_Model.Name = "txtBox_Model";
            this.txtBox_Model.Size = new System.Drawing.Size(1022, 493);
            this.txtBox_Model.TabIndex = 2;
            // 
            // cbo_Select
            // 
            this.cbo_Select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Select.FormattingEnabled = true;
            this.cbo_Select.Location = new System.Drawing.Point(242, 242);
            this.cbo_Select.Name = "cbo_Select";
            this.cbo_Select.Size = new System.Drawing.Size(121, 20);
            this.cbo_Select.TabIndex = 3;
            // 
            // lb_txt
            // 
            this.lb_txt.AutoSize = true;
            this.lb_txt.Location = new System.Drawing.Point(137, 246);
            this.lb_txt.Name = "lb_txt";
            this.lb_txt.Size = new System.Drawing.Size(101, 12);
            this.lb_txt.TabIndex = 4;
            this.lb_txt.Text = "请选择编程语言：";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 785);
            this.Controls.Add(this.lb_txt);
            this.Controls.Add(this.cbo_Select);
            this.Controls.Add(this.txtBox_Model);
            this.Controls.Add(this.btn_ConvertToModel);
            this.Controls.Add(this.txtBox_Json);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBox_Json;
        private System.Windows.Forms.Button btn_ConvertToModel;
        private System.Windows.Forms.TextBox txtBox_Model;
        private System.Windows.Forms.ComboBox cbo_Select;
        private System.Windows.Forms.Label lb_txt;
    }
}

