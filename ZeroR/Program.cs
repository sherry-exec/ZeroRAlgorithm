using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZeroR
{
    class Classes
    {
        string value = "";
        int freq = 0;

        public Classes(string _value)
        {
            value = _value;
            incFreq();
        }

        public Classes(string _value, int _freq)
        {
            value = _value;
            freq = _freq;
        }

        public int incFreq()
        {
            return ++freq;
        }

        public string Value
        {
            get { return value; }
        }

        public int Frequency
        {
            get { return freq; }
        }
    }

    class Program
    {
        //
        //  Data
        //

        static List<object> y = new List<object>();                         //Training data set of targets(results)
        static List<Classes> originalClassFreqs = new List<Classes>();      //Classes and their original frequencies
        static List<Classes> predictedClassFreqs;                           //Classes and their predicted frequencies

        //ZeroR properties
        static int[,] confusionMatrix;
        static int accuracy = 0;        //percentage accuracy

        //
        //  Functions
        //

        // Checks if a class exists of the name _value
        // If exists, returns the index of it in the originalClassFreqs
        // If does not exists, returns -1
        static int classExists(string _value)
        {
            for (int i = 0; i < originalClassFreqs.Count; i++)
            {
                if (originalClassFreqs.ElementAt(i).Value == _value)
                    return i;
            }

            return -1;
        }

        // Reads data from a file line by line and fills in into the target list i.e. y
        static bool readFromFile(string fileName = "input.txt")
        {
            try
            {    
                StreamReader inputFile = new StreamReader("input.txt");

                string line = null;
                while ((line = inputFile.ReadLine().ToLowerInvariant()) != null)
                {
                    y.Add(line);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Breaks the target list into classes,
        // put the unique classes in originalClassFreqs and 
        // generate freqency for each class in the list
        static bool classify()
        {
            try
            {
                for (int i = 0; i < y.Count; i++)
                {
                    int index = classExists(y.ElementAt(i).ToString());
                    if (index >= 0)
                    {
                        originalClassFreqs.ElementAt(index).incFreq();
                    }
                    else
                    {
                        originalClassFreqs.Add(new Classes(y.ElementAt(i).ToString()));
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Applies the algorithm ZeroR on the originalClassFreqs dataset
        // specifically it generates the confusion matrix
        static bool applyZeroR()
        {
            try
            {
                int index = 0, maxFreq = 0;
                predictedClassFreqs = new List<Classes>();

                //Obtain max frequency class
                index = maxFrequencyClass();
                maxFreq = originalClassFreqs.ElementAt(index).Frequency;

                //Assign all the classes this max frequency
                for (int i = 0; i < originalClassFreqs.Count; i++)
                {
                    predictedClassFreqs.Add(new Classes(originalClassFreqs.ElementAt(i).Value, maxFreq));
                }

                //Make confusion matrix
                confusionMatrix = new int[predictedClassFreqs.Count, predictedClassFreqs.Count];
                for (int i = 0; i < predictedClassFreqs.Count; i++)
                {
                    confusionMatrix[index, i] = originalClassFreqs.ElementAt(i).Frequency;
                }

                calculateAccuracy();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Returns the index to max frequency class in originalClassFreqs
        static int maxFrequencyClass()
        {
            try
            {
                int index = -1, maxFreq = 0;

                //Calculate the Max Frequency
                for (int i = 0; i < originalClassFreqs.Count; i++)
                {
                    if (originalClassFreqs.ElementAt(i).Frequency > maxFreq)
                    {
                        maxFreq = originalClassFreqs.ElementAt(i).Frequency;
                        index = i;
                    }
                }

                return index;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        static void calculateAccuracy()
        {
            try
            {
                accuracy = Convert.ToInt16(originalClassFreqs.ElementAt(maxFrequencyClass()).Frequency / Convert.ToDouble(y.Count) * 100 );
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Prints the result in ZeroR specific format
        static void printResult(string dataName)
        {
            // Confusion Matrix format
            Console.WriteLine("Confusion Matrix\n");
            Console.WriteLine("\t\t" + dataName);
            Console.Write("\t\t");
            for (int i = 0; i < originalClassFreqs.Count; i++)
            {
                Console.Write(originalClassFreqs.ElementAt(i).Value + "\t");
            }
            Console.Write("\n" + "ZeroR");

            for (int i = 0; i < originalClassFreqs.Count; i++)
            {
                Console.Write("\t" + originalClassFreqs.ElementAt(i).Value + "\t");
                for (int j = 0; j < originalClassFreqs.Count; j++)
                {
                    Console.Write(confusionMatrix[i, j] + "\t");
                }
                Console.Write("\n");
            }

            Console.WriteLine("\nAccuracy: " + accuracy + "%");
        }

        ///////////////////////////////

        static void Main(string[] args)
        {
            if(args.Count() < 1)
            {
                Console.Write("Count of rows(instances), 0 to read from file: ");
                int n = Convert.ToInt16(Console.ReadLine());
                if (n > 0)
                {
                    Console.WriteLine("Enter values for y: ");
                    for (int i = 0; i < n; i++)
                    {
                        y.Add(Console.ReadLine().ToLowerInvariant());
                    }
                }
                else
                {
                    readFromFile();
                }
            }
            else
            {
                readFromFile(args[0]);
            }

            classify();
            applyZeroR();
            printResult("Play Golf");

            Console.ReadKey();
        }
    }
}
