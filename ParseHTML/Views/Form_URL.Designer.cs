namespace ParseHTML.Views
{
    partial class Form_URL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_URL));
            this.btn_scanner = new System.Windows.Forms.Button();
            this.tb_adresse_site_web = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_fermer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_scanner
            // 
            this.btn_scanner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_scanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btn_scanner.Location = new System.Drawing.Point(239, 87);
            this.btn_scanner.Name = "btn_scanner";
            this.btn_scanner.Size = new System.Drawing.Size(75, 36);
            this.btn_scanner.TabIndex = 0;
            this.btn_scanner.Text = "Scanner";
            this.btn_scanner.UseVisualStyleBackColor = true;
            this.btn_scanner.Click += new System.EventHandler(this.btn_scanner_Click);
            // 
            // tb_adresse_site_web
            // 
            this.tb_adresse_site_web.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_adresse_site_web.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.tb_adresse_site_web.Location = new System.Drawing.Point(16, 36);
            this.tb_adresse_site_web.Name = "tb_adresse_site_web";
            this.tb_adresse_site_web.Size = new System.Drawing.Size(298, 22);
            this.tb_adresse_site_web.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Url site web :";
            // 
            // btn_fermer
            // 
            this.btn_fermer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_fermer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btn_fermer.Location = new System.Drawing.Point(16, 87);
            this.btn_fermer.Name = "btn_fermer";
            this.btn_fermer.Size = new System.Drawing.Size(75, 36);
            this.btn_fermer.TabIndex = 3;
            this.btn_fermer.Text = "Fermer";
            this.btn_fermer.UseVisualStyleBackColor = true;
            this.btn_fermer.Click += new System.EventHandler(this.btn_fermer_Click);
            // 
            // Form_URL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 135);
            this.Controls.Add(this.btn_fermer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_adresse_site_web);
            this.Controls.Add(this.btn_scanner);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_URL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Page d\'acceuil";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_scanner;
        private System.Windows.Forms.TextBox tb_adresse_site_web;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_fermer;
    }
}

