namespace ContentTypeTextNet.Pe.PeMain.UI.CustomControl
{
	partial class EnvUpdateControl
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.gridEnv = new System.Windows.Forms.DataGridView();
			this.gridEnv_columnKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridEnv_columnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridEnv)).BeginInit();
			this.SuspendLayout();
			// 
			// gridEnv
			// 
			this.gridEnv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridEnv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.gridEnv_columnKey,
									this.gridEnv_columnValue});
			this.gridEnv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridEnv.Location = new System.Drawing.Point(0, 0);
			this.gridEnv.Name = "gridEnv";
			this.gridEnv.RowTemplate.Height = 21;
			this.gridEnv.Size = new System.Drawing.Size(257, 150);
			this.gridEnv.TabIndex = 0;
			this.gridEnv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridEnv_CellValueChanged);
			this.gridEnv.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.GridEnv_RowsRemoved);
			// 
			// gridEnv_columnKey
			// 
			this.gridEnv_columnKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.gridEnv_columnKey.HeaderText = ":env-updater/header/key";
			this.gridEnv_columnKey.Name = "gridEnv_columnKey";
			this.gridEnv_columnKey.Width = 118;
			// 
			// gridEnv_columnValue
			// 
			this.gridEnv_columnValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gridEnv_columnValue.DefaultCellStyle = dataGridViewCellStyle1;
			this.gridEnv_columnValue.HeaderText = ":env-updater/header/value";
			this.gridEnv_columnValue.Name = "gridEnv_columnValue";
			// 
			// EnvUpdateControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridEnv);
			this.Name = "EnvUpdateControl";
			this.Size = new System.Drawing.Size(257, 150);
			((System.ComponentModel.ISupportInitialize)(this.gridEnv)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.DataGridViewTextBoxColumn gridEnv_columnValue;
		private System.Windows.Forms.DataGridViewTextBoxColumn gridEnv_columnKey;
		private System.Windows.Forms.DataGridView gridEnv;
	}
}
