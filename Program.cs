namespace CapIilume
{
    internal static class Program
    {
        private static Mutex? _mutex;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Ensure only one instance is running
            const string mutexName = "CapIilume_SingleInstance_Mutex";
            _mutex = new Mutex(true, mutexName, out bool createdNew);

            if (!createdNew)
            {
                // Another instance is already running
                MessageBox.Show("CapIilume is already running. Check the system tray.", 
                    "Already Running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ApplicationConfiguration.Initialize();

            // Determine if launched via auto-start or user action
            bool isAutoStart = args.Length > 0 && args[0] == "--autostart";

            var mainForm = new Form1();

            if (isAutoStart)
            {
                // Auto-started: start minimized to tray
                mainForm.WindowState = FormWindowState.Minimized;
                mainForm.ShowInTaskbar = false;

                // Don't show the form, just run the application
                Application.Run(mainForm);
            }
            else
            {
                // User-initiated: show the form
                Application.Run(mainForm);
            }

            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
        }
    }
}