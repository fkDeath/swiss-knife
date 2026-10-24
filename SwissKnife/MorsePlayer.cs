///ETML
///Auteur : Kyllian Gregoire
///Date   : 23.10.2026
///Description : Lecture sonore d'un code Morse : bip court pour le point,
///              bip long pour le tiret, silences entre les symboles, lettres et mots

namespace SwissKnife
{
    /// <summary>
    /// Joue un code Morse avec le haut-parleur de la console
    /// </summary>
    public static class MorsePlayer
    {
        // Durées en millisecondes (le point est l'unité de base)
        private const int DOT_DURATION = 120;
        private const int DASH_DURATION = DOT_DURATION * 3;
        private const int SYMBOL_PAUSE = DOT_DURATION;
        private const int LETTER_PAUSE = DOT_DURATION * 3;
        private const int WORD_PAUSE = DOT_DURATION * 7;

        // Fréquence du bip en hertz
        private const int BEEP_FREQUENCY = 800;

        /// <summary>
        /// Joue un code Morse (lettres séparées par un espace, mots par "/")
        /// en affichant chaque symbole au moment où il est joué
        /// </summary>
        /// <param name="morse">Le code Morse à jouer</param>
        public static void Play(string morse)
        {
            foreach (char symbol in morse)
            {
                Console.Write(symbol);

                switch (symbol)
                {
                    case '.':
                        Beep(DOT_DURATION);
                        Thread.Sleep(SYMBOL_PAUSE);
                        break;
                    case '-':
                        Beep(DASH_DURATION);
                        Thread.Sleep(SYMBOL_PAUSE);
                        break;
                    case '/':
                        Thread.Sleep(WORD_PAUSE);
                        break;
                    default:
                        Thread.Sleep(LETTER_PAUSE);
                        break;
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Émet un bip de la durée demandée. Sur Windows, la fréquence et la durée
        /// sont contrôlées ; ailleurs, on envoie le signal sonore standard du terminal
        /// </summary>
        /// <param name="duration">La durée du bip en millisecondes</param>
        private static void Beep(int duration)
        {
            if (OperatingSystem.IsWindows())
            {
                Console.Beep(BEEP_FREQUENCY, duration);
            }
            else
            {
                Console.Beep();
                Thread.Sleep(duration);
            }
        }
    }
}
