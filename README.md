# GLS-Przemyslaw-Kuczynski

Projekty GLS-API, MojaDrukarki oraz Azure Functions dla integracji z usługą GLS-API oraz drukarką. Pozwala na pobieranie informacji o przesyłkach oraz wysyłanie etykiet do drukarki.

## Wymagania

- [Visual Studio 2022](https://visualstudio.microsoft.com/pl/vs/)
- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools)

## Konfiguracja

1. Baza danych GLS-API po pierwszym uruchomienu tworzy się automatycznie przy uruchomieniu.
2. Baza danych pobranych paczek tworzy się automatycznie przy pierwszym odwołaniu do context.

## Uruchomienie lokalne

1. Uruchom aplikację wykorzystując Visual Studio 2022 dla lokalnej kompilacji.

## Struktura Projektu

Projekty GLS-API, MojaDrukarki oraz Azure Functions tworzą kompleksowe rozwiązanie do integracji z usługą GLS i obsługi drukarki. Poniżej znajdziesz krótki opis każdego z projektów:

1. **GLS-API**
    - Opis: Projekt zawiera logikę opartą na API GLS.
    - Zadania:
        - Obsługa żądań do API GLS, takich jak logowanie, pobieranie informacji o przesyłkach, czy pobieranie etykiet PDF.
        - Konfiguracja do obsługi wywołań z funkcji wyzwalacza oraz API HTTP.

2. **MojaDrukarka**
    - Opis: Projekt odpowiada za obsługę wydruku etykiet przesyłek.
    - Zadania:
        - Przygotowywanie plików PDF zawierających etykiety.
        - Wysyłanie plików PDF do drukarki za pomocą odpowiednich zapytań HTTP.

3. **Azure Functions (Triggers)**
    - Opis: Projekt zawiera funkcje wyzwalaczy, czyli logikę uruchamiającą się w określonych sytuacjach.
    - Zadania:
        - Pobieranie informacji o przesyłkach.
        - Wysyłanie etykiet do drukarki.
        - Wykorzystanie funkcji z projektów GLS-API i MojaDrukarka do kompleksowej obsługi procesu integracji.

## Funkcje Azure Triggers

### TimerTrigger

- Wywoływany co 10 minut.
- Pobiera informacje o przesyłkach.
- Zapisuje pdf etykiet do bazy danych dla paczek.

### HttpTrigger

- Wysyła etykiety do drukarki.
- Pobiera etykiety pdf z bazy i po wysłaniu do drukarki oznacza jako wydrukowaną.

## Autor

Przemysław Kuczyński
