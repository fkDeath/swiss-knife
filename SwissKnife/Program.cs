///ETML
///Auteur : Kyllian Gregoire
///Date   : 23.10.2026
///Description : Convertisseur de texte en code Morse (module 114). Le sens de
///              conversion est détecté automatiquement et le Morse est joué avec des bips

using System.Text;

namespace SwissKnife
{
    /// <summary>
    /// Programme console : convertit du texte en Morse (ou l'inverse) et joue le résultat
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Démarrage de l'application
        /// </summary>
        /// <param name="args">Arguments de la ligne de commande (non utilisés)</param>
        static void Main(string[] args)
        {
            // UTF-8 pour afficher correctement les accents
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Convertisseur de texte en code Morse ===");

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Entrez un mot ou une phrase (sans accents, lettres A-Z),");
                Console.WriteLine("ou du code Morse (points, tirets, espaces et / entre les mots).");
                Console.WriteLine("Entrée vide pour quitter :");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input.Length == 0)
                {
                    isRunning = false;
                }
                else
                {
                    Convert(input);
                }
            }

            Console.WriteLine("Au revoir !");
        }

        /// <summary>
        /// Détecte si la saisie est du texte ou du Morse, convertit dans l'autre sens,
        /// affiche le résultat puis joue le Morse en bips
        /// </summary>
        /// <param name="input">La saisie de l'utilisateur (non vide)</param>
        private static void Convert(string input)
        {
            string morse;

            if (MorseConverter.IsMorse(input))
            {
                // Morse détecté : on décode vers le texte
                morse = input;
                Console.WriteLine("Morse détecté. Résultat en texte :");
                ShowSuccess(MorseConverter.ConvertMorseToText(input));
            }
            else
            {
                // Texte détecté : on encode vers le Morse
                morse = MorseConverter.ConvertTextToMorse(input, out List<char> ignoredCharacters);
                Console.WriteLine("Texte détecté. Résultat en Morse :");
                ShowSuccess(morse);

                if (ignoredCharacters.Count > 0)
                {
                    ShowError("Caractères non pris en charge ignorés : " + string.Join(" ", ignoredCharacters));
                }
            }

            Console.WriteLine("Lecture sonore :");
            MorsePlayer.Play(morse);
        }

        /// <summary>
        /// Affiche un texte en vert (résultat)
        /// </summary>
        /// <param name="text">Le texte à afficher</param>
        private static void ShowSuccess(string text)
        {
            WriteColoredLine(text, ConsoleColor.Green);
        }

        /// <summary>
        /// Affiche un texte en rouge (erreur ou avertissement)
        /// </summary>
        /// <param name="text">Le texte à afficher</param>
        private static void ShowError(string text)
        {
            WriteColoredLine(text, ConsoleColor.Red);
        }

        /// <summary>
        /// Écrit une ligne dans la couleur demandée puis restaure la couleur d'origine
        /// </summary>
        /// <param name="text">Le texte à écrire</param>
        /// <param name="color">La couleur du texte</param>
        private static void WriteColoredLine(string text, ConsoleColor color)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previousColor;
        }
    }
}
