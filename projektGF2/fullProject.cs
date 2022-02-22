using System;
using System.IO;

namespace Projektopgave
{
    class Program
    {
        // Konstant til at ændre stien til database-filen ét sted, så man ikke skal ændre det alle steder i koden
        private const String path = @"nummer.txt";

        // Variabel der indeholder database-filen
        private string[] database;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.StartMenu();
        }

        public void StartMenu()
        {
            // Hvis database-filen ikke findes, bliver den oprettet
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            /* Brugeroplysningerne fra databasen indlæses i et array.
             * Der bruges File.ReadAllLines - ikke File.ReadAllText - da linjerne skal i array og læses individuelt.
             */
            database = File.ReadAllLines(path);

            Console.Clear();
            Console.WriteLine("Velkommen til B&B's hovedmenu.\n");
            Console.WriteLine("Du har nu følgende valgmuligheder:\n");

            // Bruger bliver præsenteret for muligheden for at oprette sig og søge i databasen
            char funktion;
            do
            {
                Console.WriteLine("[O] Opret Bruger [N] Find Navn  [T] Find Telefon  [V] Vis Alle  [Q] Afslut :");
                Console.Write("\nVælg funktion: ");
                funktion = Console.ReadKey().KeyChar;
                funktion = Char.ToUpper(funktion);
                Console.WriteLine("");
            } while (!funktion.Equals('O') && !funktion.Equals('N') && !funktion.Equals('T') && !funktion.Equals('V') && !funktion.Equals('Q'));

            // Afhængig af brugerens valg, køres forskellige funktioner
            switch (funktion)
            {
                case 'O':
                    OpretBruger();
                    break;

                case 'N':
                    FindBrugerNavn();
                    break;

                case 'T':
                    FindBrugerTelefon();
                    break;

                case 'V':
                    VisAlle();
                    break;

                default:
                    Environment.Exit(1);
                    break;
            }
        }
        public void VisAlle()
        {
            Console.Clear();
            Console.WriteLine("Du får nu en liste over alle brugere 15 linjer ad gangen\n");
            Console.WriteLine("Viser de første 15 brugere.");

            // Counter, der holder styr på hvor mange linjer/brugere, der er gennemgået.
            int tæller = 0;

            // Udskriver alle linje/bruger fra tekstfilen.
            foreach (string bruger in database)
            {
                Console.WriteLine("\n" + bruger);

                // Efter der er udskrevet 15 linjer, pauses foreach-loopet og brugeren skal taste for at se næste side
                if (++tæller % 15 == 0)
                {
                    Console.WriteLine("\nTryk Enter for at se de næste 15 brugere...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            // Meddél når der ikke er flere linjer/brugere at vise.
            Console.WriteLine("\nIkke flere brugere.");
            // Programmet vender tilbage til StartMenu
            TilbageTilStart();
        }
        public void FindBrugerNavn()
        {
            Console.Clear();
            Console.WriteLine("Du søger nu efter brugere med et bestemt navn\n");

            // Variabel til at gemme ALLE fundne brugere
            String brugerListe = "";

            // Variabel til at angive om en bruger er blevet fundet eller ej
            Boolean brugerFundet = false;

            Console.Write("Indtast navn: ");
            String navn = Console.ReadLine();

            // Tjek om navn er gyldigt
            if (navn.Length <= 0 || !IsAllLetters(navn))
            {
                Console.WriteLine("Indtast et gyldigt navn.");
            }
            else
            {
                /* Tjek hver bruger i database-filen én ad gangen 
                 * for at finde ud af om det indtastede navn allrede findes
                 */
                foreach (string line in database)
                {
                    string[] brugerData = line.Split(',');
                    String brugerNavn = brugerData[1];

                    // Hvis en bruger med navnet findes, tilføjes brugeren til en streng
                    if (navn.Equals(brugerNavn))
                    {
                        brugerListe += "\n" + line;
                        brugerFundet = true;
                    }
                }
            }
            Console.Clear();
            // Udskriv fejlmelddelse hvis ingen brugere er fundet
            if (!brugerFundet)
            {
                Console.WriteLine("Ingen bruger fundet med dette navn: " + navn);
            }
            // Udskriv alle fundne brugere
            else
            {
                Console.WriteLine("Fundne brugere: ");
                Console.WriteLine(brugerListe);
            }
            TilbageTilStart();
        }
        public void FindBrugerTelefon()
        {
            Console.Clear();
            Console.WriteLine("Du søger nu efter et telefonnummer\n");

            String telefonnummer;

            // Variabel til at angive om et telefonnummer er blevet fundet eller ej
            Boolean telefonFundet = false;

            // Tjek om postnummer er gyldigt
            do
            {
                // Tjek om telefonnummeret er på 8 tegn og alle tegnene er tal.
                Console.Write("Indtast telefonnummer: ");
                telefonnummer = Console.ReadLine();
            } while (telefonnummer.Length != 8 || !IsAllDigits(telefonnummer));

            /* Tjek hver bruger i database-filen én ad gangen 
            * for at finde ud af om det indtastede telefonnummer allrede findes
            */
            foreach (string line in database)
            {
                /* Brugers telefonnummer indhentes ved at separere brugerens data ved komma
                 * og telefonnummeret findes i første indeks
                 */
                string[] brugerData = line.Split(',');
                String brugerTelefon = brugerData[0];

                // Hvis telefonnummeret allerede findes i database-filen udskrives den fundne bruger
                if (telefonnummer.Equals(brugerTelefon))
                {
                    Console.Clear();
                    Console.WriteLine("Bruger fundet: \n\n" + line);
                    telefonFundet = true;
                    break;
                }
            }

            // Hvis telefonnummeret allerede findes i database-filen udskrives en fejlmeddelelse
            if (!telefonFundet)
            {
                Console.Clear();
                Console.WriteLine("Ingen bruger fundet med dette telefonnummer: " + telefonnummer);
            }
            // Programmet vender tilbage til StartMenu
            TilbageTilStart();

        }
        public void OpretBruger()
        {
            Console.Clear();
            Console.WriteLine("Du er nu ved at oprette en ny bruger.\n");

            String telefonnummer;

            // Variabel til at angive om et telefonnummer overholder reglerne og dermed er gyldigt
            Boolean gyldig = true;

            // Tjek om telefonnummeret er på 8 tegn og alle tegnene er tal.
            do
            {
                Console.Write("Indtast telefonnummer: ");
                telefonnummer = Console.ReadLine();
            } while (telefonnummer.Length != 8 || !IsAllDigits(telefonnummer));

            /* Tjek hver bruger i database-filen én ad gangen 
             * for at finde ud af om det indtastede telefonnummer allrede findes
             */
            foreach (string bruger in database)
            {
                /* Brugers telefonnummer indhentes ved at separere brugerens data ved komma
                 * og telefonnummeret findes i første indeks
                 */
                string[] brugerData = bruger.Split(',');
                String brugerTelefon = brugerData[0];

                // Hvis telefonnummeret allerede findes i database-filenudskrives en fejlmeddelelse
                if (telefonnummer.Equals(brugerTelefon))
                {
                    Console.Clear();
                    Console.WriteLine("Telefonnummeret findes allerede. Prøv igen.");
                    gyldig = false;
                    break;
                }
            }

            // Hvis telefonnummeret ikke findes i database-filen kan personen oprette sig som bruger
            if (gyldig)
            {
                Console.Clear();
                Console.WriteLine("OK! Kan oprettes");
                using (StreamWriter file = new StreamWriter(path))
                {
                    String navn, efternavn, adresse, postnummer, by, email;

                    // Tjek om navn er længere end 0 tegnog alle tegnene er bogstaver
                    do
                    {
                        Console.Write("Indtast fornavn: ");
                        navn = Console.ReadLine();
                    } while (navn.Length <= 0 || !IsAllLetters(navn));

                    // Tjek om efternavn er længere end 0 tegn og alle tegnene er bogstaver
                    do
                    {
                        Console.Write("Indtast efternavn: ");
                        efternavn = Console.ReadLine();
                    } while (efternavn.Length <= 0 || !IsAllLetters(efternavn));

                    // Adresse
                    Console.Write("Indtast vejnavn og -nummer: ");
                    adresse = Console.ReadLine();

                    // Tjek om postnummer er på 4 tegn og alle tegnene er tal
                    do
                    {
                        Console.Write("Indtast postnummer: ");
                        postnummer = Console.ReadLine();
                    } while (postnummer.Length != 4 || !IsAllDigits(postnummer));

                    // Tjek om bynavn er længere end 0 tegn og alle tegnene er bogstaver
                    do
                    {
                        Console.Write("Indtast by: ");
                        by = Console.ReadLine();
                    } while (by.Length <= 0 || !IsAllLetters(by));

                    // Tjek om email er gyldig
                    do
                    {
                        Console.Write("Indtast email: ");
                        email = Console.ReadLine();
                    } while (!isEmail(email));

                    // Gem bruger i database
                    file.Write("{0},{1},{2},{3},{4},{5},{6}", telefonnummer, navn, efternavn, adresse, postnummer, by, email);
                    file.WriteLine();
                    Console.WriteLine("\nDu er nu oprettet som bruger!");
                }
            }
            // Programmet vender tilbage til StartMenu
            TilbageTilStart();

        }
        // Hjælpefunktion til at teste om alle tegnene i en string udelukkende består af tal.
        public static bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
        // Hjælpefunktion til at teste om alle tegnene i en string udelukkende består af bogstaver.
        public static bool IsAllLetters(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                    return false;
            }
            return true;
        }
        /* Hjælpefunktion til at teste om en email kan være gyldig.
         * Skal indeholde mindst 5 tegn, '@' og '.' 
         */
        public static bool isEmail(string email)
        {
            if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
            {
                return false;
            }
            return true;
        }
        // Programmet vender tilbage til StartMenu
        public void TilbageTilStart()
        {
            Console.WriteLine("\nTryk på en vilkårlig tast for at gå tilbage til startmenu...");
            Console.ReadKey();
            StartMenu();
        }
    }
}