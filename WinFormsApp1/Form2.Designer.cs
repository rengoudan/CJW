namespace WinFormsApp1
{
    partial class Form2
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
            components = new System.ComponentModel.Container();
            button1 = new Button();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            originDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            endPointDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            lineLengthDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwBeamIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            locationDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwLineBindingSource = new BindingSource(components);
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwLineBindingSource).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(117, 21);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(259, 21);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 1;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { idDataGridViewTextBoxColumn, originDataGridViewTextBoxColumn, endPointDataGridViewTextBoxColumn, lineLengthDataGridViewTextBoxColumn, jwBeamIdDataGridViewTextBoxColumn, locationDataGridViewTextBoxColumn });
            dataGridView1.DataSource = jwLineBindingSource;
            dataGridView1.Location = new Point(12, 121);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(240, 150);
            dataGridView1.TabIndex = 2;
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Id";
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            // 
            // originDataGridViewTextBoxColumn
            // 
            originDataGridViewTextBoxColumn.DataPropertyName = "Origin";
            originDataGridViewTextBoxColumn.HeaderText = "Origin";
            originDataGridViewTextBoxColumn.Name = "originDataGridViewTextBoxColumn";
            // 
            // endPointDataGridViewTextBoxColumn
            // 
            endPointDataGridViewTextBoxColumn.DataPropertyName = "EndPoint";
            endPointDataGridViewTextBoxColumn.HeaderText = "EndPoint";
            endPointDataGridViewTextBoxColumn.Name = "endPointDataGridViewTextBoxColumn";
            // 
            // lineLengthDataGridViewTextBoxColumn
            // 
            lineLengthDataGridViewTextBoxColumn.DataPropertyName = "LineLength";
            lineLengthDataGridViewTextBoxColumn.HeaderText = "LineLength";
            lineLengthDataGridViewTextBoxColumn.Name = "lineLengthDataGridViewTextBoxColumn";
            // 
            // jwBeamIdDataGridViewTextBoxColumn
            // 
            jwBeamIdDataGridViewTextBoxColumn.DataPropertyName = "JwBeamId";
            jwBeamIdDataGridViewTextBoxColumn.HeaderText = "JwBeamId";
            jwBeamIdDataGridViewTextBoxColumn.Name = "jwBeamIdDataGridViewTextBoxColumn";
            // 
            // locationDataGridViewTextBoxColumn
            // 
            locationDataGridViewTextBoxColumn.DataPropertyName = "Location";
            locationDataGridViewTextBoxColumn.HeaderText = "Location";
            locationDataGridViewTextBoxColumn.Name = "locationDataGridViewTextBoxColumn";
            // 
            // jwLineBindingSource
            // 
            jwLineBindingSource.DataSource = typeof(JwData.JwLine);
            // 
            // button3
            // 
            button3.Location = new Point(209, 347);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 3;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(dataGridView1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwLineBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn originDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn endPointDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lineLengthDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn jwBeamIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn;
        private BindingSource jwLineBindingSource;
        private Button button3;
    }
}