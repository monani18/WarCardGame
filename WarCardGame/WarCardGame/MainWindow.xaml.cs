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
        List<Card> allCards { get; set; }
        List<Card> shuffled { get; set; }
        List<Card>[] playerDecks { get; set; }
        
        //UI is built for 2-player. Code is scalable.
        int numPlayers { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Start Game pressed!");

            //UI: Hide "Start Game".
            //UI: Make "Go!" button green.
            //UI: "LET THE GAMES BEGIN! Click Go! to begin the war."
            //UI: Show two card decks face-down.

            //UI is built for 2-player. Code is scalable.
            numPlayers = 2;

            allCards = initCards();
            shuffled = shuffle(allCards);

            playerDecks = new List<Card>[numPlayers];
            playerDecks = dealCards(shuffled, numPlayers);

            for (int i = 0; i < playerDecks.Length; i++)
            {
                Console.WriteLine("Deck " + i + ":");
                for (int j = 0; j < playerDecks[i].Count; j++)
                {
                    Console.WriteLine(playerDecks[i][j].number + " " + playerDecks[i][j].suit);
                }

                Console.WriteLine("Count for Deck " + i + ": " + playerDecks[i].Count);
            }
        }

        private void Battle_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Battle button pressed!");

            List<Card> battleCards = new List<Card>();
            
            //Collect cards for battle and keep them in order
            for (int i=0; i<numPlayers; i++)
            {
                Console.WriteLine(playerDecks[i][0].number + " " + playerDecks[i][0].suit);
                battleCards.Add(playerDecks[i][0]);
            }

            for (int i=0; i<battleCards.Count; i++)
            {
                Console.WriteLine(battleCards[i].number + " " + battleCards[i].suit);
            }

            //Find victor or declare war
            //Give cards to victor

            //int victor = findVictor(

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
            List<Card> undealtCards = new List<Card>(cards);
            List<Card>[] decks = new List<Card>[numDecks];
            for (int i=0; i<numDecks; i++)
            {
                decks[i] = new List<Card>();
            }

            int d = 0;
            int totalCards = undealtCards.Count;
            
            for (int u=0; u<totalCards; u++)
            {
                //Deal card 0 (top of deck) to player (last card in deck)
                decks[d].Add(undealtCards[0]);
                undealtCards.RemoveAt(0);
                
                //Change deck that next card is dealt to
                d = (d < numDecks - 1) ? d+1 : 0;
            }

            return decks;
        }
    }
}
