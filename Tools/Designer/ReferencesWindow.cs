using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Designer {
	public partial class ReferencesWindow : StyledForm {
		MainWindow mainForm = null;
		private List<string> references;

		//Get access to public stuff in main form
		public ReferencesWindow(MainWindow mainForm, List<string> references) {
			InitializeComponent();
			this.mainForm = mainForm;
			this.references = references;
			listBox1.Items.AddRange(references);
			listBox1.SelectionMode = SelectionMode.MultiExtended;
		}

		//Append selection to list
		private void addReferencesToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFileDialog referenceDialog = new OpenFileDialog(); //Instantiate new reference dialog
			referenceDialog.Multiselect = true; //Allow multiple selection
			referenceDialog.ShowDialog(); //Show dialog
			string[] result = referenceDialog.FileNames; //Get paths of all selected files

			int originalLength = listBox1.Items.Count; //Get current reference amount
			bool addRef = true;
			for (int i = originalLength; i < result.Length + originalLength; i++) { //For each selected reference
				string pathA = result[i - originalLength];
				List<int> indices = new List<int>();

				for (int j = 0; j < listBox1.Items.Count; j++) { //Compare with others
					string pathB = listBox1.Items[j].ToString();
					if (System.IO.FileUtils.ResolvePath(pathA).Equals(System.IO.FileUtils.ResolvePath(pathB), StringComparison.InvariantCultureIgnoreCase))
						addRef = false; //If an equivalent path exists do not add this one
				}

				if (addRef) listBox1.Items.Add(pathA); //Add if no equiv. path
			}
		}

		//Remove selection from list
		private void removeReferencesToolStripMenuItem_Click(object sender, EventArgs e) {
			if (listBox1.SelectedIndices.Count >= 1) {
				ListBox.SelectedIndexCollection selectedCollection = listBox1.SelectedIndices;
				for (int i = selectedCollection.Count - 1; i >= 0; i--)
					listBox1.Items.RemoveAt(selectedCollection[i]);
			}
		}

		protected override bool OnQueryClose(CloseReason reason) {
			references.Clear();
			
			references.AddRange(listBox1.Items);

			return true;
		}
	}
}
