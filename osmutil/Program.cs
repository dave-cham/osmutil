using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace osmutil
{
    public class Program
    {
        private static string _userName = null;
        private static string _password = null;
        private static string _apiKey = null;
        private static bool _dryRun = false;
        private static List<string> _sectionFilter = null;
        private static Service _service = null;

        public static void Main(string[] args)
        {
            var command = GetCommand(args);
            GetOptions(args);

            QueryHelpers.ApiKey = _apiKey;
            _service = new Service(_userName, _password, _dryRun);

            switch(command)
            {
                case "emailaddresses":
                    var query = new ReportEmailAddresses(_service, _sectionFilter);
                    query.DoIt();
                    break;
                 case "checkAllContactCheckboxes":
                    var ticker = new TickboxTicker(_service, _sectionFilter);
                    ticker.DoIt();
                    break;
                case "findmemberswithmissingdata":
                    var finder = new FindMembersWithMisingData(_service, _sectionFilter);
                    finder.DoIt();
                    break;
            }

            Console.WriteLine("\n\nPress a key to exit");
            Console.ReadLine();
        }

        private static string GetCommand(string[] args)
        {
            if(args.Length < 1)
            {
                DisplayUsage();
                Environment.Exit(1);
            }
            return args[0].ToLower();
        }

        private static void GetOptions(string[] args)
        {
            for (int index = 1; index < args.Length;)
            {
                var current = args[index].ToLower();
                if(current[0] == '-')
                {
                    switch(current)
                    {
                        case "-apikey":
                            _apiKey = GetSingleOptionData(args, index);
                            index += 2;
                            break;
                        case "-user":
                            _userName = GetSingleOptionData(args, index);
                            index += 2;
                            break;
                        case "-password":
                            _password = GetSingleOptionData(args, index);
                            index += 2;
                            break;
                        case "-sectionfilter":
                            var count = 0;
                            _sectionFilter = GetMultipleOptionData(args, index, out count).Select(s => s.ToLower()).ToList();
                            index += count;
                            break;
                        case "-dryrun":
                            _dryRun = true;
                            index++;
                            break;
                        default:
                            DisplayUsage();
                            Environment.Exit(1);
                            break;
                    }
                }
            }
        }

        private static string GetSingleOptionData(string[] args, int index)
        {
            if(args.Length < index+2)
            {
                DisplayUsage();
                Environment.Exit(1);
            }

            return args[index + 1];
        }

        private static List<string> GetMultipleOptionData(string[] args, int index, out int count)
        {
            count = 1;
            var strOut = new List<string>();
            for(int i = index+1; i < args.Length; i++)
            {
                if (args[i][0] == '-')
                    break;
                strOut.Add(args[i]);
                count++;
            }

            return strOut;
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("osmutil command options");
            Console.WriteLine("Commands:");
            Console.WriteLine("  EmailAddresses - Dumps a comma-separated list of email addresses for all members");
            Console.WriteLine("  CheckAllContactCheckboxes - Checks the 'Receive emails from leaders' checkboxes for primary contact 1 and 2 of all members");
            Console.WriteLine("  FindMembersWithMissingData - Reports members that do not have a minimum amount of data");
            
                
            Console.WriteLine("\nOptions:");
            Console.WriteLine("  -user userName");
            Console.WriteLine("  -password password");
            Console.WriteLine("  -dryrun  Ensures that no updates are made");
            Console.WriteLine("  -sectionfilter matchText   If omitted all sections are used. If specified matchText is the text to match from the section name. e.g.  -sections Andrews \"St. Paul\" beavers");
        }
    }
}
