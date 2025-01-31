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
InitialiseAirlines();
InitialiseBoardingGates();
InitialiseFlights();

// Displaying the Menu Options
int loop = 0;
while (loop != -1)
{
    Console.WriteLine("\n================================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("================================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. Process all unassigned flights to boarding gates in bulk (Advanced Feature A)");
    Console.WriteLine("9. Display Flight Fees (Advanced Feature B)");
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
        // Call the DisplayBoardingGates method ONCE
        DisplayBoardingGates();
    }
    else if (userOption == "3")
    {
        // Call the AssignBoardingGate method ONCE   
        AssignBoardingGate();
    }
    else if (userOption == "4")
    {
        // Call the CreateFlight method ONCE
        CreateFlight();
    }
    else if (userOption == "5")
    {
        //Call the DisplayFlightDetails method ONCE
        DisplayFlightDetails();
    }
    else if (userOption == "6")
    {
        // Call the ModifyFLightDetails method ONCE
        ModifyFlightDetails();
    }
    else if (userOption == "7")
    {
        // Call the DisplayFlightSchedule method ONCE
        DisplayFlightSchedule();
    }
    else if (userOption == "8")
    {
        ProcessFlights();
    }
    else if (userOption == "9")
    {
        // Call the DisplayAirlineFees method ONCE
        DisplayAirlineFees();

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
            flightHeaders = sr.ReadLine(); // store the flight headers
            string? line;
            while ((line = sr.ReadLine()) != null) // while the line is NOT NULL
            {
                string[] data = line.Split(",");

                // Retrieving flight information from flights.csv
                string flightNumber = data[0].Trim(); // Flight Number 
                string origin = data[1].Trim();       // Flight Original Destination
                string destination = data[2].Trim();  // Flight End Point Destination
                DateTime expectedTime = DateTime.Parse(data[3].Trim()); // Flight Date and Time
                string specialRequestCode = data.Length > 4 ? data[4].Trim() : ""; // Special Request Code

                // Extract airline code from flight number
                string airlineCode = flightNumber.Substring(0, 2);

                // Ensure airline exists in dictionary
                if (!airlines.ContainsKey(airlineCode))
                {
                    Console.WriteLine($"Warning: Airline code '{airlineCode}' not found. Skipping flight {flightNumber}.");
                    continue;
                }

                // Get the existing airline object from the dictionary
                Airline airline = airlines[airlineCode];

                // Create flight object based on special request code
                Flight flight;
                if (specialRequestCode == "DDJB")
                {
                    flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "On Time", airline);
                }
                else if (specialRequestCode == "CFFT")
                {
                    flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "On Time", airline);
                }
                else if (specialRequestCode == "LWTT")
                {
                    flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "On Time", airline);
                }
                else
                {
                    flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "On Time", airline);
                }

                // Add flight to dictionary
                if (!flights.ContainsKey(flightNumber))
                {
                    flights.Add(flightNumber, flight);
                    airline.AddFlight(flight); // Associate flight with the correct airline
                }
            }

            Console.WriteLine($"Loading Flights...\n{flights.Count} Flights Loaded!");
        }
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine($"File not found: {ex.Message}\nStack Trace: {ex.StackTrace}");
    }
    catch (Exception ex)
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

// Basic Feature (4) List all boarding gates
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

// Basic Feature (5) Assigning a boarding gate to a flight
void AssignBoardingGate()
{
    try
    {
        Console.WriteLine("================================================");
        Console.WriteLine("Assign a Boarding Gate to a Flight");
        Console.WriteLine("================================================");

        // Gather user input for flight number
        Console.WriteLine("Enter Flight Number: ");
        string flightNum = Console.ReadLine();

        // Checks if the user input matches any of the keys inside flights dictionary
        if (!flights.ContainsKey(flightNum))
        {
            Console.WriteLine("Flight not found");
            return;
        }
        // Performs a search function to search for flight objects with matching flightNums as keys
        Flight selectedFlight = flights[flightNum];

        // Ensure the flight does not already have a boarding gate assigned for the same day
        bool alreadyAssigned = boardingGates.Values.Any(gate =>
            gate.Flight != null &&
            gate.Flight.FlightNumber == flightNum &&
            gate.Flight.ExpectedTime.Date == selectedFlight.ExpectedTime.Date);

        if (alreadyAssigned)
        {
            Console.WriteLine($"Error: Flight {flightNum} already has a Boarding Gate assigned for {selectedFlight.ExpectedTime:dd/MM/yyyy}.");
            return;
        }

        // Gather user input for the boarding gate name
        Console.WriteLine("Enter Boarding Gate Name: ");
        string gateName = Console.ReadLine();

        // Checks if the user input matches any of the keys inside boardingGates dictionary
        if (!boardingGates.ContainsKey(gateName))
        {
            Console.WriteLine("Boarding Gate not found");
            return;
        }

        // Retrieves the boarding gate objects gate name
        BoardingGate selectedGate = boardingGates[gateName];

        // Check if gate is already assigned
        if (selectedGate.Flight != null)
        {
            Console.WriteLine($"Boarding Gate {selectedGate.GateName} is already assigned to Flight {selectedGate.Flight.FlightNumber}. Please choose another gate.");
        }

        // Assign the gate to the flight
        selectedGate.Flight = selectedFlight;

        // adding this string to change the display from NORMFlight to None
        // without changing the class
        string specialRequestCode;

        // Checking the different types of Special Request Code by type
        if (selectedFlight is DDJBFlight)
        {
            specialRequestCode = "DDJB";
        }
        else if (selectedFlight is CFFTFlight)
        {
            specialRequestCode = "CFFT";
        }
        else if (selectedFlight is LWTTFlight)
        {
            specialRequestCode = "LWTT";
        }
        else
        {
            specialRequestCode = "None";
        }

        // Display all the flight information
        Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
        Console.WriteLine($"Origin: {selectedFlight.Origin}");
        Console.WriteLine($"Destination: {selectedFlight.Destination}");
        Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime:dd/M/yyyy hh:mm:ss tt}");
        Console.WriteLine($"Special Request Code: {specialRequestCode}"); // Special Request Code stored in a variable
        Console.WriteLine($"Boarding Gate Name: {selectedGate.GateName}");
        Console.WriteLine($"Supports DDJB: {selectedGate.SupportsDDJB}");
        Console.WriteLine($"Supports CFFT: {selectedGate.SupportsCFFT}");
        Console.WriteLine($"Supports LWTT: {selectedGate.SupportsLWTT}");

        // Gather user input if they would like to change the status of the flight
        Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
        string updateStatus = Console.ReadLine().ToUpper();

        if (updateStatus == "Y")
        {
            bool validStatus = false;
            while (!validStatus)
            {
                Console.WriteLine("1. Delayed");
                Console.WriteLine("2. Boarding");
                Console.WriteLine("3. On Time");
                Console.WriteLine("Please select the new status of the flight: ");
                string statusOption = Console.ReadLine();

                switch (statusOption)
                {
                    case "1":
                        selectedFlight.Status = "Delayed";
                        validStatus = true;
                        break;
                    case "2":
                        selectedFlight.Status = "Boarding";
                        validStatus = true;
                        break;
                    case "3":
                        selectedFlight.Status = "On Time";
                        validStatus = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        break;
                }
            }
        }

        // Confirmation of flight to gate assignment
        Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName}!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

// Basic Feature (6) Create a new flight
void CreateFlight()
{
    bool addMultipleFlight = true; // a flag used to check if user enter Y to add multiple flights

    // While true keep looping until user decides to stop
    while (addMultipleFlight)
    {
        try
        {
            // Prompt user to enter the flight number
            Console.WriteLine("Enter Flight Number: ");
            string newFlightNum = Console.ReadLine();

            // Check for duplicates keys of flight number within the flight dictionary
            if (flights.ContainsKey(newFlightNum))
            {
                // This flight with this flight number already exists
                Console.WriteLine("An existing flight with this Flight Number already exists. Please try again.");
                continue;
            }

            // Validate the airline code from the flight number
            string airlineCode = newFlightNum.Substring(0, 2); // Extract airline code (e.g., SQ from SQ 115)
            if (!airlines.ContainsKey(airlineCode))
            {
                Console.WriteLine($"No airline found for code '{airlineCode}'. Please try again.");
                continue;
            }

            // Retrieve the corresponding airline
            Airline airline = airlines[airlineCode];

            // Do input validation to check if the origin destination exist within the flight.csv
            // and not given a random gibberish origin location
            string newOrigin = "";
            while (true)
            {
                // Prompt the user to enter the flight origin
                Console.Write("Enter Origin: ");
                newOrigin = Console.ReadLine().Trim(); // Assign to the newOrigin variable

                // Check if the destination matches any Origin or Destination (case-insensitive)
                if (flights.Values.Any(f =>
                    string.Equals(f.Destination, newOrigin, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(f.Origin, newOrigin, StringComparison.OrdinalIgnoreCase)))
                {
                    break; // Valid destination found
                }
                Console.WriteLine($"{newOrigin} is an Invalid Origin. Please enter an existing Origin.");
            }

            // Do input validation to check if the landing/end destination exist within the flight.csv
            // and not given a random gibberish location
            string newDestination = "";
            while (true)
            {
                // Prompt the user for the flight destination
                Console.Write("Enter Destination: ");
                newDestination = Console.ReadLine().Trim();

                // Check if the destination matches any Origin or Destination (case-insensitive)
                if (flights.Values.Any(f =>
                    string.Equals(f.Destination, newDestination, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(f.Origin, newDestination, StringComparison.OrdinalIgnoreCase)))
                {
                    break; // Valid destination found
                }
                Console.WriteLine($"{newDestination} is an Invalid Destination. Please enter an existing Destination.");
            }

            DateTime expectedTime;
            while (true)
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                if (DateTime.TryParse(Console.ReadLine(), out expectedTime))
                {
                    // Ensure the entered date is valid (not exceeding valid days and months)
                    if (expectedTime.Day <= 31 && expectedTime.Month <= 12)
                    {
                        break; // Exit loop if the date is valid
                    }
                    Console.WriteLine("Invalid date. Ensure day is between 1-31 and month is between 1-12.");
                }
                else
                {
                    Console.WriteLine("Invalid format. Please re-enter (dd/MM/yyyy HH:mm): ");
                }
            }

            // Validate the user input for Special Request Code
            string specialRequestCode;
            while (true)
            {
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                specialRequestCode = Console.ReadLine()?.ToUpper();

                if (specialRequestCode == "CFFT" || specialRequestCode == "DDJB" || specialRequestCode == "LWTT" || specialRequestCode == "NONE")
                {
                    break; // input is valid and continue
                }
                Console.WriteLine("Invalid Special Request Code. Please enter CFFT, DDJB, LWTT, or None.");
            }

            // Determine the type of flight
            Flight newFlight;
            if (specialRequestCode == "DDJB")
            {
                newFlight = new DDJBFlight(newFlightNum, newOrigin, newDestination, expectedTime, "On Time", airline);
            }
            else if (specialRequestCode == "CFFT")
            {
                newFlight = new CFFTFlight(newFlightNum, newOrigin, newDestination, expectedTime, "On Time", airline);
            }
            else if (specialRequestCode == "LWTT")
            {
                newFlight = new LWTTFlight(newFlightNum, newOrigin, newDestination, expectedTime, "On Time", airline);
            }
            else
            {
                newFlight = new NORMFlight(newFlightNum, newOrigin, newDestination, expectedTime, "On Time", airline);
            }

            // Add the newly created flight to the flights dictionary and the corresponding airline
            flights.Add(newFlightNum, newFlight);
            airline.AddFlight(newFlight);

            // Appending the newly created flight to the flight.csv file
            using (StreamWriter sw = new StreamWriter("flights.csv", append: true))
            {
                sw.WriteLine($"{newFlightNum},{newOrigin},{newDestination},{expectedTime:hh:mm tt},{specialRequestCode}");
            }

            // Print success message 
            Console.WriteLine($"Flight {newFlightNum} has been added!");
            // Prompt the user to add another flight
            string addAnother;
            while (true)
            {
                Console.WriteLine("Would you like to add another flight? (Y/N): ");
                addAnother = Console.ReadLine().ToUpper();

                if (addAnother == "Y")
                {
                    break; // Exit the loop and continue adding flights
                }
                else if (addAnother == "N")
                {
                    addMultipleFlight = false; // Stop adding flights and exit the loop
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'Y' for Yes or 'N' for No.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

// Implementing Basic Feature (7) Displaying full flight details from an airline.

void DisplayFlightDetails()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-20} {"Airline Name",-20}");

    //Display the list of available airlines
    foreach (Airline airline in airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-20} {airline.Name,-20}");
    }

    Console.Write("Enter Airline Code: ");
    string airlinecode = Console.ReadLine().ToUpper();

    try
    {
        {
            // Check if the entered airline code is valid
            if (airlinecode == "SQ" || airlinecode == "MH" || airlinecode == "JL" || airlinecode == "CX" || airlinecode == "QF" || airlinecode == "TR" || airlinecode == "EK" || airlinecode == "BA")
            {
                Console.WriteLine($"{"Flight Number",-20}{"Airline Name",-20}{"Origin",-20}{"Destination",-20}{"Expected",-20}");
                Console.WriteLine("Departure/Arrival Time");

                // Display the flights for the selected airline
                foreach (Flight flight in flights.Values)
                {
                    if (flight.Airline.Code == airlinecode)
                    {
                        Console.WriteLine($"{flight.FlightNumber,-20}{flight.Airline.Name,-20}{flight.Origin,-20}{flight.Destination,-20}{flight.ExpectedTime.ToString("MM/dd/yyyy"),-20}");
                        Console.WriteLine(flight.ExpectedTime.ToString("hh:mm:ss tt"));
                    }
                }
            }
            else
            {
                // Inform the user if the airline code is invalid
                Console.WriteLine("Airline Code is invalid. Please try again.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

//Implementing Basic Feature (8) Modify flight details

void ModifyFlightDetails()
{

    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-20} {"Airline Name",-20}");

    //Display the list of available airlines
    foreach (Airline airline in airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-20} {airline.Name,-20}");
    }

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    try
    {
        // Check if the entered airline code is valid
        if (airlineCode == "SQ" || airlineCode == "MH" || airlineCode == "JL" || airlineCode == "CX" || airlineCode == "QF" || airlineCode == "TR" || airlineCode == "EK" || airlineCode == "BA")
        {
            Console.WriteLine($"{"Flight Number",-20}{"Airline Name",-20}{"Origin",-20}{"Destination",-20}{"Expected",-20}{"Departure/Arrival Time",-30}");
            // Display the flights for the selected airline
            foreach (Flight flight in flights.Values)
            {
                if (flight.Airline.Code == airlineCode)
                {
                    Console.WriteLine($"{flight.FlightNumber,-20}{flight.Airline.Name,-20}{flight.Origin,-20}{flight.Destination,-20}{flight.ExpectedTime.ToString("MM/dd/yyyy"),-20}{flight.ExpectedTime.ToString("hh:mm:ss tt"),-30}");
                }
            }

            Console.Write("Choose an existing flight to modify or delete: ");
            string? flightChoice = Console.ReadLine()?.ToUpper();

            // Validate if the flight exists
            if (flightChoice == null || !flights.ContainsKey(flightChoice))
            {
                Console.WriteLine("Flight not found. Please enter a valid flight number.");
                return;
            }

            Console.WriteLine("1. Modify Flight");
            Console.WriteLine("2. Delete Flight");
            Console.Write("Choose an option: ");
            string? flightoption = Console.ReadLine();

            if (flightoption == "1")
            {
                // Modify Flight Options
                Flight selectedFlight = flights[flightChoice];
                Console.WriteLine("1. Modify Basic Information");
                Console.WriteLine("2. Modify Status");
                Console.WriteLine("3. Modify Special Request Code");
                Console.WriteLine("4. Modify Boarding Gate");
                Console.Write("Choose an option: ");
                string? modChoice = Console.ReadLine();

                if (modChoice == "1")
                {
                    // Prompt user for new details
                    string newOrigin = "";
                    while (true)
                    {
                        // Prompt the user to enter the flight origin
                        Console.Write("Enter Origin: ");
                        newOrigin = Console.ReadLine().Trim(); // Assign to the newOrigin variable

                        // Check if the destination matches any Origin or Destination (case-insensitive)
                        if (flights.Values.Any(f =>
                            string.Equals(f.Destination, newOrigin, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(f.Origin, newOrigin, StringComparison.OrdinalIgnoreCase)))
                        {
                            break; // Valid destination found
                        }
                        Console.WriteLine($"{newOrigin} is an Invalid Origin. Please enter an existing Origin.");
                    }

                    //Prompt user for new destination.
                    string newDestination = "";
                    while (true)
                    {
                        // Prompt the user for the flight destination
                        Console.Write("Enter Destination: ");
                        newDestination = Console.ReadLine().Trim();

                        // Check if the destination matches any Origin or Destination (case-insensitive)
                        if (flights.Values.Any(f =>
                            string.Equals(f.Destination, newDestination, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(f.Origin, newDestination, StringComparison.OrdinalIgnoreCase)))
                        {
                            break; // Valid destination found
                        }
                        Console.WriteLine($"{newDestination} is an Invalid Destination. Please enter an existing Destination.");
                    }

                    //Parse the DateTime and prints error message if input is invalid.
                    DateTime newExpectedTime;
                    Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                    if (DateTime.TryParse(Console.ReadLine(), out newExpectedTime))
                    {

                    }
                    else
                    {
                        Console.WriteLine("Invalid datetime or format. Please try again. ");
                        return;
                    }

                    // Update flight details
                    selectedFlight.Origin = newOrigin;
                    selectedFlight.Destination = newDestination;
                    selectedFlight.ExpectedTime = newExpectedTime;
                }
                else if (modChoice == "2")
                {
                    Console.WriteLine("Choose the updated status of the flight");
                    Console.WriteLine("1. Delayed");
                    Console.WriteLine("2. Boarding");
                    Console.WriteLine("3. On Time");
                    Console.Write("Enter an option: ");
                    string? flightstatus = Console.ReadLine();
                    //Depending on the option, change the flight status.
                    if (flightstatus == "1")
                    {
                        selectedFlight.Status = "Delayed";
                    }
                    else if (flightstatus == "2")
                    {
                        selectedFlight.Status = "Boarding";
                    }
                    else if (flightstatus == "3")
                    {
                        selectedFlight.Status = "On Time";

                    }
                    else
                    {
                        Console.WriteLine("Invalid Option. Please try again.");
                        return;
                    }

                }
                else if (modChoice == "3")
                {
                    Console.WriteLine("Choose the updated special request code of the flight");
                    Console.WriteLine("1. CFFT");
                    Console.WriteLine("2. DDJB");
                    Console.WriteLine("3. LWTT");
                    Console.WriteLine("4. No special code.");
                    Console.Write("Enter an option: ");
                    string? specialcode = Console.ReadLine();
                    // Depending on special code choosen, create a new flight class based on the special code, remove the old flight class from dictionary and add the new flight class to the dictionary.
                    if (specialcode == "1")
                    {
                        flights.Remove(selectedFlight.FlightNumber);
                        selectedFlight = new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, selectedFlight.Airline);
                        flights.Add(selectedFlight.FlightNumber, selectedFlight);
                    }
                    else if (specialcode == "2")
                    {
                        flights.Remove(selectedFlight.FlightNumber);
                        selectedFlight = new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, selectedFlight.Airline);
                        flights.Add(selectedFlight.FlightNumber, selectedFlight);
                    }
                    else if (specialcode == "3")
                    {
                        flights.Remove(selectedFlight.FlightNumber);
                        selectedFlight = new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, selectedFlight.Airline);
                        flights.Add(selectedFlight.FlightNumber, selectedFlight);

                    }
                    else if (specialcode == "4")
                    {
                        flights.Remove(selectedFlight.FlightNumber);
                        selectedFlight = new NORMFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, selectedFlight.Airline);
                        flights.Add(selectedFlight.FlightNumber, selectedFlight);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Option. Please try again.");
                        return;
                    }
                }
                else if (modChoice == "4")
                {
                    Console.Write("Enter the new gate number: ");
                    string? newgate = Console.ReadLine().ToUpper();
                    if (!boardingGates.ContainsKey(newgate))
                    {
                        Console.WriteLine("Boarding Gate not found");
                        return;
                    }
                    BoardingGate selectedGate = boardingGates[newgate];

                    // Check if gate is already assigned
                    if (selectedGate.Flight != null)
                    {
                        Console.WriteLine($"Boarding Gate {selectedGate.GateName} is already assigned to Flight {selectedGate.Flight.FlightNumber}. Please try again.");
                        return;
                    }
                    // Check if the selected gate supports the special code.
                    if (selectedFlight is CFFTFlight && selectedGate.SupportsCFFT == false)
                    {
                        Console.WriteLine("Boarding Gate does not support this special code.");
                        return;
                    }
                    else if (selectedFlight is CFFTFlight && selectedGate.SupportsCFFT == false)
                    {
                        Console.WriteLine("Boarding Gate does not support this special code.");
                        return;
                    }
                    else if (selectedFlight is CFFTFlight && selectedGate.SupportsCFFT == false)
                    {
                        Console.WriteLine("Boarding Gate does not support this special code.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"{selectedFlight.FlightNumber} has been assigned to gate {selectedGate.GateName}");
                        selectedGate.Flight = selectedFlight; // Assign the gate to the flight choosen.
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Option. Please choose 1,2,3 or 4.");
                    return;
                }




                // Writing update confirmation and updated details
                Console.WriteLine("Flight updated!");
                Console.WriteLine($"Flight Number: {flightChoice}");
                Console.WriteLine($"Airline Name: {selectedFlight.Airline.Name}");
                Console.WriteLine($"Origin: {selectedFlight.Origin}");
                Console.WriteLine($"Destination: {selectedFlight.Destination}");
                Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime}");
                Console.WriteLine($"Status: {selectedFlight.Status}");
                //Checking if there is a special request code and displaying the code.
                if (selectedFlight is CFFTFlight)
                {
                    Console.WriteLine("Special Request Code: CFFT");
                }
                else if (selectedFlight is LWTTFlight)
                {
                    Console.WriteLine("Special Request Code: LWTT");
                }
                else if (selectedFlight is DDJBFlight)
                {
                    Console.WriteLine("Special Request Code: DDJB");
                }
                else
                {
                    Console.WriteLine("Special Request Code: None");
                }
                //Checking if there is a boarding gate.
                bool boardingGateCheck = false; // Setting to false to print unassign by default
                foreach (BoardingGate gate in boardingGates.Values) // Checking each gate to see if flight is assign to the gate.
                {
                    if (gate.Flight == selectedFlight)
                    {
                        Console.WriteLine($"Boarding Gate: {gate.GateName}");
                        boardingGateCheck = true;
                    }
                }
                if (boardingGateCheck == false)
                {
                    Console.WriteLine("Boarding Gate: Unassigned");
                }
            }
            // Deletion of flight
            else if (flightoption == "2")
            {
                Console.WriteLine($"Please confirm the deletion of flight {flightChoice} (Y/N)"); // Confirmation of deletion.
                string? confirmationChoice = Console.ReadLine()?.ToUpper();
                if (confirmationChoice == "Y")
                {
                    // Delete Flight
                    flights.Remove(flightChoice);
                    Console.WriteLine("Flight deleted successfully."); // Input validation
                }
                else if (confirmationChoice == "N")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Your choice is invalid. Please try again.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid option. Please enter 1 or 2."); // Input validation
            }
        }
        else
        {
            // Input Validation
            Console.WriteLine("Airline Code is invalid. Please try again.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}


// Basic Feature (9) Displaying scheduled flights in chronological order with boarding gates
void DisplayFlightSchedule()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-22} {"Origin",-20} {"Destination",-20} {"Expected Departure/Arrival Time",-33} {"Status",-12} {"Boarding Gate"}");

    // Convert dictionary values to a list and sort by ExpectedTime
    List<Flight> sortedFlights = flights.Values.ToList();
    sortedFlights.Sort(); // Uses IComparable<Flight> to sort by ExpectedTime

    foreach (var flight in sortedFlights)
    {
        string boardingGate = "Unassigned";

        // Find assigned gate (if gate is assigned)
        foreach (var gate in boardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                boardingGate = gate.GateName;
                break;
            }
        }

        // Format Expected Time to match the sample output: "13/1/2025 3:40:00 pm"
        string formattedTime = flight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt");

        // Print flight details in a properly formatted single-line format
        Console.WriteLine($"{flight.FlightNumber,-15} {flight.Airline.Name,-22} {flight.Origin,-20} {flight.Destination,-20} {formattedTime,-33} {flight.Status,-12} {boardingGate}");
    }
}


// Advanced Feature (a) Process all unassigned flights to boarding gates in bulk - Zi En
void ProcessFlights()
{
    try
    {
        int unassignedFlightcount = 0;
        Queue<Flight> unassignedFlights = new Queue<Flight>();
        foreach (Flight flight in flights.Values)
        {
            bool Assigned = false; // Value determines if flight has a Boarding Gate Assigned.
            foreach (BoardingGate gate in boardingGates.Values)
            {
                if (gate.Flight == flight) // Boarding gate has a flight assigned.
                {
                    Assigned = true; // If a flight has been assigned to a boarding gate, Changes value to true
                    break;
                }
            }
            if (Assigned == false) //Add flight to the queue if no boarding gate is unassigned
            {
                unassignedFlights.Enqueue(flight);
            }
        }
        int unassignedGateCount = 0;
        if (unassignedFlights.Count > 0)
        {
            Console.WriteLine($"Total number of Flights that do not have any Boarding Gate assigned yet: {unassignedFlights.Count}");
            foreach (BoardingGate gate in boardingGates.Values)
            {
                if (gate.Flight == null) // Check if boarding gate has a flight assigned to it.
                {
                    unassignedGateCount++; //Add to counter if gate is unassigned.
                }
            }
            Console.WriteLine($"Total number of Boarding Gates without an assigned Flight: {unassignedGateCount}");
            Console.WriteLine($"{"Flight Number",-20}{"Airline Name",-20}{"Origin",-20}{"Destination",-20}{"Expected",-20}{"Departure/Arrival Time",-30}{"Special Request Code",-30}{"Boarding Gate",-20}");

            int assignedFlights = 0;
            while (unassignedFlights.Count > 0) // Use while loop to enable modification of queue.
            {
                Flight flight = unassignedFlights.Peek(); //See the first time in queue.
                string specialrequestcode = null; // Use to keep track of special request code
                unassignedFlights.Dequeue(); // Remove flight from queue.
                BoardingGate selectedGate = null; //Keep track of assigned gate

                if (flight is not NORMFlight) // Check if flight has special request.
                {
                    foreach (BoardingGate gate in boardingGates.Values)
                    {
                        if (flight is LWTTFlight && gate.SupportsLWTT == true && gate.Flight == null) // Only allow code to assign gate if gate supports special request code and is unassigned.
                        {
                            gate.Flight = flight; // Assign flight to gate
                            assignedFlights++;
                            specialrequestcode = "LWTT"; //Store the speicial request code
                            selectedGate = gate; //Store the gate
                            break; //Exit foreach loop once valid gate is assigned.
                        }
                        else if (flight is CFFTFlight && gate.SupportsCFFT == true && gate.Flight == null) // Only allow code to assign gate if gate supports special request code and is unassigned.
                        {
                            gate.Flight = flight;
                            assignedFlights++;
                            specialrequestcode = "CFFT";
                            selectedGate = gate;
                            break;
                        }
                        else if (flight is DDJBFlight && gate.SupportsDDJB == true && gate.Flight == null) // Only allow code to assign gate if gate supports special request code and is unassigned.
                        {
                            gate.Flight = flight;
                            assignedFlights++;
                            specialrequestcode = "DDJB";
                            selectedGate = gate;
                            break;
                        }
                    }
                }
                else //If flight has no special request code
                {
                    foreach (BoardingGate gate in boardingGates.Values)
                    {
                        if (gate.Flight == null) //Check if gate is unassigned
                        {
                            gate.Flight = flight;
                            assignedFlights++;
                            specialrequestcode = "None";
                            selectedGate = gate;
                            break;
                        }
                    }

                }
                Console.WriteLine($"{flight.FlightNumber,-20}{flight.Airline.Name,-20}{flight.Origin,-20}{flight.Destination,-20}{flight.ExpectedTime.ToString("MM/dd/yyyy"),-20}{flight.ExpectedTime.ToString("hh:mm:ss tt"),-30}{specialrequestcode,-30}{selectedGate.GateName,-20}");

            }
            Console.WriteLine($"Total number of Flights and Boarding Gates processed and assigned: {assignedFlights}");
            double percentage = (assignedFlights / flights.Count) * 100;
            Console.WriteLine($"Total number of Flights and Boarding Gates that were processed automatically over those that were already assigned as a percentage: {percentage:F2}%");
        }
        else
        {
            Console.WriteLine("All flights have a boarding gate.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

// Advanced Feature (B) Display the total fee per airline for the day - Zixian
// This code is highly likely be broken as the advanced feature A is required to test the code without i can't move forward.
void DisplayAirlineFees()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Daily Flight Fees for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");

    double totalFeesCollected = 0.0;
    double totalDiscountsApplied = 0.0;

    foreach (var airline in airlines.Values)
    {
        // Get only flights assigned to this airline
        var airlineFlights = flights.Values.Where(f => f.Airline?.Code == airline.Code).ToList();

        if (airlineFlights.Count == 0)
        {
            Console.WriteLine($"Skipping {airline.Name} ({airline.Code}) - No flights assigned.");
            continue;
        }

        // Ensure airline's flight list is properly maintained
        airline.Flights.Clear();
        foreach (var flight in airlineFlights)
        {
            airline.AddFlight(flight);
        }

        // Calculate the total fees before discount
        double airlineSubtotal = airlineFlights.Sum(f => f.CalculateFees());

        // If there are no flights, fees should be 0
        if (airlineSubtotal == 0)
        {
            Console.WriteLine($"Skipping {airline.Name} ({airline.Code}) - No valid flight fees.");
            continue;
        }

        // Calculate final fees after discounts
        double finalAirlineFee = airline.CalculateFees();
        double airlineDiscounts = airlineSubtotal - finalAirlineFee;

        // Ensure discount isn't negative
        if (airlineDiscounts < 0) airlineDiscounts = 0;

        // Accumulate totals
        totalFeesCollected += finalAirlineFee;
        totalDiscountsApplied += airlineDiscounts;

        // Print airline breakdown
        Console.WriteLine($"Airline: {airline.Name} ({airline.Code})");
        Console.WriteLine($"  Flights: {airlineFlights.Count}");
        Console.WriteLine($"  Subtotal Fees: ${airlineSubtotal:F2}");
        Console.WriteLine($"  Discounts Applied: -${airlineDiscounts:F2}");
        Console.WriteLine($"  Final Total Fee: ${finalAirlineFee:F2}");
        Console.WriteLine("-------------------------------------------------");
    }

    // Avoid division by zero for discount percentage
    double netRevenue = totalFeesCollected - totalDiscountsApplied;
    double discountPercentage = (totalFeesCollected > 0) ? (totalDiscountsApplied / totalFeesCollected) * 100 : 0;

    // Print Summary
    Console.WriteLine("=============================================");
    Console.WriteLine($"Total Fees Collected from All Airlines: ${totalFeesCollected:F2}");
    Console.WriteLine($"Total Discounts Applied: -${totalDiscountsApplied:F2}");
    Console.WriteLine($"Net Revenue Collected: ${netRevenue:F2}");
    Console.WriteLine($"Discount Percentage: {discountPercentage:F2}%");
}

