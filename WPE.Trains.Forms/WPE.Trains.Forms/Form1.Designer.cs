namespace WPE.Trains.Forms
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
            this.components = new System.ComponentModel.Container();
            this.LoadGalleriesButton = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxForeground = new System.Windows.Forms.PictureBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.linkLabelOpenSite = new System.Windows.Forms.LinkLabel();
            this.linkLabelOpenExplorer = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxForeground)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadGalleriesButton
            // 
            this.LoadGalleriesButton.BackColor = System.Drawing.Color.White;
            this.LoadGalleriesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadGalleriesButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LoadGalleriesButton.Location = new System.Drawing.Point(12, 12);
            this.LoadGalleriesButton.Name = "LoadGalleriesButton";
            this.LoadGalleriesButton.Size = new System.Drawing.Size(100, 23);
            this.LoadGalleriesButton.TabIndex = 5;
            this.LoadGalleriesButton.Text = "Load";
            this.LoadGalleriesButton.UseVisualStyleBackColor = false;
            this.LoadGalleriesButton.Click += new System.EventHandler(this.LoadGalleriesButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = global::WPE.Trains.Forms.Properties.Resources.landscape_background;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(12, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(761, 204);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBoxForeground
            // 
            this.pictureBoxForeground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxForeground.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxForeground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxForeground.Location = new System.Drawing.Point(12, 40);
            this.pictureBoxForeground.Name = "pictureBoxForeground";
            this.pictureBoxForeground.Size = new System.Drawing.Size(761, 204);
            this.pictureBoxForeground.TabIndex = 7;
            this.pictureBoxForeground.TabStop = false;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Enabled = false;
            this.textBoxLog.Location = new System.Drawing.Point(13, 251);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(760, 337);
            this.textBoxLog.TabIndex = 9;
            // 
            // linkLabelOpenSite
            // 
            this.linkLabelOpenSite.AutoSize = true;
            this.linkLabelOpenSite.Location = new System.Drawing.Point(667, 17);
            this.linkLabelOpenSite.Name = "linkLabelOpenSite";
            this.linkLabelOpenSite.Size = new System.Drawing.Size(106, 13);
            this.linkLabelOpenSite.TabIndex = 10;
            this.linkLabelOpenSite.TabStop = true;
            this.linkLabelOpenSite.Text = "Open gallery browser";
            this.linkLabelOpenSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOpenSite_LinkClicked);
            // 
            // linkLabelOpenExplorer
            // 
            this.linkLabelOpenExplorer.AutoSize = true;
            this.linkLabelOpenExplorer.Location = new System.Drawing.Point(577, 17);
            this.linkLabelOpenExplorer.Name = "linkLabelOpenExplorer";
            this.linkLabelOpenExplorer.Size = new System.Drawing.Size(84, 13);
            this.linkLabelOpenExplorer.TabIndex = 11;
            this.linkLabelOpenExplorer.TabStop = true;
            this.linkLabelOpenExplorer.Text = "Open in explorer";
            this.linkLabelOpenExplorer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOpenExplorer_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 600);
            this.Controls.Add(this.linkLabelOpenExplorer);
            this.Controls.Add(this.linkLabelOpenSite);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.pictureBoxForeground);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LoadGalleriesButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Catalog Browser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxForeground)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button LoadGalleriesButton;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBoxForeground;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.LinkLabel linkLabelOpenSite;
        private System.Windows.Forms.LinkLabel linkLabelOpenExplorer;
    }
}

