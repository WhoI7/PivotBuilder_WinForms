namespace TestObzore
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lbSave = new System.Windows.Forms.Label();
            this.lbPath = new System.Windows.Forms.Label();
            this.txPath = new System.Windows.Forms.TextBox();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.btNew = new System.Windows.Forms.Button();
            this.btOpens = new System.Windows.Forms.Button();
            this.btDelet = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panelSave = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lbSave
            // 
            this.lbSave.AutoSize = true;
            this.lbSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbSave.Location = new System.Drawing.Point(27, 151);
            this.lbSave.Name = "lbSave";
            this.lbSave.Size = new System.Drawing.Size(92, 20);
            this.lbSave.TabIndex = 9;
            this.lbSave.Text = "Збережені:";
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPath.Location = new System.Drawing.Point(27, 47);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(134, 20);
            this.lbPath.TabIndex = 7;
            this.lbPath.Text = "Шлях до файлу: ";
            // 
            // txPath
            // 
            this.txPath.Enabled = false;
            this.txPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.txPath.Location = new System.Drawing.Point(162, 47);
            this.txPath.Name = "txPath";
            this.txPath.Size = new System.Drawing.Size(434, 26);
            this.txPath.TabIndex = 6;
            this.txPath.Text = "Виберіть файл..";
            // 
            // btOpenFile
            // 
            this.btOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F);
            this.btOpenFile.Location = new System.Drawing.Point(602, 47);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(110, 30);
            this.btOpenFile.TabIndex = 5;
            this.btOpenFile.Text = "Обрати...";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // btNew
            // 
            this.btNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F);
            this.btNew.Location = new System.Drawing.Point(31, 86);
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(681, 42);
            this.btNew.TabIndex = 10;
            this.btNew.Text = "Відкрити";
            this.btNew.UseVisualStyleBackColor = true;
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // btOpens
            // 
            this.btOpens.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F);
            this.btOpens.Location = new System.Drawing.Point(450, 186);
            this.btOpens.Name = "btOpens";
            this.btOpens.Size = new System.Drawing.Size(262, 58);
            this.btOpens.TabIndex = 11;
            this.btOpens.Text = "Застосувати обране";
            this.btOpens.UseVisualStyleBackColor = true;
            this.btOpens.Click += new System.EventHandler(this.btOpens_Click);
            // 
            // btDelet
            // 
            this.btDelet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F);
            this.btDelet.Location = new System.Drawing.Point(450, 253);
            this.btDelet.Name = "btDelet";
            this.btDelet.Size = new System.Drawing.Size(262, 58);
            this.btDelet.TabIndex = 12;
            this.btDelet.Text = "Видалити обране";
            this.btDelet.UseVisualStyleBackColor = true;
            this.btDelet.Click += new System.EventHandler(this.btDelet_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(31, 77);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(681, 8);
            this.progressBar1.TabIndex = 13;
            this.progressBar1.Visible = false;
            // 
            // panelSave
            // 
            this.panelSave.AutoScroll = true;
            this.panelSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelSave.Location = new System.Drawing.Point(31, 188);
            this.panelSave.Name = "panelSave";
            this.panelSave.Size = new System.Drawing.Size(413, 211);
            this.panelSave.TabIndex = 26;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 411);
            this.Controls.Add(this.panelSave);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btDelet);
            this.Controls.Add(this.btOpens);
            this.Controls.Add(this.btNew);
            this.Controls.Add(this.lbSave);
            this.Controls.Add(this.lbPath);
            this.Controls.Add(this.txPath);
            this.Controls.Add(this.btOpenFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Головна";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbSave;
        private System.Windows.Forms.Label lbPath;
        private System.Windows.Forms.TextBox txPath;
        private System.Windows.Forms.Button btOpenFile;
        private System.Windows.Forms.Button btNew;
        private System.Windows.Forms.Button btOpens;
        private System.Windows.Forms.Button btDelet;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panelSave;
    }
}

