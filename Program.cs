using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

namespace Project_milestone2_PRG281
{
    //Globals Class
    public static class Globals
    {
        public static string DataFile { get; set; } = @"users.txt";

        public static List<User> users { get; set; } = new List<User>();
    }
    //User Interface
    public interface IUser
    {
        void Register();

        bool Login();
    }

    //User Class
    public class User : IUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

        //Registration Method
        public void Register()
        {
            Console.Clear();

            Console.WriteLine("Welcome to our registration page, please complete the following:");

            Console.WriteLine("\nEnter a Username you would like to use:");
            string Username = Console.ReadLine();

            Console.WriteLine("\nEnter a Password you would like to use:");
            string Password = Console.ReadLine();

            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)) // checks whether the user input is empty and if not carries out the if statement
            {
                Globals.users.Add(new User { Username = Username, Password = Password }); // adds the username and password to the global list that stores the users an dpasswords

                File.AppendAllText(Globals.DataFile, $"{Username},{Password}\n"); // adds the username and password to the global list that stores the users an dpasswords, separating the two with comma

                Console.WriteLine("\nREGISTRATION SUCCESSFUL!!\n"); // once user is added succesfully this displays

                Console.WriteLine("=======================================================================\n");

                Dashboard.displayWelcome(Username); // displays the welcome message method that is stored in the dashboard

                Dashboard.DisplayMainMenu(); // displays the dashboard and its menu

            }
            else
            {
                Console.WriteLine("Please enter a username and password");
                return;
            }
        }

        //Login Method
        public bool Login()
        {

            int attempts = 0;

            while (attempts < 3) // loops through the process 3 times incase the user gets their password wrong for the first or second time
            {
                Console.Clear();

                Console.WriteLine("\nPlease enter your Username:");
                Username = Console.ReadLine();


                Console.WriteLine("\nPlease enter your Password:");
                Password = Console.ReadLine();

                if (File.Exists(Globals.DataFile)) //checks whether a specific user exists in the datafile 
                {
                    string[] lines = File.ReadAllLines(Globals.DataFile); // this scans through each line of the data file called DataFile
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts[0] == Username && parts[1] == Password) // if a matching usernaME and password is found the login is then successful
                        {
                            Console.Clear();
                            Console.WriteLine("\nLogin succesfull!");
                            Console.WriteLine("============================================================\n");

                            Dashboard dashboard = new Dashboard(); // displays the dashboard and its main menu
                            Dashboard.displayWelcome(Username); // inserts the username into the welcom message
                            return true;
                        }
                    }

                }
                attempts++; // when a user gets their password or user name wrong it will loop and start the process and increments

                Console.WriteLine("===============================================================");
                Console.WriteLine($"\nIncorrect Username or Password. {3 - attempts} attempts remaining."); // it subtracts the amount of attempts from 3 to show the user how many attempts they are left with
                Console.WriteLine("\nPlease ENTER to continue");
                Console.ReadLine();

            }

            Console.WriteLine("Maximum attempts reached. Login failed"); // after user uses all 3 attempts this is displayed and the program ends
            return false;


        }


        // Dashboard Method
        public class Dashboard
        {
            public static void displayWelcome(string Username)
            {
                Console.WriteLine("WELCOME TO THE MAIN MENU " + Username + "\n");
                DisplayMainMenu();

            }

            public static void DisplayMainMenu() // method to display the main menu tot he user
            {
                bool logout = false;

                while (!logout)
                {

                    Console.WriteLine("What would you like to do:");
                    Console.WriteLine("1. Calculate your carbon footprint");
                    Console.WriteLine("2. View History");
                    Console.WriteLine("3. Set Goals");
                    Console.WriteLine("4. Get Recommendations");
                    Console.WriteLine("5. Logout\n");

                    Console.Write("Enter your choice:\n");
                    int select = int.Parse(Console.ReadLine());

                    switch (select)
                    {
                        case 1:// when user chooses the first case they are directed to the carbon footprint calculator method
                            Console.Clear();
                            HandleCarbonFootprintCalculator();
                            break;
                        case 2:
                            //when user chooses the second case they are directed to the history method
                            Console.Clear();
                            ViewHistory();
                            break;
                        case 3://when user chooses the second case they are directed to the handle set goals method
                            Console.Clear();
                            HandleSetGoals();
                            break;
                        case 4://when user chooses the second case they are directed to the recommendations method and class
                            Console.Clear();
                            Recommendations recommendations = new Recommendations();
                            recommendations.Recomm();
                            break;
                        case 5: // this case logs the user out and closes the application
                            Console.Clear();
                            Console.WriteLine("You have logged out");
                            logout = true;
                            break;
                        default:
                            Console.WriteLine("Please select a valid option");
                            break;

                    }
                }


            }
            //Calculations
            private const string FilePath = "carbon_footprint_records.txt"; //text file created to store the users carbonfootprint
            private static void HandleCarbonFootprintCalculator()
            {
                try
                {
                    int progressStep = 0; // this initializes the progress step as 0
                    int totalSteps = 3; // these are the total amoubt of steps which have been used

                    Console.WriteLine("\nSelect the type of activity:");
                    Console.WriteLine("1. Transportation (miles driven)");
                    Console.WriteLine("2. Electricity Usage (kWh)");
                    Console.WriteLine("3. Both Transportation and Electricity\n");

                    Console.Write("Enter your choice:\n");
                    int option = int.Parse(Console.ReadLine());

                    double carbonFootprint = 0;
                    double milesDriven = 0;
                    double electricityUsed = 0;

                    string details = "";

                    switch (option)
                    {
                        case 1:
                            Console.WriteLine("\nEnter miles driven:");
                            if (double.TryParse(Console.ReadLine(), out milesDriven)) // the readline takes input the users, the double.TryPARSE
                                                                                      // attempts to change the string into a double value
                                                                                      // out miles driven stores the resulting double value
                            {
                                carbonFootprint = CalculateTransportationFootprint(milesDriven); // the value miesDriven is then passed to the method CalculateTransportationFootprint
                                                                                                 // the  method then claculates the carbon footprint using the milesdriven value which is the returned by the method
                                                                                                 //
                                progressStep = 1; // display the progress of the user whilst using the calculator
                                details = $"Miles Driven: {milesDriven}";
                            }
                            else
                            {
                                Console.WriteLine("Invlaid input for miles driven.");
                            }

                            break;

                        case 2:
                            Console.WriteLine("\nEnter electricity used (kWh):");
                            if (double.TryParse(Console.ReadLine(), out electricityUsed))  // the readline takes input the users, the double.TryPARSE and attempts to change the string into a double value
                            {                                                              // out electricity used stores the resulting double value
                                carbonFootprint = CalculateElectricityFootprint(electricityUsed); // the value electricity used is then passed to the method CalculateElectricityFootprint
                                                                                                  // the  method then claculates the carbon footprint using the electricity used value which is the returned by the method
                                progressStep = 1; // displays progress of user whilst using calculator
                            }
                            else
                            {
                                Console.WriteLine("Invlaid input for electricity used.");

                            }
                            details = $"Electricity Used: {electricityUsed} kWh";
                            break;

                        case 3:
                            Console.WriteLine("\nEnter miles driven:");
                            if (double.TryParse(Console.ReadLine(), out milesDriven)) // the readline takes input the users, the double.TryPARSE and attempts to change the string into a double value
                                                                                      // ut miles Driven used stores the resulting double value      
                            {
                                progressStep = 1;                                     // displays progress of user whilst using calculator
                                Console.SetCursorPosition(0, Console.CursorTop - 1);  // thsi promopts where the progress will be placed
                                Console.Write($"\nProgress: {(progressStep) * 100 / totalSteps}%\n"); // outsput the progress level to the user, multiplies by 100 to give a percentage

                                Console.WriteLine("\nEnter electricity used(kwh):");
                                if (double.TryParse(Console.ReadLine(), out electricityUsed))
                                {
                                    carbonFootprint = CalculateCarbonFootprint(milesDriven, electricityUsed);
                                    progressStep = 2; // displays user progress
                                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                                    Console.Write($"\nProgress: {(progressStep) * 100 / totalSteps}%\n"); // outsput the progress level to the user, multiplies by 100 to give a percentage


                                }
                                else
                                {
                                    Console.WriteLine("Invlaid input for electricity used.");

                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invlaid input for miles driven used.");
                                return;
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid option selected. Please choose 1, 2, or 3.");
                            return;
                    }

                    Console.SetCursorPosition(0, Console.CursorTop - 1); // tells proogress where to display itself
                    Console.WriteLine("\n\nCalculating your Carbon footprint....");
                    Console.Write($"\nProgress:100% Comlpeted!\n\n"); // indicates that the progess is complete and has reached its peak
                    Console.WriteLine($"\nYour Carbon Footprint: {carbonFootprint:F2} kg CO2e\n"); // displays the users carbon footprint

                    SaveResultToFile(carbonFootprint, details); // saves the username and the users carbon footprint to the result text file
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter valid numbers.");
                }
            }
            private static void SaveResultToFile(double result, string details) // Method that saves the user's calculation result to a file
            {
                using (StreamWriter writer = new StreamWriter(FilePath, true))
                {
                    writer.WriteLine($"{result},{details}");
                }
            }
            private static double CalculateTransportationFootprint(double milesDriven) // method that calculates the Transportation Footprint of the user
            {
                const double carEmissionFactor = 0.411; // kg CO2e per mile
                return milesDriven * carEmissionFactor;
            }

            private static double CalculateElectricityFootprint(double kWhUsed) // method that calculates the Electricity Footprint of the user
            {
                const double electricityEmissionFactor = 0.92; // kg CO2e per kWh
                return kWhUsed * electricityEmissionFactor;
            }

            private static double CalculateCarbonFootprint(double milesDriven, double kWhUsed) // method that calculates the Carbon Footprint of the user
            {
                return CalculateTransportationFootprint(milesDriven) + CalculateElectricityFootprint(kWhUsed);
            }
            public static void ViewHistory() // nethod to view the carbon calculators history
            {
                if (File.Exists(FilePath))
                {
                    string[] lines = File.ReadAllLines(FilePath);

                    if (lines.Length > 0)
                    {
                        string lastEntry = lines[lines.Length - 1];
                        string[] lastEntryParts = lastEntry.Split(',');

                        Console.WriteLine($"The last entered value is: {lastEntryParts[0]} kg CO2e ({lastEntryParts[1]})\n");

                        Console.WriteLine("All stored values:\n");
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] parts = lines[i].Split(',');
                            Console.WriteLine($"Index {i + 1}: {parts[0]} kg CO2e ({parts[1]})\n\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No records found.");
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                }
            }
            // Goals Method
            public static void HandleSetGoals() // method that allows user to type in their goals and type the word complete when they are done
            {
                var goals = new List<string>(); // goals are added to list named goals and stored for future use for the user

                Console.WriteLine("Insert your personal goals for the week. Type 'complete' when done:");
                string goal;

                while ((goal = Console.ReadLine()) != "complete")
                {
                    goals.Add(goal);
                }

                Console.Clear();
                Console.WriteLine("This week's goals are:\n");

                foreach (string g in goals) // displays the goals that have been typed in by the user
                {
                    Console.WriteLine(g);
                }
            }
            // Tips/Recommendations Method
            public class Recommendations
            {
                public void Recomm()
                {

                    Console.WriteLine("Enter your Calculated result:");
                    double result = double.Parse(Console.ReadLine());


                    if (result >= 1000) // if statement that processes the carbon footprint of the user and gives the user recommendations on improvement
                    {
                        Console.WriteLine(" \nYou can limit your electricity usage, use public transportation if you do not already and limit your consumption of animal products.");
                    }
                    else if (result >= 750)
                    {
                        Console.WriteLine("\nLimit electricity usage and everyday car travel. ");
                    }
                    else if (result <= 500)
                    {
                        Console.WriteLine("\nCarbon Footprint is at a neutral level.");
                    }
                    else
                    {
                        Console.WriteLine("\nYour Carbon Footprint is at a sustainable level, well done !!! ");
                    }

                }

            }

        }

        
   //         

            internal class Program
    {
            enum Options
            {
                Register = 1,
                Login,
                Exit

            }
            static void Main(string[] args)
        {
                    //main page that the user is automatically directed to asking user what they would like to do
                Console.WriteLine("===============================================================");
                Console.WriteLine("WELCOME TO CarbonCare WHAT WOULD YOU LIKE TO DO TODAY?:");
                Console.WriteLine("===============================================================");

                foreach (Options option in Enum.GetValues(typeof(Options)))
                {
                    Console.WriteLine($"{(int)option}.{option}\n");
                }

                Console.Write("Enter your choice:\n");
                int pick = int.Parse(Console.ReadLine());

                {
                    User user = new User();

                    switch (pick)
                    {
                        case 1:
                            //take user to the registration method which then adds the user to the text file with their username and password
                            user.Register(); 
                            Console.WriteLine("===============================================================\n");
                            break;
                        case 2:

                            if (user.Login()) // if statements that makes sure that implements the Login method to verify whether a user surely is in the system
                            {
                                User.Dashboard.DisplayMainMenu(); // once they have succesfully logged in the user will be presented with the dashboard
                            }
                            Console.WriteLine("=================================================================\n");

                            break;
                        case 3:
                            Console.WriteLine("You are exiting the app\n"); // option for user to log out of the application
                            break;

                    }


                    Console.ReadLine();
                }
            }
        
        }
    }
} 
