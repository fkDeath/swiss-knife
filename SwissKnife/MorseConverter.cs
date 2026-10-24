///ETML
///Auteur : Kyllian Gregoire
///Date   : 23.10.2026
///Description : Conversion d'un texte (A-Z, 0-9, espace) en code Morse, décodage
///              inverse et détection automatique du sens de conversion

using System.Text;

namespace SwissKnife
{
    /// <summary>
    /// Convertit du texte en code Morse (points et tirets) et inversement
    /// </summary>
    public static class MorseConverter
    {
        // Séparateurs utilisés dans la représentation Morse
        public const char LETTER_SEPARATOR = ' ';
        public const string WORD_SEPARATOR = "/";

        // Symbole affiché pour un code Morse inconnu lors du décodage
        public const char UNKNOWN_SYMBOL = '?';

        // Table de correspondance caractère -> Morse (cahier des charges n°1)
        private static readonly Dictionary<char, string> _morseCodes = new Dictionary<char, string>()
        {
            { 'A', ".-" },    { 'B', "-..." },  { 'C', "-.-." },  { 'D', "-.." },   { 'E', "." },
            { 'F', "..-." },  { 'G', "--." },   { 'H', "...." },  { 'I', ".." },    { 'J', ".---" },
            { 'K', "-.-" },   { 'L', ".-.." },  { 'M', "--" },    { 'N', "-." },    { 'O', "---" },
            { 'P', ".--." },  { 'Q', "--.-" },  { 'R', ".-." },   { 'S', "..." },   { 'T', "-" },
            { 'U', "..-" },   { 'V', "...-" },  { 'W', ".--" },   { 'X', "-..-" },  { 'Y', "-.--" },
            { 'Z', "--.." },
            { '0', "-----" }, { '1', ".----" }, { '2', "..---" }, { '3', "...--" }, { '4', "....-" },
            { '5', "....." }, { '6', "-...." }, { '7', "--..." }, { '8', "---.." }, { '9', "----." }
        };

        // Table inverse Morse -> caractère, construite à partir de la première
        private static readonly Dictionary<string, char> _lettersByMorse = BuildReverseTable();

        // Caractères autorisés dans une saisie considérée comme du Morse
        private const string MORSE_CHARACTERS = ".-/ ";

        /// <summary>
        /// Détecte si une saisie est du code Morse : elle ne contient que des points,
        /// des tirets, des espaces et des "/" et au moins un point ou un tiret
        /// </summary>
        /// <param name="input">La saisie à analyser</param>
        /// <returns>true si c'est du Morse, false si c'est du texte</returns>
        public static bool IsMorse(string input)
        {
            bool hasSymbol = false;

            foreach (char character in input)
            {
                if (MORSE_CHARACTERS.IndexOf(character) < 0)
                {
                    return false;
                }

                if (character == '.' || character == '-')
                {
                    hasSymbol = true;
                }
            }

            return hasSymbol;
        }

        /// <summary>
        /// Tente de convertir un seul caractère en Morse
        /// </summary>
        /// <param name="letter">Le caractère à convertir (la casse est ignorée)</param>
        /// <param name="morseCode">Le code Morse obtenu, ou chaîne vide si non pris en charge</param>
        /// <returns>true si le caractère est pris en charge, false sinon</returns>
        public static bool TryConvertLetterToMorse(char letter, out string morseCode)
        {
            char upperLetter = char.ToUpperInvariant(letter);

            if (_morseCodes.TryGetValue(upperLetter, out string? code))
            {
                morseCode = code;
                return true;
            }

            morseCode = string.Empty;
            return false;
        }

        /// <summary>
        /// Convertit un texte complet en Morse. Les lettres sont séparées par un
        /// espace et les mots par "/". Les caractères non pris en charge sont ignorés
        /// </summary>
        /// <param name="text">Le texte à convertir</param>
        /// <param name="ignoredCharacters">Reçoit la liste des caractères ignorés</param>
        /// <returns>Le texte en Morse</returns>
        public static string ConvertTextToMorse(string text, out List<char> ignoredCharacters)
        {
            List<string> morseTokens = new List<string>();
            ignoredCharacters = new List<char>();

            foreach (char character in text.Trim())
            {
                if (char.IsWhiteSpace(character))
                {
                    morseTokens.Add(WORD_SEPARATOR);
                }
                else if (TryConvertLetterToMorse(character, out string morseCode))
                {
                    morseTokens.Add(morseCode);
                }
                else
                {
                    ignoredCharacters.Add(character);
                }
            }

            return string.Join(LETTER_SEPARATOR, morseTokens);
        }

        /// <summary>
        /// Convertit un texte complet en Morse, sans renvoyer les caractères ignorés
        /// </summary>
        /// <param name="text">Le texte à convertir</param>
        /// <returns>Le texte en Morse</returns>
        public static string ConvertTextToMorse(string text)
        {
            return ConvertTextToMorse(text, out List<char> _);
        }

        /// <summary>
        /// Convertit un code Morse en texte. Les lettres doivent être séparées par
        /// un espace et les mots par "/". Un code inconnu devient '?'
        /// </summary>
        /// <param name="morse">Le Morse à décoder</param>
        /// <returns>Le texte décodé, en majuscules</returns>
        public static string ConvertMorseToText(string morse)
        {
            StringBuilder text = new StringBuilder();
            string[] tokens = morse.Split(LETTER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                if (token == WORD_SEPARATOR)
                {
                    text.Append(' ');
                }
                else if (_lettersByMorse.TryGetValue(token, out char letter))
                {
                    text.Append(letter);
                }
                else
                {
                    text.Append(UNKNOWN_SYMBOL);
                }
            }

            return text.ToString();
        }

        /// <summary>
        /// Construit la table inverse (Morse -> caractère) à partir de la table principale
        /// </summary>
        /// <returns>Le dictionnaire inverse</returns>
        private static Dictionary<string, char> BuildReverseTable()
        {
            Dictionary<string, char> reverseTable = new Dictionary<string, char>();

            foreach (KeyValuePair<char, string> entry in _morseCodes)
            {
                reverseTable[entry.Value] = entry.Key;
            }

            return reverseTable;
        }
    }
}
