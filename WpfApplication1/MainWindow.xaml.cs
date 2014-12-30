using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region

        static string filePath = @"%UserProfile%\Desktop";
        string connString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};DBQ=" + Environment.ExpandEnvironmentVariables(filePath);

        DataTable dtTable = new DataTable();

        Awards drawingAward = Awards.A;
        Awards2 drawingAward2 = Awards2.FifthPrize;

        int allCandiNum, restCandiNum;           // 所有抽奖的总人数，剩余抽奖的人数
        int totalNumToDraw, totalTimesToDraw;    // 当前奖项共抽取多少人，需抽取多少次
        int drawingTime = 0;                    // 当前奖项在抽取第几次
        int lastRound, thisRound;          // 当前奖项最后一次抽取需抽取多少人

        int[] random;                           // 每一次滚动时的随机序列

        DispatcherTimer timer;
        Label[] labels;

        bool isRolling = false;

        bool lastRoundStop = true;              // 当前奖项最后一次抽取是否完成，用于切换至下一奖项时的暂停画面

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.04);
            timer.Tick += timer_Tick;

            labels = new Label[] { lbl0, lbl1, lbl2, lbl3, lbl4, lbl5, lbl6, lbl7, lbl8, lbl9 };
        }

        public void InitializeCandidates()
        {
            string fileName = "Candidates";

            try
            {
                using (OdbcConnection odbcConn = new OdbcConnection(connString))
                {
                    odbcConn.Open();

                    string commandString = "SELECT * FROM [" + fileName + ".csv]";
                    OdbcDataAdapter adapter = new OdbcDataAdapter(commandString, odbcConn);

                    adapter.Fill(dtTable);

                    restCandiNum = allCandiNum = dtTable.Rows.Count;

                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCandidates();

            // 首先抽取5等奖
            drawingAward2 = Awards2.FifthPrize;
            totalNumToDraw = 40;
            totalTimesToDraw = 4;
            drawingTime = 0;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space)
                return;

            #region 28-58

            //switch (drawingAward)
            //{
            //    case Awards.A:
            //        if (isRolling)
            //        {
            //            timer.Stop();
            //            // 保存数据
            //            // 从候选人员中减去
            //            SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\28.csv"));
            //            isRolling = false;
            //            if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
            //            {
            //                drawingAward = Awards.B;

            //                totalNumToDraw = 6;
            //                totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
            //                if (lastRound == 0) totalTimesToDraw--;
            //                drawingTime = 0;
            //                thisRound = 6;

            //                lastRoundStop = true;
            //            }
            //        }
            //        else
            //        {
            //            if (lastRoundStop)
            //            {
            //                for (int i = 0; i < 10; i++)
            //                {
            //                    labels[i].Content = "--------";
            //                    labels[i].Visibility = Visibility.Visible;
            //                }
            //                lastRoundStop = false;

            //                lblRest.Content = dtTable.Rows.Count;
            //                lblPercent.Content = "20%  = ";
            //                lblNumToDraw.Content = totalNumToDraw;
            //                break;
            //            }

            //            drawingTime++;
            //            if (drawingTime == totalTimesToDraw && lastRound != 0)
            //            {
            //                thisRound = lastRound;
            //                for (int i = 9; i >= lastRound; i--)
            //                {
            //                    labels[i].Visibility = Visibility.Collapsed;
            //                }
            //            }
            //            timer.Start();
            //            isRolling = true;

            //        }
            //        break;
            //    case Awards.B:
            //        if (isRolling)
            //        {
            //            timer.Stop();
            //            // 保存数据
            //            // 从候选人员中减去
            //            SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\38.csv"));
            //            isRolling = false;
            //            if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
            //            {
            //                drawingAward = Awards.C;

            //                totalNumToDraw = 8;
            //                totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
            //                if (lastRound == 0) totalTimesToDraw--;
            //                drawingTime = 0;
            //                thisRound = 9;

            //                lastRoundStop = true;
            //            }
            //        }
            //        else
            //        {
            //            if (lastRoundStop)
            //            {
            //                for (int i = 0; i < 10; i++)
            //                {
            //                    labels[i].Content = "--------";
            //                    labels[i].Visibility = Visibility.Visible;
            //                }
            //                lastRoundStop = false;

            //                lblRest.Content = dtTable.Rows.Count;
            //                lblPercent.Content = "20%  = ";
            //                lblNumToDraw.Content = totalNumToDraw;
            //                break;
            //            }

            //            drawingTime++;
            //            if (drawingTime == totalTimesToDraw && lastRound != 0)
            //            {
            //                thisRound = lastRound;
            //                for (int i = 9; i >= lastRound; i--)
            //                {
            //                    labels[i].Visibility = Visibility.Collapsed;
            //                }
            //            }
            //            timer.Start();
            //            isRolling = true;

            //        }
            //        break;
            //    case Awards.C:
            //        if (isRolling)
            //        {
            //            timer.Stop();
            //            // 保存数据
            //            // 从候选人员中减去
            //            SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\48.csv"));
            //            isRolling = false;
            //            if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
            //            {
            //                drawingAward = Awards.D;

            //                //totalNumToDraw = allCandiNum - (int)Math.Ceiling((double)allCandiNum * 0.2) - (int)Math.Ceiling((double)allCandiNum * 0.2) - (int)Math.Floor((double)allCandiNum * 0.3);
            //                totalNumToDraw = 9;
            //                totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
            //                   if (lastRound == 0) totalTimesToDraw--;
            //                drawingTime = 0;
            //                thisRound = 10;

            //                lastRoundStop = true;
            //            }
            //        }
            //        else
            //        {
            //            if (lastRoundStop)
            //            {
            //                for (int i = 0; i < 10; i++)
            //                {
            //                    labels[i].Content = "--------";
            //                    labels[i].Visibility = Visibility.Visible;
            //                }
            //                lastRoundStop = false;

            //                lblRest.Content = dtTable.Rows.Count;
            //                lblPercent.Content = "30%  = ";
            //                lblNumToDraw.Content = totalNumToDraw;
            //                break;
            //            }

            //            drawingTime++;
            //            if (drawingTime == totalTimesToDraw && lastRound != 0)
            //            {
            //                thisRound = lastRound;
            //                for (int i = 9; i >= lastRound; i--)
            //                {
            //                    labels[i].Visibility = Visibility.Collapsed;
            //                }
            //            }
            //            timer.Start();
            //            isRolling = true;

            //        }
            //        break;
            //    case Awards.D:
            //        if (isRolling)
            //        {
            //            timer.Stop();
            //            // 保存数据
            //            // 从候选人员中减去
            //            SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\58.csv"));
            //            isRolling = false;
            //            if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
            //            {
            //                drawingAward = Awards.None;

            //                //totalNumToDraw = (int)Math.Floor((double)allCandiNum * 0.2);
            //                //totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
            //                //if (lastRound == 0) totalTimesToDraw--;
            //                //drawingTime = 0;
            //                //thisRound = 10;

            //                lastRoundStop = true;
            //            }
            //        }
            //        else
            //        {
            //            if (lastRoundStop)
            //            {
            //                for (int i = 0; i < 10; i++)
            //                {
            //                    labels[i].Content = "--------";
            //                    labels[i].Visibility = Visibility.Visible;
            //                }
            //                lastRoundStop = false;

            //                lblRest.Content = dtTable.Rows.Count;
            //                lblPercent.Content = "20%  = ";
            //                lblNumToDraw.Content = totalNumToDraw;
            //                break;
            //            }

            //            drawingTime++;
            //            if (drawingTime == totalTimesToDraw && lastRound != 0)
            //            {
            //                thisRound = lastRound;
            //                for (int i = 9; i >= lastRound; i--)
            //                {
            //                    labels[i].Visibility = Visibility.Collapsed;
            //                }
            //            }
            //            timer.Start();
            //            isRolling = true;

            //        }
            //        break;
            //    case Awards.None:
            //    default:
            //        break;
            //}

            #endregion

            switch (drawingAward2)
            {
                case Awards2.GrandPrize:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\0.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward2 = Awards2.NonePrize;

                            totalNumToDraw = 1;
                            totalTimesToDraw = 1;

                            drawingTime = 0;


                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            labels[0].Content = "********";
                            labels[0].Width = 750;
                            labels[0].Height = 150;
                            labels[0].FontSize = 80;

                            lastRoundStop = false;
                            lblAwards2.Content = "特等奖";
                            lblNum.Content = "前" + (drawingTime * 1).ToString() + "/" + totalNumToDraw.ToString() + "名";

                            //lblRest.Content = dtTable.Rows.Count;
                            //lblPercent.Content = "20%  = ";
                            //lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        thisRound = 1;
                        lblNum.Content = "前" + (drawingTime * 1).ToString() + "/" + totalNumToDraw.ToString() + "名";
                        timer.Start();
                        isRolling = true;

                    }


                    break;
                case Awards2.FirstPrize:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\1.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward2 = Awards2.GrandPrize;

                            totalNumToDraw = 1;
                            totalTimesToDraw = 1;

                            drawingTime = 0;


                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            labels[0].Content = "********";
                            for (int i = 1; i < 10; i++)
                            {
                                //labels[i].Content = "********";
                                labels[i].Visibility = Visibility.Collapsed;
                            }

                            labels[0].Width = 500;
                            labels[0].Height = 100;
                            labels[0].FontSize = 60;

                            lastRoundStop = false;
                            lblAwards2.Content = "一等奖";
                            lblNum.Content = "前" + (drawingTime * 1).ToString() + "/" + totalNumToDraw.ToString() + "名";

                            // 现在正在抽取多少名更新
                            //lblRest.Content = dtTable.Rows.Count;
                            //lblPercent.Content = "20%  = ";
                            //lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        thisRound = 1;
                        lblNum.Content = "前" + (drawingTime * 1).ToString() + "/" + totalNumToDraw.ToString() + "名";
                        timer.Start();
                        isRolling = true;

                    }


                    break;
                case Awards2.SecondPrize:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\2.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward2 = Awards2.FirstPrize;

                            totalNumToDraw = 5;
                            totalTimesToDraw = 5;       // 分5次抽取，每次1个

                            drawingTime = 0;


                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                labels[i].Content = "********";
                                labels[i].Visibility = Visibility.Visible;
                            }
                            for (int j = 5; j < 10; j++)
                            {
                                labels[j].Visibility = Visibility.Collapsed;
                            }
                            lastRoundStop = false;
                            lblAwards2.Content = "二等奖";
                            lblNum.Content = "前" + (drawingTime * 5).ToString() + "/" + totalNumToDraw.ToString() + "名";

                            // 现在正在抽取多少名更新
                            //lblRest.Content = dtTable.Rows.Count;
                            //lblPercent.Content = "20%  = ";
                            //lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        thisRound = 5;
                        lblNum.Content = "前" + (drawingTime * 5).ToString() + "/" + totalNumToDraw.ToString() + "名";
                        timer.Start();
                        isRolling = true;

                    }


                    break;
                case Awards2.ThirdPrize:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\3.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward2 = Awards2.SecondPrize;

                            totalNumToDraw = 10;
                            totalTimesToDraw = 2;       // 分两次抽取，每次5个

                            drawingTime = 0;


                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                labels[i].Content = "********";
                                labels[i].Visibility = Visibility.Visible;
                            }
                            lastRoundStop = false;
                            lblAwards2.Content = "三等奖";
                            lblNum.Content = "前" + (drawingTime * 10).ToString() + "/" + totalNumToDraw.ToString() + "名";

                            // 现在正在抽取多少名更新
                            //lblRest.Content = dtTable.Rows.Count;
                            //lblPercent.Content = "20%  = ";
                            //lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        thisRound = 10;
                        lblNum.Content = "前" + (drawingTime * 10).ToString() + "/" + totalNumToDraw.ToString() + "名";
                        timer.Start();
                        isRolling = true;

                    }


                    break;
                case Awards2.FourthPrize:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\4.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward2 = Awards2.ThirdPrize;

                            totalNumToDraw = 20;
                            totalTimesToDraw = 2;

                            drawingTime = 0;


                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                labels[i].Content = "********";
                                labels[i].Visibility = Visibility.Visible;
                            }
                            lastRoundStop = false;
                            lblAwards2.Content = "四等奖";
                            lblNum.Content = "前" + (drawingTime * 10).ToString() + "/" + totalNumToDraw.ToString() + "名";

                            // 现在正在抽取多少名更新
                            //lblRest.Content = dtTable.Rows.Count;
                            //lblPercent.Content = "20%  = ";
                            //lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        thisRound = 10;
                        lblNum.Content = "前" + (drawingTime * 10).ToString() + "/" + totalNumToDraw.ToString() + "名";
                        timer.Start();
                        isRolling = true;

                    }


                    break;
                case Awards2.FifthPrize:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\5.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward2 = Awards2.FourthPrize;

                            totalNumToDraw = 30;
                            totalTimesToDraw = 3;
                            
                            drawingTime = 0;

                            RemoveLowerRank();
                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                labels[i].Content = "********";
                                labels[i].Visibility = Visibility.Visible;
                            }
                            lastRoundStop = false;
                            lblAwards2.Content = "五等奖";
                            lblNum.Content = "前" + (drawingTime * 10).ToString() + "/" + totalNumToDraw.ToString() + "名";

                            // 现在正在抽取多少名更新
                            //lblRest.Content = dtTable.Rows.Count;
                            //lblPercent.Content = "20%  = ";
                            //lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        thisRound = 10;
                        lblNum.Content = "前" + (drawingTime * 10).ToString() + "/" + totalNumToDraw.ToString() + "名";
                        timer.Start();
                        isRolling = true;

                    }

                    break;
                case Awards2.NonePrize:
                    labels[0].Content = "抽奖完成";
                    break;
                default:
                    break;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            random = GetRandom(0, restCandiNum - 1, thisRound);

            for (int i = 0; i < thisRound; i++)
            {
                labels[i].Content = dtTable.Rows[random[i]]["ID"].ToString() + "---" + dtTable.Rows[random[i]]["Name"].ToString();
            }
        }

        private static int[] GetRandom(int min, int max, int count)
        {
            int[] maxArray = new int[max - min + 1];
            for (int i = 0; i < max - min + 1; i++)
                maxArray[i] = min + i;

            int[] rArray = new int[count];
            Random rd = new Random();
            int temp = max - min + 1;
            for (int i = 0; i < count; i++)
            {
                int tIndex = rd.Next(0, temp);
                rArray[i] = maxArray[tIndex];
                maxArray[tIndex] = maxArray[--temp];
            }
            return rArray;
        }

        enum Awards{ A, B, C, D, None }

        enum Awards2
        {
            GrandPrize = 0,
            FirstPrize = 1,
            SecondPrize = 2,
            ThirdPrize = 3,
            FourthPrize = 4,
            FifthPrize = 5,
            NonePrize = 6
        }

        void SaveToCsv(string csvName)
        {
            StreamWriter sw = new StreamWriter(csvName, true, Encoding.Default);
            string data = "";

            for (int i = 0; i < random.Count(); i++)
            {
                data = "";
                for (int j = 0; j < dtTable.Columns.Count; j++)
                {
                    data += dtTable.Rows[random[i]][j].ToString();
                    if (j < dtTable.Columns.Count - 1)
                        data += ",";
                }
                sw.WriteLine(data);
                dtTable.Rows[random[i]].Delete();
            }

            dtTable.AcceptChanges();
            restCandiNum = restCandiNum - random.Count();

            sw.Close();
        }

        void RemoveLowerRank()
        {
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                string tempStr = dtTable.Rows[i]["Rank"].ToString();
                //if (tempStr == "TTSR" || tempStr == "BPR")
                //    dtTable.Rows[i].Delete();
                if (string.Equals(tempStr, "TTSR") || string.Equals(tempStr, "BPR"))
                    dtTable.Rows[i].Delete();
            }
            dtTable.AcceptChanges();
            restCandiNum = dtTable.Rows.Count;
        }
    }
}
