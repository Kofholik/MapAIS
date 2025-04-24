# MapAIS

## Opis projektu
Apkalikacja2 to aplikacja desktopowa napisana w języku C# z wykorzystaniem biblioteki GMap.NET. Aplikacja umożliwia wizualizację danych AIS (Automatic Identification System) na mapie, dekodowanie wiadomości AIS oraz wyświetlanie szczegółowych informacji o statkach.

## Funkcjonalności
- **Odczyt MMSI**: Użytkownik może wprowadzić numer MMSI (Maritime Mobile Service Identity) w celu identyfikacji statku.
- **Dekodowanie wiadomości AIS**: Aplikacja obsługuje różne typy wiadomości AIS (np. typ 1, 3, 4) i wyświetla szczegółowe dane, takie jak pozycja, prędkość, kurs, status statku itp.
- **Wizualizacja na mapie**: Pozycje statków są wyświetlane na mapie z wykorzystaniem markerów (niebieski dla statku użytkownika, czerwony dla innych statków).
- **Obsługa plików danych**: Aplikacja odczytuje dane AIS z plików tekstowych (`kom_sensor_ais.txt` i `kom_sensor_ais2.txt`).

## Wymagania systemowe
- **.NET Framework**: 4.7.2
- **Biblioteki zewnętrzne**:
  - [GMap.NET](https://github.com/radioman/greatmaps)

## Instalacja
1. Sklonuj repozytorium: git clone https://github.com/Kofholik/MapAIS.git
2. Otwórz projekt w Visual Studio 2022.
3. Upewnij się, że wszystkie wymagane biblioteki są zainstalowane (np. GMap.NET).
4. Skonfiguruj pliki danych w folderze `Resources`:
   - `kom_sensor_ais.txt`
   - `kom_sensor_ais2.txt`

## Użycie
1. Uruchom aplikację.
2. Wprowadź numer MMSI w polu tekstowym.
3. Kliknij przycisk, aby załadować dane z jednego z plików:
   - **Przycisk 1**: Ładuje dane z `kom_sensor_ais.txt`.
   - **Przycisk 2**: Ładuje dane z `kom_sensor_ais2.txt`.
4. Dane zostaną zdekodowane i wyświetlone w interfejsie użytkownika, a pozycje statków zostaną zaznaczone na mapie.

## Struktura projektu
- **Form1.cs**: Główna logika aplikacji, w tym obsługa mapy, dekodowanie wiadomości AIS oraz interakcja z interfejsem użytkownika.
- **Resources**: Folder zawierający pliki danych AIS.

## Przykładowe dane
Pliki `kom_sensor_ais.txt` i `kom_sensor_ais2.txt` powinny zawierać dane AIS w formacie NMEA, np.: !AIVDM,1,1,,A,13aG?P0000G?tOK0@<0?vN0<0,0*5C

## Cel projektu
Projekt został stworzony w ramach przedmiotu **Informatyka w Nawigacji** na studiach, aby zaprezentować praktyczne zastosowanie technologii AIS w wizualizacji danych nawigacyjnych.

## Autor
Projekt został stworzony jako przykład aplikacji wykorzystującej dane AIS i bibliotekę GMap.NET.

## Licencja
Ten projekt jest udostępniany na licencji MIT. Szczegóły znajdują się w pliku `LICENSE`.

   
