using Timer = System.Threading.Timer;


namespace PhotoMover
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceDirectory = @"C:\Users\mrtab\OneDrive\Desktop\Telegrampics";
            string destinationDirectory = @"C:\Users\mrtab\OneDrive\Desktop\PhotomoverTest File";

            // Ensure the destination directory exists
            Directory.CreateDirectory(destinationDirectory);

            // Check if the source directory is empty
            if (Directory.GetFileSystemEntries(sourceDirectory).Length == 0)
            {
                Console.WriteLine("Folder is empty.");
                ExitAfterDelay(5000); // Exit after 5 seconds if the folder is empty
                return;
            }

            // Initialize counters for successful file operations and total size
            int successfulOperations = 0;
            long totalSizeInBytes = 0;

            // Iterate through all files in the source directory
            foreach (var filePath in Directory.GetFiles(sourceDirectory))
            {
                try
                {
                    // Get the file name without extension for comparison
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);

                    // Check if a file with the same name already exists in the destination directory
                    string destinationFilePath = Path.Combine(destinationDirectory, fileNameWithoutExt + Path.GetExtension(filePath));
                    if (File.Exists(destinationFilePath))
                    {
                        Console.WriteLine($"Duplicate file found: {fileNameWithoutExt}. Skipping.");
                        continue; // Skip this iteration
                    }

                    // Calculate the size of the file before copying in bytes
                    FileInfo fileInfo = new FileInfo(filePath);
                    long fileSizeInBytes = fileInfo.Length;

                    // Copy the file from the source directory to the destination directory
                    File.Copy(filePath, destinationFilePath, true);

                    // Delete the file from the source directory after copying
                    File.Delete(filePath);

                    // Add the file size to the total size in bytes
                    totalSizeInBytes += fileSizeInBytes;

                    // Increment the counter for successful operations
                    successfulOperations++;
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

            // Convert the total size from bytes to gigabytes or megabytes
            double totalSizeInGB = totalSizeInBytes / Math.Pow(1024, 3);
            double totalSizeInMB = totalSizeInBytes / Math.Pow(1024, 2);

            // Determine the most appropriate unit for displaying the total size
            string unit = totalSizeInGB >= 1 ? "GB" : "MB";
            double totalSizeInAppropriateUnit = totalSizeInGB >= 1 ? totalSizeInGB : totalSizeInMB;

            // Print the total number of files successfully copied and deleted
            Console.WriteLine($"{successfulOperations} files copied and deleted successfully.");

            // Print the total size of the copied files in the most appropriate unit
            Console.WriteLine($"Total size of copied files: {totalSizeInAppropriateUnit:N2} {unit}.");

            // Set up a timer to exit the application after 5 seconds
            // Timer timer = new Timer(TimerElapsed, null, 5000, Timeout.Infinite);
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
