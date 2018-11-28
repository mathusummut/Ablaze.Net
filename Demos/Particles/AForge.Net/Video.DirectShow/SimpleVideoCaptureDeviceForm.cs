using System;
using System.Windows.Forms;

namespace AForge.Video.DirectShow {
	/// <summary>
	/// Local video device selection form
	/// </summary>
	public class SimpleVideoCaptureDeviceForm : Form {
		private Label label1;
		private ComboBox devicesCombo;
		private Button cancelButton;
		private Button okButton;
		private FilterInfoCollection videoDevices;
		private string device;

		/// <summary>
		/// Gets the selected device
		/// </summary>
		public string VideoDevice {
			get {
				return device;
			}
		}

		/// <summary>
		/// Inittalizes a new device selection form
		/// </summary>
		public SimpleVideoCaptureDeviceForm() {
			InitializeComponent();

			// show device list
			try {
				// enumerate video devices
				videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

				if (videoDevices.Count == 0)
					throw new ApplicationException();

				// add all devices to combo
				foreach (FilterInfo device in videoDevices) {
					devicesCombo.Items.Add(device.Name);
				}
			} catch (ApplicationException) {
				devicesCombo.Items.Add("No local capture devices");
				devicesCombo.Enabled = false;
				okButton.Enabled = false;
			}

			devicesCombo.SelectedIndex = 0;
		}

		// Ok button clicked
		private void okButton_Click(object sender, EventArgs e) {
			device = videoDevices[devicesCombo.SelectedIndex].MonikerString;
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.devicesCombo = new System.Windows.Forms.ComboBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select local video capture device:";
			// 
			// devicesCombo
			// 
			this.devicesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.devicesCombo.FormattingEnabled = true;
			this.devicesCombo.Location = new System.Drawing.Point(10, 35);
			this.devicesCombo.Name = "devicesCombo";
			this.devicesCombo.Size = new System.Drawing.Size(325, 21);
			this.devicesCombo.TabIndex = 1;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(180, 80);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(83, 80);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// SimpleVideoCaptureDeviceForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(344, 116);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.devicesCombo);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SimpleVideoCaptureDeviceForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select video capture device";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}