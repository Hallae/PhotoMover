using System.Timers;
using Timer = System.Threading.Timer;
using System.Diagnostics;

namespace PhotoMover

{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceDirectory = @"     C:\Users\mrtab\OneDrive\Desktop\Telegrampics                ";
            string destinationDirectory = @"          C:\Users\mrtab\OneDrive\Desktop\PhotomoverTest File       ";

            // Ensure the destination directory exists
            Directory.CreateDirectory(destinationDirectory);

            // Check if the source directory is empty
            if (Directory.GetFileSystemEntries(sourceDirectory).Length == 0)
            {
                Console.WriteLine("Folder is empty.");
                ExitAfterDelay(5000); // Exit after 5 seconds if the folder is empty
                return;
            }

            // Iterate through all files in the source directory
            foreach (var filePath in Directory.GetFiles(sourceDirectory))
            {
                try
                {
                    // Copy the file from the source directory to the destination directory
                    File.Copy(filePath, Path.Combine(destinationDirectory, Path.GetFileName(filePath)), true);

                    // Delete the file from the source directory after copying
                    File.Delete(filePath);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine($"File not found: {e.FileName}");
                }
                catch (IOException e)
                {
                    Console.WriteLine($"An error occurred while processing the file: {e.Message}");
                }
            }

            Console.WriteLine("Files copied and deleted successfully.");

            // Set up a timer to exit the application after 5 seconds
           //Timer timer = new Timer(TimerElapsed, null, 5000, Timeout.Infinite);
        }

        private static void TimerElapsed(object state)
        {
            Environment.Exit(0);
        }

        private static void ExitAfterDelay(int milliseconds)
        {
            Timer timer = new Timer(TimerElapsed, null, milliseconds, Timeout.Infinite);
        }
    }
}
