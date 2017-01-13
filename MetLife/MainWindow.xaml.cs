using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MetLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        Label[] labels;

        int[] randomArray;
        List<string> candiList;

        string csvFileName = "sample.csv";
        int numToDraw=10;

        bool rolling = false;
        bool nextFlag = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            randomArray = GetRandoms(0, candiList.Count, numToDraw);

            for (int i = 0; i < numToDraw; i++)
            {
                labels[i].Content = candiList[randomArray[i]];
            }
        }

        private int[] GetRandoms(int startNum,int endNum,int count)
        {
            int number, j, i = 0;
            Random random = new Random();
            int[] tempArray = new int[count];

            while (i<count)
            {
                number = random.Next(startNum, endNum);
                for (j = 0; j < i; j++)
                {
                    if (number == tempArray[j]) break;
                }

                if (j==i)
                {
                    tempArray[i] = number;
                    i++;
                }
            }

            return tempArray;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            candiList = new List<string>();
            var candidates = File.ReadAllLines(csvFileName);
            foreach (var name in candidates)
            {
                candiList.Add(name);
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.04);
            timer.Tick += Timer_Tick;

            labels = new Label[] { lbl0, lbl1, lbl2, lbl3, lbl4, lbl5, lbl6, lbl7, lbl8, lbl9 };
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space)
                return;

            if(rolling)
            {
                // stop roll
                timer.Stop();

                // remove candidates
                // List<int> tempList = new List<int>();
                // foreach (var item in randomArray)
                // {
                //     tempList.Add(item);
                // }
                randomArray.OrderBy(i => i);
                for (int i = randomArray.Length; i >0 ; i--)
                {
                    candiList.RemoveAt(randomArray[i-1]);
                }

                rolling = false;
            }
            else
            {
                if(nextFlag)
                {
                    // start roll
                    rolling = true;
                    nextFlag = false;
                }
                else
                {
                    // set some vars
                    nextFlag = true;
                }
            }
        }
    }
}
