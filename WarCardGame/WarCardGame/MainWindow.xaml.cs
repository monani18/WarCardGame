using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WarCardGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Start Game pressed!");
            int numPlayers = 2; //assume 2 players

            List<Card> allCards = initCards();
            List<Card> shuffled = shuffle(allCards);

            Console.WriteLine("Unshuffled:");
            for (int i = 0; i<allCards.Count; i++)
            {
                Console.WriteLine(allCards[i].number + " " + allCards[i].suit);
            }
            Console.WriteLine("Shuffled:");
            for (int i = 0; i < shuffled.Count; i++)
            {
                Console.WriteLine(shuffled[i].number + " " + shuffled[i].suit);
            }

            //var lists = dealCards(allCards, numPlayers);

            //Console.WriteLine(rand.Next(0, allCards.Count)); //inclusive min 0, exclusive max 52

            //List<Card> p1cards = lists.Item1;
            //List<Card> p2cards = lists.Item2;

        }

        private void Battle_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Battle button pressed!");
        }

        private List<Card> initCards()
        {
            //A=14 K=13 Q=12 J=11 10=10... 2=2
            int[] numbers = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }; 
            char[] suits = { 'D', 'H', 'S', 'C' };

            List<Card> allCards = new List<Card>();

            for (int i = 0; i < numbers.Length ; i++)
            {
                for (int j = 0; j < suits.Length; j++)
                {
                    Card card = new Card();
                    card.number = numbers[i];
                    card.suit = suits[j];
                    allCards.Add(card);
                }
            }
            return (allCards);
        }

        //Shuffles cards using Fischer-Yates method
        //https://www.coursera.org/learn/algorithms-part1/lecture/12vcF/shuffling
        //https://stackoverflow.com/questions/273313/randomize-a-listt
        private static List<Card> shuffle(List<Card> cards)
        {
            Random rand = new Random();

            List<Card> shuffledCards = new List<Card>(cards); //constructs a copy of the unshuffled cards

            int numCards = shuffledCards.Count;
            for (int i = 0; i < numCards; i++)
            {
                int r = rand.Next(0, numCards);
                Card temp = shuffledCards[r];
                shuffledCards[r] = shuffledCards[i]; //replaces rth card with 0th card
                shuffledCards[i] = temp; //replaces 0th card with former rth card
            }
            return shuffledCards;
        }

        //private Tuple<List<Card>, List<Card>> dealCards(List<Card> allCards, int numPlayers)
        //{
        //    List<Card> p1Cards;
        //    List<Card> p2Cards;

        //    Split card deck in numPlayers number of decks

        //    return (p1Cards, p2Cards);
        //}
    }
}
