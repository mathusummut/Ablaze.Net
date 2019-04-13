using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace System.Windows.Forms {
	/// <summary>
	/// Prints the data in a DataGridView.
	/// </summary>
	public sealed class DataGridViewPrinter : IDisposable {
		// Formatting the Content of Text Cell to print
		private static StringFormat StrFormat = new StringFormat() {
			Alignment = StringAlignment.Near,
			LineAlignment = StringAlignment.Center,
			Trimming = StringTrimming.EllipsisCharacter
		};
		private static StringFormat StrFormatComboBox = new StringFormat() {
			LineAlignment = StringAlignment.Center,
			FormatFlags = StringFormatFlags.NoWrap,
			Trimming = StringTrimming.EllipsisCharacter
		};
		private Button CellButton;       // Holds the Contents of Button Cell
		private CheckBox CellCheckBox;   // Holds the Contents of CheckBox Cell 
		private ComboBox CellComboBox;   // Holds the Contents of ComboBox Cell
		private int TotalWidth;          // Summation of Columns widths
		private int RowPos;              // Position of currently printing row 
		private bool NewPage;            // Indicates if a new page reached
		private int PageNo;              // Number of pages to print
		private List<int> ColumnLefts = new List<int>();  // Left Coordinate of Columns
		private List<int> ColumnWidths = new List<int>(); // Width of Columns
		private List<Type> ColumnTypes = new List<Type>();  // DataType of Columns
		private int CellHeight;          // Height of DataGrid Cell
		private int RowsPerPage;         // Number of Rows per Page
		private PrintDocument printDoc = new PrintDocument();  // PrintDocumnet Object used for printing
		private string PrintTitle = string.Empty;  // Header of pages
		private DataGridView dataGrid;        // Holds DataGridView Object to print its contents
		private List<string> AvailableColumns = new List<string>();  // All Columns avaiable in DataGrid 
		private bool PrintAllRows = true;   // True = print all rows,  False = print selected rows    
		private bool FitToPageWidth = true; // True = Fits selected columns to page width ,  False = Print columns as showed    
		private int HeaderHeight;
		private PrintDialog dialog = new PrintDialog();

		/// <summary>
		/// Initializes a new DataGridViewPrinter instance.
		/// </summary>
		/// <param name="grid">The data grid to print.</param>
		/// <param name="pageTitle">The page header.</param>
		/// <param name="printOnlySelectedRows">If true, only the rows that are selected in the DataGridView are printed.</param>
		/// <param name="fitToPageWidth">If true, the grid will be resized to fit a single page.</param>
		/// <param name="columnsToPrint">The names of the columns to print. If null, all columns are printed.</param>
		public DataGridViewPrinter(DataGridView grid, string pageTitle, bool fitToPageWidth, bool printOnlySelectedRows, params string[] columnsToPrint) {
			if (Platform.IsWindowsXPOrNewer)
				dialog.UseEXDialog = true;
			dataGrid = grid;
			if (columnsToPrint == null || columnsToPrint.Length == 0) {
				foreach (DataGridViewColumn c in grid.Columns) {
					if (c.Visible)
						AvailableColumns.Add(c.HeaderText);
				}
			} else
				AvailableColumns = new List<string>(columnsToPrint);
			PrintTitle = pageTitle;
			PrintAllRows = !printOnlySelectedRows;
			FitToPageWidth = fitToPageWidth;
		}

		/// <summary>
		/// Presents a print dialog to the user and prints the DataGridView if the user clicks OK.
		/// Returns true if printing actually took place.
		/// </summary>
		public bool Print() {
			PrintEventHandler beginPrint = PrintDoc_BeginPrint;
			printDoc.BeginPrint += beginPrint;
			PrintPageEventHandler printPage = PrintDoc_PrintPage;
			printDoc.PrintPage += printPage;
			try {
				// Showing the Print Preview Page
				if (dialog.ShowDialog() == DialogResult.OK) {
					string oldPrinterName = null;
					if (dialog.PrintToFile) {
						oldPrinterName = dialog.PrinterSettings.PrinterName;
						dialog.PrinterSettings.PrinterName = "Microsoft Print to PDF";
						dialog.PrinterSettings.PrintToFile = true;
					}
					printDoc.PrinterSettings = (PrinterSettings) dialog.PrinterSettings.Clone();
					if (oldPrinterName != null)
						dialog.PrinterSettings.PrinterName = oldPrinterName;
					// Printing the Documnet
					printDoc.Print();
				} else
					return false;
				return true;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			} finally {
				printDoc.BeginPrint -= beginPrint;
				printDoc.PrintPage -= printPage;
			}
		}

		private void PrintDoc_BeginPrint(object sender, PrintEventArgs e) {
			ColumnLefts.Clear();
			ColumnWidths.Clear();
			ColumnTypes.Clear();
			// For various column types
			CellButton = new Button();
			CellCheckBox = new CheckBox();
			CellComboBox = new ComboBox();
			// Calculating Total Widths
			TotalWidth = 0;
			foreach (DataGridViewColumn GridCol in dataGrid.Columns) {
				if (GridCol.Visible && AvailableColumns.Contains(GridCol.HeaderText))
					TotalWidth += GridCol.Width;
			}
			PageNo = 1;
			NewPage = true;
			RowPos = 0;
		}

		private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e) {
			int tmpWidth, i, tmpTop = e.MarginBounds.Top, tmpLeft = e.MarginBounds.Left;
			try {
				// Before starting first page, it saves Width & Height of Headers and CoulmnType
				if (PageNo == 1) {
					foreach (DataGridViewColumn GridCol in dataGrid.Columns) {
						// Skip if the current column not selected
						if (!GridCol.Visible || !AvailableColumns.Contains(GridCol.HeaderText))
							continue;
						// Detemining whether the columns are fitted to page or not.
						if (FitToPageWidth)
							tmpWidth = (int) (Math.Floor(GridCol.Width * (e.MarginBounds.Width / (double) TotalWidth)));
						else
							tmpWidth = GridCol.Width;
						HeaderHeight = (int) (e.Graphics.MeasureString(GridCol.HeaderText, GridCol.InheritedStyle.Font, tmpWidth).Height) + 11;

						// Save width & height of headres and ColumnType
						ColumnLefts.Add(tmpLeft);
						ColumnWidths.Add(tmpWidth);
						ColumnTypes.Add(GridCol.GetType());
						tmpLeft += tmpWidth;
					}
				}

				// Printing Current Page, Row by Row
				using (Font boldFont = new Font(dataGrid.Font, FontStyle.Bold)) {
					while (RowPos <= dataGrid.Rows.Count - 1) {
						DataGridViewRow GridRow = dataGrid.Rows[RowPos];
						if (GridRow.IsNewRow || (!PrintAllRows && !GridRow.Selected)) {
							RowPos++;
							continue;
						}

						CellHeight = GridRow.Height;

						if (tmpTop + CellHeight >= e.MarginBounds.Height + e.MarginBounds.Top) {
							DrawFooter(e, RowsPerPage);
							NewPage = true;
							PageNo++;
							e.HasMorePages = true;
							return;
						} else {
							if (NewPage) {
								// Draw Header
								e.Graphics.DrawString(PrintTitle, boldFont, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
								e.Graphics.MeasureString(PrintTitle, boldFont, e.MarginBounds.Width).Height - 13);

								string s = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();

								e.Graphics.DrawString(s, boldFont, Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
										e.Graphics.MeasureString(s, boldFont, e.MarginBounds.Width).Width), e.MarginBounds.Top -
										e.Graphics.MeasureString(PrintTitle, boldFont, e.MarginBounds.Width).Height - 13);

								// Draw Columns
								tmpTop = e.MarginBounds.Top;
								i = 0;
								foreach (DataGridViewColumn GridCol in dataGrid.Columns) {
									if (!GridCol.Visible || !AvailableColumns.Contains(GridCol.HeaderText))
										continue;

									e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(ColumnLefts[i], tmpTop, ColumnWidths[i], HeaderHeight));

									e.Graphics.DrawRectangle(Pens.Black, new Rectangle(ColumnLefts[i], tmpTop, ColumnWidths[i], HeaderHeight));
									using (SolidBrush solid = new SolidBrush(GridCol.InheritedStyle.ForeColor)) {
										e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font, solid,
											new RectangleF(ColumnLefts[i], tmpTop, ColumnWidths[i], HeaderHeight), StrFormat);
									}
									i++;
								}
								NewPage = false;
								tmpTop += HeaderHeight;
							}
							// Draw Columns Contents
							i = 0;
							foreach (DataGridViewCell Cel in GridRow.Cells) {
								if (!Cel.OwningColumn.Visible || !AvailableColumns.Contains(Cel.OwningColumn.HeaderText))
									continue;
								// For the TextBox Column
								if (ColumnTypes[i].Name == "DataGridViewTextBoxColumn" || ColumnTypes[i].Name == "DataGridViewLinkColumn") {
									using (SolidBrush solid = new SolidBrush(Cel.InheritedStyle.ForeColor)) {
										e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font, solid,
										new RectangleF(ColumnLefts[i], tmpTop, ColumnWidths[i], CellHeight), StrFormat);
									}
								}
								// For the Button Column
								else if (ColumnTypes[i].Name == "DataGridViewButtonColumn") {
									CellButton.Text = Cel.Value.ToString();
									CellButton.Size = new Size(ColumnWidths[i], CellHeight);
									using (Bitmap bmp = new Bitmap(CellButton.Width, CellButton.Height)) {
										CellButton.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
										e.Graphics.DrawImage(bmp, new Point(ColumnLefts[i], tmpTop));
									}
								}
								// For the CheckBox Column
								else if (ColumnTypes[i].Name == "DataGridViewCheckBoxColumn") {
									CellCheckBox.Size = new Size(14, 14);
									CellCheckBox.Checked = (bool) Cel.Value;
									using (Bitmap bmp = new Bitmap(ColumnWidths[i], CellHeight)) {
										using (Graphics tmpGraphics = Graphics.FromImage(bmp)) {
											tmpGraphics.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
											CellCheckBox.DrawToBitmap(bmp, new Rectangle((bmp.Width - CellCheckBox.Width) / 2,
												(bmp.Height - CellCheckBox.Height) / 2, CellCheckBox.Width, CellCheckBox.Height));
											e.Graphics.DrawImage(bmp, new Point(ColumnLefts[i], tmpTop));
										}
									}
								}
								// For the ComboBox Column
								else if (ColumnTypes[i].Name == "DataGridViewComboBoxColumn") {
									CellComboBox.Size = new Size(ColumnWidths[i], CellHeight);
									using (Bitmap bmp = new Bitmap(CellComboBox.Width, CellComboBox.Height)) {
										CellComboBox.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
										e.Graphics.DrawImage(bmp, new Point(ColumnLefts[i], tmpTop));
										using (SolidBrush solid = new SolidBrush(Cel.InheritedStyle.ForeColor)) {
											e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font, solid,
											new RectangleF(ColumnLefts[i] + 1, tmpTop, ColumnWidths[i] - 16, CellHeight), StrFormatComboBox);
										}
									}
								}
								// For the Image Column
								else if (ColumnTypes[i].Name == "DataGridViewImageColumn") {
									Rectangle CelSize = new Rectangle(ColumnLefts[i], tmpTop, ColumnWidths[i], CellHeight);
									Size ImgSize = ((Image) (Cel.FormattedValue)).Size;
									e.Graphics.DrawImage((Image) Cel.FormattedValue, new Rectangle(ColumnLefts[i] + (CelSize.Width - ImgSize.Width) / 2,
										tmpTop + (CelSize.Height - ImgSize.Height) / 2, ((Image) (Cel.FormattedValue)).Width, ((Image) (Cel.FormattedValue)).Height));

								}
								// Drawing Cells Borders 
								e.Graphics.DrawRectangle(Pens.Black, new Rectangle(ColumnLefts[i], tmpTop, ColumnWidths[i], CellHeight));
								i++;
							}
							tmpTop += CellHeight;
						}

						RowPos++;
						// For the first page it calculates Rows per Page
						if (PageNo == 1)
							RowsPerPage++;
					}
				}

				if (RowsPerPage != 0) {
					// Write Footer (Page Number)
					DrawFooter(e, RowsPerPage);
					e.HasMorePages = false;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void DrawFooter(PrintPageEventArgs e, int RowsPerPage) {
			int cnt;
			// Detemining rows number to print
			if (PrintAllRows) {
				if (dataGrid.Rows[dataGrid.Rows.Count - 1].IsNewRow)
					cnt = dataGrid.Rows.Count - 2; // When the DataGridView doesn't allow adding rows
				else
					cnt = dataGrid.Rows.Count - 1; // When the DataGridView allows adding rows
			} else
				cnt = dataGrid.SelectedRows.Count;

			// Writing the Page Number on the Bottom of Page
			string PageNum = PageNo + " of " + Math.Ceiling((double) cnt / RowsPerPage);
			e.Graphics.DrawString(PageNum, dataGrid.Font, Brushes.Black,
				e.MarginBounds.Left + (e.MarginBounds.Width -
				e.Graphics.MeasureString(PageNum, dataGrid.Font,
				e.MarginBounds.Width).Width) / 2, e.MarginBounds.Top +
				e.MarginBounds.Height + 31);
		}

		/// <summary>
		/// Disposes of the resources used by the DataGridViewPrinter.
		/// </summary>
		~DataGridViewPrinter() {
			Dispose();
		}

		/// <summary>
		/// Disposes of the resources used by the DataGridViewPrinter.
		/// </summary>
		public void Dispose() {
			if (printDoc != null) {
				printDoc.Dispose();
				printDoc = null;
			}
			if (dialog != null) {
				dialog.Dispose();
				dialog = null;
			}
			if (CellButton != null) {
				CellButton.Dispose();
				CellButton = null;
			}
			if (CellCheckBox != null) {
				CellCheckBox.Dispose();
				CellCheckBox = null;
			}
			if (CellComboBox != null) {
				CellComboBox.Dispose();
				CellComboBox = null;
			}
			GC.SuppressFinalize(this);
		}
	}
}