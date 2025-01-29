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

                // Extract airline code from flight number (first two characters)
                string airlineCode = flightNumber.Substring(0, 2);
                // Manually assign airline based on the extracted airline code
                Airline? airline = airlineCode == "SQ" ? new Airline("Singapore Airlines", "SQ") :
                                   airlineCode == "MH" ? new Airline("Malaysia Airlines", "MH") :
                                   airlineCode == "JL" ? new Airline("Japan Airlines", "JL") :
                                   airlineCode == "CX" ? new Airline("Cathay Pacific", "CX") :
                                   airlineCode == "QF" ? new Airline("Qantas Airways", "QF") :
                                   airlineCode == "TR" ? new Airline("AirAsia", "TR") :
                                   airlineCode == "EK" ? new Airline("Emirates", "EK") :
                                   airlineCode == "BA" ? new Airline("British Airways", "BA") :
                                   null;


                // Determining the flight type based off the Special Request Code from the flights.csv
                // Setting the default to "On Time"
                // Upcasting the objects and storing them in the concrete subclasses
                Flight flight;
                if (specialRequestCode == "DDJB")
                {
                    // Might need to change all the "On Time" to scheduled to meet the sample output
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
                    // if Special Request Code is an empty string "" it is defaulted to NORMFlights
                    flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "On Time", airline);
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
            Console.WriteLine($"{"Flight Number",-20}{"Airline Name",-20}{"Origin",-20}{"Destination",-20}{"Expected",-20}");
            Console.WriteLine("Departure/Arrival Time");

            // Display the flights for the selected airline
            foreach (Flight flight in flights.Values)
            {
                if (flight.Airline.Code == airlineCode)
                {
                    Console.WriteLine($"{flight.FlightNumber,-20}{flight.Airline.Name,-20}{flight.Origin,-20}{flight.Destination,-20}{flight.ExpectedTime:MM/dd/yyyy,-20}");
                    Console.WriteLine(flight.ExpectedTime.ToString("hh:mm:ss tt"));
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
            string? modChoice = Console.ReadLine();

            if (modChoice == "1")
            {
                // Modify Flight
                Flight selectedFlight = flights[flightChoice];

                // Prompt user for new details
                Console.Write("Enter new Origin: ");
                string? newOrigin = Console.ReadLine()?.Trim();

                Console.Write("Enter new Destination: ");
                string? newDestination = Console.ReadLine()?.Trim();

                DateTime newExpectedTime;
                while (true)
                {
                    Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                    if (DateTime.TryParse(Console.ReadLine(), out newExpectedTime)) // Converts user input to DateTime and store the DateTime value as newExpectedTime.
                    {
                        if (newExpectedTime.Day <= 31 && newExpectedTime.Month <= 12) // Ensuring the date and month is valid.
                        {
                            break;
                        }
                        Console.WriteLine("Invalid date. Ensure day is between 1-31 and month is between 1-12.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Please re-enter (dd/MM/yyyy HH:mm): ");
                    }
                }

                // Update flight details
                if (newOrigin != null) selectedFlight.Origin = newOrigin;
                if (newDestination != null) selectedFlight.Destination = newDestination;
                selectedFlight.ExpectedTime = newExpectedTime;
                // Writing update confirmation and updated details
                Console.WriteLine("Flight updated!");
                Console.WriteLine($"Flight Number: {flightChoice}");
                Console.WriteLine($"Airline Name: {selectedFlight.Airline.Name}");
                Console.WriteLine($"Origin: {newOrigin}");
                Console.WriteLine($"Destination: {newDestination}");
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
            else if (modChoice == "2")
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

// Advanced Feature (B) Display the total fee per airline for the day - Zixian
// This code is highly likely be broken as the advanced feature A is required to test the code without i can't move forward.
void DisplayAirlineFees()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Daily Flight Fees for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");

    // Using a lambda expression to check if ALL the flights are assigned a boarding gate
    bool unassignedFlights = flights.Values.Any(flight => !boardingGates.Values.Any(gate => gate.Flight == flight));

    // Check if unassignedFlights returns a true expression, the if statement will run showing an error message
    if (unassignedFlights)
    {
        Console.WriteLine("All flights must have an assigned boarding gate");
        return;
    }

    // Initialising variables to calculate total with
    double totalFeesCollected = 0.0;
    double totalDiscountsApplied = 0.0;

    foreach (var airline in airlines.Values)
    {
        // Get the total fee after applying discounts
        double finalAirlineFee = airline.CalculateFees();

        // Calculate airline subtotal before all the discounts
        double airlineSubtotal = airline.Flights.Values.Sum(f => f.CalculateFees());

        // Calculate total discount applied
        double airlineDiscounts = airlineSubtotal - finalAirlineFee;

        // Track total for all airlines
        totalFeesCollected += finalAirlineFee;
        totalDiscountsApplied += airlineDiscounts;

        // Display breakdown for each airline
        Console.WriteLine($"Airline: {airline.Name} ({airline.Code})");
        Console.WriteLine($"  Flights: {airline.Flights.Count}");
        Console.WriteLine($"  Subtotal Fees: ${airlineSubtotal:F2}");
        Console.WriteLine($"  Discounts Applied: -${airlineDiscounts:F2}");
        Console.WriteLine($"  Final Total Fee: ${finalAirlineFee:F2}");
        Console.WriteLine("-------------------------------------------------");
    }

    // Display Summary of Total Fees and Discounts
    Console.WriteLine("=============================================");
    Console.WriteLine($"Total Fees Collected from All Airlines: ${totalFeesCollected:F2}");
    Console.WriteLine($"Total Discounts Applied: -${totalDiscountsApplied:F2}");
    Console.WriteLine($"Net Revenue Collected: ${totalFeesCollected - totalDiscountsApplied:F2}");
    Console.WriteLine($"Discount Percentage: {((totalDiscountsApplied / totalFeesCollected) * 100):F2}%");
}