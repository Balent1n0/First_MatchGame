using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace First_MatchGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        // Добавление таймера и два поля для отслеживания прошедшего времени и количества найденных совпадений
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        // обновляет новый элемент TextBlock истекшим временем и останавливает таймер после того, как игрок найдет все совпадения
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>() // Создает список из восьми пар эмодзи
            {
                "🐮","🐮","🐯","🐯","🐶","🐶","🐺","🐺","🦝","🦝","🐱","🐱","🦒","🦒","🐗","🐗",
            };
            Random random = new Random();                   // Создает новый генератор случайных чисел

            foreach ( TextBlock textBlock in mainGrid.Children.OfType<TextBlock>()) // Находит каждый элемент TextBlock в сетке и повторяет команды для каждого элемента 
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count); // Выбирает случайное число от 0 до количества эмодзи в списке и назначает ему имя «index»
                    string nextEmoji = animalEmoji[index];      // Использует случайное число с именем «index» для получения случайного эмодзи из списка
                    textBlock.Text = nextEmoji;                 // Обновляет TextBlock случайным эмодзи из списка
                    animalEmoji.RemoveAt(index);                // Удаляет случайный эмодзи из списка
                }
            }
            // запустить таймер и сбросить содержимое полей
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked; // создание переменной последнийКликнутыйЭмодзи
        bool findingMatch = false;      // признак определяет, кликнут ли первый эмодзи

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;                  // хз

            // Игрок только что щелкнул на первом животном в паре, поэтому это животное становится невидимым,
            // а соответствующий элемент TextBlock сохраняется на случай, если его придется делать видимым снова.
            if (findingMatch == false)                                  
            {
                textBlock.Visibility = Visibility.Hidden;               // спрятать эмодзи
                lastTextBlockClicked = textBlock;                       //
                findingMatch = true;                                    //
            }

            // Игрок нашел пару! Второе животное в паре становится невидимым (а при дальнейших щелчках на нем ничего не происходит),
            // а признак findingMatch сбрасывается, чтобы следующее животное, на котором щелкнет игрок, снова считалось первым в паре.
            else if (textBlock.Text == lastTextBlockClicked.Text)       
            {
                matchesFound++;                                         // чтобы значение matchesFound увеличивалось с каждой успешно найденной парой
                textBlock.Visibility = Visibility.Hidden;               //
                findingMatch = false;                                   //
            }

            // Игрок щелкнул на животном, которое не совпадает с первым, поэтому первое выбранное
            // животное снова становится видимым, а признак findingMatch сбрасывается
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;   //
                findingMatch = false;                                   //
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Сбрасывает игру, если были найдены все 8 пар (в противном случае ничего не делает, потому что игра еще продолжается) 
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
