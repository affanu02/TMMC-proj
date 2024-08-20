using System.Drawing;
using System.Net;

class Program
{
    static void Main(string[] args)
    {
        //variables
        string imagePath = "";

        //make sure there is only one argument provided
        if (args.Length != 1)
        {
            Console.WriteLine("Error: Invalid number of arguments.");
            imagePath = ImageAddressInput();
        }
        else
        {
            // If exactly one argument is provided, use it as the image path.
            imagePath = args[0];
        }

        try
        {
            // Create a Bitmap from the MemoryStream to manipulate the image.
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(imagePath);
            using (MemoryStream ms = new MemoryStream(bytes))
            using (Bitmap img = new Bitmap(ms))
            {
                // Convert the image to a byte matrix representing grayscale values.
                byte[] matrix = ImageToByteMatrix(img);

                //Count and output the number of columns
                Console.WriteLine("The number of columns are " + CountColumns(matrix, img.Width, img.Height));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            imagePath = ImageAddressInput();
        }
    }

    static string ImageAddressInput()
    {
        // Read the image path from the console input.
        Console.WriteLine("Please Input the image path to process. ");
        string imagePath = Console.ReadLine();

        return imagePath;
    }

    static byte[] ImageToByteMatrix(Bitmap img)
    {
        //variables
        int index = 0;

        //byte matrix calculated from (width x height)
        byte[] matrix = new byte[img.Width * img.Height];

        //iterate through each pixel in the image
        for (int i = 0; i < img.Height; i++)
        {
            for (int j = 0; j < img.Width; j++)
            {
                //retrieve the color of each iteratted pixel and convert to grayscale using the most common luminosity method
                //resource used: https://insider.kelbyone.com/how-photoshop-translates-rgb-color-to-gray-by-scott-valentine/ 
                Color pixelColor = img.GetPixel(j, i);
                byte grayscale = (byte)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                matrix[index++] = grayscale;
            }
        }
        return matrix;
    }

    static int CountColumns(byte[] matrix, int width, int height)
    {
        //varoables
        int count = 0;
        bool isblack = false;

        // iterate through the columns of a row
        for (int i = 0; i < width; i++)
        {
            // get index value of middle pixels
            int index = i + (height / 2 * width);
            // Console.Write(matrix[index] + " "); // Prints the values of the middle row for debugging purposes

            // if the value is black and its before value wasnt black up the count
            if (matrix[index] == 0 && !isblack)
            {
                isblack = true;
                count++;
            }

            // if the value isnt black keep iterating
            if (matrix[index] != 0)
            {
                isblack = false;
            }
        }
        return count;
    }
}
