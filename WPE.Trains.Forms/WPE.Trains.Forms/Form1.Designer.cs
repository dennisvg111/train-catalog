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
            this.catalogListContainer = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // catalogListContainer
            // 
            this.catalogListContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.catalogListContainer.AutoScroll = true;
            this.catalogListContainer.ColumnCount = 3;
            this.catalogListContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.catalogListContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.catalogListContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.catalogListContainer.Location = new System.Drawing.Point(12, 44);
            this.catalogListContainer.Name = "catalogListContainer";
            this.catalogListContainer.RowCount = 1;
            this.catalogListContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.catalogListContainer.Size = new System.Drawing.Size(761, 544);
            this.catalogListContainer.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 600);
            this.Controls.Add(this.catalogListContainer);
            this.Name = "Form1";
            this.Text = "Catalog Browser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel catalogListContainer;
    }
}

