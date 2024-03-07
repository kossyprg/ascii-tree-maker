namespace AsciiTreeMaker
{
    partial class Mainform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.treeView = new System.Windows.Forms.TreeView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.AddChildNode_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.AddBrotherNode_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SwapPreviousNode_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SwapNextNode_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveSelectedNode_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExpandTreeView_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.CollapseTreeView_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ConvertTreeViewToText_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.File_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateNewTreeView_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenXMLFile_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveXMLFile_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsXMLFile_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Settings_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Exit_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateFromFolder_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.treeView.LabelEdit = true;
            this.treeView.Location = new System.Drawing.Point(9, 76);
            this.treeView.Margin = new System.Windows.Forms.Padding(2);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(583, 479);
            this.treeView.TabIndex = 0;
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeView_KeyDown);
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddChildNode_ToolStripButton,
            this.AddBrotherNode_ToolStripButton,
            this.SwapPreviousNode_ToolStripButton,
            this.SwapNextNode_ToolStripButton,
            this.RemoveSelectedNode_ToolStripButton,
            this.toolStripSeparator3,
            this.ExpandTreeView_ToolStripButton,
            this.CollapseTreeView_ToolStripButton,
            this.toolStripSeparator4,
            this.ConvertTreeViewToText_ToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(600, 37);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // AddChildNode_ToolStripButton
            // 
            this.AddChildNode_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddChildNode_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("AddChildNode_ToolStripButton.Image")));
            this.AddChildNode_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddChildNode_ToolStripButton.Name = "AddChildNode_ToolStripButton";
            this.AddChildNode_ToolStripButton.Size = new System.Drawing.Size(34, 34);
            this.AddChildNode_ToolStripButton.Text = "toolStripButton1";
            this.AddChildNode_ToolStripButton.ToolTipText = "子ノードを追加";
            this.AddChildNode_ToolStripButton.Click += new System.EventHandler(this.AddChildNode_ToolStripButton_Click);
            // 
            // AddBrotherNode_ToolStripButton
            // 
            this.AddBrotherNode_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddBrotherNode_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("AddBrotherNode_ToolStripButton.Image")));
            this.AddBrotherNode_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddBrotherNode_ToolStripButton.Name = "AddBrotherNode_ToolStripButton";
            this.AddBrotherNode_ToolStripButton.Size = new System.Drawing.Size(34, 34);
            this.AddBrotherNode_ToolStripButton.Text = "toolStripButton2";
            this.AddBrotherNode_ToolStripButton.ToolTipText = "兄弟ノードを追加";
            this.AddBrotherNode_ToolStripButton.Click += new System.EventHandler(this.AddBrotherNode_ToolStripButton_Click);
            // 
            // SwapPreviousNode_ToolStripButton
            // 
            this.SwapPreviousNode_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SwapPreviousNode_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SwapPreviousNode_ToolStripButton.Image")));
            this.SwapPreviousNode_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SwapPreviousNode_ToolStripButton.Name = "SwapPreviousNode_ToolStripButton";
            this.SwapPreviousNode_ToolStripButton.Size = new System.Drawing.Size(34, 34);
            this.SwapPreviousNode_ToolStripButton.Text = "toolStripButton1";
            this.SwapPreviousNode_ToolStripButton.ToolTipText = "上へ移動";
            this.SwapPreviousNode_ToolStripButton.Click += new System.EventHandler(this.SwapPreviousNode_ToolStripButton_Click);
            // 
            // SwapNextNode_ToolStripButton
            // 
            this.SwapNextNode_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SwapNextNode_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SwapNextNode_ToolStripButton.Image")));
            this.SwapNextNode_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SwapNextNode_ToolStripButton.Name = "SwapNextNode_ToolStripButton";
            this.SwapNextNode_ToolStripButton.Size = new System.Drawing.Size(34, 34);
            this.SwapNextNode_ToolStripButton.Text = "swap";
            this.SwapNextNode_ToolStripButton.ToolTipText = "下へ移動";
            this.SwapNextNode_ToolStripButton.Click += new System.EventHandler(this.SwapNextNode_ToolStripButton_Click);
            // 
            // RemoveSelectedNode_ToolStripButton
            // 
            this.RemoveSelectedNode_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveSelectedNode_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveSelectedNode_ToolStripButton.Image")));
            this.RemoveSelectedNode_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveSelectedNode_ToolStripButton.Name = "RemoveSelectedNode_ToolStripButton";
            this.RemoveSelectedNode_ToolStripButton.Size = new System.Drawing.Size(34, 34);
            this.RemoveSelectedNode_ToolStripButton.Text = "toolStripButton1";
            this.RemoveSelectedNode_ToolStripButton.ToolTipText = "ノードを削除";
            this.RemoveSelectedNode_ToolStripButton.Click += new System.EventHandler(this.RemoveSelectedNode_ToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 37);
            // 
            // ExpandTreeView_ToolStripButton
            // 
            this.ExpandTreeView_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ExpandTreeView_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ExpandTreeView_ToolStripButton.Image")));
            this.ExpandTreeView_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExpandTreeView_ToolStripButton.Name = "ExpandTreeView_ToolStripButton";
            this.ExpandTreeView_ToolStripButton.Size = new System.Drawing.Size(34, 34);
            this.ExpandTreeView_ToolStripButton.Text = "すべて展開";
            this.ExpandTreeView_ToolStripButton.Click += new System.EventHandler(this.ExpandTreeView_ToolStripButton_Click);
            // 
            // CollapseTreeView_ToolStripButton
            // 
            this.CollapseTreeView_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CollapseTreeView_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("CollapseTreeView_ToolStripButton.Image")));
            this.CollapseTreeView_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CollapseTreeView_ToolStripButton.Name = "CollapseTreeView_ToolStripButton";
            this.CollapseTreeView_ToolStripButton.Size = new System.Drawing.Size(34, 34);
            this.CollapseTreeView_ToolStripButton.Text = "すべて折りたたむ";
            this.CollapseTreeView_ToolStripButton.Click += new System.EventHandler(this.CollapseTreeView_ToolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 37);
            // 
            // ConvertTreeViewToText_ToolStripButton
            // 
            this.ConvertTreeViewToText_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ConvertTreeViewToText_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ConvertTreeViewToText_ToolStripButton.Image")));
            this.ConvertTreeViewToText_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ConvertTreeViewToText_ToolStripButton.Name = "ConvertTreeViewToText_ToolStripButton";
            this.ConvertTreeViewToText_ToolStripButton.Size = new System.Drawing.Size(34, 34);
            this.ConvertTreeViewToText_ToolStripButton.Text = "toolStripButton3";
            this.ConvertTreeViewToText_ToolStripButton.ToolTipText = "樹形図をテキスト形式へ変換";
            this.ConvertTreeViewToText_ToolStripButton.Click += new System.EventHandler(this.ConvertTreeViewToText_ToolStripButton_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(600, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // File_ToolStripMenuItem
            // 
            this.File_ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.File_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateNewTreeView_ToolStripMenuItem,
            this.OpenXMLFile_ToolStripMenuItem,
            this.CreateFromFolder_ToolStripMenuItem,
            this.SaveXMLFile_ToolStripMenuItem,
            this.SaveAsXMLFile_ToolStripMenuItem,
            this.toolStripSeparator1,
            this.Settings_ToolStripMenuItem,
            this.toolStripSeparator2,
            this.Exit_ToolStripMenuItem});
            this.File_ToolStripMenuItem.Name = "File_ToolStripMenuItem";
            this.File_ToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.File_ToolStripMenuItem.Text = "ファイル";
            // 
            // CreateNewTreeView_ToolStripMenuItem
            // 
            this.CreateNewTreeView_ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CreateNewTreeView_ToolStripMenuItem.Name = "CreateNewTreeView_ToolStripMenuItem";
            this.CreateNewTreeView_ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.CreateNewTreeView_ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.CreateNewTreeView_ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.CreateNewTreeView_ToolStripMenuItem.Text = "新規作成";
            this.CreateNewTreeView_ToolStripMenuItem.Click += new System.EventHandler(this.CreateNewTreeView_ToolStripMenuItem_Click);
            // 
            // OpenXMLFile_ToolStripMenuItem
            // 
            this.OpenXMLFile_ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OpenXMLFile_ToolStripMenuItem.Name = "OpenXMLFile_ToolStripMenuItem";
            this.OpenXMLFile_ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenXMLFile_ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.OpenXMLFile_ToolStripMenuItem.Text = "開く";
            this.OpenXMLFile_ToolStripMenuItem.Click += new System.EventHandler(this.OpenXMLFile_ToolStripMenuItem_Click);
            // 
            // SaveXMLFile_ToolStripMenuItem
            // 
            this.SaveXMLFile_ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SaveXMLFile_ToolStripMenuItem.Name = "SaveXMLFile_ToolStripMenuItem";
            this.SaveXMLFile_ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveXMLFile_ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.SaveXMLFile_ToolStripMenuItem.Text = "保存";
            this.SaveXMLFile_ToolStripMenuItem.Click += new System.EventHandler(this.SaveXMLFile_ToolStripMenuItem_Click);
            // 
            // SaveAsXMLFile_ToolStripMenuItem
            // 
            this.SaveAsXMLFile_ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SaveAsXMLFile_ToolStripMenuItem.Name = "SaveAsXMLFile_ToolStripMenuItem";
            this.SaveAsXMLFile_ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAsXMLFile_ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.SaveAsXMLFile_ToolStripMenuItem.Text = "名前を付けて保存";
            this.SaveAsXMLFile_ToolStripMenuItem.Click += new System.EventHandler(this.SaveAsXMLFile_ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(254, 6);
            // 
            // Settings_ToolStripMenuItem
            // 
            this.Settings_ToolStripMenuItem.Name = "Settings_ToolStripMenuItem";
            this.Settings_ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.Settings_ToolStripMenuItem.Text = "設定";
            this.Settings_ToolStripMenuItem.Click += new System.EventHandler(this.Settings_ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(254, 6);
            // 
            // Exit_ToolStripMenuItem
            // 
            this.Exit_ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Exit_ToolStripMenuItem.Name = "Exit_ToolStripMenuItem";
            this.Exit_ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.Exit_ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.Exit_ToolStripMenuItem.Text = "終了";
            this.Exit_ToolStripMenuItem.Click += new System.EventHandler(this.Exit_ToolStripMenuItem_Click);
            // 
            // CreateFromFolder_ToolStripMenuItem
            // 
            this.CreateFromFolder_ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CreateFromFolder_ToolStripMenuItem.Name = "CreateFromFolder_ToolStripMenuItem";
            this.CreateFromFolder_ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.CreateFromFolder_ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.CreateFromFolder_ToolStripMenuItem.Text = "フォルダから作成";
            this.CreateFromFolder_ToolStripMenuItem.Click += new System.EventHandler(this.CreateFromFolder_ToolStripMenuItem_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 564);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.treeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Mainform";
            this.Text = "アスキーツリーメーカー";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Mainform_FormClosing);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton AddChildNode_ToolStripButton;
        private System.Windows.Forms.ToolStripButton AddBrotherNode_ToolStripButton;
        private System.Windows.Forms.ToolStripButton ConvertTreeViewToText_ToolStripButton;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem File_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateNewTreeView_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenXMLFile_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsXMLFile_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton RemoveSelectedNode_ToolStripButton;
        private System.Windows.Forms.ToolStripButton SwapNextNode_ToolStripButton;
        private System.Windows.Forms.ToolStripButton SwapPreviousNode_ToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem Exit_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveXMLFile_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Settings_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ExpandTreeView_ToolStripButton;
        private System.Windows.Forms.ToolStripButton CollapseTreeView_ToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem CreateFromFolder_ToolStripMenuItem;
    }
}

