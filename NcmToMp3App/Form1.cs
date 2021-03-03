using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NcmToMp3App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string ncmFile;
        private string savePath;

        private void button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "(*.ncm)|*.ncm";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ncmFile = dialog.FileName;
                fileTb.Text = ncmFile;
            }
        }

        private void savePositionBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                savePath = dialog.SelectedPath;
                savePathTb.Text = savePath;
            }
        }

        private void transcodingBtn_Click(object sender, EventArgs e)
        {
            transcodingBtn.Enabled = false;
            transcodingBtn.Text = "正在转码...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    NcmToMp3.ProcessFile(ncmFile, savePath);
                }
                catch
                {
                    this.Invoke(new Action(() =>
                    {
                        transcodingBtn.Enabled = true;
                        transcodingBtn.Text = "转码";
                        MessageBox.Show("转码错误", "提示", MessageBoxButtons.OK);
                    }));
                    return;
                }
                this.Invoke(new Action(() =>
                {
                    transcodingBtn.Enabled = true;
                    transcodingBtn.Text = "转码";
                    MessageBox.Show("转码完成", "提示", MessageBoxButtons.OK);
                }));
            });
        }
    }
}
