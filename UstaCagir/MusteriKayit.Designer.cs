namespace UstaCagir
{
    partial class MusteriKayit
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            adıtextBox1 = new TextBox();
            soyadıtextBox2 = new TextBox();
            emailtextBox4 = new TextBox();
            adrestextBox5 = new TextBox();
            kayıtmusteributton1 = new Button();
            maskedTextBox1 = new MaskedTextBox();
            textBox1 = new TextBox();
            label7 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label1.Location = new Point(71, 18);
            label1.Name = "label1";
            label1.Size = new Size(161, 49);
            label1.TabIndex = 0;
            label1.Text = "KAYIT OL";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13.8F);
            label2.Location = new Point(76, 84);
            label2.Name = "label2";
            label2.Size = new Size(44, 25);
            label2.TabIndex = 1;
            label2.Text = "Adı:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13.8F);
            label3.Location = new Point(76, 115);
            label3.Name = "label3";
            label3.Size = new Size(72, 25);
            label3.TabIndex = 2;
            label3.Text = "Soyadı:";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 13.8F);
            label4.Location = new Point(76, 149);
            label4.Name = "label4";
            label4.Size = new Size(77, 25);
            label4.TabIndex = 3;
            label4.Text = "Telefon:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13.8F);
            label5.Location = new Point(78, 180);
            label5.Name = "label5";
            label5.Size = new Size(62, 25);
            label5.TabIndex = 4;
            label5.Text = "Email:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 13.8F);
            label6.Location = new Point(78, 215);
            label6.Name = "label6";
            label6.Size = new Size(64, 25);
            label6.TabIndex = 5;
            label6.Text = "Adres:";
            // 
            // adıtextBox1
            // 
            adıtextBox1.Location = new Point(182, 84);
            adıtextBox1.Margin = new Padding(3, 2, 3, 2);
            adıtextBox1.Multiline = true;
            adıtextBox1.Name = "adıtextBox1";
            adıtextBox1.Size = new Size(107, 24);
            adıtextBox1.TabIndex = 6;
            // 
            // soyadıtextBox2
            // 
            soyadıtextBox2.Location = new Point(182, 115);
            soyadıtextBox2.Margin = new Padding(3, 2, 3, 2);
            soyadıtextBox2.Multiline = true;
            soyadıtextBox2.Name = "soyadıtextBox2";
            soyadıtextBox2.Size = new Size(107, 24);
            soyadıtextBox2.TabIndex = 7;
            // 
            // emailtextBox4
            // 
            emailtextBox4.Location = new Point(182, 180);
            emailtextBox4.Margin = new Padding(3, 2, 3, 2);
            emailtextBox4.Multiline = true;
            emailtextBox4.Name = "emailtextBox4";
            emailtextBox4.Size = new Size(107, 24);
            emailtextBox4.TabIndex = 9;
            // 
            // adrestextBox5
            // 
            adrestextBox5.Location = new Point(182, 215);
            adrestextBox5.Margin = new Padding(3, 2, 3, 2);
            adrestextBox5.Multiline = true;
            adrestextBox5.Name = "adrestextBox5";
            adrestextBox5.Size = new Size(107, 24);
            adrestextBox5.TabIndex = 10;
            // 
            // kayıtmusteributton1
            // 
            kayıtmusteributton1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            kayıtmusteributton1.Location = new Point(159, 297);
            kayıtmusteributton1.Margin = new Padding(3, 2, 3, 2);
            kayıtmusteributton1.Name = "kayıtmusteributton1";
            kayıtmusteributton1.Size = new Size(107, 30);
            kayıtmusteributton1.TabIndex = 11;
            kayıtmusteributton1.Text = "Kayıt Ol";
            kayıtmusteributton1.UseVisualStyleBackColor = true;
            kayıtmusteributton1.Click += kayıtmusteributton1_Click;
            // 
            // maskedTextBox1
            // 
            maskedTextBox1.Location = new Point(182, 149);
            maskedTextBox1.Margin = new Padding(3, 2, 3, 2);
            maskedTextBox1.Mask = "(999) 000-0000";
            maskedTextBox1.Name = "maskedTextBox1";
            maskedTextBox1.Size = new Size(110, 23);
            maskedTextBox1.TabIndex = 24;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(182, 243);
            textBox1.Margin = new Padding(3, 2, 3, 2);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(107, 24);
            textBox1.TabIndex = 26;
            textBox1.UseSystemPasswordChar = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 13.8F);
            label7.Location = new Point(76, 243);
            label7.Name = "label7";
            label7.Size = new Size(54, 25);
            label7.TabIndex = 25;
            label7.Text = "Şifre:";
            // 
            // MusteriKayit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(383, 338);
            Controls.Add(textBox1);
            Controls.Add(label7);
            Controls.Add(maskedTextBox1);
            Controls.Add(kayıtmusteributton1);
            Controls.Add(adrestextBox5);
            Controls.Add(emailtextBox4);
            Controls.Add(soyadıtextBox2);
            Controls.Add(adıtextBox1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "MusteriKayit";
            Text = "MusteriKayit";
            Load += MusteriKayit_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox adıtextBox1;
        private TextBox soyadıtextBox2;
        private TextBox emailtextBox4;
        private TextBox adrestextBox5;
        private Button kayıtmusteributton1;
        private MaskedTextBox maskedTextBox1;
        private TextBox textBox1;
        private Label label7;
    }
}