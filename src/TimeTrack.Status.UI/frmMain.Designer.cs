namespace TimeTrack.Status.UI
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblCurrentTrackingIdentifier = new System.Windows.Forms.Label();
            this.tmrUpdateStatus = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCurrentDuration = new System.Windows.Forms.Label();
            this.lblTotalEventCountTodayForLastEvent = new System.Windows.Forms.Label();
            this.lblTotalDurationTodayForLastEvent = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblLastTrackedIdentifier = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxMenuStripTrayIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepTopmostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWhenMinimizedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableIdleReminderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableTrackingReminderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.testNotificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTotalDurationToday = new System.Windows.Forms.Label();
            this.lblTotalEventCountToday = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTotalDurationWeek = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalEventCountWeek = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTotalActivityCountToday = new System.Windows.Forms.Label();
            this.lblTotalActivityCountWeek = new System.Windows.Forms.Label();
            this.lblTotalEventCountThisWeekForLastEvent = new System.Windows.Forms.Label();
            this.picNotificationsDisabled = new System.Windows.Forms.PictureBox();
            this.picIdleWarning = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalDurationThisWeekForLastEvent = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrStatusbarInfoTimeout = new System.Windows.Forms.Timer(this.components);
            this.ttDurationLabels = new System.Windows.Forms.ToolTip(this.components);
            this.ctxMenuStripTrayIcon.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNotificationsDisabled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIdleWarning)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCurrentTrackingIdentifier
            // 
            this.lblCurrentTrackingIdentifier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentTrackingIdentifier.AutoEllipsis = true;
            this.lblCurrentTrackingIdentifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTrackingIdentifier.Location = new System.Drawing.Point(196, 34);
            this.lblCurrentTrackingIdentifier.Name = "lblCurrentTrackingIdentifier";
            this.lblCurrentTrackingIdentifier.Size = new System.Drawing.Size(226, 25);
            this.lblCurrentTrackingIdentifier.TabIndex = 0;
            this.lblCurrentTrackingIdentifier.Text = "label1";
            this.lblCurrentTrackingIdentifier.Click += new System.EventHandler(this.Label_Click);
            // 
            // tmrUpdateStatus
            // 
            this.tmrUpdateStatus.Interval = 1000;
            this.tmrUpdateStatus.Tick += new System.EventHandler(this.tmrUpdateStatus_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tracking:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Duration:";
            // 
            // lblCurrentDuration
            // 
            this.lblCurrentDuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentDuration.AutoEllipsis = true;
            this.lblCurrentDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentDuration.Location = new System.Drawing.Point(196, 62);
            this.lblCurrentDuration.Name = "lblCurrentDuration";
            this.lblCurrentDuration.Size = new System.Drawing.Size(226, 25);
            this.lblCurrentDuration.TabIndex = 3;
            this.lblCurrentDuration.Text = "label4";
            this.ttDurationLabels.SetToolTip(this.lblCurrentDuration, "---");
            this.lblCurrentDuration.Click += new System.EventHandler(this.Label_Click);
            // 
            // lblTotalEventCountTodayForLastEvent
            // 
            this.lblTotalEventCountTodayForLastEvent.AutoEllipsis = true;
            this.lblTotalEventCountTodayForLastEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalEventCountTodayForLastEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalEventCountTodayForLastEvent.Location = new System.Drawing.Point(103, 41);
            this.lblTotalEventCountTodayForLastEvent.Name = "lblTotalEventCountTodayForLastEvent";
            this.lblTotalEventCountTodayForLastEvent.Size = new System.Drawing.Size(144, 25);
            this.lblTotalEventCountTodayForLastEvent.TabIndex = 4;
            this.lblTotalEventCountTodayForLastEvent.Text = "label5";
            this.lblTotalEventCountTodayForLastEvent.Click += new System.EventHandler(this.Label_Click);
            // 
            // lblTotalDurationTodayForLastEvent
            // 
            this.lblTotalDurationTodayForLastEvent.AutoEllipsis = true;
            this.lblTotalDurationTodayForLastEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalDurationTodayForLastEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDurationTodayForLastEvent.Location = new System.Drawing.Point(103, 16);
            this.lblTotalDurationTodayForLastEvent.Name = "lblTotalDurationTodayForLastEvent";
            this.lblTotalDurationTodayForLastEvent.Size = new System.Drawing.Size(144, 25);
            this.lblTotalDurationTodayForLastEvent.TabIndex = 5;
            this.lblTotalDurationTodayForLastEvent.Text = "label6";
            this.ttDurationLabels.SetToolTip(this.lblTotalDurationTodayForLastEvent, "---");
            this.lblTotalDurationTodayForLastEvent.Click += new System.EventHandler(this.Label_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "Last:";
            // 
            // lblLastTrackedIdentifier
            // 
            this.lblLastTrackedIdentifier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastTrackedIdentifier.AutoEllipsis = true;
            this.lblLastTrackedIdentifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastTrackedIdentifier.Location = new System.Drawing.Point(196, 106);
            this.lblLastTrackedIdentifier.Name = "lblLastTrackedIdentifier";
            this.lblLastTrackedIdentifier.Size = new System.Drawing.Size(223, 25);
            this.lblLastTrackedIdentifier.TabIndex = 7;
            this.lblLastTrackedIdentifier.Text = "label8";
            this.lblLastTrackedIdentifier.Click += new System.EventHandler(this.Label_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.ctxMenuStripTrayIcon;
            this.notifyIcon.Text = "notifyIcon";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // ctxMenuStripTrayIcon
            // 
            this.ctxMenuStripTrayIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.restoreToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.ctxMenuStripTrayIcon.Name = "ctxMenuStripTrayIcon";
            this.ctxMenuStripTrayIcon.Size = new System.Drawing.Size(114, 76);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(110, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.notificationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(434, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keepTopmostToolStripMenuItem,
            this.hideWhenMinimizedToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "&Menu";
            // 
            // keepTopmostToolStripMenuItem
            // 
            this.keepTopmostToolStripMenuItem.CheckOnClick = true;
            this.keepTopmostToolStripMenuItem.Name = "keepTopmostToolStripMenuItem";
            this.keepTopmostToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.keepTopmostToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.keepTopmostToolStripMenuItem.Text = "Keep &Topmost";
            this.keepTopmostToolStripMenuItem.CheckedChanged += new System.EventHandler(this.keepTopmostToolStripMenuItem_CheckedChanged);
            // 
            // hideWhenMinimizedToolStripMenuItem
            // 
            this.hideWhenMinimizedToolStripMenuItem.CheckOnClick = true;
            this.hideWhenMinimizedToolStripMenuItem.Name = "hideWhenMinimizedToolStripMenuItem";
            this.hideWhenMinimizedToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.hideWhenMinimizedToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.hideWhenMinimizedToolStripMenuItem.Text = "&Hide when Minimized";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // notificationToolStripMenuItem
            // 
            this.notificationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableIdleReminderToolStripMenuItem,
            this.disableTrackingReminderToolStripMenuItem,
            this.toolStripMenuItem2,
            this.testNotificationToolStripMenuItem});
            this.notificationToolStripMenuItem.Name = "notificationToolStripMenuItem";
            this.notificationToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.notificationToolStripMenuItem.Text = "&Notification";
            // 
            // disableIdleReminderToolStripMenuItem
            // 
            this.disableIdleReminderToolStripMenuItem.CheckOnClick = true;
            this.disableIdleReminderToolStripMenuItem.Name = "disableIdleReminderToolStripMenuItem";
            this.disableIdleReminderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F11)));
            this.disableIdleReminderToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.disableIdleReminderToolStripMenuItem.Text = "&Disable Idle Reminder";
            this.disableIdleReminderToolStripMenuItem.CheckedChanged += new System.EventHandler(this.disableIdleReminderToolStripMenuItem_CheckedChanged);
            // 
            // disableTrackingReminderToolStripMenuItem
            // 
            this.disableTrackingReminderToolStripMenuItem.CheckOnClick = true;
            this.disableTrackingReminderToolStripMenuItem.Name = "disableTrackingReminderToolStripMenuItem";
            this.disableTrackingReminderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F12)));
            this.disableTrackingReminderToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.disableTrackingReminderToolStripMenuItem.Text = "&Disable Tracking Reminder";
            this.disableTrackingReminderToolStripMenuItem.CheckedChanged += new System.EventHandler(this.disableTrackingReminderToolStripMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(262, 6);
            // 
            // testNotificationToolStripMenuItem
            // 
            this.testNotificationToolStripMenuItem.Name = "testNotificationToolStripMenuItem";
            this.testNotificationToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.testNotificationToolStripMenuItem.Text = "&Test Notification";
            this.testNotificationToolStripMenuItem.Click += new System.EventHandler(this.testNotificationToolStripMenuItem_Click);
            // 
            // lblTotalDurationToday
            // 
            this.lblTotalDurationToday.AutoEllipsis = true;
            this.lblTotalDurationToday.AutoSize = true;
            this.lblTotalDurationToday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalDurationToday.Location = new System.Drawing.Point(103, 21);
            this.lblTotalDurationToday.Name = "lblTotalDurationToday";
            this.lblTotalDurationToday.Size = new System.Drawing.Size(144, 21);
            this.lblTotalDurationToday.TabIndex = 11;
            this.lblTotalDurationToday.Text = "lblTotalDurationToday";
            this.ttDurationLabels.SetToolTip(this.lblTotalDurationToday, "---");
            this.lblTotalDurationToday.Click += new System.EventHandler(this.Label_Click);
            // 
            // lblTotalEventCountToday
            // 
            this.lblTotalEventCountToday.AutoEllipsis = true;
            this.lblTotalEventCountToday.AutoSize = true;
            this.lblTotalEventCountToday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalEventCountToday.Location = new System.Drawing.Point(103, 62);
            this.lblTotalEventCountToday.Name = "lblTotalEventCountToday";
            this.lblTotalEventCountToday.Size = new System.Drawing.Size(144, 21);
            this.lblTotalEventCountToday.TabIndex = 12;
            this.lblTotalEventCountToday.Text = "label1";
            this.lblTotalEventCountToday.Click += new System.EventHandler(this.Label_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalDurationWeek, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalDurationToday, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalEventCountToday, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalEventCountWeek, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalActivityCountToday, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalActivityCountWeek, 2, 2);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 223);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(405, 83);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(103, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 21);
            this.label5.TabIndex = 17;
            this.label5.Text = "Today (Total):";
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(253, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 21);
            this.label6.TabIndex = 18;
            this.label6.Text = "This Week (Total):";
            // 
            // label4
            // 
            this.label4.AutoEllipsis = true;
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 21);
            this.label4.TabIndex = 16;
            this.label4.Text = "Duration:";
            // 
            // lblTotalDurationWeek
            // 
            this.lblTotalDurationWeek.AutoEllipsis = true;
            this.lblTotalDurationWeek.AutoSize = true;
            this.lblTotalDurationWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalDurationWeek.Location = new System.Drawing.Point(253, 21);
            this.lblTotalDurationWeek.Name = "lblTotalDurationWeek";
            this.lblTotalDurationWeek.Size = new System.Drawing.Size(169, 21);
            this.lblTotalDurationWeek.TabIndex = 14;
            this.lblTotalDurationWeek.Text = "label4";
            this.ttDurationLabels.SetToolTip(this.lblTotalDurationWeek, "---");
            this.lblTotalDurationWeek.Click += new System.EventHandler(this.Label_Click);
            // 
            // label8
            // 
            this.label8.AutoEllipsis = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 21);
            this.label8.TabIndex = 19;
            this.label8.Text = "Totals";
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 21);
            this.label1.TabIndex = 15;
            this.label1.Text = "Records:";
            // 
            // lblTotalEventCountWeek
            // 
            this.lblTotalEventCountWeek.AutoEllipsis = true;
            this.lblTotalEventCountWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalEventCountWeek.Location = new System.Drawing.Point(253, 62);
            this.lblTotalEventCountWeek.Name = "lblTotalEventCountWeek";
            this.lblTotalEventCountWeek.Size = new System.Drawing.Size(169, 21);
            this.lblTotalEventCountWeek.TabIndex = 13;
            this.lblTotalEventCountWeek.Text = "label1";
            this.lblTotalEventCountWeek.Click += new System.EventHandler(this.Label_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 20);
            this.label11.TabIndex = 20;
            this.label11.Text = "Activities:";
            // 
            // lblTotalActivityCountToday
            // 
            this.lblTotalActivityCountToday.AutoSize = true;
            this.lblTotalActivityCountToday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalActivityCountToday.Location = new System.Drawing.Point(103, 42);
            this.lblTotalActivityCountToday.Name = "lblTotalActivityCountToday";
            this.lblTotalActivityCountToday.Size = new System.Drawing.Size(144, 20);
            this.lblTotalActivityCountToday.TabIndex = 21;
            this.lblTotalActivityCountToday.Text = "label12";
            this.lblTotalActivityCountToday.Click += new System.EventHandler(this.Label_Click);
            // 
            // lblTotalActivityCountWeek
            // 
            this.lblTotalActivityCountWeek.AutoSize = true;
            this.lblTotalActivityCountWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalActivityCountWeek.Location = new System.Drawing.Point(253, 42);
            this.lblTotalActivityCountWeek.Name = "lblTotalActivityCountWeek";
            this.lblTotalActivityCountWeek.Size = new System.Drawing.Size(169, 20);
            this.lblTotalActivityCountWeek.TabIndex = 22;
            this.lblTotalActivityCountWeek.Text = "label13";
            this.lblTotalActivityCountWeek.Click += new System.EventHandler(this.Label_Click);
            // 
            // lblTotalEventCountThisWeekForLastEvent
            // 
            this.lblTotalEventCountThisWeekForLastEvent.AutoSize = true;
            this.lblTotalEventCountThisWeekForLastEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalEventCountThisWeekForLastEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalEventCountThisWeekForLastEvent.Location = new System.Drawing.Point(253, 41);
            this.lblTotalEventCountThisWeekForLastEvent.Name = "lblTotalEventCountThisWeekForLastEvent";
            this.lblTotalEventCountThisWeekForLastEvent.Size = new System.Drawing.Size(146, 25);
            this.lblTotalEventCountThisWeekForLastEvent.TabIndex = 26;
            this.lblTotalEventCountThisWeekForLastEvent.Text = "label15";
            this.lblTotalEventCountThisWeekForLastEvent.Click += new System.EventHandler(this.Label_Click);
            // 
            // picNotificationsDisabled
            // 
            this.picNotificationsDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picNotificationsDisabled.Location = new System.Drawing.Point(362, 34);
            this.picNotificationsDisabled.Name = "picNotificationsDisabled";
            this.picNotificationsDisabled.Size = new System.Drawing.Size(60, 60);
            this.picNotificationsDisabled.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picNotificationsDisabled.TabIndex = 14;
            this.picNotificationsDisabled.TabStop = false;
            // 
            // picIdleWarning
            // 
            this.picIdleWarning.Location = new System.Drawing.Point(129, 34);
            this.picIdleWarning.Name = "picIdleWarning";
            this.picIdleWarning.Size = new System.Drawing.Size(60, 60);
            this.picIdleWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIdleWarning.TabIndex = 15;
            this.picIdleWarning.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblTotalDurationThisWeekForLastEvent, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label14, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label15, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblTotalEventCountTodayForLastEvent, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblTotalDurationTodayForLastEvent, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblTotalEventCountThisWeekForLastEvent, 2, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(17, 146);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(402, 64);
            this.tableLayoutPanel2.TabIndex = 16;
            // 
            // lblTotalDurationThisWeekForLastEvent
            // 
            this.lblTotalDurationThisWeekForLastEvent.AutoSize = true;
            this.lblTotalDurationThisWeekForLastEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalDurationThisWeekForLastEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDurationThisWeekForLastEvent.Location = new System.Drawing.Point(253, 16);
            this.lblTotalDurationThisWeekForLastEvent.Name = "lblTotalDurationThisWeekForLastEvent";
            this.lblTotalDurationThisWeekForLastEvent.Size = new System.Drawing.Size(146, 25);
            this.lblTotalDurationThisWeekForLastEvent.TabIndex = 26;
            this.lblTotalDurationThisWeekForLastEvent.Text = "label13";
            this.ttDurationLabels.SetToolTip(this.lblTotalDurationThisWeekForLastEvent, "---");
            this.lblTotalDurationThisWeekForLastEvent.Click += new System.EventHandler(this.Label_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(103, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(144, 16);
            this.label14.TabIndex = 1;
            this.label14.Text = "Today:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(253, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(146, 16);
            this.label15.TabIndex = 2;
            this.label15.Text = "This Week:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(3, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(94, 25);
            this.label16.TabIndex = 3;
            this.label16.Text = "Duration:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 41);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(94, 25);
            this.label17.TabIndex = 4;
            this.label17.Text = "Records:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 309);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(434, 22);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslInfo
            // 
            this.tsslInfo.Name = "tsslInfo";
            this.tsslInfo.Size = new System.Drawing.Size(0, 17);
            // 
            // tmrStatusbarInfoTimeout
            // 
            this.tmrStatusbarInfoTimeout.Tick += new System.EventHandler(this.tmrStatusbarInfoTimeout_Tick);
            // 
            // ttDurationLabels
            // 
            this.ttDurationLabels.Popup += new System.Windows.Forms.PopupEventHandler(this.ttDurationLabels_Popup);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(434, 331);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.picIdleWarning);
            this.Controls.Add(this.picNotificationsDisabled);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblLastTrackedIdentifier);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblCurrentDuration);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCurrentTrackingIdentifier);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1024, 370);
            this.MinimumSize = new System.Drawing.Size(450, 140);
            this.Name = "frmMain";
            this.Text = "Time Track Status";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.ctxMenuStripTrayIcon.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNotificationsDisabled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIdleWarning)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCurrentTrackingIdentifier;
        private System.Windows.Forms.Timer tmrUpdateStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCurrentDuration;
        private System.Windows.Forms.Label lblTotalEventCountTodayForLastEvent;
        private System.Windows.Forms.Label lblTotalDurationTodayForLastEvent;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblLastTrackedIdentifier;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepTopmostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideWhenMinimizedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableIdleReminderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableTrackingReminderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem testNotificationToolStripMenuItem;
        private System.Windows.Forms.Label lblTotalDurationToday;
        private System.Windows.Forms.Label lblTotalEventCountToday;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTotalEventCountWeek;
        private System.Windows.Forms.Label lblTotalDurationWeek;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblTotalActivityCountToday;
        private System.Windows.Forms.Label lblTotalActivityCountWeek;
        private System.Windows.Forms.PictureBox picNotificationsDisabled;
        private System.Windows.Forms.PictureBox picIdleWarning;
        private System.Windows.Forms.Label lblTotalEventCountThisWeekForLastEvent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblTotalDurationThisWeekForLastEvent;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStripTrayIcon;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslInfo;
        private System.Windows.Forms.Timer tmrStatusbarInfoTimeout;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolTip ttDurationLabels;
    }
}

