using System;
using System.Drawing;
using System.Windows.Forms;

namespace FormUnitTest {
	public class TestForm : StyledForm {
		private StyledButton styledButton1;
		private StyledCheckBox styledCheckBox1;
		private StyledMenuStrip styledMenuStrip1;
		private StyledItem styledItem1;
		private StyledItem styledItem5;
		private StyledItem styledItem6;
		private StyledItem styledItem7;
		private StyledItem styledItem11;
		private StyledItem styledItem12;
		private StyledItem styledItem13;
		private StyledItem styledItem16;
		private StyledItem styledItem17;
		private StyledItem styledItem18;
		private StyledItem styledItem14;
		private StyledItem styledItem15;
		private StyledItem styledItem8;
		private StyledItem styledItem9;
		private StyledItem styledItem10;
		private StyledItem styledItem2;
		private StyledItem styledItem3;
		private StyledItem styledItem4;
		private StyledComboBox styledComboBox1;
		private System.ComponentModel.IContainer components;
		private StyledSlider styledSlider1;
		private SliderDialog sliderDialog1;
		private StyledSlider styledSlider2;
		private StyledCheckBox styledCheckBox2;
		private StyledContextMenu styledContextMenu1;
		private StyledItem styledItem19;
		private StyledItem styledItem21;
		private StyledItem styledItem20;
		private StyledItem styledItem22;
		private StyledItem styledItem23;
		private StyledItem styledItem26;
		private StyledItem styledItem27;
		private StyledItem styledItem28;
		private StyledItem styledItem29;
		private StyledItem styledItem30;
		private StyledItem styledItem31;
		private StyledItem styledItem24;
		private StyledItem styledItem25;
		private StyledLabel styledLabel1;
		private StyledArrowButton styledArrowButton1;

		public TestForm() {
			InitializeComponent();
			styledButton1.Renderer.Border = Color.FromArgb(150, Color.SteelBlue);
			styledButton1.Renderer.NormalInnerBorderColor = Color.FromArgb(100, Color.SteelBlue);
			styledButton1.Renderer.HoverInnerBorderColor = Color.FromArgb(150, Color.LightSteelBlue);
			styledButton1.Renderer.NormalBackgroundTop = Color.FromArgb(100, Color.SteelBlue);
			styledButton1.Renderer.NormalBackgroundBottom = ImageLib.ChangeLightness(styledButton1.Renderer.NormalBackgroundTop, -60);
			styledButton1.Renderer.HoverBackgroundTop = Color.FromArgb(150, Color.LightSteelBlue);
			styledButton1.Renderer.HoverBackgroundBottom = ImageLib.ChangeLightness(styledButton1.Renderer.HoverBackgroundTop, -60);
			styledButton1.Renderer.PressedBackgroundTop = Color.FromArgb(200, ImageLib.ChangeLightness(Color.SteelBlue, -40));
			styledButton1.Renderer.PressedBackgroundBottom = ImageLib.ChangeLightness(styledButton1.Renderer.PressedBackgroundTop, -60);
			styledArrowButton1.Renderer.CopyConfigFrom(styledButton1.Renderer);
			styledComboBox1.BackgroundImage = Properties.Resources.wood;
			styledComboBox1.Renderer.CopyConfigFrom(styledButton1.Renderer);
		}

		[STAThread]
		public static void Main() {
			MessageLoop.Run(new TestForm());
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.styledArrowButton1 = new System.Windows.Forms.StyledArrowButton();
			this.styledButton1 = new System.Windows.Forms.StyledButton();
			this.styledContextMenu1 = new System.Windows.Forms.StyledContextMenu(this.components);
			this.styledItem19 = new System.Windows.Forms.StyledItem();
			this.styledItem21 = new System.Windows.Forms.StyledItem();
			this.styledItem20 = new System.Windows.Forms.StyledItem();
			this.styledCheckBox1 = new System.Windows.Forms.StyledCheckBox();
			this.styledMenuStrip1 = new System.Windows.Forms.StyledMenuStrip();
			this.styledItem1 = new System.Windows.Forms.StyledItem();
			this.styledItem2 = new System.Windows.Forms.StyledItem();
			this.styledItem22 = new System.Windows.Forms.StyledItem();
			this.styledItem23 = new System.Windows.Forms.StyledItem();
			this.styledItem26 = new System.Windows.Forms.StyledItem();
			this.styledItem27 = new System.Windows.Forms.StyledItem();
			this.styledItem28 = new System.Windows.Forms.StyledItem();
			this.styledItem29 = new System.Windows.Forms.StyledItem();
			this.styledItem30 = new System.Windows.Forms.StyledItem();
			this.styledItem31 = new System.Windows.Forms.StyledItem();
			this.styledItem24 = new System.Windows.Forms.StyledItem();
			this.styledItem25 = new System.Windows.Forms.StyledItem();
			this.styledItem3 = new System.Windows.Forms.StyledItem();
			this.styledItem4 = new System.Windows.Forms.StyledItem();
			this.styledItem5 = new System.Windows.Forms.StyledItem();
			this.styledItem6 = new System.Windows.Forms.StyledItem();
			this.styledItem7 = new System.Windows.Forms.StyledItem();
			this.styledItem11 = new System.Windows.Forms.StyledItem();
			this.styledItem12 = new System.Windows.Forms.StyledItem();
			this.styledItem13 = new System.Windows.Forms.StyledItem();
			this.styledItem16 = new System.Windows.Forms.StyledItem();
			this.styledItem17 = new System.Windows.Forms.StyledItem();
			this.styledItem18 = new System.Windows.Forms.StyledItem();
			this.styledItem14 = new System.Windows.Forms.StyledItem();
			this.styledItem15 = new System.Windows.Forms.StyledItem();
			this.styledItem8 = new System.Windows.Forms.StyledItem();
			this.styledItem9 = new System.Windows.Forms.StyledItem();
			this.styledItem10 = new System.Windows.Forms.StyledItem();
			this.styledComboBox1 = new System.Windows.Forms.StyledComboBox();
			this.styledSlider1 = new System.Windows.Forms.StyledSlider();
			this.sliderDialog1 = new System.Windows.Forms.SliderDialog(this.components);
			this.styledSlider2 = new System.Windows.Forms.StyledSlider();
			this.styledCheckBox2 = new System.Windows.Forms.StyledCheckBox();
			this.styledLabel1 = new System.Windows.Forms.StyledLabel();
			this.styledContextMenu1.SuspendLayout();
			this.styledMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// styledArrowButton1
			// 
			this.styledArrowButton1.BackColor = System.Drawing.Color.Transparent;
			this.styledArrowButton1.BackgroundImage = global::FormUnitTest.Properties.Resources.wood;
			this.styledArrowButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.styledArrowButton1.Location = new System.Drawing.Point(7, 77);
			this.styledArrowButton1.Name = "styledArrowButton1";
			this.styledArrowButton1.Rotation = 10F;
			this.styledArrowButton1.Size = new System.Drawing.Size(393, 313);
			this.styledArrowButton1.TabIndex = 1;
			this.styledArrowButton1.Text = "styledArrowButton1";
			this.styledArrowButton1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			this.styledArrowButton1.Click += new System.EventHandler(this.styledArrowButton1_Click);
			// 
			// styledButton1
			// 
			this.styledButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.styledButton1.BackgroundImage = global::FormUnitTest.Properties.Resources.wood;
			this.styledButton1.CheckOnClick = true;
			this.styledButton1.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.styledButton1.ContextMenuStrip = this.styledContextMenu1;
			this.styledButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.styledButton1.Location = new System.Drawing.Point(394, 66);
			this.styledButton1.Name = "styledButton1";
			this.styledButton1.ShowCheckBox = true;
			this.styledButton1.Size = new System.Drawing.Size(100, 37);
			this.styledButton1.TabIndex = 2;
			this.styledButton1.Text = "styledButton1";
			this.styledButton1.UseVisualStyleBackColor = true;
			this.styledButton1.Click += new System.EventHandler(this.styledButton1_Click);
			// 
			// styledContextMenu1
			// 
			this.styledContextMenu1.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.styledItem19,
            this.styledItem20});
			this.styledContextMenu1.Name = "StyledContextMenu";
			this.styledContextMenu1.Size = new System.Drawing.Size(74, 50);
			// 
			// styledItem19
			// 
			this.styledItem19.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.styledItem21});
			this.styledItem19.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem19.Name = "StyledItem";
			this.styledItem19.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem19.Size = new System.Drawing.Size(74, 25);
			this.styledItem19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem19.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem21
			// 
			this.styledItem21.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem21.Name = "StyledItem";
			this.styledItem21.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem21.Size = new System.Drawing.Size(74, 25);
			this.styledItem21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem21.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem20
			// 
			this.styledItem20.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem20.Name = "StyledItem";
			this.styledItem20.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem20.Size = new System.Drawing.Size(74, 25);
			this.styledItem20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem20.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledCheckBox1
			// 
			this.styledCheckBox1.BackColor = System.Drawing.Color.Transparent;
			this.styledCheckBox1.BackgroundImage = global::FormUnitTest.Properties.Resources.wood;
			this.styledCheckBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.styledCheckBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.styledCheckBox1.Location = new System.Drawing.Point(444, 280);
			this.styledCheckBox1.Name = "styledCheckBox1";
			this.styledCheckBox1.Size = new System.Drawing.Size(156, 59);
			this.styledCheckBox1.TabIndex = 3;
			this.styledCheckBox1.Text = "styledCheckBox1";
			// 
			// styledMenuStrip1
			// 
			this.styledMenuStrip1.AutoSize = false;
			this.styledMenuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledMenuStrip1.ForeColor = System.Drawing.Color.Black;
			this.styledMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.styledItem1,
            this.styledItem2,
            this.styledItem3,
            this.styledItem4});
			this.styledMenuStrip1.Location = new System.Drawing.Point(4, 29);
			this.styledMenuStrip1.Name = "styledMenuStrip1";
			this.styledMenuStrip1.Size = new System.Drawing.Size(650, 24);
			this.styledMenuStrip1.TabIndex = 4;
			this.styledMenuStrip1.Text = "styledMenuStrip1";
			this.styledMenuStrip1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem1
			// 
			this.styledItem1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem1.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem1.Name = "StyledItem";
			this.styledItem1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem1.Size = new System.Drawing.Size(74, 25);
			this.styledItem1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem2
			// 
			this.styledItem2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.styledItem22,
            this.styledItem23,
            this.styledItem24,
            this.styledItem25});
			this.styledItem2.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem2.Name = "StyledItem";
			this.styledItem2.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem2.Size = new System.Drawing.Size(74, 25);
			this.styledItem2.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem22
			// 
			this.styledItem22.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem22.Name = "StyledItem";
			this.styledItem22.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem22.Size = new System.Drawing.Size(74, 25);
			this.styledItem22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem22.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem23
			// 
			this.styledItem23.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.styledItem26,
            this.styledItem27,
            this.styledItem28,
            this.styledItem29,
            this.styledItem30,
            this.styledItem31});
			this.styledItem23.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem23.Name = "StyledItem";
			this.styledItem23.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem23.Size = new System.Drawing.Size(74, 25);
			this.styledItem23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem23.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem26
			// 
			this.styledItem26.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem26.Name = "StyledItem";
			this.styledItem26.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem26.Size = new System.Drawing.Size(74, 25);
			this.styledItem26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem26.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem27
			// 
			this.styledItem27.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem27.Name = "StyledItem";
			this.styledItem27.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem27.Size = new System.Drawing.Size(74, 25);
			this.styledItem27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem27.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem28
			// 
			this.styledItem28.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem28.Name = "StyledItem";
			this.styledItem28.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem28.Size = new System.Drawing.Size(74, 25);
			this.styledItem28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem28.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem29
			// 
			this.styledItem29.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem29.Name = "StyledItem";
			this.styledItem29.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem29.Size = new System.Drawing.Size(74, 25);
			this.styledItem29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem29.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem30
			// 
			this.styledItem30.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem30.Name = "StyledItem";
			this.styledItem30.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem30.Size = new System.Drawing.Size(74, 25);
			this.styledItem30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem30.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem31
			// 
			this.styledItem31.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem31.Name = "StyledItem";
			this.styledItem31.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem31.Size = new System.Drawing.Size(74, 25);
			this.styledItem31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem31.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem24
			// 
			this.styledItem24.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem24.Name = "StyledItem";
			this.styledItem24.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem24.Size = new System.Drawing.Size(74, 25);
			this.styledItem24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem24.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem25
			// 
			this.styledItem25.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem25.Name = "StyledItem";
			this.styledItem25.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem25.Size = new System.Drawing.Size(74, 25);
			this.styledItem25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem25.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem3
			// 
			this.styledItem3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem3.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem3.Name = "StyledItem";
			this.styledItem3.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem3.Size = new System.Drawing.Size(74, 25);
			this.styledItem3.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem4
			// 
			this.styledItem4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem4.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem4.Name = "StyledItem";
			this.styledItem4.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem4.Size = new System.Drawing.Size(74, 25);
			this.styledItem4.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem5
			// 
			this.styledItem5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem5.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem5.Name = "StyledItem";
			this.styledItem5.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem5.Size = new System.Drawing.Size(74, 25);
			this.styledItem5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem5.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem6
			// 
			this.styledItem6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem6.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem6.Name = "StyledItem";
			this.styledItem6.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem6.Size = new System.Drawing.Size(74, 25);
			this.styledItem6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem6.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem7
			// 
			this.styledItem7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem7.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem7.Name = "StyledItem";
			this.styledItem7.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem7.Size = new System.Drawing.Size(74, 25);
			this.styledItem7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem7.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem11
			// 
			this.styledItem11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem11.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem11.Name = "StyledItem";
			this.styledItem11.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem11.Size = new System.Drawing.Size(74, 25);
			this.styledItem11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem11.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem12
			// 
			this.styledItem12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem12.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem12.Name = "StyledItem";
			this.styledItem12.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem12.Size = new System.Drawing.Size(74, 25);
			this.styledItem12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem12.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem13
			// 
			this.styledItem13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem13.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem13.Name = "StyledItem";
			this.styledItem13.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem13.Size = new System.Drawing.Size(74, 25);
			this.styledItem13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem13.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem16
			// 
			this.styledItem16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem16.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem16.Name = "StyledItem";
			this.styledItem16.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem16.Size = new System.Drawing.Size(74, 25);
			this.styledItem16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem16.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem17
			// 
			this.styledItem17.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem17.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem17.Name = "StyledItem";
			this.styledItem17.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem17.Size = new System.Drawing.Size(74, 25);
			this.styledItem17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem17.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem18
			// 
			this.styledItem18.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem18.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem18.Name = "StyledItem";
			this.styledItem18.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem18.Size = new System.Drawing.Size(74, 25);
			this.styledItem18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem18.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem14
			// 
			this.styledItem14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem14.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem14.Name = "StyledItem";
			this.styledItem14.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem14.Size = new System.Drawing.Size(74, 25);
			this.styledItem14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem14.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem15
			// 
			this.styledItem15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem15.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem15.Name = "StyledItem";
			this.styledItem15.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem15.Size = new System.Drawing.Size(74, 25);
			this.styledItem15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem15.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem8
			// 
			this.styledItem8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem8.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem8.Name = "StyledItem";
			this.styledItem8.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem8.Size = new System.Drawing.Size(74, 25);
			this.styledItem8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem8.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem9
			// 
			this.styledItem9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem9.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem9.Name = "StyledItem";
			this.styledItem9.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem9.Size = new System.Drawing.Size(74, 25);
			this.styledItem9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem9.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledItem10
			// 
			this.styledItem10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledItem10.MaximumSize = new System.Drawing.Size(0, 0);
			this.styledItem10.Name = "StyledItem";
			this.styledItem10.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
			this.styledItem10.Size = new System.Drawing.Size(74, 25);
			this.styledItem10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.styledItem10.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledComboBox1
			// 
			this.styledComboBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.styledComboBox1.DropDownHeight = 1;
			this.styledComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.styledComboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.styledComboBox1.FormattingEnabled = true;
			this.styledComboBox1.IntegralHeight = false;
			this.styledComboBox1.Items.AddRange(new object[] {
            "lol",
            "ayyyyyyyy",
            "Item :)"});
			this.styledComboBox1.Location = new System.Drawing.Point(334, 378);
			this.styledComboBox1.Name = "styledComboBox1";
			this.styledComboBox1.Size = new System.Drawing.Size(234, 24);
			this.styledComboBox1.TabIndex = 5;
			this.styledComboBox1.TextRenderingStyle = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			// 
			// styledSlider1
			// 
			this.styledSlider1.BackColor = System.Drawing.Color.Transparent;
			this.styledSlider1.BackgroundImage = global::FormUnitTest.Properties.Resources.wood;
			this.styledSlider1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.styledSlider1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.styledSlider1.Increment = 0F;
			this.styledSlider1.LabelPadding = 2;
			this.styledSlider1.LargeChange = 12.5F;
			this.styledSlider1.Location = new System.Drawing.Point(47, 417);
			this.styledSlider1.Name = "styledSlider1";
			this.styledSlider1.Size = new System.Drawing.Size(234, 23);
			this.styledSlider1.SmallChange = 5F;
			this.styledSlider1.TabIndex = 6;
			this.styledSlider1.Text = "styledSlider1";
			// 
			// sliderDialog1
			// 
			this.sliderDialog1.ActiveBorderOpacity = 0.75F;
			this.sliderDialog1.AllowDrop = true;
			this.sliderDialog1.BackColorOpacity = ((byte)(255));
			this.sliderDialog1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.sliderDialog1.BorderCursor = System.Windows.Forms.Cursors.Default;
			this.sliderDialog1.BorderWidth = 4;
			this.sliderDialog1.CausesValidation = false;
			this.sliderDialog1.ClientSize = new System.Drawing.Size(342, 174);
			this.sliderDialog1.EnableFullscreenOnAltEnter = false;
			this.sliderDialog1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.sliderDialog1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.sliderDialog1.InactiveBorderOpacity = 0.5F;
			this.sliderDialog1.InlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.sliderDialog1.KeyPreview = true;
			this.sliderDialog1.Location = new System.Drawing.Point(0, 0);
			this.sliderDialog1.MaximizeBox = false;
			this.sliderDialog1.MaximizeEnabled = false;
			this.sliderDialog1.MinimizeBox = false;
			this.sliderDialog1.MinimizeEnabled = false;
			this.sliderDialog1.MinimumSize = new System.Drawing.Size(200, 50);
			this.sliderDialog1.Name = "sliderDialog1";
			this.sliderDialog1.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(139)))));
			this.sliderDialog1.Padding = new System.Windows.Forms.Padding(4, 31, 4, 4);
			this.sliderDialog1.ShowIcon = false;
			this.sliderDialog1.ShowInTaskbar = false;
			this.sliderDialog1.ShowShadow = true;
			this.sliderDialog1.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.sliderDialog1.SystemMenu = null;
			this.sliderDialog1.SystemMenuStrip = null;
			this.sliderDialog1.Text = "sliderDialog1";
			this.sliderDialog1.TitleBarBadding = new System.Drawing.Size(0, 1);
			this.sliderDialog1.TitleBarHeight = 31;
			this.sliderDialog1.Visible = false;
			this.sliderDialog1.WindowCursor = System.Windows.Forms.Cursors.Default;
			// 
			// styledSlider2
			// 
			this.styledSlider2.BackColor = System.Drawing.Color.Transparent;
			this.styledSlider2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.styledSlider2.Increment = 0F;
			this.styledSlider2.LabelPadding = 2;
			this.styledSlider2.LargeChange = 12.5F;
			this.styledSlider2.Location = new System.Drawing.Point(394, 441);
			this.styledSlider2.Name = "styledSlider2";
			this.styledSlider2.Size = new System.Drawing.Size(185, 23);
			this.styledSlider2.SmallChange = 5F;
			this.styledSlider2.TabIndex = 7;
			this.styledSlider2.Text = "styledSlider2";
			// 
			// styledCheckBox2
			// 
			this.styledCheckBox2.BackColor = System.Drawing.Color.Transparent;
			this.styledCheckBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.styledCheckBox2.Location = new System.Drawing.Point(444, 228);
			this.styledCheckBox2.Name = "styledCheckBox2";
			this.styledCheckBox2.Size = new System.Drawing.Size(156, 23);
			this.styledCheckBox2.TabIndex = 8;
			this.styledCheckBox2.Text = "styledCheckBox2";
			// 
			// styledLabel1
			// 
			this.styledLabel1.AutoSize = true;
			this.styledLabel1.BackColor = System.Drawing.Color.SandyBrown;
			this.styledLabel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
			this.styledLabel1.Blur = 3;
			this.styledLabel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.styledLabel1.ForeColor = System.Drawing.Color.White;
			this.styledLabel1.Location = new System.Drawing.Point(530, 95);
			this.styledLabel1.Name = "styledLabel1";
			this.styledLabel1.RenderShadow = true;
			this.styledLabel1.ShadowOffsetX = 5F;
			this.styledLabel1.ShadowOffsetY = 5F;
			this.styledLabel1.ShadowOpacity = 1.5F;
			this.styledLabel1.Size = new System.Drawing.Size(91, 77);
			this.styledLabel1.TabIndex = 9;
			this.styledLabel1.Text = "styledLabel1\ndid\nouei\noeuioe";
			this.styledLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::FormUnitTest.Properties.Resources.sunset;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(658, 480);
			this.Controls.Add(this.styledLabel1);
			this.Controls.Add(this.styledCheckBox2);
			this.Controls.Add(this.styledSlider2);
			this.Controls.Add(this.styledSlider1);
			this.Controls.Add(this.styledComboBox1);
			this.Controls.Add(this.styledCheckBox1);
			this.Controls.Add(this.styledButton1);
			this.Controls.Add(this.styledArrowButton1);
			this.Controls.Add(this.styledMenuStrip1);
			this.MainMenuStrip = this.styledMenuStrip1;
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.styledContextMenu1.ResumeLayout(false);
			this.styledMenuStrip1.ResumeLayout(false);
			this.styledMenuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void styledArrowButton1_Click(object sender, EventArgs e) {
			styledArrowButton1.Size += new Size(10, 10);
		}

		private void styledButton1_Click(object sender, EventArgs e) {
			styledButton1.Size += new Size(10, 10);
		}
	}
}
