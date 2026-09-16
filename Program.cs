namespace Capillume
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
            const string mutexName = "Capillume_SingleInstance_Mutex";
            _mutex = new Mutex(true, mutexName, out bool createdNew);

            if (!createdNew)
            {
                // Another instance is already running
                MessageBox.Show("Capillume is already running. Check the system tray.", 
                    "Already Running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ApplicationConfiguration.Initialize();

            // Determine if launched via auto-start or user action
            bool isAutoStart = args.Length > 0 && args[0] == "--autostart";

            var mainForm = new Form1(isAutoStart);

            Application.Run(mainForm);

            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
        }
    }
}