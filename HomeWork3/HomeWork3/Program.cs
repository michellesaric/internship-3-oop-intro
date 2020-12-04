using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace PrvaVjezba
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventData = new Dictionary<Event, List<Person>>()
            {
                {new Event("Concert A", 1, (Event.EventType)eventType.Concert, new DateTime(2020, 12, 4), new DateTime(2020, 12, 5)), new List<Person> {} },
                {new Event("Coffee", 2, (Event.EventType)eventType.Coffee, new DateTime(2020, 12, 7), new DateTime(2020, 12, 8)), new List<Person> { } },
                {new Event("Concert B", 3, (Event.EventType)eventType.Concert, new DateTime(2020, 12, 9), new DateTime(2020, 12, 11)), new List<Person> { } },
                {new Event("Study Session", 4, (Event.EventType)eventType.StudySession, new DateTime(2020, 12, 12), new DateTime(2020, 12, 14)), new List<Person> { } },
                {new Event("Math Lecture", 5, (Event.EventType)eventType.Lecture, new DateTime(2020, 12, 16), new DateTime(2020, 12, 17)), new List<Person> { } }
            };
            var choice = 0;
            do
            {
                Console.WriteLine();
                Menu();
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch(FormatException)
                {
                    Console.WriteLine("That is not a number");
                }
                switch (choice)
                {
                    case 1:
                        AddingANewEvent(eventData);
                        break;
                    case 2:
                        DeletingAnEvent(eventData);
                        break;
                    case 3:
                        EditingAnEvent(eventData);
                        break;
                    case 4:
                        AddingAPersonToEvent(eventData);
                        break;
                    case 5:
                            RemovingAPersonFromEvent(eventData);
                        break;
                    case 6:
                        PrintingEventDetails(eventData);
                        break;
                    case 7:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("You entered a number that is not in the main menu.");
                        break;
                }
            } while (true);


        }

        static void Menu()
        {
            Console.WriteLine("Choose a number which corelates to an action you want to be executed:");
            Console.WriteLine("1 - Adding an event");
            Console.WriteLine("2 - Deleting an event");
            Console.WriteLine("3 - Editing an event");
            Console.WriteLine("4 - Adding a person to an event");
            Console.WriteLine("5 - Removing a person from an event");
            Console.WriteLine("6 - Printing details of an event");
            Console.WriteLine("7 - Exiting the aplication");
        }
        static void AddingANewEvent(Dictionary<Event, List<Person>> eventData)
        {
            var toGoAgain = false;
            var wrongInput = false;
            var decision = "";
            var eventName = "a";
            var id = 5;
            var type = eventType.False;
            var startTime = new DateTime();
            var endTime = new DateTime();

            do
            {
                Console.WriteLine("Please enter a name for the event");
                eventName = CreatingAName();
                if (eventName == null)
                    return;
                do
                {
                    id = CreatingAnId();
                    if (id == 0)
                        return;

                    toGoAgain = CheckingForSameId(eventData, id);
                    if (toGoAgain)
                    {
                        Console.WriteLine("The id you wrote is already taken for some other event. If you would like to try and type in a different id, type yes. Any other input will lead to the main menu");
                        decision = Console.ReadLine();
                        if (decision.ToLower() == "yes")
                        {
                            toGoAgain = true;
                        }
                        else
                        {
                            return;
                        }
                    }
                } while (toGoAgain);

                type = CreatingANewType();
                if (type == eventType.False)
                    return;

                do
                {

                    startTime = CreatingAStartTime();
                    if (startTime.ToString("yyyy/MM/dd") == "0000/00/00")
                        return;
                    endTime = CreatingAnEndingTime(); ;
                    if (endTime.ToString("yyyy/MM/dd") == "0000/00/00")
                        return;

                    wrongInput = CheckingForAvailabaleDates(eventData, startTime, endTime);

                } while (wrongInput);

                Console.WriteLine("Are you sure you have entered everything correctly? If yes, please enter 'yes'. If not, any other input will lead you through the cycle again.");
                decision = Console.ReadLine();
                if (decision.ToLower() == "yes")
                {
                    toGoAgain = false;
                }
                else
                {
                    toGoAgain = true;
                }
            } while (toGoAgain);
            var createdEvent = new Event(eventName, id, (Event.EventType)type, startTime, endTime);
            var list = new List<Person>();
            eventData.Add(createdEvent, list);
        }
        static bool CheckingForAvailabaleDates(Dictionary<Event, List<Person>> eventData, DateTime startTime, DateTime endingTime)
        {
            foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
            {
                if (kvp.Key.StartTime < startTime)
                {
                    if (kvp.Key.EndTime >= startTime)
                    {
                        Console.WriteLine("Some other event has already started in this period. Please try again.");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (kvp.Key.StartTime < endingTime)
                    {
                        Console.WriteLine("There is already another event in this period. Please try again.");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        static bool CheckingForSameId(Dictionary<Event, List<Person>> eventData, int id)
        {
            var isTheSame = false;

            foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
            {
                if (kvp.Key.EventId == id)
                {
                    isTheSame = true;
                    return isTheSame;
                }
            }
            isTheSame = false;
            return isTheSame;
        }
        static void DeletingAnEvent(Dictionary<Event, List<Person>> eventData)
        {
            var id = 0;
            Console.WriteLine("Please enter the id of the event you would like to erase:");
            try
            {
                id = int.Parse(Console.ReadLine());
            } catch(FormatException)
            {
                Console.WriteLine("That is not a number, you will be redirected to the main menu");
                return;
            }
           
            var _isThereAnId = CheckingForSameId(eventData, id);

            if (!_isThereAnId)
            {
                Console.WriteLine("The number you have written is non-existing. You will be redirected to the main menu. Please choose the option 2 to try and delete again");
                return;
            }
            Console.WriteLine("Are you sure you would like to erase that event? If you type in 'yes', the event will be erased. Any other input will lead to the main menu:");
            var decision = Console.ReadLine();
            if (decision.ToLower() == "yes")
            {
                foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                {
                    if (kvp.Key.EventId == id)
                        eventData.Remove(kvp.Key);
                }
            }
            else
                return;

        }
        static void EditingAnEvent(Dictionary<Event, List<Person>> eventData)
        {
            var id = 0;
            Console.WriteLine("Please enter the id of the event you would like to edit:");
            try
            {
                id = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("That is not a number, you will be redirected to the main menu");
                return;
            }

            var _isThereAnId = false;
            var edit = "";
            var editTypeOfInt = 0;
            var editTime = new DateTime();

            _isThereAnId = CheckingForSameId(eventData, id);
            if (!_isThereAnId)
            {
                Console.WriteLine("The number you have written is non-existing. You will be redirected to the main menu. Please choose the option 3 to try and edit again");
                return;
            }

            var option = 0;
            var _toGoAgain = false;
            Console.WriteLine("Please choose what you would like to edit:");
            Console.WriteLine("1 - Event name");
            Console.WriteLine("2 - Even Id");
            Console.WriteLine("3 - Type of event");
            Console.WriteLine("4 - Starting time");
            Console.WriteLine("5 - Ending time");
            do
            {
                try
                {
                    option = int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("You didn't type in a number, would you like to try again? If you type in 'yes', the event name will be edited. Any other input will lead to the main menu.");
                    var decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        _toGoAgain = true;
                    }
                    else
                        return;
                }
            } while (_toGoAgain);
            switch (option)
            {
                case 1:
                    Console.WriteLine("You will now type in the edit for the name.");
                    edit = CreatingAName();
                    Console.WriteLine("Are you sure you would like to edit the name? If you type in 'yes', the event name will be edited. Any other input will lead to the main menu.");
                    var decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                        {
                            if (kvp.Key.EventId == id)
                            {
                                kvp.Key.Name = edit;
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                case 2:
                    Console.WriteLine("You will now type in the edit of the id.");
                    editTypeOfInt = CreatingAnId();
                    Console.WriteLine("Are you sure you would like to edit the id? If you type in 'yes', the event id will be edited. Any other input will lead to the main menu.");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                        {
                            if (kvp.Key.EventId == id)
                            {
                                kvp.Key.EventId = editTypeOfInt;
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                case 3:
                    var editTypeOfEnum = eventType.False;
                    Console.WriteLine("You will now choose a new type of event.");
                    editTypeOfEnum = CreatingANewType();
                    Console.WriteLine("Are you sure you would like to edit the type of event? If you type in 'yes', the type of event will be edited. Any other input will lead to the main menu.");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                        {
                            if (kvp.Key.EventId == id)
                            {
                                kvp.Key.TypeOfEvent = (Event.EventType)editTypeOfEnum;
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                case 4:
                    Console.WriteLine("You will now choose a new starting time");
                    editTime = CreatingAStartTime();
                    Console.WriteLine("Are you sure you would like to edit the starting time of the event? If you type in 'yes', the starting time of the event will be edited. Any other input will lead to the main menu.");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                        {
                            if (kvp.Key.EventId == id)
                            {
                                var wrongInput = CheckingForAvailabaleDates(eventData, editTime, kvp.Key.EndTime);
                                if (wrongInput)
                                {
                                    Console.WriteLine("You will be redirected to the main menu.");
                                    return;
                                }
                                else
                                {
                                    kvp.Key.StartTime = editTime;
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                case 5:
                    Console.WriteLine("You will now choose a new ending time");
                    editTime = CreatingAnEndingTime();
                    Console.WriteLine("Are you sure you would like to edit the ending time of the event? If you type in 'yes', the ending time of the event will be edited. Any other input will lead to the main menu.");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                        {
                            if (kvp.Key.EventId == id)
                            {
                                var wrongInput = CheckingForAvailabaleDates(eventData, kvp.Key.StartTime, editTime);
                                if (wrongInput)
                                {
                                    Console.WriteLine("You will be redirected to the main menu.");
                                    return;
                                }
                                else
                                {
                                    kvp.Key.EndTime = editTime;
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("You typed in a number that isn't in the menu. You will be redirected to the main menu. Please choose option 3 to edit again.");
                    return;
            }

        }
        static void AddingAPersonToEvent(Dictionary<Event, List<Person>> eventData)
        {
            var oib = "";
            var id = 0;
            var phoneNumber = "";
            var toGoAgain = false;
            var decision = "";
            Console.WriteLine("Please enter the id of the event to which you would like to add a person:");
            try
            {
                id = int.Parse(Console.ReadLine());
            }catch(FormatException)
            {
                Console.WriteLine("You didn't write a number, you will be relocated to the main menu");
                return;
            }
            var _isThereAnId = CheckingForSameId(eventData, id);

            if (!_isThereAnId)
            {
                Console.WriteLine("The number you have written is non-existing. You will be redirected to the main menu. Please choose the option 4 to try again");
                return;
            }

            Console.WriteLine("Enter the first name of the person:");
            var firstName = CreatingAName();
            if (firstName == null)
                return;
            Console.WriteLine("Enter the last name of the person");
            var lastName = CreatingAName();
            if (lastName == null)
                return;

            Console.WriteLine("Enter the Phone Number of the person(it has to be in 10 digits)");
            do
            {
                do
                {
                    phoneNumber = CreatingAName();
                    if (phoneNumber == null)
                        return;

                    if (phoneNumber.Length != 10)
                    {
                        Console.WriteLine("Your phone number isn't the right length.");
                        toGoAgain = true;
                    }
                    foreach (char letter in oib)
                    {
                        if ((int)letter < 48 || (int)letter > 58)
                        {
                            Console.WriteLine("Your phone number contains letters.");
                            toGoAgain = true;
                        }
                    }
                    if (toGoAgain)
                    {
                        Console.WriteLine("If you would like to try and type in a different phone number, type yes. Any other input will lead to the main menu");
                        decision = Console.ReadLine();
                        if (decision.ToLower() == "yes")
                        {
                            toGoAgain = true;
                            Console.WriteLine("Enter phone number again:");
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                while (toGoAgain);
                toGoAgain = CheckingForSamePhoneNumber(eventData, id, phoneNumber);

                if (toGoAgain)
                {
                    Console.WriteLine("The phone number you wrote already exists. If you would like to try and type in a different phone number, type yes. Any other input will lead to the main menu");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        toGoAgain = true;
                    }
                    else
                    {
                        return;
                    }
                }
            } while (toGoAgain);
            if (oib == null)
                return;



            Console.WriteLine("Enter the OIB of the person(it has to be 11 digits)");
            do {
                do
                {
                    oib = CreatingAName();
                    if (oib == null)
                        return;

                    if (oib.Length != 11)
                    {
                        Console.WriteLine("Your oib isn't the right length.");
                        toGoAgain = true;
                    }
                    foreach (char letter in oib)
                    {
                        if ((int)letter < 48 || (int)letter > 58)
                        {
                            Console.WriteLine("Your oib contains letters.");
                            toGoAgain = true;
                        }
                    }
                    if (toGoAgain)
                    {
                        Console.WriteLine("If you would like to try and type in a different oib, type yes. Any other input will lead to the main menu");
                        decision = Console.ReadLine();
                        if (decision.ToLower() == "yes")
                        {
                            toGoAgain = true;
                            Console.WriteLine("Enter oib again:");
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                while (toGoAgain);
                toGoAgain = CheckingForSameOIB(eventData, id, oib);

                if (toGoAgain)
                {
                    Console.WriteLine("The oib you wrote is already in the event. If you would like to try and type in a different oib, type yes. Any other input will lead to the main menu");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        toGoAgain = true;
                    }
                    else
                    {
                        return;
                    }
                }
            } while (toGoAgain);
            if (oib == null)
                return;

            var person = new Person(firstName, lastName, oib, phoneNumber);
            foreach(KeyValuePair<Event, List<Person>> kvp in eventData)
            {
                if(kvp.Key.EventId == id) 
                {
                    kvp.Value.Add(person);
                }
            }
        }
        static void RemovingAPersonFromEvent(Dictionary<Event, List<Person>> eventData)
        {
            var oib = "";
            var id = 0;
            var toGoAgain = false;
            var decision = "";
            Console.WriteLine("Please enter the id of the event to which you would like to remove a person from:");
            try
            {
                id = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("You didn't write a number, you will be relocated to the main menu");
                return;
            }
            var _isThereAnId = CheckingForSameId(eventData, id);

            if (!_isThereAnId)
            {
                Console.WriteLine("The number you have written is non-existing. You will be redirected to the main menu. Please choose the option 4 to try again");
                return;
            }
            Console.WriteLine("Enter the OIB of the person(it has to be 11 digits)");
            do
            {
                do
                {
                    oib = CreatingAName();
                    if (oib == null)
                        return;

                    if (oib.Length != 11)
                    {
                        Console.WriteLine("Your oib isn't the right length.");
                        toGoAgain = true;
                    }
                    foreach (char letter in oib)
                    {
                        if ((int)letter < 48 || (int)letter > 58)
                        {
                            Console.WriteLine("Your oib contains letters.");
                            toGoAgain = true;
                        }
                    }
                    if (toGoAgain)
                    {
                        Console.WriteLine("If you would like to try and type in a different oib, type yes. Any other input will lead to the main menu");
                        decision = Console.ReadLine();
                        if (decision.ToLower() == "yes")
                        {
                            toGoAgain = true;
                            Console.WriteLine("Enter oib again:");
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                while (toGoAgain);
                toGoAgain = RemovingThePerson(eventData, id, oib);

                if (toGoAgain)
                {
                    Console.WriteLine("The oib you wrote was erased from the event. Would you like to remove another person from the event? Type yes if you want to, any other input will send you to main menu.");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        toGoAgain = true;
                    }
                    else
                    {
                        return;
                    }
                }
                if(!toGoAgain)
                {
                    Console.WriteLine("Would you like to try again? Type yes if you want to, any other input will send you to main menu.");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        toGoAgain = true;
                    }
                    else
                    {
                        return;
                    }
                }
                } while (toGoAgain);
            if (oib == null)
                return;
        }
        static bool RemovingThePerson(Dictionary<Event, List<Person>> eventData, int id, string oib)
        {
            foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
            {
                if (kvp.Key.EventId == id)
                {
                    foreach (var person in kvp.Value)
                    {
                        if (person.OIB == oib)
                        {
                            Console.WriteLine("Are you sure you want to remove {0} {1}, {2} from event {3}? Type yes for confirmation", person.FirstName, person.LastName, person.PhoneNumber, kvp.Key.Name);
                            var decision = Console.ReadLine();
                            if (decision.ToLower() == "yes")
                            {
                                kvp.Value.Remove(person);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        static void PrintingEventDetails(Dictionary<Event, List<Person>> eventData)
        {
            var option = 0;
            var id = 0;
            Console.WriteLine("Choose one of these printing options:");
            Console.WriteLine("1 - Printing the details of the event in format: name – event type – start time – end time – duration – printing number of people");
            Console.WriteLine("2 - Printing the details about all people in format: [Order number of the person]. name – last name – phone number");
            Console.WriteLine("3 - Printing all the details(combination of 1 and 2)");
            Console.WriteLine("4 - Exiting this mini menu");

            try
            {
                option = int.Parse(Console.ReadLine());
            } catch(FormatException)
            {
                Console.WriteLine("Not a number,  you will go back to the main menu.");
            }


            switch (option)
            {
                case 1:
                    Console.WriteLine("Please enter the id of the event:");
                    try
                    {
                        id = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Not a number, you will go back to the main menu.");
                        return;
                    }
                    var _isThereAnId = CheckingForSameId(eventData, id);

                    if (!_isThereAnId)
                    {
                        Console.WriteLine("The number you have written is non-existing. You will be redirected to the main menu. Please choose the option 2 to try and delete again");
                        return;
                    }
                    foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                    {
                        if (kvp.Key.EventId == id)
                        {
                            kvp.Key.PrintingEvent();
                            Console.WriteLine(" - " + kvp.Value.Count);
                        }
                    }
                    break;
                case 2:
                    Console.WriteLine("Please enter the id of the event:");
                    try
                    {
                        id = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Not a number, you will go back to the main menu.");
                        return;
                    }
                    _isThereAnId = CheckingForSameId(eventData, id);

                    if (!_isThereAnId)
                    {
                        Console.WriteLine("The number you have written is non-existing. You will be redirected to the main menu. Please choose the option 2 to try and delete again");
                        return;
                    }
                    foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                    {
                        if (kvp.Key.EventId == id)
                        {
                            foreach(var person in kvp.Value)
                            {
                                
                                    Console.Write("{0}.", kvp.Value.IndexOf(person)+1);
                                    person.PrintingPerson();
                                
                            }
                        }
                    }
                    break;
                case 3:
                    Console.WriteLine("Please enter the id of the event:");
                    try
                    {
                        id = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Not a number, you will go back to the main menu.");
                        return;
                    }
                    _isThereAnId = CheckingForSameId(eventData, id);

                    if (!_isThereAnId)
                    {
                        Console.WriteLine("The number you have written is non-existing. You will be redirected to the main menu. Please choose the option 2 to try and delete again");
                        return;
                    }
                    foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
                    {
                        if (kvp.Key.EventId == id)
                        {
                            kvp.Key.PrintingEvent();
                            Console.WriteLine(" - " + kvp.Value.Count);
                            foreach (var person in kvp.Value)
                            { 
                                    Console.Write("{0}.", kvp.Value.IndexOf(person)+1);
                                    person.PrintingPerson();
                                
                            }
                        }
                    }
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("You typed in a number that isn't in the menu. You will be redirected to the main menu. Please choose option 6 to print again.");
                    return;
            }
        }
        static string CreatingAName()
        {
            var name = "";
            var toGoAgain = false;
            var decision = "";
            do
            {
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("You haven't written a anything. If you want to try again, type 'yes'. Any other input will lead to the main menu");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        toGoAgain = true;
                        Console.WriteLine("Please enter again:");
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                    toGoAgain = false;
            } while (toGoAgain);
            return name;
        }
        static int CreatingAnId()
        {
            var id = 0;
            var toGoAgain = true;
            var decision = "";
            do
            {
                Console.WriteLine("Input your event id:");
                try
                {
                    id = int.Parse(Console.ReadLine());
                }
                catch(FormatException)
                {
                    Console.WriteLine("You wrote something that isn't a number or a zero, would you like to try again? Please write 'yes' if the answer is yes. Any other input will take you to the main menu.");
                    decision = System.Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        toGoAgain = true;
                    }
                    else
                    {
                        return 0;
                    }
                }
                
                toGoAgain = false;
            } while (toGoAgain);
            return id;
        }
        static eventType CreatingANewType()
        {
            var type = eventType.False;
            var toGoAgain = false;
            var decision = "";
            var typeOfEvent = 0;
            do
            {
                Console.WriteLine("Please choose a type of event:");
                Console.WriteLine("1 - Coffee");
                Console.WriteLine("2 - Lecture");
                Console.WriteLine("3 - Concert");
                Console.WriteLine("4 - Study Session");
                
                
                try
                {
                    typeOfEvent = int.Parse(Console.ReadLine());
                }
                catch(FormatException)
                {
                    Console.WriteLine("You haven't typed in a number.");
                }
                switch (typeOfEvent)
                {
                    case 1:
                        type = eventType.Coffee;
                        toGoAgain = false;
                        continue;
                    case 2:
                        type = eventType.Lecture;
                        toGoAgain = false;
                        continue;
                    case 3:
                        type = eventType.Concert;
                        toGoAgain = false;
                        continue;
                    case 4:
                        type = eventType.StudySession;
                        toGoAgain = false;
                        continue;
                    default:
                        Console.WriteLine("You didn't type in the correct number. Would you like to try again? If yes, please type 'yes'. Any other input will lead to the main menu.");
                        decision = Console.ReadLine();
                        if (decision.ToLower() == "yes")
                        {
                            toGoAgain = true;
                            continue;
                        }
                        else
                        {
                            return eventType.False;
                        }
                }
            } while (toGoAgain);
            return type;
        }
        static DateTime CreatingAStartTime()
        {
            var startTime = new DateTime();
            var toGoAgain = false;
            var decision = "";
            CultureInfo provider = CultureInfo.InvariantCulture;
            do
            {
                Console.WriteLine("Please choose a starting time in a form of date/month/year hour:minutes");
                var dateString = Console.ReadLine();
                var format = "dd/MM/yyyy HH:mm";
                try
                {
                    startTime = DateTime.ParseExact(dateString, format, provider);
                    toGoAgain = false;
                }
                catch (FormatException)
                {
                    Console.WriteLine("The format isn't correct. Please try again by typing the word 'yes'. Any other input will lead to the main menu");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        toGoAgain = true;
                    }
                    else
                    {
                        return new DateTime(0000, 00, 00);
                    }
                }
            } while (toGoAgain);
            return startTime;
        }
        static DateTime CreatingAnEndingTime()
        {
            var endingTime = new DateTime();
            var toGoAgain = false;
            var decision = "";
            CultureInfo provider = CultureInfo.InvariantCulture;
            do
            {
                Console.WriteLine("Please choose an ending time in a form of date/month/year hour:minutes");
                var dateString = Console.ReadLine();
                var format = "dd/MM/yyyy HH:mm";
                try
                {
                    endingTime = DateTime.ParseExact(dateString, format, provider);
                    toGoAgain = false;
                }
                catch (FormatException)
                {
                    Console.WriteLine("The format isn't correct. Please try again by typing the word 'yes'. Any other input will lead to the main menu");
                    decision = Console.ReadLine();
                    if (decision.ToLower() == "yes")
                    {
                        toGoAgain = true;
                    }
                    else
                    {
                        return new DateTime(0000, 00, 00);
                    }
                }
            } while (toGoAgain);
            return endingTime;
        }
        static bool CheckingForSameOIB(Dictionary<Event, List<Person>> eventData, int id, string oib)
        {
            foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
            {
                if (kvp.Key.EventId == id)
                {
                    foreach (var person in kvp.Value)
                    {
                        if (person.OIB == oib)
                            return true;
                    }
                }
            }
            return false;
        }
        static bool CheckingForSamePhoneNumber(Dictionary<Event, List<Person>> eventData, int id, string phoneNumber)
        {
        foreach (KeyValuePair<Event, List<Person>> kvp in eventData)
            {
            if (kvp.Key.EventId == id)
                {
                foreach (var person in kvp.Value)
                    {
                    if (person.PhoneNumber == phoneNumber)
                        return true;
                    }
                }
            }
        return false;
        }
    }
    enum eventType
    {
        False = 0,
        Coffee = 1,
        Lecture = 2,
        Concert = 3,
        StudySession = 4
    }



}
