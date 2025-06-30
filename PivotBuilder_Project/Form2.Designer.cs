namespace ExelProb
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btOpenFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txPath = new System.Windows.Forms.TextBox();
            this.lbPath = new System.Windows.Forms.Label();
            this.flpSave = new System.Windows.Forms.FlowLayoutPanel();
            this.lbSave = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btOpenFile
            // 
            this.btOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOpenFile.Location = new System.Drawing.Point(620, 30);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(90, 30);
            this.btOpenFile.TabIndex = 0;
            this.btOpenFile.Text = "Вибрати...";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txPath
            // 
            this.txPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txPath.Location = new System.Drawing.Point(160, 30);
            this.txPath.Name = "txPath";
            this.txPath.Size = new System.Drawing.Size(450, 29);
            this.txPath.TabIndex = 1;
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPath.Location = new System.Drawing.Point(25, 30);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(134, 20);
            this.lbPath.TabIndex = 2;
            this.lbPath.Text = "Шлях до файлу: ";
            // 
            // flpSave
            // 
            this.flpSave.AutoScroll = true;
            this.flpSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpSave.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSave.Location = new System.Drawing.Point(25, 171);
            this.flpSave.Name = "flpSave";
            this.flpSave.Size = new System.Drawing.Size(685, 225);
            this.flpSave.TabIndex = 3;
            this.flpSave.WrapContents = false;
            // 
            // lbSave
            // 
            this.lbSave.AutoSize = true;
            this.lbSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbSave.Location = new System.Drawing.Point(25, 148);
            this.lbSave.Name = "lbSave";
            this.lbSave.Size = new System.Drawing.Size(92, 20);
            this.lbSave.TabIndex = 4;
            this.lbSave.Text = "Збережені:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 461);
            this.Controls.Add(this.lbSave);
            this.Controls.Add(this.flpSave);
            this.Controls.Add(this.lbPath);
            this.Controls.Add(this.txPath);
            this.Controls.Add(this.btOpenFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txPath;
        private System.Windows.Forms.Label lbPath;
        private System.Windows.Forms.FlowLayoutPanel flpSave;
        private System.Windows.Forms.Label lbSave;
    }
}

