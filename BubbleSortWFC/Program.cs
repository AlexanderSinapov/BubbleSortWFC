using Raylib_cs;

namespace BubbleSortWFC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int screenWidth = 800;
            const int screenHeight = 600;

            Raylib.InitWindow(screenWidth, screenHeight, "Bubble Sort");
            Raylib.SetTargetFPS(60);

            // Initialize the array as a wavefunction
            int numPoints = 100;
            int[] array = new int[numPoints];
            float[] probabilityDensity = new float[numPoints];
            Random rand = new Random();

            // Initialize array with random values
            for (int i = 0; i < numPoints; i++)
            {
                array[i] = rand.Next(1, 100); // Random integers between 1 and 100
                probabilityDensity[i] = array[i] * array[i]; // Probability density
            }

            Normalize(probabilityDensity);

            int iIndex = 0;
            int jIndex = 0;
            bool sortingComplete = false;
            int minIndex = 0, maxIndex = numPoints - 1;

            while (!Raylib.WindowShouldClose())
            {
                if (!sortingComplete)
                {
                    // Bubble Sort Step
                    if (iIndex < array.Length - 1)
                    {
                        if (jIndex < array.Length - iIndex - 1)
                        {
                            if (array[jIndex] > array[jIndex + 1])
                            {
                                // Swap elements
                                int temp = array[jIndex];
                                array[jIndex] = array[jIndex + 1];
                                array[jIndex + 1] = temp;

                                // Update probability density
                                probabilityDensity[jIndex] = array[jIndex] * array[jIndex];
                                probabilityDensity[jIndex + 1] = array[jIndex + 1] * array[jIndex + 1];
                            }
                            jIndex++;
                        }
                        else
                        {
                            jIndex = 0;
                            iIndex++;
                        }
                    }
                    else
                    {
                        sortingComplete = true;
                        // Find min and max indices
                        minIndex = 0;
                        maxIndex = 0;
                        for (int i = 1; i < array.Length; i++)
                        {
                            if (array[i] < array[minIndex]) minIndex = i;
                            if (array[i] > array[maxIndex]) maxIndex = i;
                        }
                    }

                    Normalize(probabilityDensity);
                }

                // Draw visualization
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                // Draw array elements as rectangles
                int rectWidth = screenWidth / numPoints;
                int maxArrayValue = 100; // Maximum value in the array
                for (int i = 0; i < numPoints; i++)
                {
                    int rectHeight = (int)((float)array[i] / maxArrayValue * screenHeight / 2);
                    int x = i * rectWidth;
                    int y = screenHeight - rectHeight;

                    Color rectColor;
                    if (sortingComplete)
                    {
                        if (i == minIndex)
                            rectColor = Color.Blue;
                        else if (i == maxIndex)
                            rectColor = Color.Red;
                        else
                            rectColor = Color.White;
                    }
                    else
                    {
                        rectColor = (i == jIndex || i == jIndex + 1) ? Color.Green : Color.White;
                    }

                    Raylib.DrawRectangle(x, y, rectWidth - 1, rectHeight, rectColor);
                }

                // Draw wave above the array visualization
                for (int i = 0; i < numPoints - 1; i++)
                {
                    int x1 = (int)((float)screenWidth / numPoints * i);
                    int y1 = (int)(screenHeight / 4 - probabilityDensity[i] * screenHeight / 4);
                    int x2 = (int)((float)screenWidth / numPoints * (i + 1));
                    int y2 = (int)(screenHeight / 4 - probabilityDensity[i + 1] * screenHeight / 4);

                    Color lineColor = (i == jIndex || i == jIndex + 1) ? Color.Green : Color.White;
                    Raylib.DrawLine(x1, y1, x2, y2, lineColor);
                }

                // Display min and max values if sorting is complete
                if (sortingComplete)
                {
                    string minText = $"Min: {array[minIndex]:F3}";
                    string maxText = $"Max: {array[maxIndex]:F3}";
                    Raylib.DrawText(minText, 10, 10, 20, Color.Blue);
                    Raylib.DrawText(maxText, 10, 40, 20, Color.Red);
                }
                else
                {
                    string sortingText = "Sorting in progress...";
                    Raylib.DrawText(sortingText, 10, 10, 20, Color.Green);
                }

                Raylib.EndDrawing();

                if (!sortingComplete)
                {
                    Raylib.WaitTime(0.01); // 10ms delay during sorting
                }
            }

            Raylib.CloseWindow();
        }

        // Normalize function for probability density
        static void Normalize(float[] array)
        {
            float sum = 0;
            foreach (float val in array)
                sum += val;

            if (sum > 0)
            {
                for (int i = 0; i < array.Length; i++)
                    array[i] /= sum;
            }
        }
    }
}
