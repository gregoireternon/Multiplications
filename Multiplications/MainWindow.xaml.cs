using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Multiplications
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int val1,val2=0;
        int tempassse=0;
        bool isstarted = false;
        Random rand = null;
        long[,] doneTable = { { 0,0,0,0,0,0,0,0,0} ,
            { 0,0,0,0,0,0,0,0,0} ,
            { 0,0,0,0,0,0,0,0,0} ,
            { 0,0,0,0,0,0,0,0,0} ,
            { 0,0,0,0,0,0,0,0,0} ,
            { 0,0,0,0,0,0,0,0,0} ,
            { 0,0,0,0,0,0,0,0,0} ,
            { 0,0,0,0,0,0,0,0,0} ,
            { 0,0,0,0,0,0,0,0,0} };
        //long[,] doneTable = { { 1,1,1,1,1,1,1,1,1} ,
        //    { 1,1,1,1,1,1,1,0,1} ,
        //    { 1,1,1,1,1,1,1,1,1} ,
        //    { 1,1,1,1,1,1,1,1,1} ,
        //    { 1,1,1,1,1,1,1,1,1} ,
        //    { 1,1,1,1,1,1,1,1,1} ,
        //    { 1,1,1,1,1,1,1,1,1} ,
        //    { 1,1,1,1,1,1,1,1,1} ,
        //    { 1,1,1,1,1,1,1,1,1} };
        Stopwatch currentStopwatch = null;

        private void valider_Click(object sender, RoutedEventArgs e)
        {
            int reponseValue = 0;
            if (isstarted)
            {
                try
                {
                    reponseValue = int.Parse(reponse.Text);
                }catch(Exception ex)
                {
                    message.Content = ex.Message;
                }
                if (reponseValue!=val1*val2)
                {
                    message.Content = "Mauvaise réponse";
                    return;
                }
                else
                {
                    message.Content = "";
                    reponse.Text = "";
                    doneTable[val1-1, val2-1] = currentStopwatch.ElapsedMilliseconds;
                }
            }

            currentStopwatch = new Stopwatch();
            currentStopwatch.Start();

            
            if (!isstarted)
            {
                this.horloge.Content = "0ms";
                this.valider.Content = "Valider";
                isstarted = true;
            }
            try
            {
                InitVals();
            }
            catch (GameFinishException ee)
            {
                currentStopwatch.Stop();
            }
            nbr1.Content = val1;
            nbr2.Content = val2;
        }

        private void InitVals()
        {
            val1 = rand.Next(1, 9);
            val2 = rand.Next(1, 9);
            if (doneTable[val1-1, val2-1]!=0)
            {
                CheckRemaining();
            }
        }

        private void CheckRemaining()
        {
            long timeTotal = 0;
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (doneTable[i, j]==0)
                    {
                        val1 = i + 1;
                        val2 = j + 1;
                        return;
                    }
                    else
                    {
                        timeTotal += doneTable[i, j];
                    }
                }
            }

            message.Content = $"Le jeu est fini! duree moyenne: {timeTotal/(9*9)}ms";
            throw new GameFinishException();
        }

        private void Timer_Tick(object? sender, ElapsedEventArgs e)
        {
            if (currentStopwatch != null)
            {
                horloge.Dispatcher.BeginInvoke(new Action(() =>
                {
                    horloge.Content = currentStopwatch.Elapsed.TotalMilliseconds/1000;
                }));
            }
        }

        private void reponse_GotFocus(object sender, RoutedEventArgs e)
        {
            this.reponse.Text = "";
        }

        private void reponse_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                valider_Click(null, null);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            rand = new Random(DateTime.Now.Millisecond);
            Task t = Task.Run(() =>
            {
                var timer = new Timer()
                {
                    Interval = 10,
                    Enabled = true
                };
                timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
                timer.Start();
            });
        }
    }
}
