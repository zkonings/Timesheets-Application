<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.TimeSheetTBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.deleteRecordButton = New System.Windows.Forms.Button()
        Me.addTimeButton = New System.Windows.Forms.Button()
        Me.employeeNameCb = New System.Windows.Forms.ComboBox()
        Me.EmployeeTBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.logoPictureBox = New System.Windows.Forms.PictureBox()
        Me.startTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.endTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.AntonTimesheetBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.refreshDataButton = New System.Windows.Forms.Button()
        Me.dateLabel = New System.Windows.Forms.Label()
        Me.endTimeLabel = New System.Windows.Forms.Label()
        Me.startTimeLabel = New System.Windows.Forms.Label()
        Me.storeLabel2 = New System.Windows.Forms.Label()
        Me.storeLabel1 = New System.Windows.Forms.Label()
        Me.nameLabel = New System.Windows.Forms.Label()
        Me.storeCb2 = New System.Windows.Forms.ComboBox()
        Me.storeCb1 = New System.Windows.Forms.ComboBox()
        Me.StoreTBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TravelTBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DirectoryEntry1 = New System.DirectoryServices.DirectoryEntry()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.WarningLogRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dateDownButton = New System.Windows.Forms.Button()
        Me.dateUpButton = New System.Windows.Forms.Button()
        Me.ShowDeletedCheckBox = New System.Windows.Forms.CheckBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ComboBox4 = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TimeSheetTBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmployeeTBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AntonTimesheetBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StoreTBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TravelTBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(8, 282)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(4)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 62
        Me.DataGridView1.RowTemplate.Height = 28
        Me.DataGridView1.Size = New System.Drawing.Size(2327, 504)
        Me.DataGridView1.TabIndex = 0
        '
        'deleteRecordButton
        '
        Me.deleteRecordButton.Location = New System.Drawing.Point(1834, 153)
        Me.deleteRecordButton.Margin = New System.Windows.Forms.Padding(4)
        Me.deleteRecordButton.Name = "deleteRecordButton"
        Me.deleteRecordButton.Size = New System.Drawing.Size(205, 72)
        Me.deleteRecordButton.TabIndex = 12
        Me.deleteRecordButton.Text = "Delete Record"
        Me.deleteRecordButton.UseVisualStyleBackColor = True
        '
        'addTimeButton
        '
        Me.addTimeButton.Location = New System.Drawing.Point(1562, 153)
        Me.addTimeButton.Margin = New System.Windows.Forms.Padding(4)
        Me.addTimeButton.Name = "addTimeButton"
        Me.addTimeButton.Size = New System.Drawing.Size(205, 72)
        Me.addTimeButton.TabIndex = 7
        Me.addTimeButton.Text = "Add Time"
        Me.addTimeButton.UseVisualStyleBackColor = True
        '
        'employeeNameCb
        '
        Me.employeeNameCb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.employeeNameCb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.employeeNameCb.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.employeeNameCb.FormattingEnabled = True
        Me.employeeNameCb.ItemHeight = 55
        Me.employeeNameCb.Location = New System.Drawing.Point(291, 43)
        Me.employeeNameCb.Margin = New System.Windows.Forms.Padding(4)
        Me.employeeNameCb.Name = "employeeNameCb"
        Me.employeeNameCb.Size = New System.Drawing.Size(606, 63)
        Me.employeeNameCb.TabIndex = 1
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePicker1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!)
        Me.DateTimePicker1.Location = New System.Drawing.Point(929, 163)
        Me.DateTimePicker1.Margin = New System.Windows.Forms.Padding(4)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(478, 62)
        Me.DateTimePicker1.TabIndex = 6
        '
        'logoPictureBox
        '
        Me.logoPictureBox.Image = Global.Cranked_Out___Timesheets_Application.My.Resources.Resources.Logo_2021
        Me.logoPictureBox.Location = New System.Drawing.Point(43, 10)
        Me.logoPictureBox.Margin = New System.Windows.Forms.Padding(4)
        Me.logoPictureBox.Name = "logoPictureBox"
        Me.logoPictureBox.Size = New System.Drawing.Size(124, 105)
        Me.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.logoPictureBox.TabIndex = 4
        Me.logoPictureBox.TabStop = False
        '
        'startTimePicker
        '
        Me.startTimePicker.CustomFormat = "hh:mm tt"
        Me.startTimePicker.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.startTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.startTimePicker.Location = New System.Drawing.Point(291, 163)
        Me.startTimePicker.Margin = New System.Windows.Forms.Padding(4)
        Me.startTimePicker.Name = "startTimePicker"
        Me.startTimePicker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.startTimePicker.ShowUpDown = True
        Me.startTimePicker.Size = New System.Drawing.Size(279, 68)
        Me.startTimePicker.TabIndex = 4
        Me.startTimePicker.Value = New Date(2023, 7, 7, 6, 0, 0, 0)
        '
        'endTimePicker
        '
        Me.endTimePicker.CustomFormat = "hh:mm tt"
        Me.endTimePicker.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.endTimePicker.Location = New System.Drawing.Point(618, 163)
        Me.endTimePicker.Margin = New System.Windows.Forms.Padding(4)
        Me.endTimePicker.Name = "endTimePicker"
        Me.endTimePicker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.endTimePicker.ShowUpDown = True
        Me.endTimePicker.Size = New System.Drawing.Size(279, 68)
        Me.endTimePicker.TabIndex = 5
        Me.endTimePicker.Value = New Date(2023, 7, 25, 23, 59, 59, 0)
        '
        'refreshDataButton
        '
        Me.refreshDataButton.Location = New System.Drawing.Point(1834, 34)
        Me.refreshDataButton.Margin = New System.Windows.Forms.Padding(4)
        Me.refreshDataButton.Name = "refreshDataButton"
        Me.refreshDataButton.Size = New System.Drawing.Size(205, 72)
        Me.refreshDataButton.TabIndex = 8
        Me.refreshDataButton.Text = "Refresh"
        Me.refreshDataButton.UseVisualStyleBackColor = True
        '
        'dateLabel
        '
        Me.dateLabel.AutoSize = True
        Me.dateLabel.Location = New System.Drawing.Point(924, 135)
        Me.dateLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.dateLabel.Name = "dateLabel"
        Me.dateLabel.Size = New System.Drawing.Size(63, 25)
        Me.dateLabel.TabIndex = 5
        Me.dateLabel.Text = "Date:"
        '
        'endTimeLabel
        '
        Me.endTimeLabel.AutoSize = True
        Me.endTimeLabel.Location = New System.Drawing.Point(613, 135)
        Me.endTimeLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.endTimeLabel.Name = "endTimeLabel"
        Me.endTimeLabel.Size = New System.Drawing.Size(109, 25)
        Me.endTimeLabel.TabIndex = 5
        Me.endTimeLabel.Text = "End Time:"
        '
        'startTimeLabel
        '
        Me.startTimeLabel.AutoSize = True
        Me.startTimeLabel.Location = New System.Drawing.Point(292, 135)
        Me.startTimeLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.startTimeLabel.Name = "startTimeLabel"
        Me.startTimeLabel.Size = New System.Drawing.Size(116, 25)
        Me.startTimeLabel.TabIndex = 5
        Me.startTimeLabel.Text = "Start Time:"
        '
        'storeLabel2
        '
        Me.storeLabel2.AutoSize = True
        Me.storeLabel2.Location = New System.Drawing.Point(1360, 10)
        Me.storeLabel2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.storeLabel2.Name = "storeLabel2"
        Me.storeLabel2.Size = New System.Drawing.Size(148, 25)
        Me.storeLabel2.TabIndex = 5
        Me.storeLabel2.Text = "Second Store:"
        '
        'storeLabel1
        '
        Me.storeLabel1.AutoSize = True
        Me.storeLabel1.Location = New System.Drawing.Point(924, 10)
        Me.storeLabel1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.storeLabel1.Name = "storeLabel1"
        Me.storeLabel1.Size = New System.Drawing.Size(117, 25)
        Me.storeLabel1.TabIndex = 5
        Me.storeLabel1.Text = "First Store:"
        '
        'nameLabel
        '
        Me.nameLabel.AutoSize = True
        Me.nameLabel.Location = New System.Drawing.Point(292, 10)
        Me.nameLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.nameLabel.Name = "nameLabel"
        Me.nameLabel.Size = New System.Drawing.Size(175, 25)
        Me.nameLabel.TabIndex = 5
        Me.nameLabel.Text = "Employee Name:"
        '
        'storeCb2
        '
        Me.storeCb2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.storeCb2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.storeCb2.DisplayMember = "Store"
        Me.storeCb2.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.storeCb2.FormattingEnabled = True
        Me.storeCb2.ItemHeight = 55
        Me.storeCb2.Location = New System.Drawing.Point(1365, 43)
        Me.storeCb2.Margin = New System.Windows.Forms.Padding(4)
        Me.storeCb2.Name = "storeCb2"
        Me.storeCb2.Size = New System.Drawing.Size(402, 63)
        Me.storeCb2.TabIndex = 3
        Me.storeCb2.ValueMember = "Store"
        '
        'storeCb1
        '
        Me.storeCb1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.storeCb1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.storeCb1.DisplayMember = "Store"
        Me.storeCb1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.storeCb1.FormattingEnabled = True
        Me.storeCb1.ItemHeight = 55
        Me.storeCb1.Location = New System.Drawing.Point(929, 43)
        Me.storeCb1.Margin = New System.Windows.Forms.Padding(4)
        Me.storeCb1.MaxDropDownItems = 12
        Me.storeCb1.Name = "storeCb1"
        Me.storeCb1.Size = New System.Drawing.Size(402, 63)
        Me.storeCb1.TabIndex = 2
        Me.storeCb1.ValueMember = "Store"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(2361, 1070)
        Me.TabControl1.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.WarningLogRichTextBox)
        Me.TabPage1.Controls.Add(Me.Panel2)
        Me.TabPage1.Controls.Add(Me.DataGridView1)
        Me.TabPage1.Location = New System.Drawing.Point(8, 39)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(2345, 1023)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Timesheets"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'WarningLogRichTextBox
        '
        Me.WarningLogRichTextBox.Location = New System.Drawing.Point(8, 794)
        Me.WarningLogRichTextBox.Name = "WarningLogRichTextBox"
        Me.WarningLogRichTextBox.Size = New System.Drawing.Size(2327, 222)
        Me.WarningLogRichTextBox.TabIndex = 2
        Me.WarningLogRichTextBox.Text = ""
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.dateDownButton)
        Me.Panel2.Controls.Add(Me.dateUpButton)
        Me.Panel2.Controls.Add(Me.ShowDeletedCheckBox)
        Me.Panel2.Controls.Add(Me.nameLabel)
        Me.Panel2.Controls.Add(Me.dateLabel)
        Me.Panel2.Controls.Add(Me.employeeNameCb)
        Me.Panel2.Controls.Add(Me.endTimeLabel)
        Me.Panel2.Controls.Add(Me.storeCb1)
        Me.Panel2.Controls.Add(Me.startTimeLabel)
        Me.Panel2.Controls.Add(Me.storeCb2)
        Me.Panel2.Controls.Add(Me.storeLabel2)
        Me.Panel2.Controls.Add(Me.refreshDataButton)
        Me.Panel2.Controls.Add(Me.storeLabel1)
        Me.Panel2.Controls.Add(Me.DateTimePicker1)
        Me.Panel2.Controls.Add(Me.addTimeButton)
        Me.Panel2.Controls.Add(Me.endTimePicker)
        Me.Panel2.Controls.Add(Me.logoPictureBox)
        Me.Panel2.Controls.Add(Me.deleteRecordButton)
        Me.Panel2.Controls.Add(Me.startTimePicker)
        Me.Panel2.Location = New System.Drawing.Point(8, 6)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(2327, 269)
        Me.Panel2.TabIndex = 1
        '
        'dateDownButton
        '
        Me.dateDownButton.Location = New System.Drawing.Point(1471, 163)
        Me.dateDownButton.Name = "dateDownButton"
        Me.dateDownButton.Size = New System.Drawing.Size(45, 68)
        Me.dateDownButton.TabIndex = 9
        Me.dateDownButton.Text = "-"
        Me.dateDownButton.UseVisualStyleBackColor = True
        '
        'dateUpButton
        '
        Me.dateUpButton.Location = New System.Drawing.Point(1420, 162)
        Me.dateUpButton.Name = "dateUpButton"
        Me.dateUpButton.Size = New System.Drawing.Size(45, 69)
        Me.dateUpButton.TabIndex = 9
        Me.dateUpButton.Text = "+"
        Me.dateUpButton.UseVisualStyleBackColor = True
        '
        'ShowDeletedCheckBox
        '
        Me.ShowDeletedCheckBox.AutoSize = True
        Me.ShowDeletedCheckBox.Location = New System.Drawing.Point(43, 153)
        Me.ShowDeletedCheckBox.Name = "ShowDeletedCheckBox"
        Me.ShowDeletedCheckBox.Size = New System.Drawing.Size(177, 54)
        Me.ShowDeletedCheckBox.TabIndex = 7
        Me.ShowDeletedCheckBox.Text = "Show Deleted" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " Records"
        Me.ShowDeletedCheckBox.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel1)
        Me.TabPage2.Controls.Add(Me.DataGridView2)
        Me.TabPage2.Controls.Add(Me.Button3)
        Me.TabPage2.Controls.Add(Me.PictureBox1)
        Me.TabPage2.Controls.Add(Me.Button1)
        Me.TabPage2.Controls.Add(Me.Panel3)
        Me.TabPage2.Location = New System.Drawing.Point(8, 39)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(2345, 1023)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Expense Sheets"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckBox2)
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.ComboBox4)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.TextBox2)
        Me.Panel1.Location = New System.Drawing.Point(513, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(481, 379)
        Me.Panel1.TabIndex = 16
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(281, 204)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(171, 29)
        Me.CheckBox2.TabIndex = 7
        Me.CheckBox2.Text = "GST Exempt:"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(26, 204)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(169, 29)
        Me.CheckBox1.TabIndex = 7
        Me.CheckBox1.Text = "PST Exempt:"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Red
        Me.Button2.Location = New System.Drawing.Point(21, 274)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(431, 84)
        Me.Button2.TabIndex = 0
        Me.Button2.Text = "Add Product to Reciept Total"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(24, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(368, 25)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Individual Item Catagory in Purchase:"
        '
        'ComboBox4
        '
        Me.ComboBox4.FormattingEnabled = True
        Me.ComboBox4.Location = New System.Drawing.Point(19, 63)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(431, 33)
        Me.ComboBox4.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(24, 114)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(181, 25)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Product Net Cost:"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(19, 151)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(431, 31)
        Me.TextBox2.TabIndex = 5
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(16, 428)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersWidth = 82
        Me.DataGridView2.RowTemplate.Height = 33
        Me.DataGridView2.Size = New System.Drawing.Size(1782, 390)
        Me.DataGridView2.TabIndex = 15
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.Yellow
        Me.Button3.Location = New System.Drawing.Point(1014, 824)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(784, 84)
        Me.Button3.TabIndex = 11
        Me.Button3.Text = "Take Image of Reciept"
        Me.Button3.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(1014, 16)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(784, 379)
        Me.PictureBox1.TabIndex = 14
        Me.PictureBox1.TabStop = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Lime
        Me.Button1.Location = New System.Drawing.Point(16, 824)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(978, 84)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "Add Expense to Database"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.ComboBox2)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.RadioButton1)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.RadioButton2)
        Me.Panel3.Controls.Add(Me.DateTimePicker2)
        Me.Panel3.Controls.Add(Me.TextBox1)
        Me.Panel3.Location = New System.Drawing.Point(16, 16)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(481, 379)
        Me.Panel3.TabIndex = 13
        '
        'ComboBox2
        '
        Me.ComboBox2.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.EmployeeTBindingSource, "EmployeeName", True))
        Me.ComboBox2.DataSource = Me.EmployeeTBindingSource
        Me.ComboBox2.DisplayMember = "EmployeeName"
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(16, 225)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(431, 33)
        Me.ComboBox2.TabIndex = 1
        Me.ComboBox2.ValueMember = "EmployeeName"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 197)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(113, 25)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Employee:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 273)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 25)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Supplier:"
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(23, 146)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(186, 29)
        Me.RadioButton1.TabIndex = 4
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Company Card"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 98)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(187, 25)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Purchase Method:"
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Checked = True
        Me.RadioButton2.Location = New System.Drawing.Point(257, 146)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(190, 29)
        Me.RadioButton2.TabIndex = 4
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Employee Card"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Location = New System.Drawing.Point(23, 33)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(404, 31)
        Me.DateTimePicker2.TabIndex = 2
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(16, 301)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(431, 31)
        Me.TextBox1.TabIndex = 5
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(2372, 1081)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form1"
        Me.Text = "Cranked Out - Manager Console"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TimeSheetTBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmployeeTBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.logoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AntonTimesheetBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StoreTBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TravelTBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents TimeSheetTBindingSource As BindingSource
    Friend WithEvents previousRecordButton As Button
    Friend WithEvents nextRecordButton As Button
    Friend WithEvents deleteRecordButton As Button
    Friend WithEvents addTimeButton As Button
    Friend WithEvents employeeNameCb As ComboBox
    Friend WithEvents EmployeeTBindingSource As BindingSource
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents logoPictureBox As PictureBox
    Friend WithEvents startTimePicker As DateTimePicker
    Friend WithEvents endTimePicker As DateTimePicker
    Friend WithEvents AntonTimesheetBindingSource As BindingSource
    Friend WithEvents TimeSheetIDDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents EmployeeNameDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents LocationDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents SecondLocationDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DateDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents StartTimeDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents EndTimeDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents refreshDataButton As Button
    Friend WithEvents dateLabel As Label
    Friend WithEvents endTimeLabel As Label
    Friend WithEvents startTimeLabel As Label
    Friend WithEvents storeLabel2 As Label
    Friend WithEvents storeLabel1 As Label
    Friend WithEvents nameLabel As Label
    Friend WithEvents storeCb2 As ComboBox
    Friend WithEvents storeCb1 As ComboBox
    Friend WithEvents TravelTBindingSource As BindingSource
    Friend WithEvents StoreTBindingSource As BindingSource
    Friend WithEvents StoreTBindingSource1 As BindingSource
    Friend WithEvents DirectoryEntry1 As DirectoryServices.DirectoryEntry
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Panel1 As Panel
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents ComboBox4 As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents Button3 As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents DateTimePicker2 As DateTimePicker
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents dateDownButton As Button
    Friend WithEvents dateUpButton As Button
    Friend WithEvents ShowDeletedCheckBox As CheckBox
    Friend WithEvents WarningLogRichTextBox As RichTextBox
End Class
