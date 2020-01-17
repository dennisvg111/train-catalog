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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.LoadGalleriesButton = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxForeground = new System.Windows.Forms.PictureBox();
            this.pictureBoxTrain = new System.Windows.Forms.PictureBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxForeground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrain)).BeginInit();
            this.SuspendLayout();
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
            this.pictureBoxForeground.BackgroundImage = global::WPE.Trains.Forms.Properties.Resources.landscape_foreground;
            this.pictureBoxForeground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxForeground.Location = new System.Drawing.Point(12, 40);
            this.pictureBoxForeground.Name = "pictureBoxForeground";
            this.pictureBoxForeground.Size = new System.Drawing.Size(761, 204);
            this.pictureBoxForeground.TabIndex = 7;
            this.pictureBoxForeground.TabStop = false;
            // 
            // pictureBoxTrain
            // 
            this.pictureBoxTrain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxTrain.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxTrain.BackgroundImage = global::WPE.Trains.Forms.Properties.Resources.landscape_train;
            this.pictureBoxTrain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxTrain.Location = new System.Drawing.Point(12, 40);
            this.pictureBoxTrain.Name = "pictureBoxTrain";
            this.pictureBoxTrain.Size = new System.Drawing.Size(104, 204);
            this.pictureBoxTrain.TabIndex = 8;
            this.pictureBoxTrain.TabStop = false;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(13, 251);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(760, 337);
            this.textBoxLog.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 600);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.pictureBoxTrain);
            this.Controls.Add(this.pictureBoxForeground);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LoadGalleriesButton);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Catalog Browser";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxForeground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button LoadGalleriesButton;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBoxForeground;
        private System.Windows.Forms.PictureBox pictureBoxTrain;
        private System.Windows.Forms.TextBox textBoxLog;
    }
}

