
namespace NcmToMp3App
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.selectNcmBtn = new System.Windows.Forms.Button();
            this.savePositionBtn = new System.Windows.Forms.Button();
            this.fileTb = new System.Windows.Forms.TextBox();
            this.savePathTb = new System.Windows.Forms.TextBox();
            this.transcodingBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectNcmBtn
            // 
            this.selectNcmBtn.Location = new System.Drawing.Point(111, 44);
            this.selectNcmBtn.Name = "selectNcmBtn";
            this.selectNcmBtn.Size = new System.Drawing.Size(92, 33);
            this.selectNcmBtn.TabIndex = 0;
            this.selectNcmBtn.Text = "选择ncm文件";
            this.selectNcmBtn.UseVisualStyleBackColor = true;
            this.selectNcmBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // savePositionBtn
            // 
            this.savePositionBtn.Location = new System.Drawing.Point(111, 113);
            this.savePositionBtn.Name = "savePositionBtn";
            this.savePositionBtn.Size = new System.Drawing.Size(92, 28);
            this.savePositionBtn.TabIndex = 1;
            this.savePositionBtn.Text = "选择保存位置";
            this.savePositionBtn.UseVisualStyleBackColor = true;
            this.savePositionBtn.Click += new System.EventHandler(this.savePositionBtn_Click);
            // 
            // fileTb
            // 
            this.fileTb.Location = new System.Drawing.Point(235, 49);
            this.fileTb.Name = "fileTb";
            this.fileTb.Size = new System.Drawing.Size(248, 23);
            this.fileTb.TabIndex = 2;
            // 
            // savePathTb
            // 
            this.savePathTb.Location = new System.Drawing.Point(235, 118);
            this.savePathTb.Name = "savePathTb";
            this.savePathTb.Size = new System.Drawing.Size(248, 23);
            this.savePathTb.TabIndex = 3;
            // 
            // transcodingBtn
            // 
            this.transcodingBtn.Location = new System.Drawing.Point(261, 221);
            this.transcodingBtn.Name = "transcodingBtn";
            this.transcodingBtn.Size = new System.Drawing.Size(134, 24);
            this.transcodingBtn.TabIndex = 4;
            this.transcodingBtn.Text = "转码";
            this.transcodingBtn.UseVisualStyleBackColor = true;
            this.transcodingBtn.Click += new System.EventHandler(this.transcodingBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 354);
            this.Controls.Add(this.transcodingBtn);
            this.Controls.Add(this.savePathTb);
            this.Controls.Add(this.fileTb);
            this.Controls.Add(this.savePositionBtn);
            this.Controls.Add(this.selectNcmBtn);
            this.Name = "Form1";
            this.Text = "ncm转mp3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectNcmBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox fileTb;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button savePositionBtn;
        private System.Windows.Forms.TextBox savePathTb;
        private System.Windows.Forms.Button transcodingBtn;
    }
}

