namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.DiaLog
{
    partial class VendorForm
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
            this.button_Gooil = new System.Windows.Forms.Button();
            this.button_LGD = new System.Windows.Forms.Button();
            this.button_TestMode_VirtualVendor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Gooil
            // 
            this.button_Gooil.BackColor = System.Drawing.Color.DarkGreen;
            this.button_Gooil.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Gooil.ForeColor = System.Drawing.Color.White;
            this.button_Gooil.Location = new System.Drawing.Point(10, 12);
            this.button_Gooil.Name = "button_Gooil";
            this.button_Gooil.Size = new System.Drawing.Size(212, 220);
            this.button_Gooil.TabIndex = 0;
            this.button_Gooil.Text = "Gooil";
            this.button_Gooil.UseVisualStyleBackColor = false;
            this.button_Gooil.Click += new System.EventHandler(this.Gooil_Click);
            // 
            // button_LGD
            // 
            this.button_LGD.BackColor = System.Drawing.Color.Brown;
            this.button_LGD.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LGD.ForeColor = System.Drawing.Color.White;
            this.button_LGD.Location = new System.Drawing.Point(228, 12);
            this.button_LGD.Name = "button_LGD";
            this.button_LGD.Size = new System.Drawing.Size(212, 220);
            this.button_LGD.TabIndex = 1;
            this.button_LGD.Text = "LGD";
            this.button_LGD.UseVisualStyleBackColor = false;
            this.button_LGD.Click += new System.EventHandler(this.button_LGD_Click);
            // 
            // button_TestMode_VirtualVendor
            // 
            this.button_TestMode_VirtualVendor.BackColor = System.Drawing.Color.Indigo;
            this.button_TestMode_VirtualVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_TestMode_VirtualVendor.ForeColor = System.Drawing.Color.White;
            this.button_TestMode_VirtualVendor.Location = new System.Drawing.Point(447, 12);
            this.button_TestMode_VirtualVendor.Name = "button_TestMode_VirtualVendor";
            this.button_TestMode_VirtualVendor.Size = new System.Drawing.Size(212, 220);
            this.button_TestMode_VirtualVendor.TabIndex = 2;
            this.button_TestMode_VirtualVendor.Text = "TestMode Virtual Vendor";
            this.button_TestMode_VirtualVendor.UseVisualStyleBackColor = false;
            this.button_TestMode_VirtualVendor.Click += new System.EventHandler(this.button_TestMode_VirtualVendor_Click);
            // 
            // VendorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(668, 237);
            this.Controls.Add(this.button_TestMode_VirtualVendor);
            this.Controls.Add(this.button_LGD);
            this.Controls.Add(this.button_Gooil);
            this.Name = "VendorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VendorForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Gooil;
        private System.Windows.Forms.Button button_LGD;
        private System.Windows.Forms.Button button_TestMode_VirtualVendor;
    }
}