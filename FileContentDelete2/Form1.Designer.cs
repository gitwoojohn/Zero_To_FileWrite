namespace FileContentDelete
{
    partial class Form
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView = new System.Windows.Forms.ListView();
            this.FileList = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnFileSelect = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnErase = new System.Windows.Forms.Button();
            this.btnListBoxClear = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.선택파일삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.파일속성변경ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(13, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(409, 321);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "선택 파일 목록";
            // 
            // listView
            // 
            this.listView.AllowDrop = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileList,
            this.Size});
            this.listView.FullRowSelect = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.Location = new System.Drawing.Point(7, 30);
            this.listView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView.Name = "listView";
            this.listView.Scrollable = false;
            this.listView.Size = new System.Drawing.Size(395, 283);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_DragDrop);
            this.listView.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_DragEnter);
            this.listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseClick);
            // 
            // FileList
            // 
            this.FileList.Text = "파일 이름";
            this.FileList.Width = 297;
            // 
            // Size
            // 
            this.Size.Text = "파일 크기(KB)";
            this.Size.Width = 100;
            // 
            // btnFileSelect
            // 
            this.btnFileSelect.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFileSelect.Location = new System.Drawing.Point(17, 409);
            this.btnFileSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFileSelect.Name = "btnFileSelect";
            this.btnFileSelect.Size = new System.Drawing.Size(113, 42);
            this.btnFileSelect.TabIndex = 2;
            this.btnFileSelect.Text = "파일 선택";
            this.btnFileSelect.UseVisualStyleBackColor = true;
            this.btnFileSelect.Click += new System.EventHandler(this.btnFileSelect_Click);
            // 
            // btnErase
            // 
            this.btnErase.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnErase.Location = new System.Drawing.Point(161, 409);
            this.btnErase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(113, 42);
            this.btnErase.TabIndex = 3;
            this.btnErase.Text = "Zero Fill";
            this.btnErase.UseVisualStyleBackColor = true;
            this.btnErase.Click += new System.EventHandler(this.btnErase_Click);
            // 
            // btnListBoxClear
            // 
            this.btnListBoxClear.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnListBoxClear.Location = new System.Drawing.Point(305, 409);
            this.btnListBoxClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnListBoxClear.Name = "btnListBoxClear";
            this.btnListBoxClear.Size = new System.Drawing.Size(113, 42);
            this.btnListBoxClear.TabIndex = 4;
            this.btnListBoxClear.Text = "파일 삭제";
            this.btnListBoxClear.UseVisualStyleBackColor = true;
            this.btnListBoxClear.Click += new System.EventHandler(this.btnListBoxClear_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.선택파일삭제ToolStripMenuItem,
            this.toolStripSeparator1,
            this.파일속성변경ToolStripMenuItem});
            this.contextMenuStrip.Name = "ContextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(179, 58);
            this.contextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextStripMenu_ItemClicked);
            // 
            // 선택파일삭제ToolStripMenuItem
            // 
            this.선택파일삭제ToolStripMenuItem.Name = "선택파일삭제ToolStripMenuItem";
            this.선택파일삭제ToolStripMenuItem.Size = new System.Drawing.Size(178, 24);
            this.선택파일삭제ToolStripMenuItem.Text = "선택 파일 삭제";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
            // 
            // 파일속성변경ToolStripMenuItem
            // 
            this.파일속성변경ToolStripMenuItem.Name = "파일속성변경ToolStripMenuItem";
            this.파일속성변경ToolStripMenuItem.Size = new System.Drawing.Size(178, 24);
            this.파일속성변경ToolStripMenuItem.Text = "파일 속성 변경";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(8, 22);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(394, 23);
            this.progressBar.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(12, 343);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(410, 57);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "진행 상태";
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(434, 460);
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnListBoxClear);
            this.Controls.Add(this.btnErase);
            this.Controls.Add(this.btnFileSelect);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "File Content Erase";
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader FileList;
        private new System.Windows.Forms.ColumnHeader Size;
        private System.Windows.Forms.Button btnFileSelect;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnListBoxClear;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 선택파일삭제ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 파일속성변경ToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

