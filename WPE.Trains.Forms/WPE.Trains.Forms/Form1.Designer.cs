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
            this.GalleryListProgress = new System.Windows.Forms.ProgressBar();
            this.GalleryProgress = new System.Windows.Forms.ProgressBar();
            this.GalleryName = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.GalleryListProgressText = new System.Windows.Forms.Label();
            this.LoadGalleriesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GalleryListProgress
            // 
            this.GalleryListProgress.Location = new System.Drawing.Point(12, 60);
            this.GalleryListProgress.Name = "GalleryListProgress";
            this.GalleryListProgress.Size = new System.Drawing.Size(761, 23);
            this.GalleryListProgress.TabIndex = 0;
            // 
            // GalleryProgress
            // 
            this.GalleryProgress.Location = new System.Drawing.Point(12, 137);
            this.GalleryProgress.Name = "GalleryProgress";
            this.GalleryProgress.Size = new System.Drawing.Size(761, 23);
            this.GalleryProgress.TabIndex = 1;
            // 
            // GalleryName
            // 
            this.GalleryName.AutoSize = true;
            this.GalleryName.Location = new System.Drawing.Point(12, 121);
            this.GalleryName.Name = "GalleryName";
            this.GalleryName.Size = new System.Drawing.Size(35, 13);
            this.GalleryName.TabIndex = 2;
            this.GalleryName.Text = "label1";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(255, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // GalleryListProgressText
            // 
            this.GalleryListProgressText.AutoSize = true;
            this.GalleryListProgressText.Location = new System.Drawing.Point(12, 86);
            this.GalleryListProgressText.Name = "GalleryListProgressText";
            this.GalleryListProgressText.Size = new System.Drawing.Size(35, 13);
            this.GalleryListProgressText.TabIndex = 4;
            this.GalleryListProgressText.Text = "label1";
            // 
            // LoadGalleriesButton
            // 
            this.LoadGalleriesButton.Location = new System.Drawing.Point(273, 11);
            this.LoadGalleriesButton.Name = "LoadGalleriesButton";
            this.LoadGalleriesButton.Size = new System.Drawing.Size(75, 23);
            this.LoadGalleriesButton.TabIndex = 5;
            this.LoadGalleriesButton.Text = "Load";
            this.LoadGalleriesButton.UseVisualStyleBackColor = true;
            this.LoadGalleriesButton.Click += new System.EventHandler(this.LoadGalleriesButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 600);
            this.Controls.Add(this.LoadGalleriesButton);
            this.Controls.Add(this.GalleryListProgressText);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.GalleryName);
            this.Controls.Add(this.GalleryProgress);
            this.Controls.Add(this.GalleryListProgress);
            this.Name = "Form1";
            this.Text = "Catalog Browser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar GalleryListProgress;
        private System.Windows.Forms.ProgressBar GalleryProgress;
        private System.Windows.Forms.Label GalleryName;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label GalleryListProgressText;
        private System.Windows.Forms.Button LoadGalleriesButton;
    }
}

