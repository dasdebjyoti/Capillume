using System.Reflection;

namespace CapIilume
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            LoadIcon();
        }

        private void LoadIcon()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream("CapIilume.icon.png"))
                {
                    if (stream != null)
                    {
                        pictureBoxLogo.Image = Image.FromStream(stream);
                    }
                }
            }
            catch
            {
                // Logo is optional
            }
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
                    FileName = "https://github.com/dasdebjyoti/CapIilume",
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
