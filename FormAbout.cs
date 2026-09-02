using System.Reflection;

namespace Capillume
{
    public partial class FormAbout : Form
    {
        private Icon? _appIcon;
        public FormAbout()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            var assembly = Assembly.GetExecutingAssembly();
            // Load icon for the system tray and form
            try
            {
                using (var stream = assembly.GetManifestResourceStream("Capillume.icon.ico"))
                {
                    if (null != stream)
                    {
                        _appIcon = new Icon(stream);
                        this.Icon = _appIcon;
                    }
                    else
                    {
                        // Fallback to generated icon
                        _appIcon = FallbackIcon.CreateAppIconAdvanced();
                        this.Icon = _appIcon;
                    }
                }
            }
            catch
            {
                // Fallback to generated icon
                _appIcon = FallbackIcon.CreateAppIconAdvanced();
                this.Icon = _appIcon;
            }

            try
            {
                using (var stream = assembly.GetManifestResourceStream("Capillume.icon.png"))
                {
                    if (null != stream)
                    {
                        pictureBoxLogo.Image = Image.FromStream(stream);
                    }
                }
            }
            catch
            {
                // Logo is optional
            }

            // Load settings into UI
            this.Text = $"About {Application.ProductName}"; // v{assembly.GetName().Version}";
            this.labelAppName.Text = $"{Application.ProductName}";
            this.labelVersion.Text = $"Version {assembly.GetName().Version}";
        }

        private void ButtonClose_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void LinkLabelGitHub_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://github.com/dasdebjyoti/Capillume",
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Unable to open the link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
