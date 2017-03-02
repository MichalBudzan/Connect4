using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class Program
    {
        static void Main(string[] args)
        {


            bool czlowiek = true;     // zaczyna gre czlowiek 
            

            Connect4.Witaj();
            Connect4.Rysuj_plansze();

            do
            {
                //Sleep(1000);
                Connect4.Czyja_kolej();

                if (czlowiek)
                {    // zaczyna czlowiek
                    Connect4.Ruch_czlowieka();
                    czlowiek = false;
                }
                else
                {
                    Connect4.Ruch_komputera();
                    czlowiek = true;
                }

                Console.Write("koniec");

                Connect4.Rysuj_plansze();
            }
            while (!Connect4.Znajdz_wygranego(true) && !Connect4.Czy_plansza_pelna() && Connect4.Czy_mozna_wygrac());

            Console.ReadKey();
            Console.ReadKey();
            

        }
    }
}
