using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms {
	/// <summary>
	/// An interactive list box supporting 'Add New...', and move up and move down arrow buttons.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DesignTimeVisible(true)]
	[DesignerCategory("CommonControls")]
	[DefaultEvent(nameof(ItemAdded))]
	[Description("An interactive list box supporting 'Add New...', and move up and move down arrow buttons.")]
	[DisplayName(nameof(UserListBox))]
	public class UserListBox : UserControl {
		private ListBox listBox;
		private StyledArrowButton upButton, downButton;
		private SplitContainer SplitContainer;
		private int defaultButtonWidth = 45, minButtonWidth = 16;
		private bool removeDuplicates, showDirections = true;
		/// <summary>
		/// A delegate representing a method called when an item was added or removed.
		/// </summary>
		/// <param name="items">A new list of all iems in the list box.</param>
		public delegate void ItemsChangedEventHandler(string[] items);
		/// <summary>
		/// A delegate representing a method called when an item has beem moved up or moved down.
		/// </summary>
		/// <param name="upperItem">The first (starting from top) item whose order was changed.</param>
		/// <param name="lowerItem">The botton (starting from top) item whose order was changed.</param>
		/// <param name="upperItemIndex">The index of upperItem (the index of lowerItem is upperItem + 1).</param>
		public delegate void ItemOrderChangedEventHandler(string upperItem, string lowerItem, int upperItemIndex);
		/// <summary>
		/// Fired when an item was added or removed.
		/// </summary>
		[Description("Fired when an item was added or removed.")]
		public event ItemsChangedEventHandler ItemsChanged;
		/// <summary>
		/// Fired when an item was added.
		/// </summary>
		[Description("Fired when an item was added.")]
		public event Action<string> ItemAdded;
		/// <summary>
		/// Fired when an item was removed.
		/// </summary>
		[Description("Fired when an item was removed.")]
		public event Action<string> ItemRemoved;
		/// <summary>
		/// Fired when an item has beem moved up or moved down.
		/// </summary>
		[Description("Fired when an item has beem moved up or moved down.")]
		public event ItemOrderChangedEventHandler ItemOrderChanged;
		/// <summary>
		/// The add item prompt text.
		/// </summary>
		public string PromptText = "Add Entry:";
		/// <summary>
		/// The add item prompt caption.
		/// </summary>
		public string PromptTitle = "Add";
		/// <summary>
		/// The add item prompt button text.
		/// </summary>
		public string PromptButton = "Add";

		/// <summary>
		/// Gets a list of all items in the list.
		/// </summary>
		[Browsable(false)]
		public string[] Items {
			get {
				string[] items = new string[listBox.Items.Count - 1];
				for (int i = 0; i < listBox.Items.Count - 1; i++)
					items[i] = listBox.Items[i].ToString();
				return items;
			}
		}

		/// <summary>
		/// Whether to remove or keep duplicates in the list.
		/// </summary>
		[Description("Whether to remove or keep duplicates in the list.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool RemoveDuplicates {
			get {
				return removeDuplicates;
			}
			set {
				removeDuplicates = value;
				if (removeDuplicates) {
					string[] currentElements = Items;
					List<string> newList = new List<string>();
					foreach (string str in currentElements) {
						if (!newList.Contains(str))
							newList.Add(str);
					}
					listBox.Items.Clear();
					foreach (string s in newList)
						listBox.Items.Add(s);
					listBox.Items.Add("Add New...");
				}
			}
		}

		/// <summary>
		/// The minimum arrow button widths.
		/// </summary>
		[Description("The minimum arrow button widths.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int MinButtonWidth {
			get {
				return minButtonWidth;
			}
			set {
				minButtonWidth = value;
				if (showDirections)
					SplitContainer.Panel2MinSize = value;
			}
		}

		/// <summary>
		/// Whether to show the move up and down arrow buttons.
		/// </summary>
		[Description("Whether to show the move up and down arrow buttons.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool ShowDirectionButtons {
			get {
				return showDirections;
			}
			set {
				if (value == showDirections)
					return;
				showDirections = value;
				if (value) {
					SplitContainer.Panel2.Controls.Add(upButton);
					SplitContainer.Panel2.Controls.Add(downButton);
				} else {
					SplitContainer.Panel2.Controls.Remove(upButton);
					SplitContainer.Panel2.Controls.Remove(downButton);
				}
				OnResize(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The ListBox control.
		/// </summary>
		[Description("The ListBox control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public ListBox ListBox {
			get {
				return listBox;
			}
		}

		/// <summary>
		/// The up arrow button control.
		/// </summary>
		[Description("The up arrow button control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public StyledArrowButton UpButton {
			get {
				return upButton;
			}
		}

		/// <summary>
		/// The down arrow button control.
		/// </summary>
		[Description("The down arrow button control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public StyledArrowButton DownButton {
			get {
				return downButton;
			}
		}

		/// <summary>
		/// Gets or sets whether to trim trailing spaces to user input.
		/// </summary>
		[Description("Gets or sets whether to trim trailing spaces to user input.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool TrimSpaces {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the default button width, which is overriden when the splitter is resized.
		/// </summary>
		[Description("Gets or sets the default button width, which is overriden when the splitter is resized.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int DefaultButtonWidth {
			get {
				return defaultButtonWidth;
			}
			set {
				if (value == defaultButtonWidth)
					return;
				defaultButtonWidth = value;
				OnResize(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Initializes an empty list box.
		/// </summary>
		public UserListBox() : this(true, null) {
		}

		/// <summary>
		/// Initializes a list box, spcifying whether to show the move up and down arrow buttons.
		/// </summary>
		/// <param name="showButtons">Whether to show the move up and down arrow buttons.</param>
		public UserListBox(bool showButtons) : this(showButtons, null) {
		}

		/// <summary>
		/// Initializes a list box with the specified initial items.
		/// </summary>
		/// <param name="items">The items to add to the list box.</param>
		public UserListBox(params string[] items) : this(true, items) {
		}

		/// <summary>
		/// Initializes a list box with the specified initial items.
		/// </summary>
		/// <param name="showButtons">Whether to show the move up and down arrow buttons.</param>
		/// <param name="items">The items to add to the list box (can be null).</param>
		public UserListBox(bool showButtons, params string[] items) {
			TrimSpaces = true;
			listBox = new ListBox();
			SplitContainer = new SplitContainer();
			downButton = new StyledArrowButton();
			upButton = new StyledArrowButton();
			SplitContainer.Panel1.SuspendLayout();
			SplitContainer.Panel2.SuspendLayout();
			SplitContainer.SuspendLayout();
			SuspendLayout();
			listBox.BorderStyle = BorderStyle.None;
			listBox.Dock = DockStyle.Fill;
			listBox.Font = new Font("Calibri Light", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			listBox.FormattingEnabled = true;
			listBox.IntegralHeight = false;
			listBox.ItemHeight = 19;
			if (!(items == null || items.Length == 0))
				listBox.Items.AddRange(items);
			listBox.Items.Add("Add New...");
			listBox.Location = Point.Empty;
			listBox.Name = nameof(ListBox);
			listBox.ScrollAlwaysVisible = true;
			listBox.Size = new Size(320, 356);
			listBox.TabIndex = 0;
			listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
			listBox.MouseDown += ListBox_MouseUp;
			SplitContainer.Dock = DockStyle.Fill;
			SplitContainer.Location = Point.Empty;
			SplitContainer.Name = nameof(SplitContainer);
			SplitContainer.Panel1.Controls.Add(listBox);
			SplitContainer.Size = new Size(382, 356);
			SplitContainer.SplitterDistance = 320;
			SplitContainer.SplitterWidth = 4;
			SplitContainer.TabIndex = 3;
			SplitContainer.SplitterMoved += SplitContainer_SplitterMoved;
			downButton.BackColor = BackColor;
			downButton.BackgroundImageLayout = ImageLayout.Stretch;
			downButton.Dock = DockStyle.Bottom;
			downButton.Location = new Point(0, 295);
			downButton.Name = nameof(DownButton);
			downButton.Rotation = 180F;
			downButton.Size = new Size(61, 61);
			downButton.TabIndex = 2;
			downButton.Click += button2_Click;
			upButton.BackColor = BackColor;
			upButton.BackgroundImageLayout = ImageLayout.Stretch;
			upButton.Dock = DockStyle.Top;
			upButton.Location = Point.Empty;
			upButton.Name = nameof(UpButton);
			upButton.Size = new Size(61, 61);
			upButton.TabIndex = 1;
			upButton.Click += button1_Click;
			Controls.Add(SplitContainer);
			Name = nameof(UserListBox);
			Size = new Size(382, 356);
			this.showDirections = showButtons;
			if (showButtons) {
				SplitContainer.Panel2.Controls.Add(upButton);
				SplitContainer.Panel2.Controls.Add(downButton);
			}
			SplitContainer.Panel1.ResumeLayout(false);
			SplitContainer.Panel2.ResumeLayout(false);
			SplitContainer.ResumeLayout(false);
			ResumeLayout(false);
		}

		/// <summary>
		/// Called when the list box is resized.
		/// </summary>
		protected override void OnResize(EventArgs e) {
			if (e != null)
				base.OnResize(e);
			if (showDirections) {
				SplitContainer.Panel2MinSize = minButtonWidth;
				upButton.Height = Math.Min(upButton.Width, ClientSize.Height / 2);
				downButton.Height = Math.Min(upButton.Width, ClientSize.Height / 2);
				if (SplitContainer.Panel1MinSize <= Width)
					SplitContainer.SplitterDistance = ClientSize.Width - (defaultButtonWidth + SplitContainer.SplitterWidth);
			} else {
				SplitContainer.Panel2MinSize = 0;
				if (SplitContainer.Panel1MinSize <= Width)
					SplitContainer.SplitterDistance = ClientSize.Width;
			}
		}

		private void SplitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
			if (showDirections) {
				upButton.Height = Math.Min(upButton.Width, ClientSize.Height / 2);
				downButton.Height = Math.Min(upButton.Width, ClientSize.Height / 2);
			}
		}

		private void ListBox_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right && !(listBox.SelectedIndex == -1 || listBox.SelectedIndex == listBox.Items.Count - 1)) {
				string item = listBox.Items[listBox.SelectedIndex].ToString();
				listBox.Items.RemoveAt(listBox.SelectedIndex);
				Action<string> itemRemoved = ItemRemoved;
				if (itemRemoved != null)
					itemRemoved(item);
				ItemsChangedEventHandler itemsChanged = ItemsChanged;
				if (itemsChanged != null)
					itemsChanged(Items);
			}
		}

		private void ListBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (listBox.SelectedIndex == listBox.Items.Count - 1)
				ShowAddItemPrompt();
		}

		/// <summary>
		/// Shows a new item prompt to the user and adds the item if the user accepts.
		/// </summary>
		protected virtual void ShowAddItemPrompt() {
			TextDialog dialog = new TextDialog(PromptText, PromptTitle, PromptButton);
			if (MessageLoop.ShowDialog(dialog) == DialogResult.OK)
				AddItem(dialog.Input);
		}

		/// <summary>
		/// Adds the specified item to list box.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void AddItem(string item) {
			if (item == null)
				item = string.Empty;
			if (TrimSpaces)
				item = item.Trim();
			if (item.Length == 0)
				return;
			string[] items = Items;
			if (removeDuplicates && Contains(items, item))
				return;
			listBox.Items.Insert(listBox.Items.Count - 1, item);
			Action<string> itemAdded = ItemAdded;
			if (itemAdded != null)
				itemAdded(item);
			ItemsChangedEventHandler itemsChanged = ItemsChanged;
			if (itemsChanged != null)
				itemsChanged(Items);
		}

		/// <summary>
		/// Gets whether the specified array contains the specified element.
		/// </summary>
		/// <typeparam name="T">The type of the array.</typeparam>
		/// <param name="array">The array to search for the item into.</param>
		/// <param name="element">The item to search for in the array.</param>
		public static bool Contains<T>(T[] array, T element) {
			if (array == null)
				return false;
			for (int i = 0; i < array.Length; i++) {
				if (array[i] == null) {
					if (element == null)
						return true;
				} else if (array[i].Equals(element))
					return true;
			}
			return false;
		}

		private void button1_Click(object sender, EventArgs e) {
			int index = listBox.SelectedIndex;
			if (index <= 0 || index == listBox.Items.Count - 1)
				return;
			string belowItem = listBox.Items[index].ToString();
			string upperItem = listBox.Items[index - 1].ToString();
			listBox.Items[index] = upperItem;
			index--;
			listBox.Items[index] = belowItem;
			listBox.SelectedIndex = index;
			ItemOrderChangedEventHandler itemOrderChanged = ItemOrderChanged;
			if (itemOrderChanged != null)
				itemOrderChanged(belowItem, upperItem, index);
		}

		private void button2_Click(object sender, EventArgs e) {
			int index = listBox.SelectedIndex;
			if (index == -1 || index >= listBox.Items.Count - 2)
				return;
			string belowItem = listBox.Items[index].ToString();
			string upperItem = listBox.Items[index + 1].ToString();
			listBox.Items[index] = upperItem;
			listBox.Items[index + 1] = belowItem;
			listBox.SelectedIndex = index + 1;
			ItemOrderChangedEventHandler itemOrderChanged = ItemOrderChanged;
			if (itemOrderChanged != null)
				itemOrderChanged(upperItem, belowItem, index);
		}
	}
}