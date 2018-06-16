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

            //UI is built for 2-player. Code is scalable.
            int numPlayers = 2;

            List<Card> allCards = initCards();
            List<Card> shuffled = shuffle(allCards);

            List<Card>[] playerDecks = new List<Card>[numPlayers];
            playerDecks = dealCards(shuffled, numPlayers);

            //for (int i=0; i<numPlayers; i++)
            //{
            //    Console.WriteLine("Player 1 Deck:");
            //    for (int j=0; j<playerDecks[0].Count; j++)
            //    {
            //        Console.WriteLine(playerDecks[0][j].number + " " + playerDecks[0][j].suit);
            //    }
            //}

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

            //constructs a copy of the unshuffled cards so 'cards' is unmodified
            List<Card> shuffledCards = new List<Card>(cards); 

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

        private List<Card>[] dealCards(List<Card> cards, int numDecks)
        {
            Console.WriteLine(numDecks);
            List<Card> undealtCards = new List<Card>(cards);
            List<Card>[] decks = new List<Card>[numDecks];
            for (int i=0; i<numDecks; i++)
            {
                decks[i] = new List<Card>();
            }

            int d = 0;
            int totalCards = undealtCards.Count;

            Console.WriteLine("Number of decks: " + decks.Length);
            
            for (int u=0; u<totalCards; u++)
            {
                //Deal card 0 (top of deck) to player (last card in deck)
                Console.WriteLine(undealtCards[0].number + " " + undealtCards[0].suit);

                decks[d].Add(undealtCards[0]);
                undealtCards.RemoveAt(0);
                Console.WriteLine(decks[d].Last().number + " " + decks[d].Last().suit);

                Console.WriteLine("Cards not dealt: " + undealtCards.Count);
                Console.WriteLine("Cards dealt: " + (u+1));
                Console.WriteLine("Total Cards: " + totalCards);
                //Change deck that next card is dealt to
                d = (d < numDecks - 1) ? d+1 : 0;
                Console.WriteLine("d = " + d);
            }

            for (int i = 0; i < decks.Length; i++)
            {
                Console.WriteLine("Deck " + i + ":");
                for (int j = 0; j < decks[i].Count; j++)
                {
                    Console.WriteLine(decks[0][j].number + " " + decks[0][j].suit);
                }

                Console.WriteLine("Count for Deck " + i + ": " + decks[i].Count);
            }
            return decks;
        }
    }
}
