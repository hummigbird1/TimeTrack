using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTrack.Application.Common;
using TimeTrack.Interfaces;

namespace TimeTrack.Status.UI
{
    public partial class frmMain : Form
    {
        private IDataStorage _dataStorage;
        private Icon _idleIcon;
        private IReminder _reminder;
        private TimeTrackStatus _status;
        private bool _tooltipUpdating = false;
        private Icon _trackingIcon;

        public frmMain()
        {
            InitializeComponent();
        }

        internal IServiceProvider ServiceProvider { get; set; }

        private void disableIdleReminderToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            _reminder.DisableIdleReminder = disableIdleReminderToolStripMenuItem.Checked;
        }

        private void disableTrackingReminderToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            _reminder.DisableTrackingReminder = disableTrackingReminderToolStripMenuItem.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var configuration = ServiceProvider.GetRequiredService<Configuration.Configuration>();
            _dataStorage = ServiceProvider.GetRequiredService<IDataStorage>();
            _reminder = ServiceProvider.GetRequiredService<IReminder>();
            _status = new TimeTrackStatus(_dataStorage);

            LoadIconsAndImages();
            var trayIconsLoaded = _idleIcon != null && _trackingIcon != null;
            hideWhenMinimizedToolStripMenuItem.Checked = trayIconsLoaded;
            hideWhenMinimizedToolStripMenuItem.Enabled = trayIconsLoaded;

            tmrUpdateStatus.Interval = (int)configuration.ApplicationSettings.UpdateInterval.TotalMilliseconds;
            if (configuration.ApplicationSettings.DisableNotifications)
            {
                notificationToolStripMenuItem.Enabled = false;
            }

            SetControlsDatabinding();

            _status.Update();
            UpdateContextMenuInfoItem();

            tmrUpdateStatus.Enabled = true;
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && hideWhenMinimizedToolStripMenuItem.Checked)
            {
                Hide();
            }
        }

        private Binding GetCurrentTrackingForeColorBinding()
        {
            var binding = new Binding(nameof(Label.ForeColor), _status, nameof(TimeTrackStatus.IsTrackingActive));
            binding.Format += (s, e) =>
            {
                if (!(bool)e.Value)
                {
                    e.Value = Color.Red;
                }
                else
                {
                    e.Value = Color.FromKnownColor(KnownColor.ControlText);
                }
            };
            return binding;
        }

        private Binding GetTitlebarTextBinding()
        {
            var timespanFormatter = new OptimizedTimeSpanFormatProvider();
            var binding = new Binding(nameof(Text), _status, nameof(TimeTrackStatus.IsTrackingActive));
            binding.Format += (s, e) =>
            {
                var sb = new StringBuilder();
                sb.Append("TimeTrack Status");

                if (_status.IsTrackingActive)
                {
                    sb.AppendFormat(timespanFormatter, " Tracking: {0} - {1:NoDays}", _status.CurrentlyTrackingIdentifier, _status.Duration);
                }
                else
                {
                    sb.AppendFormat(timespanFormatter, " IDLE for {0:NoDays} - Last: {1}", _status.Duration, _status.LastTrackedIdentifier);
                }

                e.Value = sb.ToString();
            };
            return binding;
        }

        private async void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateStatus();
        }

        private void keepTopmostToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = keepTopmostToolStripMenuItem.Checked;
        }

        private void Label_Click(object sender, EventArgs e)
        {
            try
            {
                var label = sender as Label;
                Clipboard.SetText(label.Text);
                SetStatusbarInfoWithTimeout("Copied to clipboard successfully", TimeSpan.FromSeconds(3));
            }
            catch (Exception ex)
            {
                SetStatusbarInfoWithTimeout($"Clipboard copy failed: {ex.Message}", TimeSpan.FromSeconds(3));
            }
        }

        private void LoadIconsAndImages()
        {
            var tracking = MakeIconFilePath("Tracking");
            if (File.Exists(tracking))
            {
                _trackingIcon = new Icon(tracking);
            }

            var idle = MakeIconFilePath("Idle");
            if (File.Exists(idle))
            {
                _idleIcon = new Icon(idle);
            }

            var notificationsDisabled = MakePictureFilePath("NotificationsDisabled");

            if (File.Exists(notificationsDisabled))
            {
                picNotificationsDisabled.ImageLocation = notificationsDisabled;
            }

            var idleImage = MakePictureFilePath("IdleImage");
            if (File.Exists(idleImage))
            {
                picIdleWarning.ImageLocation = idleImage;
            }
        }

        private string MakeIconFilePath(string type)
        {
            var applicationBase = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var iconBasePath = Path.Combine(applicationBase, "Icons");
            var file = Path.Combine(iconBasePath, $"{type}.ico");
            return file;
        }

        private string MakePictureFilePath(string type)
        {
            var applicationBase = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var iconBasePath = Path.Combine(applicationBase, "Icons");
            var file = Path.Combine(iconBasePath, $"{type}.png");
            return file;
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private async void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateStatus();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private string SafeText()
        {
            if (Text.Length > 63)
            {
                return Text.Substring(0, 63);
            }
            return Text;
        }

        private void SetControlsDatabinding()
        {
            lblCurrentTrackingIdentifier.DataBindings.AddWithFormatting(nameof(Label.Text), _status, nameof(TimeTrackStatus.CurrentlyTrackingIdentifier), x =>
            {
                if (!_status.IsTrackingActive)
                {
                    x.Value = "< IDLE >";
                }
            });
            lblCurrentTrackingIdentifier.DataBindings.Add(GetCurrentTrackingForeColorBinding());

            lblCurrentDuration.DataBindings.AddWithFormatting(nameof(Label.Text), _status, nameof(TimeTrackStatus.Duration), new OptimizedTimeSpanFormatProvider(), "{0:NoDays}");
            lblCurrentDuration.DataBindings.Add(nameof(Label.Tag), _status, nameof(TimeTrackStatus.Duration));

            lblTotalEventCountTodayForLastEvent.DataBindings.Add(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalTodayForLastIdentifier)}.{nameof(TimeTrackStatus.TotalTodayForLastIdentifier.EventCount)}");

            lblTotalDurationTodayForLastEvent.DataBindings.AddWithFormatting(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalTodayForLastIdentifier)}.{nameof(TimeTrackStatus.TotalTodayForLastIdentifier.TotalDuration)}", new OptimizedTimeSpanFormatProvider(), "{0:NoDays}");
            lblTotalDurationTodayForLastEvent.DataBindings.Add(nameof(Label.Tag), _status, $"{nameof(TimeTrackStatus.TotalTodayForLastIdentifier)}.{nameof(TimeTrackStatus.TotalTodayForLastIdentifier.TotalDuration)}");

            lblLastTrackedIdentifier.DataBindings.Add(nameof(Label.Text), _status, nameof(TimeTrackStatus.LastTrackedIdentifier));

            lblTotalEventCountThisWeekForLastEvent.DataBindings.Add(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalThisWeekForLastIdentifier)}.{nameof(TimeTrackStatus.TotalThisWeekForLastIdentifier.EventCount)}");
            lblTotalDurationThisWeekForLastEvent.DataBindings.AddWithFormatting(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalThisWeekForLastIdentifier)}.{nameof(TimeTrackStatus.TotalThisWeekForLastIdentifier.TotalDuration)}", new OptimizedTimeSpanFormatProvider(), "{0:NoDays}");
            lblTotalDurationThisWeekForLastEvent.DataBindings.Add(nameof(Label.Tag), _status, $"{nameof(TimeTrackStatus.TotalThisWeekForLastIdentifier)}.{nameof(TimeTrackStatus.TotalThisWeekForLastIdentifier.TotalDuration)}");

            DataBindings.Add(GetTitlebarTextBinding());

            lblTotalDurationToday.DataBindings.AddWithFormatting(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalToday)}.{nameof(TimeTrackStatus.TotalToday.TotalDuration)}", new OptimizedTimeSpanFormatProvider(), "{0:NoDays}");
            lblTotalDurationToday.DataBindings.Add(nameof(Label.Tag), _status, $"{nameof(TimeTrackStatus.TotalToday)}.{nameof(TimeTrackStatus.TotalToday.TotalDuration)}");

            lblTotalEventCountToday.DataBindings.Add(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalToday)}.{nameof(TimeTrackStatus.TotalToday.EventCount)}");
            lblTotalActivityCountToday.DataBindings.Add(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalToday)}.{nameof(TimeTrackStatus.TotalToday.DistinctEventCount)}");

            lblTotalDurationWeek.DataBindings.AddWithFormatting(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalThisWeek)}.{nameof(TimeTrackStatus.TotalThisWeek.TotalDuration)}", new OptimizedTimeSpanFormatProvider(), "{0:NoDays}");
            lblTotalDurationWeek.DataBindings.Add(nameof(Label.Tag), _status, $"{nameof(TimeTrackStatus.TotalThisWeek)}.{nameof(TimeTrackStatus.TotalThisWeek.TotalDuration)}");

            lblTotalEventCountWeek.DataBindings.Add(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalThisWeek)}.{nameof(TimeTrackStatus.TotalThisWeek.EventCount)}");
            lblTotalActivityCountWeek.DataBindings.Add(nameof(Label.Text), _status, $"{nameof(TimeTrackStatus.TotalThisWeek)}.{nameof(TimeTrackStatus.TotalThisWeek.DistinctEventCount)}");
        }

        private void SetStatusbarInfoWithTimeout(string infoText, TimeSpan timeout)
        {
            tmrStatusbarInfoTimeout.Enabled = false;
            tsslInfo.Text = infoText;
            tmrStatusbarInfoTimeout.Interval = (int)timeout.TotalMilliseconds;
            tmrStatusbarInfoTimeout.Enabled = true;
        }

        private async void testNotificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await _reminder.TestNotificationAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tmrStatusbarInfoTimeout_Tick(object sender, EventArgs e)
        {
            tsslInfo.Text = null;
        }

        private async void tmrUpdateStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                await UpdateStatus();
            }
            catch (Exception ex)
            {
                SetStatusbarInfoWithTimeout(ex.Message, TimeSpan.FromSeconds(5));
            }
        }

        private void ttDurationLabels_Popup(object sender, PopupEventArgs e)
        {
            if (_tooltipUpdating) return;
            var tag = e.AssociatedControl.Tag;
            if (tag != null)
            {
                if (tag is TimeSpan timeSpan)
                {
                    _tooltipUpdating = true;
                    ttDurationLabels.SetToolTip(e.AssociatedControl, string.Format(new OptimizedTimeSpanFormatProvider(), "{0}", timeSpan));
                    _tooltipUpdating = false;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void UpdateContextMenuInfoItem()
        {
            var timespanFormatter = new OptimizedTimeSpanFormatProvider();
            var sb = new StringBuilder();

            if (_status.IsTrackingActive)
            {
                sb.AppendFormat(timespanFormatter, "Tracking: {0} - {1:NoDays}", _status.CurrentlyTrackingIdentifier, _status.Duration);
            }
            else
            {
                sb.AppendFormat(timespanFormatter, "IDLE for {0:NoDays} - Last: {1}", _status.Duration, _status.LastTrackedIdentifier);
            }

            infoToolStripMenuItem.Text = sb.ToString();
        }

        private async Task UpdateStatus()
        {
            var trackingBeforeUpdate = _status.IsTrackingActive;

            try
            {
                _status.Update();
                UpdateContextMenuInfoItem();
                picIdleWarning.Visible = !_status.IsTrackingActive;
                if (_status.IsTrackingActive)
                {
                    notifyIcon.Text = SafeText();
                    notifyIcon.Icon = _trackingIcon;
                }
                else
                {
                    notifyIcon.Text = SafeText();
                    notifyIcon.Icon = _idleIcon;
                }

                if (notifyIcon.Icon != null)
                {
                    Icon = notifyIcon.Icon;
                }
            }
            catch (Exception ex)
            {
                tmrUpdateStatus.Enabled = false;
                MessageBox.Show(ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (notificationToolStripMenuItem.Enabled)
            {
                try
                {
                    if (trackingBeforeUpdate != _status.IsTrackingActive)
                    {
                        _reminder.OnTrackingStatusChanged();
                    }
                    picNotificationsDisabled.Visible = !_reminder.NotificationsActive;
                    await _reminder.UpdateAsync();
                }
                catch (Exception ex)
                {
                    disableIdleReminderToolStripMenuItem.Checked = true;
                    disableTrackingReminderToolStripMenuItem.Checked = true;
                    MessageBox.Show(ex.Message, "Notification Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                picNotificationsDisabled.Visible = true;
            }
        }
    }
}