namespace WindowsFormsApp1
{
    partial class AllTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllTest));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ShowCheck = new System.Windows.Forms.CheckBox();
            this.imageBoxEx1 = new LMI.Gocator.Tools.ImageBoxEx();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(509, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(564, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "none";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(564, 179);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Draw";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ShowCheck
            // 
            this.ShowCheck.AutoSize = true;
            this.ShowCheck.Location = new System.Drawing.Point(561, 300);
            this.ShowCheck.Name = "ShowCheck";
            this.ShowCheck.Size = new System.Drawing.Size(78, 16);
            this.ShowCheck.TabIndex = 4;
            this.ShowCheck.Text = "Show Flag";
            this.ShowCheck.UseVisualStyleBackColor = true;
            this.ShowCheck.CheckedChanged += new System.EventHandler(this.ShowCheck_CheckedChanged);
            // 
            // imageBoxEx1
            // 
            this.imageBoxEx1.Image = ((System.Drawing.Image)(resources.GetObject("imageBoxEx1.Image")));
            this.imageBoxEx1.InvertMouse = true;
            this.imageBoxEx1.Location = new System.Drawing.Point(12, 12);
            this.imageBoxEx1.Name = "imageBoxEx1";
            this.imageBoxEx1.Size = new System.Drawing.Size(424, 378);
            this.imageBoxEx1.TabIndex = 0;
            this.imageBoxEx1.Paint += new System.Windows.Forms.PaintEventHandler(this.imageBoxEx1_Paint);
            // 
            // AllTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 501);
            this.Controls.Add(this.ShowCheck);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.imageBoxEx1);
            this.Name = "AllTest";
            this.Text = "AllTest";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LMI.Gocator.Tools.ImageBoxEx imageBoxEx1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox ShowCheck;
    }
}

