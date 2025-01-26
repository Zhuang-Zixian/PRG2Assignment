//==================================
// Student Number : S10270525J
// Student Number : S10268018C
// Student Name   : Zhuang Zixian
// Student Name   : Tan Zi En
//==================================

using S10270525_PRG2Assignment;
// Initialising Generic Collections:
// Created a new dictionary to store the different flights
Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
string flightHeaders = ""; // storing the flight headers here dynamically
Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();

// Initialise all the Flights/Airline details once
InitialiseFlights();
InitialiseAirlines();
InitialiseBoardingGates();

// Displaying the Menu Options
int loop = 0;
while (loop != -1)
{
    Console.WriteLine("\n==============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("================================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("0. Exit\n");

    Console.WriteLine("Please select your option: "); //input is entered in the next line

    string userOption = Console.ReadLine();

    if (userOption == "1")
    {
        // Call the DisplayFlights method ONCE
        DisplayFlights();
    }
    else if (userOption == "2")
    {
        DisplayBoardingGates();
    }
    else if (userOption == "3")
    {
        // Call the DisplayBoardingGates method ONCE
    }
    else if (userOption == "0")
    {
        Console.WriteLine("Goodbye!");
        loop = -1; // Exit the loop
    }
    else
    {
        Console.WriteLine("Invalid Option. Please Try Again."); // Handles invalid options outside of menu
    }
}

//Basic Feature (1) Load files (airlines and boarding gates)
void InitialiseAirlines()
{
    try
    {
        using (StreamReader sr = new StreamReader("airlines.csv"))
        {
            sr.ReadLine(); // Skip the header line
            string? line;
            while ((line = sr.ReadLine()) != null) // while the line is NOT NULL
            {
                string[] data = line.Split(","); // Split the lines by comma
                string airlineName = data[0].Trim(); // Airline Code
                string airlineCode = data[1].Trim(); // Airline Name
                // Create a new airline object
                Airline airline = new Airline(airlineName, airlineCode);
                // Add the airline to the dictionary
                airlines.Add(airlineCode, airline);
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
    }
}
void InitialiseBoardingGates()
{
    try
    {
        using (StreamReader sr = new StreamReader("boardinggates.csv"))
        {
            sr.ReadLine(); // Skip the header line
            string? line;
            while ((line = sr.ReadLine()) != null) // while the line is NOT NULL
            {
                string[] data = line.Split(","); // Split the lines by comma
                string gateName = data[0].Trim(); // Gate Name
                bool supportDDJB = bool.Parse(data[1].Trim()); // Supports DDJB
                bool supportCFFT = bool.Parse(data[2].Trim()); // Supports CFFT
                bool supportLWTT = bool.Parse(data[3].Trim()); // Supports LWTT
                // Create a new boarding gate object
                BoardingGate boardingGate = new BoardingGate(gateName, supportCFFT, supportDDJB, supportLWTT, null);
                // Add the boarding gate to the dictionary
                boardingGates.Add(gateName, boardingGate);
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
    }
}

// Basic Feature (2) Loading flights.csv file
// Created a method to execute ONLY once to intialise flight info from flights.csv
void InitialiseFlights()
{
    try
    {
        using (StreamReader sr = new StreamReader("flights.csv"))
        {
            // Since the first line of the CSV is the headers it is stored in a variable
            flightHeaders = sr.ReadLine(); // store the flight headers
            string? line;
            while ((line = sr.ReadLine()) != null) // while the line is NOT NULL
            {
                // Split the lines by comma
                string[] data = line.Split(",");

                // Retrieving flight information from flights.csv
                string flightNumber = data[0].Trim(); // Flight Number 
                string origin = data[1].Trim();       // Flight Original Destination
                string destination = data[2].Trim();  // Flight End Point Destination
                DateTime expectedTime = DateTime.Parse(data[3].Trim()); // Flight Date and Time
                // If 5th column is empty it defaults to empty string ""
                string specialRequestCode = data.Length > 4 ? data[4].Trim() : ""; // Flight Special Request Code

                // Determining the flight type based off the Special Request Code from the flights.csv
                // Setting the default to "On Time"
                // Upcasting the objects and storing them in the concrete subclasses
                Flight flight;
                if (specialRequestCode == "DDJB")
                {
                    flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "On Time");
                }
                else if (specialRequestCode == "CFFT")
                {
                    flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "On Time");
                }
                else if (specialRequestCode == "LWTT")
                {
                    flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "On Time");
                }
                else
                {
                    // if Special Request Code is an empty string "" it is defaulted to NORMFlights
                    flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "On Time");
                }

                // Adding the flight into the dictionary flights
                if (!flights.ContainsKey(flightNumber))
                {
                    flights.Add(flightNumber, flight);

                    // Adding flight to the corresponding airline
                    if (airlines.ContainsKey(origin)) // Assuming `origin` is the airline code
                    {
                        airlines[origin].AddFlight(flight); // Ensure `AddFlight` in Airline works
                    }
                }
            }
            // Show the number of flights loaded from flights.csv file
            Console.WriteLine($"Loading Flights...\n{flights.Count} Flights Loaded!");
        }
    }
    catch (FileNotFoundException ex) // If flights.csv cannot be found
    {
        Console.WriteLine($"File not found: {ex.Message}\nStack Trace: {ex.StackTrace}");
    }
    catch (Exception ex) // generic exceptions and stack trace
    {
        Console.WriteLine($"Error Initialising Flights: {ex.Message}\nStack Trace: {ex.StackTrace}");
    }
}

// Basic Feature (3) Listing all flights with their basic info
void DisplayFlights()
{
    Console.WriteLine("================================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("================================================");

    // Split the headers by commas from flightHeaders
    string[] headerCols = flightHeaders.Split(",");
    // Trim the excess/trailing white spaces if there is any and add formatting
    Console.WriteLine($"{headerCols[0].Trim(),-15} {"Airline Name",-20} {headerCols[1].Trim(),-20} {headerCols[2].Trim(),-20} {headerCols[3].Trim(),-15}");

    foreach (var flight in flights.Values)
    {
        // Retrieve airline name dynamically using the airline code
        string airlineName = "Unknown Airline"; // Default to "Unknown Airline"
        string airlineCode = flight.FlightNumber.Substring(0, 2); // only the first two characters is needed SQ,JL etc

        if (airlines.ContainsKey(airlineCode))
        {
            airlineName = airlines[airlineCode].Name; // Retrieve airline name
        }

        // Print flight details
        Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime:dd/M/yyyy hh:mm:ss tt}");
    }
}

//Basic Feature (4) List all boarding gates
void DisplayBoardingGates()
{
    Console.WriteLine("================================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("================================================");

    // Print table headers with alignment
    Console.WriteLine($"{"Gate Name",-10} {"DDJB",-10} {"CFFT",-10} {"LWTT",-10}");


    // Iterate over gates in the original order
    foreach (BoardingGate boardingGate in boardingGates.Values)
    {
        Console.WriteLine(
            $"{boardingGate.GateName,-10} " +
            $"{boardingGate.SupportsDDJB,-10} " +
            $"{boardingGate.SupportsCFFT,-10} " +
            $"{boardingGate.SupportsLWTT,-10}"
        );
    }
}