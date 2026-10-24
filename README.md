# Module 114 : Codification du Morse

Application console C# (.NET 8) réalisée selon le cahier des charges
`i114_cdc_01_morse.pdf` et les conventions techniques de l'ETML.

## Fichiers

- `SwissKnife/Program.cs` : saisie, affichage du résultat et lecture sonore.
- `SwissKnife/MorseConverter.cs` : table de correspondance lettre -> Morse,
  conversion `ConvertTextToMorse`, décodage `ConvertMorseToText` et détection `IsMorse`.
- `SwissKnife/MorsePlayer.cs` : lecture sonore (bip court = point, bip long = tiret).
- `SwissKnife.Tests/` : tests unitaires xUnit (SOS, Bienvenue, espaces, caractères ignorés).

## Lancer

```bash
dotnet run --project SwissKnife
dotnet test
```

## Règles appliquées

- Détection automatique : une saisie composée uniquement de `.`, `-`, espaces et `/`
  est décodée en texte ; tout autre texte est converti en Morse.
- Le Morse est ensuite joué en bips (fréquence et durée contrôlées sous Windows ;
  sur Linux/macOS, c'est le signal sonore standard du terminal, qui doit être activé).
- Lettres A-Z et chiffres 0-9 convertis, majuscules/minuscules ignorées.
- Les espaces entre les mots sont représentés par `/`.
- Les caractères non pris en charge (accents, ponctuation) sont ignorés et
  signalés en rouge, sans faire planter le programme.
- Conventions ETML : entête `///ETML`, identifiants en anglais, constantes en
  MAJUSCULES, pas de `var`, commentaires XML sur chaque classe et méthode.
