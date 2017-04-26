namespace osmutilFrontEnd
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
            this.listEmailAddresses = new System.Windows.Forms.RadioButton();
            this.findMovers = new System.Windows.Forms.RadioButton();
            this.findMissingData = new System.Windows.Forms.RadioButton();
            this.checkContactCheckboxes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dryRun = new System.Windows.Forms.CheckBox();
            this.results = new System.Windows.Forms.TextBox();
            this.go = new System.Windows.Forms.Button();
            this.userName = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.apiKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.login = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listEmailAddresses
            // 
            this.listEmailAddresses.AutoSize = true;
            this.listEmailAddresses.Location = new System.Drawing.Point(39, 36);
            this.listEmailAddresses.Name = "listEmailAddresses";
            this.listEmailAddresses.Size = new System.Drawing.Size(160, 21);
            this.listEmailAddresses.TabIndex = 0;
            this.listEmailAddresses.TabStop = true;
            this.listEmailAddresses.Text = "List Email Addresses";
            this.listEmailAddresses.UseVisualStyleBackColor = true;
            // 
            // findMovers
            // 
            this.findMovers.AutoSize = true;
            this.findMovers.Location = new System.Drawing.Point(39, 117);
            this.findMovers.Name = "findMovers";
            this.findMovers.Size = new System.Drawing.Size(106, 21);
            this.findMovers.TabIndex = 2;
            this.findMovers.TabStop = true;
            this.findMovers.Text = "Find movers";
            this.findMovers.UseVisualStyleBackColor = true;
            // 
            // findMissingData
            // 
            this.findMissingData.AutoSize = true;
            this.findMissingData.Location = new System.Drawing.Point(39, 63);
            this.findMissingData.Name = "findMissingData";
            this.findMissingData.Size = new System.Drawing.Size(229, 21);
            this.findMissingData.TabIndex = 3;
            this.findMissingData.TabStop = true;
            this.findMissingData.Text = "Find members with missing data";
            this.findMissingData.UseVisualStyleBackColor = true;
            // 
            // checkContactCheckboxes
            // 
            this.checkContactCheckboxes.AutoSize = true;
            this.checkContactCheckboxes.Location = new System.Drawing.Point(39, 90);
            this.checkContactCheckboxes.Name = "checkContactCheckboxes";
            this.checkContactCheckboxes.Size = new System.Drawing.Size(222, 21);
            this.checkContactCheckboxes.TabIndex = 4;
            this.checkContactCheckboxes.TabStop = true;
            this.checkContactCheckboxes.Text = "Check all \'Contact\' checkboxes";
            this.checkContactCheckboxes.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Operation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(365, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sections";
            // 
            // dryRun
            // 
            this.dryRun.AutoSize = true;
            this.dryRun.Location = new System.Drawing.Point(39, 153);
            this.dryRun.Name = "dryRun";
            this.dryRun.Size = new System.Drawing.Size(82, 21);
            this.dryRun.TabIndex = 8;
            this.dryRun.Text = "Dry Run";
            this.dryRun.UseVisualStyleBackColor = true;
            // 
            // results
            // 
            this.results.Location = new System.Drawing.Point(12, 248);
            this.results.Multiline = true;
            this.results.Name = "results";
            this.results.Size = new System.Drawing.Size(1136, 389);
            this.results.TabIndex = 9;
            // 
            // go
            // 
            this.go.Location = new System.Drawing.Point(1025, 215);
            this.go.Name = "go";
            this.go.Size = new System.Drawing.Size(75, 27);
            this.go.TabIndex = 10;
            this.go.Text = "Go";
            this.go.UseVisualStyleBackColor = true;
            this.go.Click += new System.EventHandler(this.go_Click);
            // 
            // userName
            // 
            this.userName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.userName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.userName.Location = new System.Drawing.Point(85, 656);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(251, 22);
            this.userName.TabIndex = 11;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(429, 656);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(251, 22);
            this.password.TabIndex = 12;
            // 
            // apiKey
            // 
            this.apiKey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.apiKey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.apiKey.Location = new System.Drawing.Point(776, 656);
            this.apiKey.Name = "apiKey";
            this.apiKey.Size = new System.Drawing.Size(251, 22);
            this.apiKey.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 659);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "UserName";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(354, 659);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(703, 659);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "APIKey";
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(1062, 655);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(75, 32);
            this.login.TabIndex = 17;
            this.login.Text = "Login";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(842, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "Term Dates";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(862, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 17);
            this.label7.TabIndex = 19;
            this.label7.Text = "TODO!!";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 690);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.login);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.apiKey);
            this.Controls.Add(this.password);
            this.Controls.Add(this.userName);
            this.Controls.Add(this.go);
            this.Controls.Add(this.results);
            this.Controls.Add(this.dryRun);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkContactCheckboxes);
            this.Controls.Add(this.findMissingData);
            this.Controls.Add(this.findMovers);
            this.Controls.Add(this.listEmailAddresses);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton listEmailAddresses;
        private System.Windows.Forms.RadioButton findMovers;
        private System.Windows.Forms.RadioButton findMissingData;
        private System.Windows.Forms.RadioButton checkContactCheckboxes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox dryRun;
        private System.Windows.Forms.TextBox results;
        private System.Windows.Forms.Button go;
        private System.Windows.Forms.TextBox userName;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox apiKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button login;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

