namespace SpamDetection
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.datasetSpamDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputSpamDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataSetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(801, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataSetToolStripMenuItem
            // 
            this.dataSetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataSetToolStripMenuItem1,
            this.datasetSpamDetectionToolStripMenuItem,
            this.inputSpamDetectionToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.dataSetToolStripMenuItem.Name = "dataSetToolStripMenuItem";
            this.dataSetToolStripMenuItem.Size = new System.Drawing.Size(153, 20);
            this.dataSetToolStripMenuItem.Text = "YouTube Spam Detection";
            // 
            // dataSetToolStripMenuItem1
            // 
            this.dataSetToolStripMenuItem1.Name = "dataSetToolStripMenuItem1";
            this.dataSetToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.dataSetToolStripMenuItem1.Text = "Home";
            this.dataSetToolStripMenuItem1.Click += new System.EventHandler(this.dataSetToolStripMenuItem1_Click);
            // 
            // datasetSpamDetectionToolStripMenuItem
            // 
            this.datasetSpamDetectionToolStripMenuItem.Name = "datasetSpamDetectionToolStripMenuItem";
            this.datasetSpamDetectionToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.datasetSpamDetectionToolStripMenuItem.Text = "Dataset Spam Detection";
            this.datasetSpamDetectionToolStripMenuItem.Click += new System.EventHandler(this.datasetSpamDetectionToolStripMenuItem_Click);
            // 
            // inputSpamDetectionToolStripMenuItem
            // 
            this.inputSpamDetectionToolStripMenuItem.Name = "inputSpamDetectionToolStripMenuItem";
            this.inputSpamDetectionToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.inputSpamDetectionToolStripMenuItem.Text = "Input Spam Detection";
            this.inputSpamDetectionToolStripMenuItem.Click += new System.EventHandler(this.inputSpamDetectionToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.exitToolStripMenuItem.Text = "Show Dataset";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.exitToolStripMenuItem1.Text = "&Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(83, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(624, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "YouTube Comment Spam Detection Using Naive Bayesian Theory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(402, 329);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 60);
            this.label2.TabIndex = 13;
            this.label2.Text = "Presented By:\r\n            Ma Htoi Bu Aung\r\n            6MSE-10\r\n";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(22, 101);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(767, 363);
            this.dataGridView1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 514);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YouTube Spam Detection";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dataSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataSetToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem datasetSpamDetectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputSpamDetectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}